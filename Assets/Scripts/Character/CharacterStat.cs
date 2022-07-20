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
    public List<BuffDebuff> ListBuffDebuff = new List<BuffDebuff>(); 

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
    }
}
