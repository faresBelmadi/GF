using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

[Serializable]
public class PlayerStatsHandler : StatsHandler<JoueurStat>
{
    private class PercentIncrease
    {
        public StatEnum StatEnum;
        public float value;
    }
    #region BASE STATS
    public int BaseConscienceMax => _baseStat.ConscienceMax;
    public int BaseClairvoyance => _baseStat.Clairvoyance;
    #endregion

    #region STATS PROPERTY
    [field: SerializeField, ReadOnly] public int Lvl { get; private set; }
    [field: SerializeField, ReadOnly] public int Volonte { get; set; }
    [field: SerializeField, ReadOnly] public int VolonteMax { get; set; }
    [field: SerializeField, ReadOnly] public int Conscience { get; set; }
    protected int _conscienceMaxModifier;
    public int ConscienceMaxModifier
    {
        get => _conscienceMaxModifier;
        protected set => _conscienceMaxModifier = value;
    }
    public int ConscienceMaxTotal { get => ConscienceMaxModifier + BaseConscienceMax; }
    protected int _clairvoyanceModifier;
    public int ClairvoyanceModifier
    {
        get => _clairvoyanceModifier;
        protected set => _clairvoyanceModifier = value;
    }
    public int ClairvoyanceTotal { get => ClairvoyanceModifier + BaseClairvoyance; }
    [field: SerializeField, ReadOnly] public List<Spell> ListSpell { get; set; } = new List<Spell>();
    [field: SerializeField, ReadOnly] public int SlotsSouvenir { get; set; }
    [field: SerializeField, ReadOnly] public List<Souvenir> ListSouvenir { get; set; } = new List<Souvenir>();
    #endregion

    private List<PercentIncrease> _percentIncreaseList = new List<PercentIncrease>();
    #region EVENTS
    public event Action OnConscienceIncrease;
    public event Action OnConscienceDecrease;
    #endregion
    public PlayerStatsHandler(PlayerStatsHandler playerStat) : base(playerStat)
    {
        Lvl = playerStat.Lvl;
        Volonte = playerStat.Volonte;
        VolonteMax = playerStat.VolonteMax;
        Conscience = playerStat.Conscience;
       // ConscienceMaxTotal = playerStat.ConscienceMaxModifier;
        ClairvoyanceModifier = playerStat.ClairvoyanceModifier;

        ListSpell = new List<Spell>(playerStat.ListSpell);
        SlotsSouvenir = playerStat.SlotsSouvenir;
        ListSouvenir = new List<Souvenir>(playerStat.ListSouvenir);
        _baseStat = ScriptableObject.Instantiate<JoueurStat>(playerStat.BaseStat);
    }
    public PlayerStatsHandler(JoueurStat charStat) : base(charStat)
    {
        Lvl = charStat.Lvl;
        Volonte = charStat.Volonter;
        VolonteMax = charStat.VolonterMax;
        Conscience = charStat.Conscience;
       // ConscienceMaxTotal = charStat.ConscienceMax;
        ClairvoyanceModifier = charStat.Clairvoyance;

        ListSpell = new List<Spell>(charStat.ListSpell);
        SlotsSouvenir = charStat.SlotsSouvenir;
        ListSouvenir = new List<Souvenir>(charStat.ListSouvenir);

        _baseStat = ScriptableObject.Instantiate<JoueurStat>(charStat);
    }

    protected override int GetPercentValue(StatModif statToGet, float percentValue) => statToGet switch
    {
        StatModif.ConscienceMax => GetPercentValue(StatEnum.ConscienceMax, percentValue),
        StatModif.Clairvoyance  => GetPercentValue(StatEnum.Clairvoyance, percentValue),
        _                       => base.GetPercentValue(statToGet, percentValue),
    };
    /// <summary>
    /// Return the fraction of the given stat, round to the nearest integer.
    /// </summary>
    /// <param name="statToGet">Stat to get the fraction</param>
    /// <param name="percentValue">The percebnt value</param>
    /// <returns>The value rounded to the nearest integer</returns>
    public override int GetPercentValue(StatEnum statToGet, float percentValue) => statToGet switch
    {
        StatEnum.ConscienceMax  => Mathf.RoundToInt((BaseConscienceMax * percentValue) / 100f),
        StatEnum.Clairvoyance   => Mathf.RoundToInt((BaseClairvoyance * percentValue) / 100f),
        _                       => base.GetPercentValue(statToGet, percentValue),
    };
    /// <summary>
    /// Update stat from percent increase from equiped souvenir
    /// </summary>
    public void UpdatePercentIncreaseFromSouvenir()
    {
        // Remove all stats to refresh
        foreach (var souv in ListSouvenir)
        {
            foreach (var modif in souv.ModificationStat)
            {
                if (modif.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                {
                   
                    switch (modif.StatModif)
                    {
                        case StatModif.RadianceMax:
                            RadianceMaxModifier -= modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.ForceAme:
                            ForceDameModifier -= modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Vitesse:
                            VitesseModifier -= modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Clairvoyance:
                            ClairvoyanceModifier -= modif.ParametreModifStat.ValeurModifier;
                            break;
                    }
                }
            }
        }

        JoueurStat joueurStat = ScriptableObject.CreateInstance<JoueurStat>();
        // Settings new values
        foreach (var souv in ListSouvenir )
        {
            if (!souv.Equiped)
                continue;
            foreach (var modif in souv.ModificationStat)
            {
                if (modif.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                {
                    modif.ParametreModifStat.ValeurModifier = Mathf.FloorToInt(GetPercentValue(modif.StatModif, modif.ParametreModifStat.Valeur));
                    switch (modif.StatModif)
                    {
                        case StatModif.RadianceMax:
                            joueurStat.RadianceMax = modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.ForceAme:
                            joueurStat.ForceAme = modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Vitesse:
                            joueurStat.Vitesse = modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Clairvoyance:
                            joueurStat.Clairvoyance = modif.ParametreModifStat.ValeurModifier;
                            break;
                    }
                }
                
            }
        }
        UpdateStat(joueurStat);
        ScriptableObject.DestroyImmediate(joueurStat);
    }
    public override void UpdateBaseStat(JoueurStat modifStat)
    {
        Debug.Log($"Player Base Stat Modification for player {_baseStat.name}");
        //Base Stat Modification
        _baseStat.ModifStateAll(modifStat);
        //Current Stat modification;
        //UpdateStat(modifStat);
        UpdateStatFromUpgrade();
    }
 
    public void UpdateStat(JoueurStat charStatModifier)
    {
        base.UpdateStat(charStatModifier);

        Volonte += charStatModifier.Volonter;
        VolonteMax += charStatModifier.VolonterMax;

        Conscience += charStatModifier.Conscience;
        ConscienceMaxModifier += charStatModifier.ConscienceMax;
        ClairvoyanceModifier += charStatModifier.Clairvoyance;


       
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
        if (Conscience > ConscienceMaxModifier)
        {
            Conscience = ConscienceMaxModifier;
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
        ConscienceMaxModifier = 0;
        ClairvoyanceModifier = 0;
        VolonteMax = 0;

        base.SetZero();
    }
    public override void ResetStat()
    {
        ClairvoyanceModifier = BaseClairvoyance;
        base.ResetStat();
    }
}