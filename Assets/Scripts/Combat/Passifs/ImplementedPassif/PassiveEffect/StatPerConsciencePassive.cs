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
public class StatPerConsciencePassive : ScriptableObject, IPassiveEffect
{

    [SerializeField]
    private StatModif _mainStat;
    [SerializeField]
    private List<StatToModif> _statToModif;
    private JoueurStat _joueurStat;
    private int modificator = 1;
    private bool _isInit = false;

    private int GetMainStatValue(CharacterStat charStat) => _mainStat switch
    {
        StatModif.ConscienceMax => (charStat as JoueurStat).Conscience,
        _ => 0
    };
    public void InitPassif(JoueurStat stat)
    {
     
        _joueurStat = stat;
        stat.OnConscienceIncrease += Apply;
        stat.OnConscienceDecrease += Apply;
        _isInit = true;
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

    public void Apply(CharacterStat charStat)
    {
        int mainStat = GetMainStatValue(charStat);
        JoueurStat modifStat = CreateInstance<JoueurStat>();
        foreach (var item in _statToModif)
        {
            switch (item.Stat)
            {
                case BaseStats.Resilience:
                    _joueurStat.ResiliencePassif = mainStat * item.Ratio * modificator;
                    break;
                case BaseStats.Clairvoyance:
                    modifStat.Clairvoyance += mainStat * item.Ratio * modificator;
                    break;
                case BaseStats.Conviction:
                    modifStat.Conviction += mainStat * item.Ratio * modificator;
                    break;
            }
        }
        charStat.ModifStateAll(modifStat);
    }
}
