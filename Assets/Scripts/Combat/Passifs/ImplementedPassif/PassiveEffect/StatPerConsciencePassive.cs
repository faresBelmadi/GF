using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatToModif
{
    public BaseStats Stat;
    public int Ratio;
}
[CreateAssetMenu(fileName = "New Stat By Stat effect", menuName = "PassiveEffect/New Effect Stat By Stat")]
public class StatPerConsciencePassive : AbstractPassive, IEffectOnStat<PlayerStatsHandler>, IPassiveEffect
{
    [Space]
    [Header("StatPerConsciencePassive")]
    [SerializeField]
    private StatModif _mainStat;
    [SerializeField]
    private List<StatToModif> _statToModif;
    private PlayerStatsHandler _joueurStat;
    private int modificator = 1;

    private int GetMainStatValue(PlayerStatsHandler charStat) => _mainStat switch
    {
        StatModif.ConscienceMax => charStat.Conscience,
        _ => 0
    };
    public void InitPassif(PlayerStatsHandler stat)
    {
     
        _joueurStat = stat;
        stat.OnConscienceIncrease += Apply;
        stat.OnConscienceDecrease += Apply;
        Apply(stat);
    }
    private void OnDestroy()
    {
        _joueurStat.OnConscienceIncrease -= Apply;
        _joueurStat.OnConscienceDecrease -= Apply;
    }
    private void Apply()
    {
        Apply(_joueurStat);
    }

    public void Apply(PlayerStatsHandler charStat)
    {
        int mainStat = GetMainStatValue(charStat);
        JoueurStat modifStat = CreateInstance<JoueurStat>();
        foreach (var item in _statToModif)
        {
            switch (item.Stat)
            {
                case BaseStats.Resilience:
                    charStat.SetResiliencePassit(mainStat * item.Ratio * modificator);
                    break;
                case BaseStats.Clairvoyance:
                    modifStat.Clairvoyance += mainStat * item.Ratio * modificator;
                    break;
                case BaseStats.Conviction:
                    modifStat.Conviction += mainStat * item.Ratio * modificator;
                    break;
            }
        }
        charStat.UpdateStat(modifStat);
    }
}
