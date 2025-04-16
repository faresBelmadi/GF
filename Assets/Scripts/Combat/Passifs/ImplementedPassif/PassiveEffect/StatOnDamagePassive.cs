using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New StatOnDamagePassive passiv", menuName = "PassiveEffect/New StatOnDamagePassive passiv")]
public class StatOnDamagePassive : AbstractPassive, IOnDamagePassive, IStartCombatPassive
{
    [SerializeField]
    private List<StatToModif> _statsToModif;
    [SerializeField]
    private int _numberOfDamageSourceNeeded;

    private int _currentSource;
    public void Apply(CharacterStat charStat)
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
            charStat.ModifStateAll(modifStat);
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
}
