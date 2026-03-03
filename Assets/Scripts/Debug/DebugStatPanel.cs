using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugStatPanel : MonoBehaviour
{
    [SerializeField] private GameObject _debugPanel;
    [SerializeField] private TMP_Text _faValueText;
    [SerializeField] private TMP_Text _radValueText;
    [SerializeField] private TMP_Text _radMaxValueText;
    [SerializeField] private TMP_Text _resValueText;
    [SerializeField] private TMP_Text _vitValueText;
    [SerializeField] private TMP_Text _convicValueText;
    [SerializeField] private TMP_Text _calmeValueText;
    [SerializeField] private TMP_Text _conscienceValueText;
    [SerializeField] private TMP_Text _conscienceMaxValueText;
    [SerializeField] private TMP_Text _clairvoyanceValueText;

    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private List<Effet> _effectList;

    private Dictionary<string, Effet> _dictionnaryEffect = new Dictionary<string, Effet>();
    // Start is called before the first frame update
    void Start()
    {
        List<string> effectNames = new List<string>();
        foreach (var effet in _effectList)
        {
            effectNames.Add(effet.name);
            _dictionnaryEffect.Add(effet.name, ScriptableObject.Instantiate<Effet>(effet));
        }
        _dropdown.AddOptions(effectNames);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.playerStatHandler != null)
        {
            _faValueText.text = GameManager.Instance.playerStatHandler.ForceDameTotal.ToString();
            _radValueText.text = GameManager.Instance.playerStatHandler.Radiance.ToString();
            _radMaxValueText.text = GameManager.Instance.playerStatHandler.RadianceMaxTotal.ToString();
            _resValueText.text = GameManager.Instance.playerStatHandler.ResilienceTotal.ToString();
            _vitValueText.text = GameManager.Instance.playerStatHandler.VitesseTotal.ToString();
            _convicValueText.text = GameManager.Instance.playerStatHandler.ConvictionTotal.ToString();
            _calmeValueText.text = GameManager.Instance.playerStatHandler.CalmeTotal.ToString();
            _conscienceValueText.text = GameManager.Instance.playerStatHandler.Conscience.ToString();
            _conscienceMaxValueText.text = GameManager.Instance.playerStatHandler.ConscienceMaxTotal.ToString();
            _clairvoyanceValueText.text = GameManager.Instance.playerStatHandler.ClairvoyanceTotal.ToString();
        }
    }

    public void TogglePanel()
    {
        _debugPanel.SetActive(!_debugPanel.activeSelf);
    }
    public void ApplyEffect()
    {
        string name = _dropdown.options[_dropdown.value].text;
        Effet EffectToApply = _dictionnaryEffect[name];
        if (EffectToApply == null) return;
        GameManager.Instance.playerStatHandler.UpdateStat(EffectToApply.ResultEffet(GameManager.Instance.playerStatHandler, Cible: GameManager.Instance.playerStatHandler));

    }
    
}