using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum ConditionComparaison
{
    CompareRadiance
}

[CreateAssetMenu(fileName = "New EnnemyStat per JoueurStat passiv", menuName = "PassiveEffect/New EnnemyStatPerPlayerStat passiv")]
public class EnnemyStatPerPlayerStat : ScriptableObject, IUpdateStatPassive
{
    [Serializable]
    public struct ConditionStat
    {
        public BaseStats StatToModif;
        public int IfGreaterBonus;
        public int IfEqualsBonus;
        public int IfLesserBonus;
    }

    [SerializeField]
    private ConditionComparaison _conditionComparaison;
    [SerializeField]
    private ConditionStat _statToModif;
    private EnnemiStat _ennemiStat;

    public void InitPassif(CharacterStat stat)
    {
        _ennemiStat = stat as EnnemiStat;
        _ennemiStat.OnRadianceChange += UpdateStat;
        GameManager.Instance.playerStat.OnRadianceChange += UpdateStat;
    }
    public void Clear()
    {
        _ennemiStat.OnRadianceChange -= UpdateStat;
        GameManager.Instance.playerStat.OnRadianceChange -= UpdateStat;
    }

    public void Apply(CharacterStat charStat)
    {
        //Nothing to do
    }

    private int ChangeStat(int modificator)
    {
        switch (_statToModif.StatToModif)
        {
            case BaseStats.ForceAme:
                _ennemiStat.ForceAmeBonus = Mathf.FloorToInt(((modificator / 100f) * _ennemiStat._forceAme));
                break;
        }
        return 0;
    }
    public void UpdateStat()
    {
        switch(_conditionComparaison)
        {
            case ConditionComparaison.CompareRadiance:
                int modificator = 0;
                if (_ennemiStat.Radiance > GameManager.Instance.playerStat.Radiance)
                    modificator = _statToModif.IfGreaterBonus;
                if (_ennemiStat.Radiance < GameManager.Instance.playerStat.Radiance)
                    modificator = _statToModif.IfLesserBonus;
                if (_ennemiStat.Radiance == GameManager.Instance.playerStat.Radiance)
                    modificator = _statToModif.IfEqualsBonus;
                break;
        }
    }
}
