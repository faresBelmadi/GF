using GF.GgsToCsv;
using GF.Tools.Build;
using UnityEditor;

public static class GFMenuEntries
{
    [MenuItem("Eternals' Path/Import loca files...")]
    public static void OpenLocaFileImportWindow()
    {
        GgsToCsvImporterWindow.ShowWindow();
    }

    [MenuItem("Eternals' Path/Validate loca files")]
    public static void ValidateLocaFiles()
    {
        var validator = new GgsToCsvValidator();
        validator.Validate();
    }

    [MenuItem("Eternals' Path/Build/Prepare build", priority=50)]
    public static void PrepareBuild()
    {
        var preparator = new BuildPreparator();
        preparator.Prepare();
    }
}