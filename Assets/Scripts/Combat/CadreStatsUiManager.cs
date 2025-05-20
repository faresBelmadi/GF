using TMPro;
using UnityEngine;

public class CadreStatsUiManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI StatConviction;
    [SerializeField] private TextMeshProUGUI StatResilience;
    [SerializeField] private TextMeshProUGUI StatForceAme;
    [SerializeField] private TextMeshProUGUI StatConvictionValue;
    [SerializeField] private TextMeshProUGUI StatResilienceValue;
    [SerializeField] private TextMeshProUGUI StatForceAmeValue;

    public void ShowStats(EnnemiStat stats)
    {
        var ConvictionTrad = GameManager.Instance.CommonNameData.Conviction;
        var FATrad = GameManager.Instance.CommonNameData.ForceDame;
        var ResilienceTrad = GameManager.Instance.CommonNameData.Resilience;
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
