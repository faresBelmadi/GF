using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

[Serializable]
public class EnemyStatsHandler : StatsHandler
{
    #region BASE STATS
    private EnnemiStat _baseStat;
    public EnnemiStat BaseEnemyStat { get { return _baseStat; } }
    public int BaseDissimulation => _baseStat.DissimulationOriginal;
    #endregion

    #region STATS PROPERTY
    [field: SerializeField, ReadOnly] public int Dissimulation { get; private set; }
    public bool NoTension { get; private set; }

    //Uniquement pour Jeanne
    [field: SerializeField, ReadOnly] private int _customStat;
    public int CustomStat
    {
        get => _customStat;
        set
        {
            if (value != _customStat)
            {
                _customStat = value;
                OnCustomStatModification?.Invoke();
            }
        }
    }
    #endregion
    #region EVENTS
    public event Action OnCustomStatModification;
    #endregion
    public EnemyStatsHandler(EnnemiStat charStat) : base(charStat)
    {
        Dissimulation = charStat.Dissimulation;
        _customStat = charStat.CustomStat;

        _baseStat = charStat;
    }
    public override void ResetStat()
    {
        Dissimulation = BaseDissimulation;
        base.ResetStat();
    }

}
