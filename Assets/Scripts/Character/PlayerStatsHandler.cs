using System;
using System.Collections.Generic;
using System.Text;
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
    [field: SerializeField, ReadOnly] public int Conscience { get; private set; }
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
    #endregion

    #region OBJECTSLIST
    [field: SerializeField, ReadOnly] public List<Spell> ListSpell { get; set; } = new List<Spell>();
    [field: SerializeField, ReadOnly] public int SlotsSouvenir { get; set; }
    [field: SerializeField, ReadOnly] public List<Souvenir> ListSouvenir { get; set; } = new List<Souvenir>();
    
    private List<PercentIncrease> _percentIncreaseList = new List<PercentIncrease>();
    #endregion

    #region EVENTS
    public event Action OnConscienceIncrease;
    public event Action OnConscienceDecrease;
    #endregion

    #region CONSTRUCTORS
    public PlayerStatsHandler(PlayerStatsHandler playerStat) : base(playerStat)
    {
        Lvl = playerStat.Lvl;
        Volonte = playerStat.Volonte;
        VolonteMax = playerStat.VolonteMax;
        Conscience = playerStat.Conscience;
       // ConscienceMaxTotal = playerStat.ConscienceMaxModifier;
        //ClairvoyanceModifier = playerStat.ClairvoyanceModifier;

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
       //ClairvoyanceModifier = charStat.Clairvoyance;

        ListSpell = new List<Spell>(charStat.ListSpell);
        SlotsSouvenir = charStat.SlotsSouvenir;
        ListSouvenir = new List<Souvenir>(charStat.ListSouvenir);

        _baseStat = ScriptableObject.Instantiate<JoueurStat>(charStat);
    }
    #endregion
    /// <summary>
    /// Return the fraction of the given stat, round to the nearest integer.
    /// </summary>
    /// <param name="statToGet">Stat to get the fraction</param>
    /// <param name="percentValue">The percebnt value</param>
    /// <returns>The value rounded to the nearest integer</returns>
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
    public void UpdatePercentIncreaseFromSouvenir() => UpdatePercentIncreaseFromSouvenir(ListSouvenir);

    /// <summary>
    /// Update stat from percent increase from equiped souvenir
    /// </summary>
    /// <param name="listSouv">the souvenir list to use</param>
    public void UpdatePercentIncreaseFromSouvenir(List<Souvenir> listSouv)
    {
        // Remove all stats to refresh
        foreach (var souv in listSouv)
        {
            foreach (var modif in souv.ModificationStat)
            {
                if (modif.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                {
                   
                    switch (modif.StatModif)
                    {
                        case StatModif.Radiance:
                            Radiance -= modif.ParametreModifStat.ValeurModifier;
                            modif.ParametreModifStat.ValeurModifier = 0;
                            break;
                        case StatModif.RadianceMax:
                            RadianceMaxModifier -= modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.ForceAme:
                            ForceDameModifier -= modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Vitesse:
                            VitesseModifier -= modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Resilience:
                            ResilienceModifier -= modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Clairvoyance:
                            ClairvoyanceModifier -= modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Calme:
                            CalmeModifier -= modif.ParametreModifStat.ValeurModifier;
                            break;
                    }
                }
            }
        }

        JoueurStat joueurStat = ScriptableObject.CreateInstance<JoueurStat>();
        // Settings new values
        foreach (var souv in listSouv)
        {
            int radianceModifier = 0;
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
                            radianceModifier += Mathf.FloorToInt(GetPercentValue(StatEnum.Radiance, modif.ParametreModifStat.Valeur));
                            joueurStat.RadianceMax = modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.ForceAme:
                            joueurStat.ForceAme += modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Vitesse:
                            joueurStat.Vitesse += modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Resilience:
                            joueurStat.Resilience += modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Clairvoyance:
                            joueurStat.Clairvoyance += modif.ParametreModifStat.ValeurModifier;
                            break;
                        case StatModif.Calme:
                            joueurStat.Calme += modif.ParametreModifStat.ValeurModifier;
                            break;
                    }
                }
            }
            if (radianceModifier != 0)
            {
                ModificationStatSouvenir modificationStatSouvenir = new();
                modificationStatSouvenir.StatModif = StatModif.Radiance;
                ParametreModifStat parametreModifStat = new();
                parametreModifStat.ParametreStat = ParametreStat.ValeurBrut;
                parametreModifStat.ValeurModifier = radianceModifier;
                modificationStatSouvenir.ParametreModifStat = parametreModifStat;
                souv.ModificationStat.Add(modificationStatSouvenir);

                joueurStat.Radiance += radianceModifier;
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
        if (Conscience > ConscienceMaxTotal)
        {
            Conscience = ConscienceMaxTotal ;
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
        ClairvoyanceModifier = 0;
        base.ResetStat();
    }
    public void SetConscience(int valueToSet)
    {
        Conscience = valueToSet;
        RectificationStat();
    }
    public void GainConscience(int valueToGain)
    {
        Conscience += valueToGain;
        if (Conscience > ConscienceMaxTotal)
        {
            Conscience = ConscienceMaxTotal;
        }
    }
    public void LoseConscience(int valueToLose)
    {
        Conscience -= valueToLose;
        if (Conscience < 0)
            Conscience = 0;
    }
    public string PrintBaseStat()
    {
        StringBuilder strb = new();
        strb.AppendLine("-------PLAYER STAT-------");
        strb.AppendLine();
        strb.AppendLine($"BASE STATS");
        strb.AppendLine($"BASE FORCE D'AME : {BaseForceDame}");
        strb.AppendLine($"BASE FORCE D'AME : {BaseRadianceMax}");
        strb.AppendLine($"BASE FORCE D'AME : {BaseVitesse}");
        strb.AppendLine($"BASE FORCE D'AME : {BaseConscienceMax}");
        strb.AppendLine($"BASE FORCE D'AME : {BaseConviction}");
        strb.AppendLine($"BASE FORCE D'AME : {BaseClairvoyance}");
        strb.AppendLine($"BASE FORCE D'AME : {BaseRadianceMax}");
        strb.AppendLine($"BASE FORCE D'AME : {BaseCalme}");
        strb.AppendLine("------END BASE STAT------");
        return strb.ToString();
    }
    public override string ToString()
    {
        StringBuilder strb = new();
        strb.AppendLine("*************************");
        strb.AppendLine("-------PLAYER STAT-------");
        strb.AppendLine();
        strb.AppendLine("Total (base + modifier)");
        strb.AppendLine($"FORCE D'AME       : {ForceDameTotal} ({BaseForceDame} + {_forceDameModifier})");
        strb.AppendLine($"RADIANCE MAX      : {RadianceMaxTotal} ({BaseRadianceMax} + {_radianceMaxModifier})");
        strb.AppendLine($"VITESSE           : {VitesseTotal} ({BaseVitesse} + {_vitesseModifier})");
        strb.AppendLine($"CONSCIENCE MAX    : {ConscienceMaxTotal} ({BaseConscienceMax} + {_conscienceMaxModifier})");
        strb.AppendLine($"CONVICTION        : {ConvictionTotal} ({BaseConviction} + {_convictionModifier})");
        strb.AppendLine($"CLAIRVOYANCE      : {ClairvoyanceTotal} ({BaseClairvoyance} + {_clairvoyanceModifier})");
        strb.AppendLine($"RADIANCE MAX      : {RadianceMaxTotal} ({BaseRadianceMax} + {_radianceMaxModifier})");
        strb.AppendLine($"CALME             : {CalmeTotal} ({BaseCalme} + {_calmeModifier}) ");
        strb.AppendLine("--------END  STAT--------");

        strb.AppendLine();

        return strb.ToString();
    }
}