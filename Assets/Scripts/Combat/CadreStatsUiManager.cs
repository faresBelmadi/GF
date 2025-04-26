using System.Collections.Generic;
using TMPro;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

public class CadreStatsUiManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _contentHolder;
    public TextMeshProUGUI StatConviction;
    public TextMeshProUGUI StatResilience;
    public TextMeshProUGUI StatForceAme;

    public void ShowStats(EnnemiStat stats)
    {
        var ConvictionTrad = TradManager.instance.GetTranslation("G3N", "Conviction");
        var FATrad = TradManager.instance.GetTranslation("G6N", "Force d'Ame");
        var ResilienceTrad = TradManager.instance.GetTranslation("G4N", "Resilience");
        StatConviction.text = $"{ConvictionTrad} : {stats.Conviction}";
        StatForceAme.text = $"{FATrad} : {stats.ForceAme}";
        StatResilience.text = $"{ResilienceTrad} : {stats.Resilience}";
    }

    public void HideStats()
    {
        StatConviction.text = "";
        StatResilience.text = "";
        StatForceAme.text = "";
    }
}
