﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStat : ScriptableObject
{
    public int Radiance;
    public int RadianceMax;
    public int RadianceMaxOriginal;

    public int ForceAme
    {
        get => _forceAme + ForceAmeBonus;
        set => _forceAme = value;
    }

    public int _forceAme;
    public int ForceAmeBonus;
    public int ForceAmeOriginal;
    public int Vitesse;
    public int VitesseOriginal;
    public int Conviction;
    public int ConvictionMin = -10;
    public int ConvictionMax = 10;
    public int ConvictionOriginal;

    public int Resilience
    {
        get => _resilience + Mathf.FloorToInt(ResilienceBonus);

        set => _resilience = value;
    }

    private int _resilience;
    //[HideInInspector]
    public float ResilienceBonus;
    public int ResilienceMin = -10;
    public int ResilienceMax = 10;
    public int ResilienceOriginal;
    public int Calme;
    public int Essence;
    public float MultiplDef = 1;
    public float MultiplSoin = 1;
    public float MultiplDegat = 1;
    public float MultipleBuffDebuff = 1;
    public List<BuffDebuff> ListBuffDebuff = new List<BuffDebuff>();
    public int TensionAttaque = 4;
    public int TensionDebuff = 3;
    public int TensionSoin = -1;
    public int TensionDot = 1;
    public float Tension = 0;
    public float TensionMax = 0;
    public float ValeurPalier = 0;
    public int NbPalier = 3;
    public int PalierChangement = 0;
    public int nbAttaqueRecu = 0;
    public List<Passif> ListPassif;
    public Action ActionPassif;

    public void ModifStateAll(CharacterStat ModifState)
    {
        if (ModifState.MultiplDef != 1)
        {
            var multiplicateurDef = ModifState.MultiplDef % 1;
            this.MultiplDef += multiplicateurDef;
        }
        if (ModifState.MultiplSoin != 1)
            this.MultiplSoin = ModifState.MultiplSoin;
        if (ModifState.MultiplDegat != 1)
            this.MultiplDegat = ModifState.MultiplDegat;
        if (ModifState.MultipleBuffDebuff != 1)
            this.MultipleBuffDebuff = ModifState.MultipleBuffDebuff;

        this.RadianceMax += ModifState.RadianceMax;
        this.ForceAme += ModifState.ForceAme;
        this.Vitesse += ModifState.Vitesse;
        this.Conviction += ModifState.Conviction;
        this.Resilience += ModifState.Resilience;
        this.Calme += ModifState.Calme;
        this.Essence += ModifState.Essence;

        if (ModifState.Radiance < 0)
        {
            this.Radiance += ModifState.Radiance;
        }
        else
        {
            this.Radiance += ModifState.Radiance;
        }
        
    }

    public void RectificationStat()
    {
        if (this.Radiance > this.RadianceMax && RadianceMax > 0)
        {
            this.Radiance = this.RadianceMax;
        }
        if (this.Conviction > this.ConvictionMax)
        {
            this.Conviction = this.ConvictionMax;
        }
        else if (this.Conviction < this.ConvictionMin)
        {
            this.Conviction = this.ConvictionMin;
        }
        if (this.Resilience > this.ResilienceMax)
        {
            this.Resilience = this.ResilienceMax - (int)ResilienceBonus;
        }
        else if (this.Resilience < this.ResilienceMin)
        {
            this.Resilience = this.ResilienceMin + (int)ResilienceBonus;
        }

        if (this.ForceAme < 0)
            this.ForceAme = 0;
    }

    internal void setZero()
    {
        this.Calme = 0;
        this.Conviction = 0;
        this.Resilience = 0;
        this.Essence = 0;
        this.ForceAme = 0;
        this.Radiance = 0;
        this.ResilienceBonus= 0;
        this.RadianceMax = 0;
    }
}
