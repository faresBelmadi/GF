using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

using UnityEngine.UI;
public class PulseBloom_System : MonoBehaviour
{

    [SerializeField]
    bool startActive = false;
    [SerializeField]
    float defaultIntensity = 0f;
    [SerializeField]
    float bloomDuration = 1f;
    [SerializeField]
    public bool loop = false;
    [SerializeField]
    float bloomIntensity = 1f;
    [SerializeField]
    AnimationCurve bloomIntensityShape;
    [SerializeField]
    private Gradient bloomGradient;

    [SerializeField]
    private Material bloomMaterial;
    private Coroutine bloomRoutine = null;
    private CultureInfo en_us = CultureInfo.GetCultureInfo("en-US");
    private void Start()
    {
        bloomMaterial.SetFloat("_Intensity", .5f);
        if(startActive) TriggerBloom();
    }
    public void TriggerBloom(bool doOverride = true)
    {
        Debug.Log($"PulseBloomTriggered on {gameObject.name}");
        if (bloomRoutine != null)
        {
            if (doOverride) StopCoroutine(bloomRoutine);
            else return;
        }
        Debug.Log($"StartRoutine");
        bloomRoutine = StartCoroutine(BloomingRoutine());
    }
    private IEnumerator BloomingRoutine()
    {
        float timePassed = 0f;

        do
        {
            float applyedIntensity = bloomIntensityShape.Evaluate(timePassed / bloomDuration) * bloomIntensity;
            bloomMaterial.SetFloat("_Intensity", applyedIntensity);
            bloomMaterial.SetColor("_Color", bloomGradient.Evaluate(timePassed / bloomDuration));

            timePassed += Time.deltaTime;
            yield return null;

        } while (timePassed < bloomDuration);
        bloomMaterial.SetFloat("_Intensity", .5f);
        bloomMaterial.SetColor("_Color", Color.white);
        bloomRoutine = null;
        if (loop) { TriggerBloom(); }
    }
    //Duration
    public void OnDurationValueChanged(float duration)
    {
        UpdateDurationValue(duration);
    }
    public void OnDurationValueChanged(string duration)
    {
        UpdateDurationValue(float.Parse(duration, en_us));
    }
    private void UpdateDurationValue(float duration)
    {
        bloomDuration = duration;
    }
    //Loop
    public void OnToggleLoop()
    {
        loop = !loop;
    }
    //Intensity
    public void OnIntensityValueChanged(float intensity)
    {
        UpdateIntensityValue(intensity);
    }
    public void OnIntensityValueChanged(string intensity)
    {
        UpdateIntensityValue(float.Parse(intensity, en_us));
    }
    private void UpdateIntensityValue(float intensity)
    {
        bloomIntensity = intensity;
    }

}
