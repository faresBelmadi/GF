using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    public int HP;
    public int MaxHP;
    public int Volonté;
    public int MaxVolonté;
    public int Conscience;
    public int MaximumConscience;
    public int Essence;
    public int Dmg;
    public int armor;
    public int Speed;
    public int Calme;
    public int MinResilience;
    public int Resilience;
    public int MaxResilience;
    public int MinConviction;
    public int Conviction;
    public int MaxConviction;

    [Header("Stats Tension")]
    public int TensionAttaque = 4;
    public int TensionDebuff = 2;
    public int TensionDot = 1;
    public int TensionSoin = -3;
    public float Tension;
    public float TensionMax;
    public float ValeurPalier;
    public int NbPalier = 3;

    public List<Spell> AvailableSpell;
}
