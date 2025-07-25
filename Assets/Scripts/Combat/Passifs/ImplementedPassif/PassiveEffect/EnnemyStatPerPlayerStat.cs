using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum ConditionComparaison
{
    ComparePercentRadiance
}

[CreateAssetMenu(fileName = "New EnnemyStat per JoueurStat passiv", menuName = "PassiveEffect/New EnnemyStatPerPlayerStat passiv")]
public class EnnemyStatPerPlayerStat : AbstractPassive, IUpdateStatPassive
{
    [Serializable]
    public struct ConditionStat
    {
        public BaseStats StatToModif;
        public int IfGreaterBonus;
        public int IfEqualsBonus;
        public int IfLesserBonus;
    }

    [Space]
    [Header("EnnemyStatPerPlayerStat")]
    [SerializeField]
    private ConditionComparaison _conditionComparaison;
    [SerializeField]
    private ConditionStat _statToModif;
    private EnemyStatsHandler _ennemiStat;

    public void InitPassif(StatsHandler stat)
    {
        _ennemiStat = stat as EnemyStatsHandler;
        _ennemiStat.OnRadianceChange += UpdateStat;
        GameManager.Instance.playerStat.OnRadianceChange += UpdateStat;
    }
    public void Clear()
    {
        _ennemiStat.OnRadianceChange -= UpdateStat;
        GameManager.Instance.playerStat.OnRadianceChange -= UpdateStat;
    }

    public void Apply(StatsHandler charStat)
    {
        //Nothing to do
    }

    private void ChangeStat(int modificator)
    {
        switch (_statToModif.StatToModif)
        {
            case BaseStats.ForceAme:
                int newValue = Mathf.FloorToInt(((modificator / 100f) * _ennemiStat.ForceDameWithoutBonus));
                _ennemiStat.SetForceDameBonus(newValue);
                break;
        }
    }
    public void UpdateStat()
    {
        switch(_conditionComparaison)
        {
            case ConditionComparaison.ComparePercentRadiance:
                int modificator = 0;
                float enemyPercent = ((float)_ennemiStat.Radiance / (float)_ennemiStat.RadianceMax) * 100f;
                float playerPercent = ((float)GameManager.Instance.playerStat.Radiance / (float)GameManager.Instance.playerStat.RadianceMax) * 100f;
                if (enemyPercent > playerPercent)
                    modificator = _statToModif.IfGreaterBonus;
                if (enemyPercent < playerPercent)
                    modificator = _statToModif.IfLesserBonus;
                if (enemyPercent == playerPercent)
                    modificator = _statToModif.IfEqualsBonus;
                ChangeStat(modificator);
                break;
        }
    }
}
