using UnityEditor;

public static class GFMenuEntries
{
    [MenuItem("Eternal's Path/Import loca files...")]
    public static void OpenMyPopup()
    {
        GgsToCsvImporterWindow.ShowWindow();
    }
}