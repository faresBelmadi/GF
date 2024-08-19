using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    [SerializeField] private Image barDelta;
    [SerializeField] private Image bar;
    [SerializeField] private float hitsustainTime;
    [SerializeField] private float hitfallOffSpeed;

    [SerializeField] private Color HitColor;
    [SerializeField] private Color HealColor;
    [SerializeField] private PulseBloom_System bloomSystem;
    private Coroutine SmoothProgressRoutine;
    private Coroutine SmoothProgressPreviewRoutine;
    private float valueBuffer;

    private bool _isPreviewing = false;
    private float _previousBarValue;
    private float _previousBarDeltaValue;

    public void InitPBar(int value, int max)
    {
        bar.fillAmount = (float)value/max;
        barDelta.fillAmount = (float)value/max;
    }
    public void UpdatePBar(int value, int max)
    {
        if (!this.isActiveAndEnabled) return;
        if (_isPreviewing) StopPreview();
        if (SmoothProgressRoutine != null) StopCoroutine(SmoothProgressRoutine);
        SmoothProgressRoutine = StartCoroutine(SmoothUpdateBarCoroutine(value, max));
    }
    public void PreviewBar(int newValue, int maxValue)
    {
        if (!_isPreviewing)
        {
            _isPreviewing = true;
            _previousBarValue = bar.fillAmount;
            _previousBarDeltaValue = barDelta.fillAmount;

            //float tempValue = (float)newValue / (float)maxValue;
            //barDelta.color = HitColor;
            //bar.fillAmount = tempValue;


            if (!this.isActiveAndEnabled) return;
            if (SmoothProgressPreviewRoutine != null) StopCoroutine(SmoothProgressPreviewRoutine);
            SmoothProgressPreviewRoutine = StartCoroutine(SmoothPreviewBarCoroutine(newValue, maxValue));
        }
    }
    public void StopPreview()
    {
        if (_isPreviewing)
        {
            if (SmoothProgressPreviewRoutine != null)
                StopCoroutine(SmoothProgressPreviewRoutine);
            barDelta.fillAmount = _previousBarDeltaValue;
            bar.fillAmount = _previousBarValue;
            _isPreviewing = false;
        }
    }

    public void ToggleBloomPulses()
    {
        bloomSystem.OnToggleLoop();
        bloomSystem.TriggerBloom();
    }
    public void ToggleBloomPulses(bool doLoop)
    {
        bloomSystem.loop = doLoop;
        bloomSystem.TriggerBloom();
    }
    IEnumerator SmoothUpdateBarCoroutine(int value, int max)
    {
        float oldValue = valueBuffer;
        valueBuffer = (float)value / (float)max;
        bool isHeal = valueBuffer > oldValue ? true : false;

        Color deltaColor = isHeal ? HealColor : HitColor;
        Image instantBar = isHeal ? barDelta : bar;
        Image delayedBar = isHeal ? bar : barDelta;

        delayedBar.fillAmount = instantBar.fillAmount;
        barDelta.color = deltaColor;
        instantBar.fillAmount = valueBuffer;
        
        yield return new WaitForSecondsRealtime(hitsustainTime);
        while ((!isHeal && delayedBar.fillAmount > instantBar.fillAmount)
            || (isHeal && delayedBar.fillAmount < instantBar.fillAmount))
        {
            float step = Time.unscaledDeltaTime * hitfallOffSpeed;
            delayedBar.fillAmount += (isHeal?1f:-1f)* step;
            yield return null;
        }
        delayedBar.fillAmount = instantBar.fillAmount;
        SmoothProgressRoutine = null;
        SmoothProgressPreviewRoutine = null;
    }

    IEnumerator SmoothPreviewBarCoroutine(int value, int max)
    {
        float oldValue = _previousBarValue;
        valueBuffer = (float)value / (float)max;
        bool isHeal = valueBuffer > oldValue ? true : false;

        Color deltaColor = isHeal ? HealColor : HitColor;
        Image instantBar = isHeal ? barDelta : bar;
        Image delayedBar = isHeal ? bar : barDelta;

        delayedBar.fillAmount = instantBar.fillAmount;
        barDelta.color = deltaColor;
        //yield return new WaitForSecondsRealtime(hitsustainTime);

        while ((!isHeal && instantBar.fillAmount > valueBuffer)||(isHeal && instantBar.fillAmount < valueBuffer))
        {
            float step = Time.unscaledDeltaTime * hitfallOffSpeed * 2;
            instantBar.fillAmount += (isHeal ? 1f : -1f) * step;
            yield return null;
        }

        instantBar.fillAmount = valueBuffer;

        SmoothProgressPreviewRoutine = null;
    }
}
