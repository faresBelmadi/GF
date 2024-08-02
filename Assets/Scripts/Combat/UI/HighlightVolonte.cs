using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightVolonte : MonoBehaviour
{
    [SerializeField]
    private GameObject _highlightGameObject;
    [SerializeField]
    private PulseBloom_System _bloomComponent;
    [SerializeField]
    private Material _highlightMaterial;
    [SerializeField]
    private Slider _highlightSlider;

    [SerializeField]
    private Image _conscienceImage;
    [SerializeField]
    private Image _highlightConscienceImage;

    private int _selectVolonteCost = 0;
    private int _selectConscCost = 0;
    private int _selectRadCost = 0;

    public void EnableHighlighting(int volonteCost, int radCost, int conscCost)
    {
        
        _conscienceImage.enabled = true;
        _highlightConscienceImage.gameObject.SetActive(false);

        if (volonteCost > 0)
        {
            _highlightGameObject.SetActive(true);
            _highlightSlider.value = volonteCost;
            if (_bloomComponent.loop == true) _bloomComponent.OnToggleLoop();
            _bloomComponent.TriggerBloom(true);
        }

        if (radCost > 0)
        {
            // to do
        }

        if (conscCost > 0)
        {
        _conscienceImage.enabled = false;
        _highlightConscienceImage.gameObject.SetActive(true);

        _highlightConscienceImage.fillAmount = _conscienceImage.fillAmount - (conscCost / 10f);
        }

    }
    public void DisableHighlighting()
    {
        _conscienceImage.enabled = true;
        _highlightConscienceImage.gameObject.SetActive(false);

        _highlightSlider.value = _selectVolonteCost;
       
        if (_selectConscCost == 0 && _selectRadCost == 0 && _selectVolonteCost == 0)
        {

            _highlightGameObject.SetActive(false);
            return;
        }

        if (_selectVolonteCost > 0)
        {
            if (_bloomComponent.loop == false) _bloomComponent.OnToggleLoop();
            _bloomComponent.TriggerBloom(true);
        }
        
        if (_selectConscCost > 0)
        {
            _conscienceImage.enabled = false;
            _highlightConscienceImage.gameObject.SetActive(true);
            _highlightConscienceImage.fillAmount = _conscienceImage.fillAmount - (_selectConscCost / 10f);
        }
    }
    public void DisableHighlightingBetweenTarget()
    {
        _highlightSlider.value = 0;
        _conscienceImage.enabled = true;
        _highlightConscienceImage.gameObject.SetActive(false);

        _selectVolonteCost = 0;
        _selectConscCost = 0;
        _selectRadCost = 0;

    }
    public void SelectCostForHighlighing(int volonteCost, int radCost, int conscCost)
    {
        _selectVolonteCost = volonteCost;


        //if selecCOns > 0
        _selectConscCost = conscCost;
        _conscienceImage.enabled = false;
        _highlightConscienceImage.gameObject.SetActive(true);

        
        if (_bloomComponent.loop == false) _bloomComponent.OnToggleLoop();
        _bloomComponent.TriggerBloom(true);
    }
}
