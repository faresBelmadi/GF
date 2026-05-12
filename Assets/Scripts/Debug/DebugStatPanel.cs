using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DebugStatPanel : MonoBehaviour
{
    [Header("Debug Stat")]
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

    [SerializeField] private TMP_Dropdown _typeEffectdropdown;
    [SerializeField] private TMP_Dropdown _dropdown;
    [Space]
    [Header("Debug Stat Panel")]
    [SerializeField] private TMP_Text _effectNameValueText;
    [SerializeField] private TMP_Text _typeEffectValueText;
    [SerializeField] private TMP_Text _targetEffectValueText;
    [SerializeField] private TMP_Text _percentEffectValueText;
    [SerializeField] private TMP_Text _rawValueValueText;

    [SerializeField] public List<Effet> EffectList;
    private Dictionary<TypeEffet,Dictionary<string, Effet>> _dictionnaryEffect = new Dictionary<TypeEffet, Dictionary<string, Effet>>();
    // Start is called before the first frame update
    void Start()
    {
        _debugPanel.SetActive(false);
        List<string> effectNames = new List<string>();
        foreach (var effet in EffectList)
        {

            effectNames.Add(effet.name);
            if (!_dictionnaryEffect.ContainsKey(effet.TypeEffet))
            {
                _dictionnaryEffect.Add(effet.TypeEffet, new Dictionary<string, Effet>());
            }

            if (_dictionnaryEffect[effet.TypeEffet].ContainsKey(effet.name))
            {
                Debug.LogError($"Duplicate effect name detected: {effet.name}. Only the first one will be added to the dictionary.");
                continue;
            }
            _dictionnaryEffect[effet.TypeEffet].Add(effet.name, ScriptableObject.Instantiate<Effet>(effet));
        }
        _typeEffectdropdown.AddOptions((Enum.GetNames(typeof(TypeEffet))).ToList());
        PopulateDropDown();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.playerStatHandler != null)
        {
            _faValueText.text = $"({GameManager.Instance.playerStatHandler.BaseForceDame} + {GameManager.Instance.playerStatHandler.ForceDameModifier}) = {GameManager.Instance.playerStatHandler.ForceDameTotal.ToString()}";
            _radValueText.text = GameManager.Instance.playerStatHandler.Radiance.ToString();
            _radMaxValueText.text = $"({GameManager.Instance.playerStatHandler.BaseRadianceMax} + {GameManager.Instance.playerStatHandler.RadianceMaxModifier}) = {GameManager.Instance.playerStatHandler.RadianceMaxTotal.ToString()}"; ;
            _resValueText.text = $"({GameManager.Instance.playerStatHandler.BaseResilience} + {GameManager.Instance.playerStatHandler.ResilienceModifier}) = {GameManager.Instance.playerStatHandler.ResilienceTotal.ToString()}";
            _vitValueText.text = $"({GameManager.Instance.playerStatHandler.BaseVitesse} + {GameManager.Instance.playerStatHandler.VitesseModifier}) = {GameManager.Instance.playerStatHandler.VitesseTotal.ToString()}"; ;
            _convicValueText.text = $"({GameManager.Instance.playerStatHandler.BaseConviction} + {GameManager.Instance.playerStatHandler.ConvictionModifier}) = {GameManager.Instance.playerStatHandler.ConvictionTotal.ToString()}"; ;
            _calmeValueText.text = $"({GameManager.Instance.playerStatHandler.BaseCalme} + {GameManager.Instance.playerStatHandler.CalmeModifier}) = {GameManager.Instance.playerStatHandler.CalmeTotal.ToString()}";
            _conscienceValueText.text = GameManager.Instance.playerStatHandler.Conscience.ToString();
            _conscienceMaxValueText.text = $"({GameManager.Instance.playerStatHandler.BaseConscienceMax} + {GameManager.Instance.playerStatHandler.ConscienceMaxModifier}) = {GameManager.Instance.playerStatHandler.ConscienceMaxTotal.ToString()}"; ;
            _clairvoyanceValueText.text = $"({GameManager.Instance.playerStatHandler.BaseClairvoyance} + {GameManager.Instance.playerStatHandler.ClairvoyanceModifier}) = {GameManager.Instance.playerStatHandler.ClairvoyanceTotal.ToString()}";
        }
        string effectTypeName = _typeEffectdropdown.options[_typeEffectdropdown.value].text;
        var type = (TypeEffet)Enum.Parse(typeof(TypeEffet), effectTypeName);
        string name = _dropdown.options[_dropdown.value].text;
        Effet effectToApply = _dictionnaryEffect[type][name.Replace("(Clone)", string.Empty)];
        if (effectToApply!= null)
        {
            _effectNameValueText.text = effectToApply.name;
            _typeEffectValueText.text = effectToApply.TypeEffet.ToString();
            _targetEffectValueText.text = effectToApply.Cible.ToString();
            _percentEffectValueText.text = effectToApply.Pourcentage.ToString();
            _rawValueValueText.text = effectToApply.ValeurBrut.ToString();
        }

    }
    public void PopulateDropDown()
    {
        string effectTypeName = _typeEffectdropdown.options[_typeEffectdropdown.value].text;
        var type = (TypeEffet)Enum.Parse(typeof(TypeEffet), effectTypeName) ;
        var t =_dictionnaryEffect[type].Values.ToList();
        var listName = t.Select(effet => effet.name).ToList();
        _dropdown.ClearOptions();
        _dropdown.AddOptions(listName);
    }

    public void TogglePanel()
    {
        _debugPanel.SetActive(!_debugPanel.activeSelf);
    }
    public void ApplyEffect()
    {
        string effectTypeName = _typeEffectdropdown.options[_typeEffectdropdown.value].text; 
        var type = (TypeEffet)Enum.Parse(typeof(TypeEffet), effectTypeName);
        string name = _dropdown.options[_dropdown.value].text;
        Effet EffectToApply = _dictionnaryEffect[type][name.Replace("(Clone)", string.Empty)];
        if (EffectToApply == null) return;
        GameManager.Instance.playerStatHandler.UpdateStat(EffectToApply.ResultEffet(GameManager.Instance.playerStatHandler, Cible: GameManager.Instance.playerStatHandler));
    }
    
    public void ResetStat()
    {
        GameManager.Instance.playerStatHandler.ResetStat();
        GameManager.Instance.playerStatHandler.SetConscience(4);
    }
}