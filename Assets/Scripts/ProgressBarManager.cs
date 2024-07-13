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

    private Coroutine SmoothProgressRoutine;
    private float valueBuffer;

    public void InitPBar(int value, int max)
    {
        bar.fillAmount = (float)value/max;
        barDelta.fillAmount = (float)value/max;
    }
    public void UpdatePBar(int value, int max)
    {
        //Debug.Log($"Trigger Update Bars: {value}/{max}");
        if (!this.isActiveAndEnabled) return;
        if (SmoothProgressRoutine != null) StopCoroutine(SmoothProgressRoutine);
        SmoothProgressRoutine = StartCoroutine(SmoothUpdateBarCoroutine(value, max));

    }
    IEnumerator SmoothUpdateBarCoroutine(int value, int max)
    {
        //Debug.Log($"Update Bars: {value}/{max}");
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
            //Debug.Log($"Process Delayed Bar:({value}/{max}) +={step}\n{delayedBar.fillAmount}/{instantBar.fillAmount}");
            delayedBar.fillAmount += (isHeal?1f:-1f)* step;
            yield return null;
        }
        delayedBar.fillAmount = instantBar.fillAmount;
        //Debug.Log($"End Update Bars: {value}/{max}\nFill: {instantBar.fillAmount}\nDeltaFill:{delayedBar.fillAmount}\n");
        SmoothProgressRoutine = null;
    }

}
