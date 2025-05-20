using UnityEditor;
using UnityEngine;
using System.Net;

public class GgsToCsvImporterWindow : EditorWindow
{
    private const string WindowTitle = "GoogleSheet to CSV";

    // temp link, this will be obsolete very very soon :3
    private string GGSheetLink = @"https://script.google.com/macros/s/AKfycbzoAHGegv6U88mpjGT7bWesvIQoCC4dEBSHLgaDFfJNHShKFmMKKBekcg_g9Pe6HDkvLA/exec";
    private string SavePath = "Assets/NonoSheet.csv";

    public static void ShowWindow()
    {
        GgsToCsvImporterWindow window = GetWindow<GgsToCsvImporterWindow>(true, WindowTitle, true);
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 300, 150);
        window.ShowPopup();
    }

    private void OnGUI()
    {
        GUILayout.Label("GoogleSheet link:", EditorStyles.boldLabel);
        GGSheetLink = GUILayout.TextField(GGSheetLink);
        if (GUILayout.Button("Import"))
        {
            Import();
            Close();
        }
    }

    private void Import()
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(GGSheetLink, SavePath);
            }

            AssetDatabase.Refresh();
            Debug.Log("CSV downloaded to " + SavePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to download CSV: " + ex.Message);
        }
    }
}
