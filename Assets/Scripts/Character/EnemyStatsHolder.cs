using System;

public class EnemyStatsHolder : StatsHolder
{
    #region BASE STATS
    private readonly EnnemiStat _baseStat;

    public int BaseDissimulation => _baseStat.DissimulationOriginal;
    #endregion

    #region STATS PROPERTY
    public int Dissimulation { get; private set; }
    public bool NoTension { get; private set; }
    //Uniquement pour Jeanne
    private int _customStat;
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
    public EnemyStatsHolder(EnnemiStat charStat) : base(charStat)
    {
        Dissimulation = charStat.Dissimulation;
        _customStat = charStat.CustomStat;

        _baseStat = charStat;
    }

}
