using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace LocalPackageManager
{
    internal static class AdditionalPackageInfo
    {
        [System.Serializable]
        private class PackageManifest
        {
            public Dictionary<string, string> dependencies;
        }

        public static string GetPackageInfoFromManifestJson(PackageInfo packageInfo)
        {
            string rawJson = GetManifestJsonContent();
            var manifest = JsonUtility.FromJson<PackageManifest>(rawJson);
            return "";
            // return manifest.dependencies[packageInfo.name];
        } 

        private static string GetManifestJsonContent()
        {
            // プロジェクト生成設定にjsonを追加
            //? いらないかも
            var extensions = EditorSettings.projectGenerationUserExtensions;
            if (!extensions.Contains("json"))
            {
                EditorSettings.projectGenerationUserExtensions = extensions.Concat(new[] { "json" }).ToArray();
                AssetDatabase.SaveAssets();
            }
            string rawJson = File.ReadAllText("./Packages/manifest.json");
            return rawJson;
        }
    }
}