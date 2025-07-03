using UnityEditor;
using UnityEngine;

namespace GF.GgsToCsv
{
    public class GgsToCsvImporterWindow : EditorWindow
    {
        private const string WindowTitle = "GoogleSheet to CSV";

        private readonly GgsToCsvImporter _importer = new GgsToCsvImporter();
        private readonly GgsToCsvValidator _validator = new GgsToCsvValidator();

        public static void ShowWindow()
        {
            GgsToCsvImporterWindow window = GetWindow<GgsToCsvImporterWindow>(true, WindowTitle, true);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 900, 200);
            window.ShowPopup();
        }

        private void OnGUI()
        {
            GUILayout.Label("Provide links to import macros:", new GUIStyle(EditorStyles.boldLabel) { fixedHeight = 30 });

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Game:", GUILayout.Width(80));
                _importer.ImportGameLocaMacroLink = EditorGUILayout.TextField(_importer.ImportGameLocaMacroLink);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Capa:", GUILayout.Width(80));
                _importer.ImportCapaLocaMacroLink = EditorGUILayout.TextField(_importer.ImportCapaLocaMacroLink);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Misc:", GUILayout.Width(80));
                _importer.ImportMiscLocaMacroLink = EditorGUILayout.TextField(_importer.ImportMiscLocaMacroLink);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);
            var isALinkMissing = string.IsNullOrEmpty(_importer.ImportGameLocaMacroLink) 
                        || string.IsNullOrEmpty(_importer.ImportCapaLocaMacroLink)
                        || string.IsNullOrEmpty(_importer.ImportMiscLocaMacroLink);
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
                    _importer.SaveMacroLinks();
                    _importer.ImportAll();
                    _validator.Validate();
                    Close();
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}