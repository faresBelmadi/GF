using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class BuffByRangeStat
{
    [SerializeField]
    [Range(0f, 100f)]
    public int MinExclusive;
    [Range(0f, 100f)]
    public int MaxInclusive;
    [SerializeField]
    public string IdTradDesc;
    [SerializeField]
    public BuffDebuff BuffToApply;
}
[CreateAssetMenu(fileName = "New ApplyBuffOnStatRange passiv", menuName = "PassiveEffect/New ApplyBuffOnStatRange passiv")]
public class ApplyBuffOnStatRange : AbstractPassive, IUpdateEnemyStatPassive, IStartCombatPassive
{
    [Space]
    [Header("ApplyBuffOnStatRange")]
    [SerializeField]
    private BaseStats _triggerStat;
    [SerializeField]
    private List<BuffByRangeStat> _buffs;
    EnemyStatsHandler _stat;

    private BuffByRangeStat _currentBuff;
    public override string IdTradDesc
    {
        get
        {
            return GetDebuffToApply().IdTradDesc;
        }
    }

    public void Apply(StatsHandler<CharacterStat> charStat)
    {
       
    }

    public void Clear()
    {
        switch (_triggerStat)
        {
            case BaseStats.Radiance:
                _stat.OnRadianceChange -= ApplyBuff;
                break;
        }
    }

    public void InitPassif(EnemyStatsHandler stat)
    {
        _stat = stat;
        switch (_triggerStat)
        {
            case BaseStats.Radiance:
                stat.OnRadianceChange += ApplyBuff;
                break;
        }
    }

    private BuffByRangeStat GetDebuffToApply()
    {
        float radPercent = (_stat.Radiance * 100f) / _stat.RadianceMax;
        foreach (var item in _buffs)
        {
            if (item.MaxInclusive >= radPercent && item.MinExclusive < radPercent)
                return item;
        }
        return null;
    }

   
    public void ApplyBuff()
    {
       
        if (!GameManager.Instance.BattleMan.IsCombatOn)
            return;
        BuffByRangeStat buffToApply = GetDebuffToApply();
        if (buffToApply == null) return;

        if (_currentBuff != buffToApply)
        {
            _currentBuff = buffToApply;
        }
        else
            return;
        
        foreach (EnnemyBehavior ennemy in GameManager.Instance.BattleMan.EnemyScripts)
        {
            //Retirer Buff, ajouter buff
            ennemy.RemoveBuffByIdTradName(buffToApply.BuffToApply.idTradName);
            ennemy.RefreshPassiveDescription();
        }
        GameManager.Instance.BattleMan.GiveBuffDebuff(new List<BuffDebuff> { buffToApply.BuffToApply });
       
    }
    public void UpdateStat()
    {
        throw new System.NotImplementedException();
    }

    public void ApplyEffectOnStartCombat()
    {
        ApplyBuff();
    }
}
