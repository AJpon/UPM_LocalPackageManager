using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace LocalPackageManager
{
    internal static class AdditionalPackageInfo
    {
        public static string GetPackageInfoFromManifestJson(PackageInfo packageInfo)
        {
            string rawJson = GetManifestJsonContent();
            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(rawJson);
            string dependencyVal = jsonObject["dependencies"][packageInfo.name].ToString();
            return dependencyVal;
        }

        internal static void SetPackageInfoToManifestJson(PackageInfo packageInfo, string packageInfoVal){
            string rawJson = GetManifestJsonContent();
            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(rawJson);
            jsonObject["dependencies"][packageInfo.name] = packageInfoVal;
            string outputJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            File.WriteAllText("./Packages/manifest.json", outputJson);
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
