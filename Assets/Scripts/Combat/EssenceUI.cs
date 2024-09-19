using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EssenceUI : MonoBehaviour
{
    public GameObject buttonConsumation;

    [SerializeField]
    private Button _consumeButton;
    [SerializeField]
    private Essence _essenceRootObject;
    [SerializeField]
    private TMP_Text _soulAmountText;
    [SerializeField]
    private Color _idleTextColor;
    [SerializeField]
    private Color _healTextColor;

    private void Start()
    {
        buttonConsumation.GetComponentInChildren<TMP_Text>().text += $" ({_essenceRootObject.Heal})";
        _soulAmountText.text = _essenceRootObject.amount.ToString();
        _soulAmountText.color = _idleTextColor;
        activate();
    }

    public void activate()
    {
        if (TutoManager.Instance != null && !TutoManager.Instance.ShowSoulConsumation)
            return;
       //buttonConsumation.SetActive(true); //Legacy code
        _consumeButton.interactable = true;
    }

    public void deactivate()
    {
        Invoke("invokedDeactivate", 2f);
    }


    private void invokedDeactivate()
    {
        //buttonConsumation.SetActive(false);
        _consumeButton.interactable = false;
    }
    public void ShowHeal()
    {
        _soulAmountText.text = _essenceRootObject.Heal.ToString();
        _soulAmountText.color = _healTextColor;
    }
    public void HideHeal()
    {
        _soulAmountText.text = _essenceRootObject.amount.ToString();
        _soulAmountText.color = _idleTextColor;
    }
    public void ShowPreviewOnHP()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BattleMan.player.PreviewHPBarUpdate(
                GameManager.Instance.BattleMan.player.Stat.Radiance + _essenceRootObject.Heal,
                GameManager.Instance.BattleMan.player.Stat.RadianceMax);

            GameManager.Instance.BattleMan.player.PreviewTensionBarUpddate();
        }
        else
        {
            TutoManager.Instance.BattleManager.player.PreviewHPBarUpdate(
                TutoManager.Instance.BattleManager.player.Stat.Radiance + _essenceRootObject.Heal,
                TutoManager.Instance.BattleManager.player.Stat.RadianceMax);

            TutoManager.Instance.BattleManager.player.PreviewTensionBarUpddate();
        }
    }
    public void StopPreviewOnHP()
    {
        if (GameManager.Instance.BattleMan != null)
        {
            GameManager.Instance.BattleMan.player.StopPReviewHPBarUpdate();
            GameManager.Instance.BattleMan.player.StopPreviewTensionBar();
        }
    }

}
