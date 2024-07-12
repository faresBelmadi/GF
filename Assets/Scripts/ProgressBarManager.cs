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

    private Coroutine playerGetDamagedRoutine;
    private float valueBuffer;

    public void InitPBar(int value, int max)
    {
        bar.fillAmount = (float)value/max;
        barDelta.fillAmount = (float)value/max;
    }
    public void UpdatePBar(int value, int max)
    {
        if (playerGetDamagedRoutine != null) StopCoroutine(playerGetDamagedRoutine);
        playerGetDamagedRoutine = StartCoroutine(PlayerGetDamagedCoroutine(value, max));

    }
    IEnumerator PlayerGetDamagedCoroutine(int value, int max)
    {
        //Debug.Log("Update HP Bars");
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
        do
        {
            delayedBar.fillAmount += isHeal?1f:-1f* Time.unscaledDeltaTime * hitfallOffSpeed;
            yield return null;
        } while ((!isHeal && delayedBar.fillAmount > instantBar.fillAmount)
            || (isHeal && delayedBar.fillAmount < instantBar.fillAmount));
        delayedBar.fillAmount = instantBar.fillAmount;
        playerGetDamagedRoutine = null;
    }

}
