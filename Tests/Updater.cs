using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace DOTools.Internal {

    public class Updater : Editor {

        public struct Package {

            public string version;

        }

        private const string PackageName = "com.rellfy.dotools";
        private const string PackageURL = "https://raw.githubusercontent.com/rellfy/dotools/master/package.json";
        private const string CanUpdateKey = "dotools.CanUpdate";
        private const string CheckForUpdateText = "DOTS/DOTools/Check for updates";
        private const string UpdateText = "DOTS/DOTools/Update";

        private static string PackageManifestPath =>
            Path.Combine(
                Directory.GetParent(Application.dataPath).FullName,
                "Packages",
                "manifest.json"
            );

        private static async Task<bool> IsUpdateAvailable() {
            WebClient wc = new WebClient();
            string json = await wc.DownloadStringTaskAsync(PackageURL);
            string versionText = JsonUtility.FromJson<Package>(json).version;
            Version version = Version.Parse(versionText);
            Version currentVersion = await GetLocalVersion();

            if (currentVersion == null)
                return false;

            bool updateAvailable = currentVersion.CompareTo(version) <= 0;
            return updateAvailable;
        }

        private static async Task<Version> GetLocalVersion() {
            ListRequest listRequest = Client.List(true);

            while (!listRequest.IsCompleted) {
                await Task.Delay(1);
            }

            return (
                from pack 
                in listRequest.Result
                where pack.name == PackageName
                where pack.source != PackageSource.Local
                select Version.Parse(pack.version)
            ).FirstOrDefault();
        }

        [MenuItem(CheckForUpdateText, false, 0)]
        [DidReloadScripts]
        private static async void CheckForUpdates() {
            bool canUpdate = await IsUpdateAvailable();
            EditorPrefs.SetBool(CanUpdateKey, canUpdate);
        }

        [MenuItem(UpdateText, false, 0)]
        public static void Update() {
            if (!File.Exists(PackageManifestPath))
                return;

            string text = File.ReadAllText(PackageManifestPath);
            int index = text.IndexOf("\"lock\"", StringComparison.Ordinal);
            int start = index + text.Substring(index).IndexOf("\"" + PackageName + "\"", StringComparison.Ordinal);
            int end = start + text.Substring(start).IndexOf("}", StringComparison.Ordinal) + 2;
            string entry = text.Substring(start, end - start);
                
            if (!entry.EndsWith(",")) {
                // Add missing coma.
                if (text.Substring(start - 2).Contains(",")) {
                    //  Spaces for tabs and 3 for quote, comma and }.
                    int comma = (start - 7) + text.Substring(start - 7).IndexOf(",", StringComparison.Ordinal);
                    text = text.Remove(comma, 1);
                }
            }

            text = text.Replace(entry, "");
            File.WriteAllText(PackageManifestPath, text);

            AssetDatabase.Refresh();
        }

        [MenuItem(UpdateText, true)]
        private static bool CanUpdate() {
            return EditorPrefs.GetBool(CanUpdateKey);
        }

    }

}