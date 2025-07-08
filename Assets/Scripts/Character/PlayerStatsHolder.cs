using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStatsHolder : StatsHolder
{
    #region BASE STATS
    protected readonly JoueurStat _baseStat;

    public int BaseConscienceMax => _baseStat.ConscienceMax;
    public int BaseClairvoyance => _baseStat.ClairvoyanceOriginal;
    #endregion

    #region STATS PROPERTY
    public int Lvl { get; private set; }
    public int Volonte { get; private set; }
    public int VolonteMax { get; private set; }
    public int Conscience { get; private set; }
    public int ConscienceMax { get; private set; }
    public int Clairvoyance { get; private set; }
    #endregion

    #region EVENTS
    public event Action OnConscienceIncrease;
    public event Action OnConscienceDecrease;
    #endregion
    public PlayerStatsHolder(JoueurStat charStat) : base(charStat)
    {
        Lvl = charStat.Lvl;
        Volonte = charStat.Volonter;
        VolonteMax = charStat.VolonterMax;
        Conscience = charStat.Conscience;
        ConscienceMax = charStat.ConscienceMax;
        Clairvoyance = charStat.Clairvoyance;
    }

    public void UpdateStat(JoueurStat charStatModifier)
    {
        base.UpdateStat(charStatModifier);

        Volonte += charStatModifier.Volonter;
        VolonteMax += charStatModifier.VolonterMax;

        Conscience += charStatModifier.Conscience;
        ConscienceMax += charStatModifier.ConscienceMax;
        Clairvoyance += charStatModifier.Clairvoyance;


       
        RectificationStat();

        //La conscience a été modif, on notifie
        if (charStatModifier.Conscience > 0 || charStatModifier.ConscienceMax > 0)
        {
            OnConscienceIncrease?.Invoke();
        }
        else if (charStatModifier.Conscience < 0 || charStatModifier.ConscienceMax < 0)
        {
            OnConscienceDecrease?.Invoke();
        }
    }

    public override void RectificationStat()
    {
        if (Volonte > VolonteMax)
        {
            Volonte = VolonteMax;
        }
        if (Conscience > ConscienceMax)
        {
            Conscience = ConscienceMax;
        }
        if (Conscience < 0)
        {
            Conscience = 0;
        }
        base.RectificationStat();
    }
    public override void SetZero()
    {
        Volonte = 0;
        Conscience = 0;
        ConscienceMax = 0;
        Clairvoyance = 0;
        VolonteMax = 0;

        base.SetZero();
    }
}