using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class AbstractStatsHandler
{
    #region PROPERTY STATS
    [field: SerializeField, ReadOnly] public int Radiance { get; protected set; }
    protected int _radianceMaxModifier;
    public virtual int RadianceMaxModifier
    { 
        get => _radianceMaxModifier; 
        protected set=> _radianceMaxModifier = value; 
    }
    public virtual int RadianceMaxTotal { get; }
    [field: SerializeField, ReadOnly] protected int _forceDameModifier;
    public int ForceDameModifier
    {
        get => _forceDameModifier;
        protected set => _forceDameModifier = value;
    }
    public virtual int ForceDameTotal { get; }
    //Legacy, a suppr apres verif
    //public virtual int ForceAme
    //{
    //    get => _forceDameModifier + ForceAmeBonus;
    //    protected set => _forceDameModifier = value;
    //}

    //public int ForceDameWithoutBonus
    //{
    //    get => _forceDameModifier;
    //}

    [field: SerializeField, ReadOnly] public int ForceAmeBonus { get; protected set; }
    protected int _vitesseModifier;
    public int VitesseModifier
    {
        get => _vitesseModifier;
        protected set => _vitesseModifier = value; 
    }
    public virtual int VitesseTotal { get; }
    protected int _convictionModifier;
    public int ConvictionModifier
    { 
        get => _convictionModifier;
        protected set=> _convictionModifier = value;
    }
    public virtual int ConvictionTotal { get; }
    protected int _calmeModifier;
    public int CalmeModifier
    { 
        get => _calmeModifier; 
        protected set => _calmeModifier = value;
    }
    public virtual int CalmeTotal { get; }
    [field: SerializeField, ReadOnly] protected int _resilienceModifier;
    public int ResilienceModifier
    {
        get => _resilienceModifier + ResiliencePassif;
        set => _resilienceModifier = value;
    }
    public virtual int ResilienceTotal { get; }
    [field: SerializeField, ReadOnly] public virtual int ResiliencePassif { get; protected set; }
    [field: SerializeField, ReadOnly] public virtual int Essence { get; set; }
    [field: SerializeField, ReadOnly] public virtual float MultiplDef { get; protected set; }
    [field: SerializeField, ReadOnly] public virtual float MultiplSoin { get; protected set; }
    [field: SerializeField, ReadOnly] public virtual float MultiplDegat { get; protected set; }
    [field: SerializeField, ReadOnly] public virtual float MultiplBuffDebuff { get; protected set; }
    [field: SerializeField, ReadOnly] public virtual float MultiplTension { get; protected set; }
    [field: SerializeField, ReadOnly] public virtual float Tension { get; set; }
    [field: SerializeField, ReadOnly] public virtual float TensionMax { get; set; }
    [field: SerializeField, ReadOnly] public virtual float ValeurPalier { get; set; }
    [field: SerializeField, ReadOnly] public virtual int PalierChangement { get; protected set; }
    [field: SerializeField, ReadOnly] public bool IsStun { get; set; }

    [field: SerializeField, ReadOnly] public List<BuffDebuff> ListBuffDebuff { get; set; }
    #endregion

    #region EVENTS
    public event Action OnConvictionChanged;
    public event Action OnRadianceChange;
    #endregion

    public AbstractStatsHandler(AbstractStatsHandler statsHandler)
    {
        Radiance = statsHandler.Radiance;
       // RadianceMax = statsHandler.RadianceMax;
        //ForceAme = statsHandler.ForceAme;
        ForceAmeBonus = statsHandler.ForceAmeBonus;
       // Vitesse = statsHandler.Vitesse; // ajouter modifier
       // ConvictionTotal = statsHandler.ConvictionTotal;
      //  CalmeTotal = statsHandler.CalmeTotal;
       // ResilienceTotal = statsHandler.ResilienceTotal;
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
       
    }

    public AbstractStatsHandler(CharacterStat charStat)
    {
        Radiance = charStat.Radiance;
       // RadianceMax = charStat.RadianceMax;
        //ForceAme = charStat.ForceAmeOriginal;
        ForceAmeBonus = charStat.ForceAmeBonus;
        //Vitesse = charStat.Vitesse;
        //ConvictionTotal = charStat.Conviction;
        //CalmeTotal = charStat.Calme;
        //ResilienceTotal = charStat.ResilienceOriginal;
        ResiliencePassif = (int)charStat.ResiliencePassif;
        Essence = charStat.Essence;
        MultiplDef = charStat.MultiplDef;
        MultiplSoin = charStat.MultiplSoin;
        MultiplDegat = charStat.MultiplDegat;
        MultiplBuffDebuff = charStat.MultipleBuffDebuff;
        MultiplTension = charStat.MultipleTension;
        Tension = charStat.Tension;
        TensionMax = charStat.TensionMax;
        ValeurPalier = charStat.ValeurPalier;
        PalierChangement = charStat.PalierChangement;
        IsStun = charStat.isStun;

        ListBuffDebuff = new List<BuffDebuff>(charStat.ListBuffDebuff);
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

        RadianceMaxModifier += charStatModifier.RadianceMax;
        _forceDameModifier += charStatModifier._forceAme;
        VitesseModifier += charStatModifier.Vitesse;
        if ((ConvictionTotal > 0 && ConvictionTotal + charStatModifier.Conviction <= 0) && (ConvictionTotal < 0 && ConvictionTotal + charStatModifier.Conviction >= 0))
            OnConvictionChanged?.Invoke();
        ConvictionModifier += charStatModifier.Conviction;
        _resilienceModifier += charStatModifier._resilience;
        CalmeModifier += charStatModifier.Calme;
        Essence += charStatModifier.Essence;
        Tension += charStatModifier.Tension * MultiplTension;
        PalierChangement += charStatModifier.PalierChangement;
        if (charStatModifier.Radiance < 0)
        {
            Radiance += Mathf.FloorToInt(charStatModifier.Radiance * MultiplDef);
            OnRadianceChange?.Invoke();
        }
        else
        {
            Radiance += Mathf.FloorToInt(charStatModifier.Radiance * MultiplSoin);
            OnRadianceChange?.Invoke();
        }

        IsStun = charStatModifier.isStun;
        RectificationStat();
    }

    public virtual void RectificationStat()
    {
        //  TODO: ne fonctionne plus comme ça, a modifier si besoin
        //if (Radiance > RadianceMax && RadianceMax > 0)
        //{
        //    Radiance = RadianceMax;
        //}

        //if (ConvictionTotal > GameManager.Instance.CommonStatsData.ConvictionMax)
        //{
        //    ConvictionTotal = GameManager.Instance.CommonStatsData.ConvictionMax;
        //}
        //else if (ConvictionTotal < GameManager.Instance.CommonStatsData.ConvictionMin)
        //{
        //    ConvictionTotal = GameManager.Instance.CommonStatsData.ConvictionMin;
        //}

        //if (ResilienceTotal > GameManager.Instance.CommonStatsData.ResilienceMax)
        //{
        //    ResilienceTotal = GameManager.Instance.CommonStatsData.ResilienceMax - ResiliencePassif;
        //}
        //else if (ResilienceTotal < GameManager.Instance.CommonStatsData.ResilienceMin)
        //{
        //    ResilienceTotal = GameManager.Instance.CommonStatsData.ResilienceMin + ResiliencePassif;
        //}

        //if (ForceDameT < 0) 
        //    ForceAme = 0;
    }
    public virtual void SetZero()
    {
        //CalmeTotal = 0;
        //ConvictionTotal = 0;
        //ResilienceTotal = 0;
        //Essence = 0;
        ////ForceAme = 0;
        //Radiance = 0;
        //ResiliencePassif = 0;
        //RadianceMax = 0;
    }
    public void SetTension(float newValue)
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

    public void ChangeForceDame(int modifier)
    {
        CharacterStat newValue = ScriptableObject.CreateInstance<CharacterStat>();
        newValue.ForceAme = modifier;
        UpdateStat(newValue);
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

        RadianceMaxModifier -= charStatModifier.RadianceMax;
        ForceDameModifier -= charStatModifier.ForceAme;
        VitesseModifier -= charStatModifier.Vitesse;
        ConvictionModifier -= charStatModifier.Conviction;
        ResilienceModifier -= charStatModifier._resilience;
        CalmeModifier -= charStatModifier.Calme;
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
}
