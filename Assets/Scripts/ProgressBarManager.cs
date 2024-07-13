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
        if (!this.isActiveAndEnabled) return;
        if (SmoothProgressRoutine != null) StopCoroutine(SmoothProgressRoutine);
        SmoothProgressRoutine = StartCoroutine(SmoothUpdateBarCoroutine(value, max));

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
    }

}
