using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using UnityEditor.PackageManager.UI.Internal;
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

        [InitializeOnLoadMethod]
        private static void InitializeOnLoadMethod()
        {
            // Debug.Log("UpmLPM:InitializeOnLoadMethod");
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
            // Debug.Log($"[UpmLPM] UPM package info: name=" + packageInfo.name);
            // Debug.Log($"[UpmLPM] UPM package info: source=" + packageInfo.source);
            // Debug.Log($"[UpmLPM] UPM package info: packageId=" + packageInfo.packageId);
            // Debug.Log($"[UpmLPM] UPM package info: resolvedPath=" + packageInfo.resolvedPath);
            // Debug.Log($"[UpmLPM] UPM package info: assetPath=" + packageInfo.assetPath);

            if (packageInfo.source == PackageSource.Local)
            {
                Debug.Log($"[UpmLPM] This package is local package.");
                LocalPackageManageWindow.OpenModal();
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
                Debug.Log($"[UpmLPM] This package is local package.");
                // TODO: ローカルパッケージの場合は、マニフェストを編集できるUIを作る
            }
            Initialize();
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        private void Initialize()
        {
            if (_initialized) return;

            // Find root element.
            VisualElement root = this;
            while (root != null && root.parent != null)
            {
                root = root.parent;
            }

            // Add open local package manager button.
            // TODO: UI作成(超ざっくりでいいのでとりあえず動くようにする)
            // TODO: UIの中身の実装

            _initialized = true;
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
    }
}