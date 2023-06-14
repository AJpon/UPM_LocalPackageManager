using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace LocalPackageManager
{
    /// <summary>
    /// UPM Local Package Manager Extention.
    /// </summary>

    [InitializeOnLoad]
    internal sealed class UpmLocalPackageManageExtention : VisualElement, IPackageManagerExtension
    {
        //################################
        // Private Members.
        //################################
        private bool _initialized;
        // private Button _openLocalPackageManagerButton;
        private PackageInfo _packageInfo;

        [InitializeOnLoadMethod]
        private static void InitializeOnLoadMethod()
        {
            // Register extension.
            var ext = new UpmLocalPackageManageExtention();
            PackageManagerExtensions.RegisterExtension(ext as IPackageManagerExtension);
        }

        //################################
        // IPackageManagerExtension Members.
        //################################
        VisualElement IPackageManagerExtension.CreateExtensionUI()
        {
            _initialized = false;
            return this;
        }
        void IPackageManagerExtension.OnPackageAddedOrUpdated(PackageInfo packageInfo)
        {
            // Debug.Log("[UpmLPM] OnPackageAddedOrUpdated");
            if (packageInfo == null) return;
            if (packageInfo.source == PackageSource.Local)
            {
                // Debug.Log($"[UpmLPM] This package is local package.");
                LocalPackageManageWindow.OpenModal(packageInfo);
            }
        }

        void IPackageManagerExtension.OnPackageRemoved(PackageInfo packageInfo)
        {
        }

        void IPackageManagerExtension.OnPackageSelectionChange(PackageInfo packageInfo)
        {
            // Debug.Log("[UpmLPM] OnPackageSelectionChange");
            if (packageInfo == null) return;
            // Debug.Log($"[UpmLPM] UPM package info: name=" + packageInfo.name);
            // Debug.Log($"[UpmLPM] UPM package info: source=" + packageInfo.source);
            // Debug.Log($"[UpmLPM] UPM package info: packageId=" + packageInfo.packageId);
            // Debug.Log($"[UpmLPM] UPM package info: resolvedPath=" + packageInfo.resolvedPath);
            // Debug.Log($"[UpmLPM] UPM package info: assetPath=" + packageInfo.assetPath);
            if (packageInfo.source == PackageSource.Local)
            {
                // Debug.Log($"[UpmLPM] {packageInfo.displayName} is local package.");
                // TODO: ローカルパッケージの場合は、マニフェストを編集できるUI出す
                _packageInfo = packageInfo; //? 暫定処置
                // CreateLocalPackageManageButton();
            } else {
                // RemoveLocalPackageManageButton();
            }
            Initialize();
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        private void Initialize()
        {
            if (_initialized) return;
            CreateLocalPackageManageButton(); // TODO この実装だとローカルパッケージ以外でもUIが表示されるので後で修正
            _initialized = true;
        }

        /// <summary>
        /// UPM でローカルパッケージを選択したときに追加のUIを作成する
        /// </summary>
        private void CreateLocalPackageManageButton()
        {
            // Find root element.
            VisualElement root = this;
            while (root != null && root.parent != null)
            {
                root = root.parent;
            }

            // Add open local package manager button.
            // TODO: UI作成(超ざっくりでいいのでとりあえず動くようにする)
            Button openLocalPackageManagerButton = new(() => { LocalPackageManageWindow.Open(_packageInfo); });
            openLocalPackageManagerButton.text = "Edit"; //* 仮ラベル
            if (FindElement(root, x => x.name == "PackageRemoveCustomButton") is Button removeButton)
            {
                removeButton.parent.Insert(0, openLocalPackageManagerButton);
            }
        }

        /// <summary>
        /// 作成したUIを削除する
        /// </summary>
        private void RemoveLocalPackageManageButton()
        {
            // Find root element.
            VisualElement root = this;
            while (root != null && root.parent != null)
            {
                root = root.parent;
            }

            // Remove open local package manager button.
            // TODO: 追加したボタンの削除処理実装
            if (FindElement(root, x => x.name == "PackageRemoveCustomButton") is Button removeButton)
            {
                var parent = removeButton.parent;
                Debug.Log(parent.childCount);
                // removeButton.parent.RemoveAt(0);
            }
        }

        /// <summary>
        /// Open manifest.json in current project.
        /// </summary>
        /*
        private void OpenManifestJson()
        {
            // json files will be opend with code editor.
            var extensions = EditorSettings.projectGenerationUserExtensions;
            if (!extensions.Contains("json"))
            {
                EditorSettings.projectGenerationUserExtensions = extensions.Concat(new[] { "json" }).ToArray();
                AssetDatabase.SaveAssets();
            }

            // Open manifest.json with current code editor.
            Unity.CodeEditor.CodeEditor.CurrentEditor.OpenProject(Path.GetFullPath("./Packages/manifest.json"));
        } 
        */

        /// <summary>
        /// Execute VisualElementExtention.FindElements method.
        /// </summary>
        /// <seealso cref="https://github.com/Unity-Technologies/UnityCsReference/blob/2023.2/Modules/UIBuilder/Editor/Utilities/VisualElementExtensions/VisualElementExtensions.cs#L387"/>
        /// <param name="element">検索対象の VisualElement</param>
        /// <param name="predicate">検索条件。検索対象のVisualElementを引数として受け取った際にtrueを返す関数</param>
        /// <returns>条件に該当する VisualElement のリスト</returns>
        private static List<VisualElement> FindElements(VisualElement element, Func<VisualElement, bool> predicate)
        {
            string engineAssemblyPath = InternalEditorUtility.GetEngineAssemblyPath();
            string engineAssemblyDirectoryName = Path.GetDirectoryName(engineAssemblyPath).Replace("\\", "/");
            Assembly assembly = Assembly.LoadFile($@"{engineAssemblyDirectoryName}/UnityEditor.UIBuilderModule.dll");
            Type type = assembly.GetType("Unity.UI.Builder.VisualElementExtensions");
            if (type == null)
            {
                Debug.LogError("[UpmLPM] Unity.UI.Builder.VisualElementExtention is not found.");
                return null;
            }
            MethodInfo findElementsMethod = type.GetMethod("FindElements", BindingFlags.Static | BindingFlags.Public);
            if (findElementsMethod == null)
            {
                Debug.LogError("[UpmLPM] Unity.UI.Builder.VisualElementExtention.FindElements() is not found.");
                return null;
            }

            List<VisualElement> result = (List<VisualElement>)(findElementsMethod?.Invoke(null, new object[] { element, predicate }));
            return result;
        }

        /// <summary>
        /// 条件に合ったVisualElementを取得する
        /// </summary>
        /// <param name="element">検索対象の VisualElement</param>
        /// <param name="predicate">検索条件。検索対象のVisualElementを引数として受け取った際にtrueを返す関数</param>
        /// <returns>一番最初に条件に該当した VisualElement</returns>
        private static VisualElement FindElement(VisualElement element, Func<VisualElement, bool> predicate)
        {
            List<VisualElement> result = FindElements(element, predicate);
            if (result == null || result.Count == 0) return null;
            return result[0];
        }
    }
}