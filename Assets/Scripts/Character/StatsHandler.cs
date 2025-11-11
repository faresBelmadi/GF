using System.Collections.Generic;
using UnityEngine;


public class StatsHandler <T> : AbstractStatsHandler where T : CharacterStat
{
    #region BASE STATS
    protected T _baseStat;
    public T BaseStat { get { return _baseStat; } }
    public int BaseRadianceMax => _baseStat.RadianceMax;
    public int BaseForceDame => _baseStat._forceAme;
    public int BaseVitesse => _baseStat.Vitesse;
    public int BaseConviction => _baseStat.Conviction;
    public int BaseCalme => _baseStat.Calme;
    public int BaseResilience => _baseStat.Resilience;
    #endregion

    public override int RadianceMaxTotal     { get => base.RadianceMaxModifier + BaseRadianceMax; }
    public override int ForceDameTotal       { get => base.ForceDameModifier + BaseForceDame; }
    public override int VitesseTotal         { get => base.VitesseModifier + BaseVitesse; }
    public override int ConvictionTotal
    {
        get
        {
            if ((base.ConvictionModifier + BaseConviction) >= maxConviction)
                return maxConviction;
            else if ((base.ConvictionModifier + BaseConviction) <= minConviction)
                return minConviction;
            return base.ConvictionModifier + BaseConviction;
        }
    }
        
    public override int CalmeTotal           { get => base.CalmeModifier + BaseCalme; }
    public override int ResilienceTotal
    {
        get
        {
            if ((base.ResilienceModifier + BaseResilience) >= maxResilience)
                return maxResilience;
            else if ((base.ResilienceModifier + BaseResilience) <= minResilience)
                return minResilience;
            return base.ResilienceModifier + BaseResilience;
        }
    }
    public StatsHandler(StatsHandler<T> statsHandler) : base(statsHandler)
    {
        ListBuffDebuff = new List<BuffDebuff>(statsHandler.ListBuffDebuff);
        _baseStat = ScriptableObject.Instantiate<T>(statsHandler.BaseStat);
    }
    public StatsHandler(T charStat) : base(charStat)
    {
        _baseStat = ScriptableObject.Instantiate<T>(charStat);
    }
    protected virtual void UpdateStatFromUpgrade()
    {
        //RadianceMaxModifier = BaseRadianceMax;
       // ForceAme = BaseForceDame;
        //Vitesse = BaseVitesse;

    }
    protected virtual int GetPercentValue(StatModif statToGet, float percentValue) => statToGet switch
    {
        StatModif.RadianceMax   => GetPercentValue(StatEnum.RadianceMax, percentValue),
        StatModif.ForceAme      => GetPercentValue(StatEnum.ForceDame, percentValue),
        StatModif.Vitesse       => GetPercentValue(StatEnum.Vitesse, percentValue),
        StatModif.Conviction    => GetPercentValue(StatEnum.Conviction, percentValue),
        StatModif.Calme         => GetPercentValue(StatEnum.Calme, percentValue),
        StatModif.Resilience    => GetPercentValue(StatEnum.Resilience, percentValue),
        _                       => 0,
    };
    public virtual int GetPercentValue(StatEnum statToGet, float percentValue) => statToGet switch
    {
        StatEnum.RadianceMax    => Mathf.RoundToInt((BaseRadianceMax * percentValue) / 100f),
        StatEnum.ForceDame      => Mathf.RoundToInt((BaseForceDame * percentValue) / 100f),
        StatEnum.Vitesse        => Mathf.RoundToInt((BaseVitesse * percentValue) / 100f),
        StatEnum.Conviction     => Mathf.RoundToInt((BaseConviction * percentValue) / 100f),
        StatEnum.Calme          => Mathf.RoundToInt((BaseCalme * percentValue) / 100f),
        StatEnum.Resilience     => Mathf.RoundToInt((BaseResilience * percentValue) / 100f),
        _                       => 0,
    };




    public virtual void UpdateBaseStat(T modifStat)
    {
        Debug.Log($"Base Stat Modification for character {_baseStat.name}");
        //Base Stat Modification
        _baseStat.ModifStateAll(modifStat);
        //Current Stat modification;
        UpdateStat(modifStat);
    }
    
    public virtual void ResetStat()
    {
        RadianceMaxModifier = BaseRadianceMax;
        Radiance = BaseRadianceMax;
        ForceDameModifier = 0;
        VitesseModifier = 0;
        ConvictionModifier = BaseConviction;
        ResilienceModifier = BaseResilience;
        CalmeModifier = BaseCalme;
        MultiplDef = 1;
        MultiplSoin = 1;
        MultiplDegat = 1;
        MultiplBuffDebuff = 1;
        Tension = 0;
        TensionMax = 0;
        ValeurPalier = 0;
        PalierChangement = 0;
        IsStun = false;

        // TODO
        //this.nbAttaqueRecu = 0;
        ListBuffDebuff.Clear();
    }


}
