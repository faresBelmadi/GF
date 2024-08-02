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

    private int _selectedCost = 0;

    public void EnableHighlighting(int cost)
    {
        _highlightGameObject.SetActive(true);
        _highlightSlider.value = cost;
        if (_bloomComponent.loop == true) _bloomComponent.OnToggleLoop();
        _bloomComponent.TriggerBloom(true);
    }
    public void DisableHighlighting()
    {
        _highlightSlider.value = _selectedCost;
        if (_selectedCost > 0)
        {
            if (_bloomComponent.loop == false) _bloomComponent.OnToggleLoop();
            _bloomComponent.TriggerBloom(true);
        }
        else
        {
            _highlightGameObject.SetActive(false);
        }
    }
    public void SelectCostForHighlighing(int cost)
    {
        _selectedCost = cost;
        if (_bloomComponent.loop == false) _bloomComponent.OnToggleLoop();
        _bloomComponent.TriggerBloom(true);
    }
}
