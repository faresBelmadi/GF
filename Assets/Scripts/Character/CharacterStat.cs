using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStat : ScriptableObject
{
    public int Radiance;
    public int RadianceMax;
    public int RadianceMaxOriginal;
    public int ForceAme;
    public int ForceAmeOriginal;
    public int Vitesse;
    public int VitesseOriginal;
    public int Conviction;
    public int ConvictionMin = -10;
    public int ConvictionMax = 10;
    public int ConvictionOriginal;

    public int Resilience
    {
        get => _resilience + ResilienceBonus;

        set => _resilience = value;
    }

    private int _resilience;
    public int ResilienceBonus;
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
    public int TensionAttaque = 0;
    public int TensionDebuff = 0;
    public int TensionSoin = 0;
    public int TensionDot = 0;
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
            this.MultiplDef = ModifState.MultiplDef;
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
        this.ConvictionMin += ModifState.ConvictionMin;
        this.ConvictionMax += ModifState.ConvictionMax;
        this.Resilience += ModifState.Resilience;
        this.ResilienceMin += ModifState.ResilienceMin;
        this.ResilienceMax += ModifState.ResilienceMax;
        this.Calme += ModifState.Calme;
        this.Essence += ModifState.Essence;


        if (ModifState.Radiance < 0)
        {
            var toRemove = Mathf.FloorToInt(ModifState.Radiance * this.MultiplDef);
            toRemove += Mathf.FloorToInt(((Resilience * 3) / 100f) * toRemove);
            this.Radiance += toRemove;
        }
        else
            this.Radiance += ModifState.Radiance;

    }

    public void RectificationStat()
    {
        if (this.Radiance > this.RadianceMax)
        {
            this.Radiance = this.RadianceMax;
        }
        if (this.Conviction > this.ConvictionMax)
        {
            this.Conviction = this.ConvictionMax;
        }else if(this.Conviction < this.ConvictionMin)
        {
            this.Conviction = this.ConvictionMin;
        }
        if (this.Resilience > this.ResilienceMax)
        {
            this.Resilience = this.ResilienceMax;
        }
        else if (this.Resilience < this.ResilienceMin)
        {
            this.Resilience = this.ResilienceMin;
        }

        if (this.ForceAme <= 0)
            this.ForceAme = 0;
    }
}
