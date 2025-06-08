using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GF.GgsToCsv
{
    public class GgsToCsvImporterWindow : EditorWindow
    {
        private const string WindowTitle = "GoogleSheet to CSV";

        private const string LocaSaveFolder = "Assets/StreamingAssets/Traduction";
        private string SettingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GFTools", "SheetToCsvImporter.settings");

        private string ImportGameLocaMacroLink = string.Empty;
        private string GameLocaSaveFile = "GameTraductionFile.csv";
        private string ImportCapaLocaMacroLink = string.Empty;
        private string CapaLocaSaveFile = "CapaTraductionFile.csv";
        private string ImportMiscLocaMacroLink = string.Empty;
        private string MiscLocaSaveFile = "MiscTraductionFile.csv";

        private readonly GgsToCsvImporter _importer = new GgsToCsvImporter();
        private readonly GgsToCsvValidator _validator = new GgsToCsvValidator();

        public static void ShowWindow()
        {
            GgsToCsvImporterWindow window = GetWindow<GgsToCsvImporterWindow>(true, WindowTitle, true);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 900, 200);
            window.LoadMacroLinks();
            window.ShowPopup();
        }

        private void OnGUI()
        {
            GUILayout.Label("Provide links to import macros:", new GUIStyle(EditorStyles.boldLabel) { fixedHeight = 30 });

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Game:", GUILayout.Width(80));
                ImportGameLocaMacroLink = EditorGUILayout.TextField(ImportGameLocaMacroLink);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Capa:", GUILayout.Width(80));
                ImportCapaLocaMacroLink = EditorGUILayout.TextField(ImportCapaLocaMacroLink);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Misc:", GUILayout.Width(80));
                ImportMiscLocaMacroLink = EditorGUILayout.TextField(ImportMiscLocaMacroLink);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);
            var isALinkMissing = string.IsNullOrEmpty(ImportGameLocaMacroLink) || string.IsNullOrEmpty(ImportCapaLocaMacroLink) || string.IsNullOrEmpty(ImportMiscLocaMacroLink);
            if (isALinkMissing)
            {
                EditorGUILayout.HelpBox("All three macro links must be provided.", MessageType.Error);
            }
            else
            {
                EditorGUILayout.HelpBox("Import might take a couple minutes, be patient!", MessageType.Info);
            }
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.Space();

                EditorGUI.BeginDisabledGroup(isALinkMissing);
                if (GUILayout.Button("Import", new GUIStyle(EditorStyles.miniButtonRight) { fixedWidth = 200, fixedHeight = 25 }))
                {
                    if (string.IsNullOrEmpty(ImportGameLocaMacroLink) || string.IsNullOrEmpty(ImportCapaLocaMacroLink) || string.IsNullOrEmpty(ImportMiscLocaMacroLink))
                    {
                        EditorUtility.DisplayDialog("Error", "Please provide all three macro links.", "OK");
                        return;
                    }

                    SaveMacroLinks();
                    ImportAll();
                    _validator.Validate();
                    Close();
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
        }

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

        private void SaveMacroLinks()
        {
            var settingsFolder = Path.GetDirectoryName(SettingsFilePath);
            if (!Directory.Exists(settingsFolder))
            {
                Directory.CreateDirectory(settingsFolder);
            }

            var settings = $"{ImportGameLocaMacroLink}\n{ImportCapaLocaMacroLink}\n{ImportMiscLocaMacroLink}";
            File.WriteAllText(SettingsFilePath, settings);
        }

        private void ImportAll()
        {
            if (!Directory.Exists(LocaSaveFolder))
            {
                Directory.CreateDirectory(LocaSaveFolder);
            }

            _importer.Import(ImportGameLocaMacroLink, Path.Combine(LocaSaveFolder, GameLocaSaveFile), "Game");
            _importer.Import(ImportCapaLocaMacroLink, Path.Combine(LocaSaveFolder, CapaLocaSaveFile), "Capa");
            _importer.Import(ImportMiscLocaMacroLink, Path.Combine(LocaSaveFolder, MiscLocaSaveFile), "Misc");

            AssetDatabase.Refresh();
        }
    }
}