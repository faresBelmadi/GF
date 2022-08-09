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
    public int Resilience;
    public int ResilienceMin = -10;
    public int ResilienceMax = 10;
    public int ResilienceOriginal;
    public int Calme;
    public int Essence;
    public float MultiplDef = 1;
    public float MultiplSoin = 1;
    public float MultiplDegat = 1;
    public int TensionAttaque = 4;
    public int TensionDebuff = 2;
    public int TensionDot = 1;
    public int TensionSoin = -3;
    public float Tension;
    public float TensionMax;
    public int PalierChangement;
    public float ValeurPalier = 10;
    public int NbPalier = 1;
    public List<BuffDebuff> ListBuffDebuff = new List<BuffDebuff>();
    public int MultipleBuffDebuff;

    public void ModifStateAll(CharacterStat ModifState)
    {
        this.Radiance += ModifState.Radiance;
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
        this.TensionAttaque += ModifState.TensionAttaque;
        this.TensionDebuff += ModifState.TensionDebuff;
        this.TensionDot += ModifState.TensionDot;
        this.TensionSoin += ModifState.TensionSoin;
        this.Tension += ModifState.Tension;
        this.TensionMax += ModifState.TensionMax;
    }
}