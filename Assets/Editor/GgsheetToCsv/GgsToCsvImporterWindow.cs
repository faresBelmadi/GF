using UnityEditor;
using UnityEngine;
using System.Net;

public class GgsToCsvImporterWindow : EditorWindow
{
    private const string WindowTitle = "GoogleSheet to CSV";

    // temp links, this will be obsolete very very soon :3
    private string ImportGameLocaMacroLink = @"https://script.google.com/macros/s/AKfycbzoAHGegv6U88mpjGT7bWesvIQoCC4dEBSHLgaDFfJNHShKFmMKKBekcg_g9Pe6HDkvLA/exec";
    private string GameLocaSavePath = "Assets/NonoGameSheet.csv";
    private string ImportCapaLocaMacroLink = string.Empty;
    private string CapaLocaSavePath = "Assets/NonoCapaSheet.csv";
    private string ImportMiscLocaMacroLink = string.Empty;
    private string MiscLocaSavePath = "Assets/NonoMiscSheet.csv";

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
                Import();
                Close();
            }
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void LoadMacroLinks()
    {

    }

    private void SaveMacroLinks()
    {

    }

    private void Import()
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(ImportGameLocaMacroLink, GameLocaSavePath);
            }

            AssetDatabase.Refresh();
            Debug.Log("CSV downloaded to " + GameLocaSavePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to download CSV: " + ex.Message);
        }
    }
}
