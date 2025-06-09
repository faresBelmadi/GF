using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GF.GgsToCsv
{
    public class GgsToCsvImporter
    {
        public GgsToCsvImporter()
        {
            LoadMacroLinks();
        }

        private const string LocaSaveFolder = "Assets/StreamingAssets/Traduction";
        private string SettingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GFTools", "SheetToCsvImporter.settings");

        public string ImportGameLocaMacroLink { get; set; } = string.Empty;
        private string GameLocaSaveFile = "GameTraductionFile.csv";
        public string ImportCapaLocaMacroLink { get; set; } = string.Empty;
        private string CapaLocaSaveFile = "CapaTraductionFile.csv";
        public string ImportMiscLocaMacroLink { get; set; } = string.Empty;
        private string MiscLocaSaveFile = "MiscTraductionFile.csv";

        private void LoadMacroLinks()
        {
            if (!File.Exists(SettingsFilePath))
                return;

            var settings = File.ReadAllText(SettingsFilePath);
            var macros = settings.Split('\n');
            if (macros.Length >= 0)
                ImportGameLocaMacroLink = macros[0];
            if (macros.Length >= 1)
                ImportCapaLocaMacroLink = macros[1];
            if (macros.Length >= 2)
                ImportMiscLocaMacroLink = macros[2];
        }

        public void SaveMacroLinks()
        {
            var settingsFolder = Path.GetDirectoryName(SettingsFilePath);
            if (!Directory.Exists(settingsFolder))
            {
                Directory.CreateDirectory(settingsFolder);
            }

            var settings = $"{ImportGameLocaMacroLink}\n{ImportCapaLocaMacroLink}\n{ImportMiscLocaMacroLink}";
            File.WriteAllText(SettingsFilePath, settings);
        }

        public void ImportAll()
        {
            if (!Directory.Exists(LocaSaveFolder))
            {
                Directory.CreateDirectory(LocaSaveFolder);
            }

            Import(ImportGameLocaMacroLink, Path.Combine(LocaSaveFolder, GameLocaSaveFile), "Game");
            Import(ImportCapaLocaMacroLink, Path.Combine(LocaSaveFolder, CapaLocaSaveFile), "Capa");
            Import(ImportMiscLocaMacroLink, Path.Combine(LocaSaveFolder, MiscLocaSaveFile), "Misc");

            AssetDatabase.Refresh();
        }

        private void Import(string macroLink, string outputPath, string logInfo)
        {
            string content = string.Empty;
            try
            {
                using (WebClient client = new WebClient())
                {
                    content = client.DownloadString(macroLink);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to download {logInfo} from Web App: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError($"Web App provided empty content for {logInfo}!");
                return;
            }
            else if (content.StartsWith("Error:"))
            {
                Debug.LogError($"Web App returned for {logInfo}: {content}");
                return;
            }

            content = CleanupContent(content);

            File.WriteAllText(outputPath, content);
            Debug.Log($"Downloaded CSV for {logInfo}.", AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(outputPath));
        }

        private string CleanupContent(string content)
        {
            var result = content;

            result = Regex.Replace(result, @" (?=[!?:])", "\u00A0"); // non-breakable space before punctuations
            result = Regex.Replace(result, @"[\r\n\u0085\u2028\u2029]+(?=\""?;)", ""); // remove unwanted line jumps

            return result;
        }
    }
}