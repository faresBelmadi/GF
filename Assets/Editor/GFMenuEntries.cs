using GF.GgsToCsv;
using UnityEditor;

public static class GFMenuEntries
{
    [MenuItem("Eternal's Path/Import loca files...")]
    public static void OpenLocaFileImportWindow()
    {
        GgsToCsvImporterWindow.ShowWindow();
    }

    [MenuItem("Eternal's Path/Validate loca files")]
    public static void ValidateLocaFiles()
    {
        var validator = new GgsToCsvValidator();
        validator.Validate();
    }
}