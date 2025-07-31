using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


[CreateAssetMenu(fileName = "New StatOnDamagePassive passiv", menuName = "PassiveEffect/New StatOnDamagePassive passiv")]
public class StatOnDamagePassive : AbstractPassive, IEffectOnStat<StatsHandler>, IOnDamagePassive, IStartCombatPassive, IAddStackPassive
{
    [Space]
    [Header("StatOnDamagePassive")]
    [SerializeField]
    private List<StatToModif> _statsToModif;
    [SerializeField]
    private int _numberOfDamageSourceNeeded;

    private int _currentSource;
    public void Apply(StatsHandler charStat)
    {
        _currentSource++;

        if (_currentSource >= _numberOfDamageSourceNeeded)
        {
            CharacterStat modifStat = CreateInstance<CharacterStat>();
            foreach (var statToModif in _statsToModif)
            {
                ModifStat(statToModif, modifStat);
            }
            _currentSource = 0;
            charStat.UpdateStat(modifStat);
        }


    }

    private void ModifStat(StatToModif statToModif, CharacterStat stat)
    {
        switch (statToModif.Stat)
        {
            case BaseStats.Resilience:
                stat.Resilience += statToModif.Ratio;
                break;
            case BaseStats.Conviction:
                stat.Conviction += statToModif.Ratio;
                break;
        }

    }

    public void ApplyEffectOnStartCombat()
    {
        _currentSource = 0;
    }

    public void AddStack(StatsHandler stat,int numberOfStackToAdd)
    {
        CharacterStat modifStat = CreateInstance<CharacterStat>();
        foreach (var statToModif in _statsToModif)
        {
            ModifStat(statToModif, modifStat);
        }
        stat.UpdateStat(modifStat);
    }

    public JoueurStat GetStackModifStat(StatsHandler stat, int numberOfStackToAdd)
    {
        JoueurStat modifStat = CreateInstance<JoueurStat>();
        foreach (var statToModif in _statsToModif)
        {
            ModifStat(statToModif, modifStat);
        }
        return modifStat;
    }
}
