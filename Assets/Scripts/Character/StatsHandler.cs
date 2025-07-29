using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI.Extensions;


public class StatsHandler
{
    #region BASE STATS
    private CharacterStat _baseStat;
    public CharacterStat BaseStat { get { return _baseStat; } }
    public int BaseRadianceMax => _baseStat.RadianceMax;
    public int BaseForceDame => _baseStat.ForceAmeOriginal;
    public int BaseVitesse => _baseStat.VitesseOriginal;
    public int BaseConviction => _baseStat.ConvictionOriginal;
    public int BaseCalme => _baseStat.Calme;
    public int BaseResilience => _baseStat.ResilienceOriginal;
    #endregion

    #region PROPERTY STATS
    [field:SerializeField, ReadOnly] public int     Radiance { get; private set; }
    [field:SerializeField, ReadOnly] public int     RadianceMax { get; private set; }
    [field: SerializeField, ReadOnly] private int _forceDame;
    public int ForceAme
    {
        get => _forceDame + ForceAmeBonus;
        set => _forceDame = value;
    }
    
    public int ForceDameWithoutBonus
    {
        get => _forceDame;
    }

    [field:SerializeField, ReadOnly] public int     ForceAmeBonus { get; private set; }
    [field:SerializeField, ReadOnly] public int     Vitesse { get; private set; }
    [field:SerializeField, ReadOnly] public int     Conviction { get; private set; }
    [field:SerializeField, ReadOnly] public int     Calme { get; private set; }
    [field:SerializeField, ReadOnly] public int     Resilience { get; private set; }
    [field:SerializeField, ReadOnly] public int     ResiliencePassif { get; private set; }
    [field:SerializeField, ReadOnly] public int     Essence { get; set; }
    [field:SerializeField, ReadOnly] public float   MultiplDef { get; private set; }
    [field:SerializeField, ReadOnly] public float   MultiplSoin { get; private set; }
    [field:SerializeField, ReadOnly] public float   MultiplDegat { get; private set; }
    [field:SerializeField, ReadOnly] public float   MultiplBuffDebuff { get; private set; }
    [field:SerializeField, ReadOnly] public float   MultiplTension { get; private set; }
    [field:SerializeField, ReadOnly] public float   Tension { get; set; }
    [field:SerializeField, ReadOnly] public float   TensionMax { get; set; }
    [field:SerializeField, ReadOnly] public float   ValeurPalier { get; set; }
    [field:SerializeField, ReadOnly] public int     PalierChangement { get; private set; }
    [field:SerializeField, ReadOnly] public bool    IsStun { get; set; }

    [field: SerializeField, ReadOnly] public List<BuffDebuff> ListBuffDebuff { get; set; }
    #endregion

    #region EVENTS
    public event Action OnConvictionChanged;
    public event Action OnRadianceChange;
    #endregion

    public StatsHandler(StatsHandler statsHandler)
    {
        Radiance = statsHandler.Radiance;
        RadianceMax = statsHandler.RadianceMax;
        ForceAme = statsHandler.ForceAme;
        ForceAmeBonus = statsHandler.ForceAmeBonus;
        Vitesse = statsHandler.Vitesse;
        Conviction = statsHandler.Conviction;
        Calme = statsHandler.Calme;
        Resilience = statsHandler.Resilience;
        ResiliencePassif = (int)statsHandler.ResiliencePassif;
        Essence = statsHandler.Essence;
        MultiplDef = statsHandler.MultiplDef;
        MultiplSoin = statsHandler.MultiplSoin;
        MultiplDegat = statsHandler.MultiplDegat;
        MultiplBuffDebuff = statsHandler.MultiplBuffDebuff;
        MultiplTension = statsHandler.MultiplTension;
        Tension = statsHandler.Tension;
        TensionMax = statsHandler.TensionMax;
        ValeurPalier = statsHandler.ValeurPalier;
        PalierChangement = statsHandler.PalierChangement;
        IsStun = statsHandler.IsStun;

        ListBuffDebuff = new List<BuffDebuff>(statsHandler.ListBuffDebuff);
        _baseStat = ScriptableObject.Instantiate<CharacterStat>(statsHandler.BaseStat);
    }
    public StatsHandler(CharacterStat charStat)
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

        ListBuffDebuff = new List<BuffDebuff>(charStat.ListBuffDebuff);
        _baseStat = charStat;

    }
    public void SetTension (float newValue)
    {
        ChangeTension(-Tension + newValue);
    }
    public void ChangeTension(float modifier)
    {
        CharacterStat newValue = ScriptableObject.CreateInstance<CharacterStat>();
        newValue.Tension = modifier;
        UpdateStat(newValue);
    }
    public void ChangeRadiance(int modifier)
    {
        CharacterStat newValue = ScriptableObject.CreateInstance<CharacterStat>();
        newValue.Radiance = modifier;
        UpdateStat(newValue);
    }
    /// <summary>
    /// Set Radiance without raise the OnRadianceChange Event
    /// </summary>
    /// <param name="newValue">the new value for Radiance</param>
    public void SetRadiance(int newValue)
    {
        Radiance = newValue;
    }
    public void RemoveAmountRadiance(int amount)
    {
        Radiance -= amount;
    }
    public void ChangeForceDame(int modifier)
    {
        CharacterStat newValue = ScriptableObject.CreateInstance<CharacterStat>();
        newValue.ForceAme = modifier;
        UpdateStat(newValue);
    }
    public void SetForceDameBonus(int newValue)
    {
        ForceAmeBonus = newValue;
    }
    public void SetResiliencePassit(int newValue)
    {
        ResiliencePassif = newValue;
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
        RectificationStat();
    }


    public virtual void RectificationStat()
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

    public virtual void SetZero()
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
            OnRadianceChange?.Invoke();
        }
        else if (charStatModifier.Radiance > 0)
        {
            Radiance -= Mathf.FloorToInt(charStatModifier.Radiance * MultiplSoin);
            OnRadianceChange?.Invoke();
        }

        if (charStatModifier.isStun)
            IsStun = !charStatModifier.isStun;
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
