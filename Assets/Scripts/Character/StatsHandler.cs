using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI.Extensions;


public class StatsHandler <T> : AbstractStatsHandler where T : CharacterStat
{
    #region BASE STATS
    protected T _baseStat;
    public T BaseStat { get { return _baseStat; } }
    public int BaseRadianceMax => _baseStat.RadianceMax;
    public int BaseForceDame => _baseStat.ForceAmeOriginal;
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
