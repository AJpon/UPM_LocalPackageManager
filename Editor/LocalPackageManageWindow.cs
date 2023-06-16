using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace LocalPackageManager
{
    /// <summary>
    /// Local Package Manager Window.
    /// </summary>
    // TODO 対象パッケージのソースがローカルでない場合、その旨を表示する
    internal sealed class LocalPackageManageWindow : EditorWindow
    {
        const string URI_PREFIX = "file:";
        private Rect _positionRect = new(0, 0, 600, 340);

        private PackageInfo _packageInfo;
        public PackageInfo PackageInfo
        {
            get => _packageInfo;
            private set => _packageInfo = value;
        }
        public string PackageInfoFromManifest { get; private set; }
        public string PackageResolvedPath { get; private set; }

        private void OnGUI()
        {
            EditorApplication.projectChanged += () => UpdatePackageInfoFromManifest(); // プロジェクトが変更されたらパッケージ情報を更新
            minSize = new(440, 260);
            bool isAbsolutePath = PackageInfo.resolvedPath == PackageInfoFromManifest.Replace("file:", "").Replace("/", "\\");
            bool isLocalPackage = PackageInfo.source == UnityEditor.PackageManager.PackageSource.Local;

            titleContent = new GUIContent("Local Package Manager");
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.richText = true;

            // GUILayout.Label("<size=12><b>Local Package Manager</b></size>", style);
            // GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

            // 各種パッケージ情報を表示
            GUILayout.Label($"<size=16><b>{PackageInfo.displayName}</b></size>", style);
            GUILayout.Label(PackageInfo.name);
            GUILayout.Label(PackageInfo.version);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            GUILayout.Label("Installed From", EditorStyles.boldLabel);
            GUILayout.Label(PackageInfo.resolvedPath);
            // GUILayout.Label(PackageInfo.packageId);

            // manifest.json の状態を表示するエリアを作成
            GUILayout.Space(12);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            if (isLocalPackage)
            {
                var pathType = isAbsolutePath ? "絶対パス" : "相対パス";
                GUILayout.Label($"<size=14>Manifest.json には<color=yellow>{pathType}</color>で記録されています。</size>", style);
            }
            else
            {
                GUILayout.Label($"<size=14><color=red>このパッケージはローカルパッケージではありません。</color></size>", style);
            }
            GUILayout.Label($"<size=14><b>・ {PackageInfoFromManifest}</b></size>", style);
            GUILayout.FlexibleSpace();

            //################################
            // 以下、ボタン類
            //################################

            if (isLocalPackage)
            {
                if (GUILayout.Button("絶対パスに変更"))
                {
                    var abusolutePath = PackageInfo.resolvedPath;
                    abusolutePath = URI_PREFIX + abusolutePath.Replace("\\", "/");
                    AdditionalPackageInfo.SetPackageInfoToManifestJson(PackageInfo, abusolutePath);
                    // ウィンドウを更新
                    UpdatePackageInfoFromManifest();
                }
                if (GUILayout.Button("相対パスに変更"))
                {
                    var abusolutePath = PackageInfo.resolvedPath;
                    var pwd = Path.GetFullPath("./Packages/"); //? ここ "Packages" フォルダ起点であってる？
                    Uri sourceDir = new Uri(pwd);
                    Uri targetDir = new Uri(abusolutePath);
                    Uri relativeUri = sourceDir.MakeRelativeUri(targetDir);

                    // 相対パスを取得を取得できたか否かで処理分岐
                    bool isSucceeded = targetDir.ToString().Replace("file:///", "") != relativeUri.ToString();
                    if (isSucceeded)
                    {
                        var relativePath = relativeUri.ToString();
                        relativePath = URI_PREFIX + relativePath;
                        AdditionalPackageInfo.SetPackageInfoToManifestJson(PackageInfo, relativePath);
                    }
                    else
                    {
                        // 何らかの理由で相対パスを取得できなかった場合
                        Debug.LogError("[UpmLPM] 相対パスを取得できませんでした。");
                        bool close = EditorUtility.DisplayDialog("Error", "相対パスを取得できませんでした。", "マネージャーを閉じる", "マネージャーに戻る");
                        if (close) Close();
                    }

                    // ウィンドウを更新
                    UpdatePackageInfoFromManifest();
                }
            }
            if (GUILayout.Button("Open manifest.json"))
            {
                Unity.CodeEditor.CodeEditor.CurrentEditor.OpenProject(Path.GetFullPath("./Packages/manifest.json"));
            }
            if (GUILayout.Button("Close"))
            {
                Close();
                // if (Event.current.type == EventType.Repaint) _positionRect = GUILayoutUtility.GetLastRect();
            }

            // _positionRect = new Rect(GUIUtility.GUIToScreenPoint(_positionRect.center), new(960, 640));
            // position = _positionRect;
        }

        /// <summary>
        /// <see cref="PackageInfoFromManifest"/> を更新する。
        /// </summary>
        private void UpdatePackageInfoFromManifest(PackageInfo packageInfo = null)
        {
            if (packageInfo != null) PackageInfo = packageInfo;
            PackageInfoFromManifest = AdditionalPackageInfo.GetPackageInfoFromManifestJson(PackageInfo);
            // PackageResolvedPath = Path.GetFullPath("./Packages/"+relativePath);
        }

        /// <summary>
        /// Open the window.
        /// </summary>
        public static void Open(PackageInfo packageInfo)
        {
            var window = GetWindow<LocalPackageManageWindow>(); //* 既に開いている場合はそれをアクティブにする
            window.PackageInfo = packageInfo;
            window.UpdatePackageInfoFromManifest(packageInfo);
            window.Show();
        }

        /// <summary>
        /// Open the window as modal.
        /// </summary>
        public static void OpenModal(PackageInfo packageInfo)
        {
            var window = CreateInstance<LocalPackageManageWindow>();
            window.PackageInfo = packageInfo;
            window.UpdatePackageInfoFromManifest(packageInfo);
            window.ShowModal();
        }
    }
}