using System;
using System.Collections;
using System.Collections.Generic;
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
    public BuffDebuff BuffToApply;
}
[CreateAssetMenu(fileName = "New ApplyBuffOnStatRange passiv", menuName = "PassiveEffect/New ApplyBuffOnStatRange passiv")]
public class ApplyBuffOnStatRange : AbstractPassive, IUpdateStatPassive, IStartCombatPassive
{
    [Space]
    [Header("ApplyBuffOnStatRange")]
    [SerializeField]
    private BaseStats _triggerStat;
    [SerializeField]
    private List<BuffByRangeStat> _buffs;
    CharacterStat _stat;

    public void Apply(CharacterStat charStat)
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

    public void InitPassif(CharacterStat stat)
    {
        _stat = stat;
        switch (_triggerStat)
        {
            case BaseStats.Radiance:
                stat.OnRadianceChange += ApplyBuff;
                break;
        }
    }

    private BuffDebuff GetDebuffToApply()
    {
        float radPercent = (_stat.Radiance * 100f) / _stat.RadianceMax;
        foreach (var item in _buffs)
        {
            if (item.MaxInclusive >= radPercent && item.MinExclusive < radPercent)
                return item.BuffToApply;
        }
        return null;
    }

   
    public void ApplyBuff()
    {
        if (!GameManager.Instance.BattleMan.IsCombatOn)
            return;
        BuffDebuff buffToApply = GetDebuffToApply();
        if (buffToApply == null) return;
        foreach (EnnemyBehavior ennemy in GameManager.Instance.BattleMan.EnemyScripts)
        {
            //Retirer Buff, ajouter buff
            ennemy.RemoveBuffByIdTradName(buffToApply.idTradName);
        }
        GameManager.Instance.BattleMan.GiveBuffDebuff(new List<BuffDebuff> { buffToApply });
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
