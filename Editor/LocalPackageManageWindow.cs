using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace LocalPackageManager
{
    /// <summary>
    /// Local Package Manager Window.
    /// WIP
    /// </summary>
    internal sealed class LocalPackageManageWindow : EditorWindow
    {
        private Rect _positionRect = new(0, 0, 960, 640);
        private PackageInfo _packageInfo;
        public PackageInfo PackageInfo
        {
            get => _packageInfo;
            private set => _packageInfo = value;
        }
        public string PackageInfoFromManifest { get; private set; }


        // TODO: ウィンドウの中身作る
        private void OnGUI()
        {
            titleContent = new GUIContent("Local Package Manager");

            GUILayout.Label("Local Package Manager");
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

            // 各種パッケージ情報を
            GUILayout.Label(PackageInfo.displayName);
            GUILayout.Label(PackageInfo.name);
            GUILayout.Label(PackageInfo.version);
            GUILayout.Label(PackageInfo.resolvedPath);
            GUILayout.Label(PackageInfo.packageId);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            GUILayout.Label(PackageInfoFromManifest);

            // Manifest.json の状態を表示するエリアを作成
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

            var pathType = (PackageInfo.resolvedPath == PackageInfoFromManifest) ? "絶対パス" : "相対パス";
            GUILayout.Label("Manifest.json には" + (pathType) + "で記録されています。");


            // DropDown
            if (GUILayout.Button("Show As Dropdown"))
            {
                var window = CreateInstance<LocalPackageManageWindow>();
                var windowSize = new Vector2(960, 640);
                window.ShowAsDropDown(new Rect(GUIUtility.GUIToScreenPoint(_positionRect.center), Vector2.zero), windowSize);
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
        /// Open the window.
        /// </summary>
        public static void Open(PackageInfo packageInfo)
        {
            var window = CreateInstance<LocalPackageManageWindow>();
            window.PackageInfo = packageInfo;
            window.PackageInfoFromManifest = AdditionalPackageInfo.GetPackageInfoFromManifestJson(packageInfo);
            window.Show();
        }

        /// <summary>
        /// Open the window as modal.
        /// </summary>
        public static void OpenModal(PackageInfo packageInfo)
        {
            var window = CreateInstance<LocalPackageManageWindow>();
            window.PackageInfo = packageInfo;
            window.PackageInfoFromManifest = AdditionalPackageInfo.GetPackageInfoFromManifestJson(packageInfo);
            window.ShowModal();
        }
    }
}