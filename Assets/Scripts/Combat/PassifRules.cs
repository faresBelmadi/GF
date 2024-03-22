using UnityEngine;

public class PassifRules : ScriptableObject
{

    [Header("Passif Guerrier 1")]
    public float nbPtsResilience;
    public float nbPtsConscience;

    [Header("Passif Guerrier 2")]
    public float nbPtsConscienceEarned;
    public float PercentEssenceBonus;

    [Header("Passif Martyr")]
    public int nbSourcesDegats;
    public float nbPtsResilienceMartyr;
    public float nbPtsConvictionMartyr;

    [Header("Passif Cultiste")]
    public int nbChargeZealoteEarned;
    public BuffDebuff BuffZealote;

    [Header("Passif Chef Cultiste")]
    public BuffDebuff BuffFullLife;
    public BuffDebuff Buff3QuarterLife;
    public BuffDebuff BuffHalfLife;
    public BuffDebuff BuffQuarterLife;
    public int Percent3QuarterLife;
    public int PercentHalfLife;
    public int PercentQuarterLife;

    [Header("Passif Jeanne")]
    public BuffDebuff CurrentDivin;
    //public float nbPtsRadianceJeanne;
    //public float nbPtsForceAmeJeanne;
    //public float nbPtsDivinJeanne;

    [Header("Passif Papa")]
    public float nbPercentBuffForceAmePapa;
    public float nbPercentDebuffForceAmePapa;

    [Header("Passif Papy")]
    public int nbBuffTriggerPapy;
    public BuffDebuff DebuffFaPapy;
    public BuffDebuff DebuffVitessePapy;
    public BuffDebuff DebuffClairvoyancePapy;
}