using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ApplyBuff passiv", menuName = "PassiveEffect/New ApplyBuff passiv")]
public class ApplyBuffPassif : AbstractPassive, IStartTurnPassive, IUpdateEnnemyBehaviorPassive
{
    [Space]
    [Header("ApplyBuffPassif")]
    [SerializeField]
    private BaseStats _triggerStat;
    [SerializeField]
    private int _levelStatToTrigger;
    [SerializeField]
    private bool _gainTension;
    [Tooltip("Choisis un des buff al�atoirement a appliquer")]
    [SerializeField]
    private List<BuffDebuff> _listBuffToApply;
    EnnemyBehavior _behavior;

    public void Apply(CharacterStat charStat)
    {
        charStat = charStat as EnnemiStat;
        switch (_triggerStat)
        {
            case BaseStats.None:
                break;
            case BaseStats.NombreBuff:
                if (GameManager.Instance.BattleMan.player.ListBuffDebuffGO.Count >= _levelStatToTrigger)
                {
                    if (_gainTension)
                    {
                        charStat.Tension += charStat.ValeurPalier;
                    }
                    ApplyBuff();
                }
                break;
            case BaseStats.PalierTension:
                //This case is handled in real time.
                break;
            default:
                Debug.LogError("Case not supported in ApplyBuffPassif - " + name);
                break;
        }
    }
  

    public void Clear()
    {
        switch (_triggerStat)
        {
            case BaseStats.PalierTension:
                _behavior.OnGainTensionLevel -= ApplyBuff;
                break;
        }
    }

    public void InitPassif(EnnemyBehavior behavior)
    {
        _behavior = behavior;
        switch (_triggerStat)
        {
            case BaseStats.PalierTension:
                _behavior.OnGainTensionLevel += ApplyBuff;
                break;
        }
    }

    public void UpdateStat()
    {
      
    }

    private void ApplyBuff()
    {
        int ind = Random.Range(0, _listBuffToApply.Count);
        GameManager.Instance.BattleMan.GiveBuffDebuff(new List<BuffDebuff>{ _listBuffToApply[ind]}, _behavior.combatID);
    }
}
