using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ApplyBuff passiv", menuName = "PassiveEffect/New ApplyBuff passiv")]
public class ApplyBuffPassif : AbstractPassive, IEffectOnStat<StatsHandler<CharacterStat>>, IStartTurnPassive<StatsHandler<CharacterStat>>, IDynamicEventPassive<EnnemyBehavior>
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

    public void Apply(StatsHandler<CharacterStat> charStat)
    {
        //charStat = charStat as EnemyStatsHandler;
        switch (_triggerStat)
        {
            case BaseStats.None:
                break;
            case BaseStats.NombreBuff:
                if (GameManager.Instance.BattleMan.player.ListBuffDebuffGO.Count >= _levelStatToTrigger)
                {
                    if (_gainTension)
                    {
                        charStat.ChangeTension(charStat.ValeurPalier);
                        
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

    

    public void UpdateStat()
    {
      
    }

    private void ApplyBuff()
    {
        int ind = Random.Range(0, _listBuffToApply.Count);
        GameManager.Instance.BattleMan.GiveBuffDebuff(new List<BuffDebuff>{ _listBuffToApply[ind]}, _behavior.combatID);
    }

    public void ApplyOnTurnStart(StatsHandler<CharacterStat> stat)
    {
        Apply(stat);
    }

    public void SubscribeEvents(EnnemyBehavior reference)
    {
        _behavior = reference;
        switch (_triggerStat)
        {
            case BaseStats.PalierTension:
                _behavior.OnGainTensionLevel += ApplyBuff;
                break;
        }
    }

    public void UnsubscribeEvents()
    {
        throw new System.NotImplementedException();
    }
}
