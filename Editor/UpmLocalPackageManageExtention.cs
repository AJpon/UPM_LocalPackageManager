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
        private PackageInfo _packageInfo;
        private Button openLocalPackageManagerButton;

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
            if (packageInfo == null) return;
            if (packageInfo.source == PackageSource.Local)
            {
                LocalPackageManageWindow.Open(packageInfo);
            }
        }

        void IPackageManagerExtension.OnPackageRemoved(PackageInfo packageInfo)
        {
        }

        void IPackageManagerExtension.OnPackageSelectionChange(PackageInfo packageInfo)
        {
            // Debug.Log("[UpmLPM] OnPackageSelectionChange");

            Initialize();
            if (packageInfo == null)
            {
                openLocalPackageManagerButton.style.display = DisplayStyle.None;
                return;
            }

            // Debug.Log($"[UpmLPM] UPM package info: name=" + packageInfo.name);
            // Debug.Log($"[UpmLPM] UPM package info: source=" + packageInfo.source);
            // Debug.Log($"[UpmLPM] UPM package info: packageId=" + packageInfo.packageId);
            // Debug.Log($"[UpmLPM] UPM package info: resolvedPath=" + packageInfo.resolvedPath);
            // Debug.Log($"[UpmLPM] UPM package info: assetPath=" + packageInfo.assetPath);

            _packageInfo = packageInfo; //? 暫定処置
            if (packageInfo.source == PackageSource.Local)
            {
                // Debug.Log($"[UpmLPM] {packageInfo.displayName} is local package.");
                openLocalPackageManagerButton.style.display = DisplayStyle.Flex;
            }
            else
            {
                openLocalPackageManagerButton.style.display = DisplayStyle.None;
            }
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        private void Initialize()
        {
            if (_initialized) return;
            CreateLocalPackageManageButton();
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
            // Debug.Log($"[UpmLPM] _packageInfo.source: {_packageInfo.source}");
            openLocalPackageManagerButton = new(() => { LocalPackageManageWindow.Open(_packageInfo); });
            openLocalPackageManagerButton.text = "Open LPM"; //* 仮ラベル
            openLocalPackageManagerButton.name = "PackageOpenLocalPackageManagerButton";

            // Display property is none by default. (It set to flex when the package is local.)
            openLocalPackageManagerButton.style.display = DisplayStyle.None;

            // builtInActions はUPM のパッケージ詳細画面の右上にあるボタン群をまとめたVisualElement
            if (FindElement(root, x => x.name == "builtInActions") is VisualElement builtInActions)
            {
                builtInActions.Insert(0, openLocalPackageManagerButton);
            }
        }

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

            List<VisualElement> result =
                (List<VisualElement>)(findElementsMethod?.Invoke(null, new object[] { element, predicate }));
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