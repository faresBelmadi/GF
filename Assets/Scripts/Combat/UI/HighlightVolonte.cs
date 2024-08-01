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


    public void EnableHighlighting(int cost)
    {
        _highlightGameObject.SetActive(true);
        _highlightSlider.value = cost;
        _bloomComponent.TriggerBloom(true);
    }
    public void DisableHighlighting()
    {
        _highlightSlider.value = 0;
        _highlightGameObject.SetActive(false);
    }
}
