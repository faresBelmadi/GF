using UnityEngine;

public class PassifRules : ScriptableObject
{

    [Header("Passif Guerrier 1")]
    public int nbPtsResilience;
    public int nbPtsConscience;

    [Header("Passif Guerrier 2")]
    public int nbPtsConscienceEarned;
    public int PercentEssenceBonus;

    [Header("Passif Martyr")]
    public int nbSourcesDegats;
    public int nbPtsResilienceMartyr;
    public int nbPtsConvictionMartyr;

    [Header("Passif Cultiste")]
    public int nbChargeZealoteEarned;

    [Header("Passif Chef Cultiste")]
    public BuffDebuff BuffFullLife;
    public BuffDebuff Buff3QuarterLife;
    public BuffDebuff BuffHalfLife;
    public BuffDebuff BuffQuarterLife;

    [Header("Passi Jeanne")]
    public BuffDebuff TrenteDivin;
    public BuffDebuff VingtDivin;
    public BuffDebuff DixDivin;
    public int nbPtsRadianceJeanne;
    public int nbPtsForceAmeJeanne;

    [Header("Passif Papa")]
    public int nbPercentBuffForceAmePapa;
    public int nbPercentDebuffForceAmePapa;

    [Header("Passif Papy")]
    public int nbBuffTriggerPapy;
    public BuffDebuff DebuffFaPapy;
    public BuffDebuff DebuffVitessePapy;
    public BuffDebuff DebuffClairvoyancePapy;
}