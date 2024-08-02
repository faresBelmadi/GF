using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightCost : MonoBehaviour
{

    [Header("Volonte componant")]
    [SerializeField]
    private GameObject _volonteHighlightGO;
    [SerializeField]
    private PulseBloom_System _bloomVolonteComponent;
    [SerializeField]
    private Slider _highlightSlider;

    [Header("Conscience bar for highlighting")]
    [SerializeField]
    private GameObject _conscienceHighlightGO;
    [SerializeField]
    private Image _conscienceImage;
    [SerializeField]
    private Image _highlightConscienceImage;
    [SerializeField]
    private Image _conscienceCalqueImage;
    [SerializeField]
    private PulseBloom_System _bloomConscienceComponent;

    [Header("Radiance bar for highlighting")]
    [SerializeField]
    private GameObject _radianceHighlightGO;
    [SerializeField]
    private Image _radianceImage;
    [SerializeField]
    private Image _highlightRadianceImage;
    [SerializeField]
    private Image _radianceCalqueImage;
    [SerializeField]
    private PulseBloom_System _bloomRadianceComponent;

    private int _selectVolonteCost = 0;
    private int _selectConscCost = 0;
    private int _selectRadCost = 0;

    public void EnableHighlighting(int volonteCost, int radCost, int conscCost)
    {

        _conscienceHighlightGO.SetActive(false);
        _radianceHighlightGO.SetActive(false);

        if (volonteCost > 0 
            && GameManager.instance.BattleMan.player.Stat.Volonter >= volonteCost
            && GameManager.instance.BattleMan.player.Stat.Radiance >= radCost
            && GameManager.instance.BattleMan.player.Stat.Conscience >= conscCost)
        {
            _volonteHighlightGO.SetActive(true);
            _highlightSlider.value = volonteCost;
            if (_bloomVolonteComponent.loop == true) _bloomVolonteComponent.OnToggleLoop();
            _bloomVolonteComponent.TriggerBloom(true);
        }

        if (radCost > 0 
            && GameManager.instance.BattleMan.player.Stat.Volonter >= volonteCost
            && GameManager.instance.BattleMan.player.Stat.Radiance >= radCost
            && GameManager.instance.BattleMan.player.Stat.Conscience >= conscCost)
        {
            _radianceHighlightGO.SetActive(true);
            _radianceCalqueImage.fillAmount = _radianceImage.fillAmount;

            float percentCost = ((float)radCost) / (float)GameManager.instance.playerStat.RadianceMax;
            _highlightRadianceImage.fillAmount = _radianceImage.fillAmount - percentCost;

            if (_bloomRadianceComponent.loop == true) _bloomRadianceComponent.OnToggleLoop();
            _bloomRadianceComponent.TriggerBloom(true);
        }

        if (conscCost > 0 
            && GameManager.instance.BattleMan.player.Stat.Volonter >= volonteCost
            && GameManager.instance.BattleMan.player.Stat.Radiance >= radCost
            && GameManager.instance.BattleMan.player.Stat.Conscience >= conscCost)
        {
            _conscienceHighlightGO.SetActive(true);
            _conscienceCalqueImage.fillAmount = _conscienceImage.fillAmount;
            
            _highlightConscienceImage.fillAmount = _conscienceImage.fillAmount - (conscCost / 10f);

            if (_bloomConscienceComponent.loop == true) _bloomConscienceComponent.OnToggleLoop();
            _bloomConscienceComponent.TriggerBloom(true);
        }

    }
    public void DisableHighlighting()
    {
        _conscienceHighlightGO.SetActive(false);
        _volonteHighlightGO.SetActive(false);
        _radianceHighlightGO.SetActive(false);

        _highlightSlider.value = _selectVolonteCost;

        //if (_selectConscCost == 0 && _selectRadCost == 0 && _selectVolonteCost == 0)
        //{
        //    _conscienceHighlightGO.SetActive(false);
        //    _volonteHighlightGO.SetActive(false);
        //    _radianceHighlightGO.SetActive(false);
        //    return;
        //}

        if (_selectVolonteCost > 0)
        {
            _volonteHighlightGO.SetActive(true);
            if (_bloomVolonteComponent.loop == false) _bloomVolonteComponent.OnToggleLoop();
            _bloomVolonteComponent.TriggerBloom(true);
        }

        if (_selectConscCost > 0)
        {
            _conscienceHighlightGO.SetActive(true);
            _conscienceCalqueImage.fillAmount = _conscienceImage.fillAmount;

            _highlightConscienceImage.fillAmount = _conscienceImage.fillAmount - (_selectConscCost / 10f);

            _bloomConscienceComponent.TriggerBloom(true);
        }
        if (_selectRadCost > 0)
        { 
            _radianceHighlightGO.SetActive(true);
            _radianceCalqueImage.fillAmount = _radianceImage.fillAmount;

            float percentCost = ((float)_selectRadCost) / (float)GameManager.instance.playerStat.RadianceMax;
            _highlightRadianceImage.fillAmount = _radianceImage.fillAmount - percentCost;

            _bloomRadianceComponent.TriggerBloom(true);
        }
    }
    public void DisableHighlightingBetweenTarget()
    {
        _highlightSlider.value = 0;
        _conscienceHighlightGO.SetActive(false);
        _radianceHighlightGO.SetActive(false);

        _selectVolonteCost = 0;
        _selectConscCost = 0;
        _selectRadCost = 0;

    }
    public void SelectCostForHighlighing(int volonteCost, int radCost, int conscCost)
    {
        if (_conscienceHighlightGO.activeSelf && conscCost > 0)
        {
             _selectConscCost = conscCost;
            _conscienceHighlightGO.SetActive(true);

            if (_bloomConscienceComponent.loop == true) _bloomConscienceComponent.OnToggleLoop();
            _bloomConscienceComponent.TriggerBloom(true);
        }
        if (_radianceHighlightGO.activeSelf && radCost > 0)
        {
            _selectRadCost = radCost;
            _radianceHighlightGO.SetActive(true);

            if (_bloomRadianceComponent.loop == true) _bloomRadianceComponent.OnToggleLoop();
            _bloomRadianceComponent.TriggerBloom(true);
        }

        if (_volonteHighlightGO.activeSelf)
        {
            _selectVolonteCost = volonteCost;
            if (_bloomVolonteComponent.loop == false) _bloomVolonteComponent.OnToggleLoop();
            _bloomVolonteComponent.TriggerBloom(true);
        }
    }
}
