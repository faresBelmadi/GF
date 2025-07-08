using Codice.Client.BaseCommands.Differences;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHolder
{

    #region BASE STATS
    private readonly CharacterStat _baseStat;

    public int BaseRadianceMax => _baseStat.RadianceMax;
    public int BaseForceDame => _baseStat.ForceAmeOriginal;
    public int BaseVitesse => _baseStat.VitesseOriginal;
    public int BaseConviction => _baseStat.ConvictionOriginal;
    public int BaseCalme => _baseStat.Calme;
    public int BaseResilience => _baseStat.ResilienceOriginal;
    #endregion

    #region PROPERTY STATS
    public int     Radiance { get; private set; }
    public int     RadianceMax { get; private set; }
    public int     ForceAme { get; private set; }
    public int     ForceAmeBonus { get; private set; }
    public int     Vitesse { get; private set; }
    public int     Conviction { get; private set; }
    public int     Calme { get; private set; }
    public int     Resilience { get; private set; }
    public int     ResiliencePassif { get; private set; }
    public int     Essence { get; private set; }
    public float   MultiplDef { get; private set; }
    public float   MultiplSoin { get; private set; }
    public float   MultiplDegat { get; private set; }
    public float   MultiplBuffDebuff { get; private set; }
    public float   MultiplTension { get; private set; }
    public float   Tension { get; private set; }
    public float   TensionMax { get; private set; }
    public float   ValeurPalier { get; private set; }
    public int     PalierChangement { get; private set; }
    public bool    IsStun { get; private set; }
    #endregion

    #region EVENTS
    public event Action OnConvictionChanged;
    #endregion

    public StatsHolder(CharacterStat charStat)
    {
        Radiance               = charStat.Radiance;
        RadianceMax            = charStat.RadianceMax;
        ForceAme               = charStat.ForceAmeOriginal;
        ForceAmeBonus          = charStat.ForceAmeBonus;
        Vitesse                = charStat.Vitesse;
        Conviction             = charStat.Conviction;
        Calme                  = charStat.Calme;
        Resilience             = charStat.ResilienceOriginal;
        ResiliencePassif       = (int) charStat.ResiliencePassif;
        Essence                = charStat.Essence;
        MultiplDef             = charStat.MultiplDef;
        MultiplSoin            = charStat.MultiplSoin;
        MultiplDegat           = charStat.MultiplDegat;
        MultiplBuffDebuff      = charStat.MultipleBuffDebuff;
        MultiplTension         = charStat.MultipleTension;
        Tension                = charStat.Tension;
        TensionMax             = charStat.TensionMax;
        ValeurPalier           = charStat.ValeurPalier;
        PalierChangement       = charStat.PalierChangement;
        IsStun                 = charStat.isStun;

        _baseStat = charStat;

    }

    public void UpdateStat(CharacterStat charStatModifier)
    {
        if (charStatModifier.MultiplDef != 1)
        {
            if (charStatModifier.MultiplDef > 1)
            {
                var multiplicateurDef = charStatModifier.MultiplDef % 1;
                MultiplDef += multiplicateurDef;

            }
            else
            {
                var multiplicateurDef = charStatModifier.MultiplDef - 1;
                MultiplDef += multiplicateurDef;
            }
        }
        if (charStatModifier.MultiplSoin != 1)
        {
            if (charStatModifier.MultiplSoin > 1)
            {
                var multiplicateurSoin = charStatModifier.MultiplSoin % 1;
                MultiplSoin += multiplicateurSoin;
            }
            else
            {
                var multiplicateurSoin = charStatModifier.MultiplSoin - 1;
                MultiplSoin += multiplicateurSoin;
            }
        }
        if (charStatModifier.MultiplDegat != 1)
        {
            if (charStatModifier.MultiplDegat > 1)
            {
                var multiplicateurDegat = charStatModifier.MultiplDegat % 1;
                MultiplDegat += multiplicateurDegat;

            }
            else
            {
                var multiplicateurDegat = charStatModifier.MultiplDegat - 1;
                MultiplDegat += multiplicateurDegat;
            }
        }
        if (charStatModifier.MultipleBuffDebuff != 1)
            MultiplBuffDebuff = charStatModifier.MultipleBuffDebuff;

        RadianceMax += charStatModifier.RadianceMax;
        ForceAme += charStatModifier._forceAme;
        Vitesse += charStatModifier.Vitesse;
        if ((Conviction > 0 && Conviction + charStatModifier.Conviction <= 0) && (Conviction < 0 && Conviction + charStatModifier.Conviction >= 0))
            OnConvictionChanged?.Invoke();
        Conviction += charStatModifier.Conviction;
        this.Resilience += charStatModifier._resilience;
        Calme += charStatModifier.Calme;
        Essence += charStatModifier.Essence;
        Tension += charStatModifier.Tension * MultiplTension;
        PalierChangement += charStatModifier.PalierChangement;
        if (charStatModifier.Radiance < 0)
        {
            Radiance += Mathf.FloorToInt(charStatModifier.Radiance * MultiplDef);
        }
        else
        {
            Radiance += Mathf.FloorToInt(charStatModifier.Radiance * MultiplSoin);
        }

        IsStun = charStatModifier.isStun;
    }


    public void RectificationStat()
    {
        if (Radiance > RadianceMax && RadianceMax > 0)
        {
            Radiance = RadianceMax;
        }

        if (Conviction > GameManager.Instance.CommonStatsData.ConvictionMax)
        {
            Conviction = GameManager.Instance.CommonStatsData.ConvictionMax;
        }
        else if (Conviction < GameManager.Instance.CommonStatsData.ConvictionMin)
        {
            Conviction = GameManager.Instance.CommonStatsData.ConvictionMin;
        }

        if (Resilience > GameManager.Instance.CommonStatsData.ResilienceMax)
        {
            Resilience = GameManager.Instance.CommonStatsData.ResilienceMax - ResiliencePassif;
        }
        else if (Resilience < GameManager.Instance.CommonStatsData.ResilienceMin)
        {
            Resilience = GameManager.Instance.CommonStatsData.ResilienceMin + ResiliencePassif;
        }

        if (ForceAme < 0)
            ForceAme = 0;
    }

    internal void SetZero()
    {
        Calme = 0;
        Conviction = 0;
        Resilience = 0;
        Essence = 0;
        ForceAme = 0;
        Radiance = 0;
        ResiliencePassif = 0;
        RadianceMax = 0;
    }

    public void RemoveStat(CharacterStat charStatModifier)
    {
        if (charStatModifier.MultiplDef != 1)
        {
            if (charStatModifier.MultiplDef > 1)
            {
                var multiplicateurDef = charStatModifier.MultiplDef % 1;
                MultiplDef -= multiplicateurDef;

            }
            else
            {
                var multiplicateurDef = charStatModifier.MultiplDef - 1;
                MultiplDef -= multiplicateurDef;
            }
        }

        if (charStatModifier.MultiplSoin != 1)
        {
            if (charStatModifier.MultiplSoin > 1)
            {
                var multiplicateurSoin = charStatModifier.MultiplSoin % 1;
                MultiplSoin -= multiplicateurSoin;

            }
            else
            {
                var multiplicateurSoin = charStatModifier.MultiplSoin - 1;
                MultiplSoin -= multiplicateurSoin;
            }
        }
        if (charStatModifier.MultiplDegat != 1)
        {
            if (charStatModifier.MultiplDegat > 1)
            {
                var multiplicateurAtk = charStatModifier.MultiplDegat % 1;
                MultiplDegat -= multiplicateurAtk;

            }
            else
            {
                var multiplicateurAtk = charStatModifier.MultiplDegat - 1;
                MultiplDegat -= multiplicateurAtk;
            }
        }
        if (charStatModifier.MultipleBuffDebuff != 1)
            MultiplBuffDebuff = charStatModifier.MultipleBuffDebuff;

        RadianceMax -= charStatModifier.RadianceMax;
        ForceAme -= charStatModifier.ForceAme;
        Vitesse -= charStatModifier.Vitesse;
        Conviction -= charStatModifier.Conviction;
        Resilience -= charStatModifier._resilience;
        Calme -= charStatModifier.Calme;
        Essence -= charStatModifier.Essence;
        Tension -= charStatModifier.Tension;

        if (charStatModifier.Radiance < 0)
        {
            Radiance -= Mathf.FloorToInt(charStatModifier.Radiance * MultiplDef);
        }
        else
        {
            Radiance -= Mathf.FloorToInt(charStatModifier.Radiance * MultiplSoin);
        }

        if (charStatModifier.isStun)
            IsStun = !charStatModifier.isStun;
    }

    public void ResetStat()
    {
        RadianceMax = BaseRadianceMax;
        Radiance = BaseRadianceMax;
        ForceAme = BaseForceDame;
        Vitesse = BaseVitesse;
        Conviction = BaseConviction;
        Resilience = BaseResilience;
        Calme = 0;
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
        /*this.nbAttaqueRecu = 0;
        this.ListBuffDebuff.Clear();*/
    }

}
