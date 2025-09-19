using System.Collections.Generic;
using UnityEngine;


public class StatsHandler <T> : AbstractStatsHandler where T : CharacterStat
{
    #region BASE STATS
    protected T _baseStat;
    public T BaseStat { get { return _baseStat; } }
    public int BaseRadianceMax => _baseStat.RadianceMax;
    public int BaseForceDame => _baseStat._forceAme;
    public int BaseVitesse => _baseStat.VitesseOriginal;
    public int BaseConviction => _baseStat.ConvictionOriginal;
    public int BaseCalme => _baseStat.Calme;
    public int BaseResilience => _baseStat.ResilienceOriginal;
    #endregion

   

    public StatsHandler(StatsHandler<T> statsHandler) : base(statsHandler)
    {
        ListBuffDebuff = new List<BuffDebuff>(statsHandler.ListBuffDebuff);
        _baseStat = ScriptableObject.Instantiate<T>(statsHandler.BaseStat);
    }
    public StatsHandler(T charStat) : base(charStat)
    {
        _baseStat = charStat;

    }
    protected virtual void UpdateStatFromUpgrade()
    {
        RadianceMax = BaseRadianceMax;
        ForceAme = BaseForceDame;
        Vitesse = BaseVitesse;

    }
    protected virtual float GetPercentValue(StatModif statToGet, float percentValue) => statToGet switch
    {
        StatModif.ForceAme => GetPercentValue(StatEnum.ForceDame, percentValue),
        StatModif.RadianceMax => GetPercentValue(StatEnum.RadianceMax, percentValue),
        _ => 0,
    };
    public virtual float GetPercentValue(StatEnum statToGet, float percentValue) => statToGet switch
    {
        StatEnum.ForceDame => (_baseStat.ForceAme * percentValue) / 100f,
        StatEnum.Radiance => (_baseStat.Radiance * percentValue) / 100f,
        StatEnum.RadianceMax => (_baseStat.RadianceMax * percentValue) / 100f,
        _ => 0,
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
        RadianceMax = BaseRadianceMax;
        Radiance = BaseRadianceMax;
        ForceAme = BaseForceDame;
        Vitesse = BaseVitesse;
        Conviction = BaseConviction;
        Resilience = BaseResilience;
        Calme = BaseCalme;
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
