using TMPro;
using UnityEngine;

public class CadreStatsUiManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _contentHolder;
    public TextMeshProUGUI StatConviction;
    public TextMeshProUGUI StatResilience;
    public TextMeshProUGUI StatForceAme;
    public TextMeshProUGUI StatConvictionValue;
    public TextMeshProUGUI StatResilienceValue;
    public TextMeshProUGUI StatForceAmeValue;

    public void ShowStats(EnnemiStat stats)
    {
        var ConvictionTrad = TradManager.instance.GetTranslation("G3N", "Conviction");
        var FATrad = TradManager.instance.GetTranslation("G6N", "Force d'Ame");
        var ResilienceTrad = TradManager.instance.GetTranslation("G4N", "Resilience");
        StatConviction.text = $"{ConvictionTrad}";
        StatConvictionValue.text = $" {stats.Conviction}";
        StatForceAme.text = $"{FATrad}";
        StatForceAmeValue.text = $" {stats.ForceAme}";
        StatResilience.text = $"{ResilienceTrad}";
        StatResilienceValue.text = $" {stats.Resilience}";
    }

    public void HideStats()
    {
        StatConviction.text = "";
        StatResilience.text = "";
        StatForceAme.text = "";
        StatForceAmeValue.text = "";
        StatResilienceValue.text = "";
        StatConvictionValue.text = "";
    }
}
