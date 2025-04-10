using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "New ApplyBuff passiv", menuName = "PassiveEffect/New ApplyBuff passiv")]
public class ApplyBuffPassif : ScriptableObject, IStartTurnPassive
{
    [SerializeField]
    private BaseStats _triggerStat;
    [SerializeField]
    private int _levelStatToTrigger;
    [SerializeField]
    private bool _gainTension;
    [Tooltip("Choisis un des buff aléatoirement a appliquer")]
    [SerializeField]
    private List<BuffDebuff> _listBuffToApply;
    
    
    public void Apply(CharacterStat charStat)
    {
        charStat = charStat as EnnemiStat;
        switch (_triggerStat)
        {
            case BaseStats.NombreBuff:
                if (GameManager.Instance.BattleMan.player.ListBuffDebuffGO.Count >= _levelStatToTrigger)
                {
                    charStat.Tension += charStat.ValeurPalier;
                    int ind = Random.Range(0, _listBuffToApply.Count);
                    GameManager.Instance.BattleMan.player.AddDebuff(_listBuffToApply[ind], _listBuffToApply[ind].Decompte, _listBuffToApply[ind].timerApplication);
                }
                break;
            default:
                Debug.LogError("Case not supported in ApplyBuffPassif - " + name);
                break;
        }
    }
}
