﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Capacité/Create New Effet", order = 11)]
public class Effet : ScriptableObject
{
    public TypeEffet TypeEffet;
    public Cible Cible;
    public int Pourcentage;
    public int ValeurBrut;
    public int RandomX;
    public int RandomY;
    public int NbAttaque;

    public JoueurStat ResultEffet(JoueurStat Caster, int LastDamageTake = 0, JoueurStat Cible = null)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        switch (this.TypeEffet)
        {
            default:
                ModifState = ResultEffetCommun(Caster);
                break;
        }
        return ModifState;
    }

    public JoueurStat ResultEffet(EnnemiStat Caster, int LastDamageTaken = 0, JoueurStat Cible = null)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = ResultEffetCommun(Caster, LastDamageTaken);
        return ModifState;
    }

    private JoueurStat ResultEffetCommun(CharacterStat Caster, int LastDamageTaken = 0, JoueurStat Cible = null)
    {
        int valueToChange = ValeurBrut * NbAttaque;
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = JoueurStat.CreateFromCharacter(ResultEffetBase(Caster, LastDamageTaken));
        switch (this.TypeEffet)
        {
            case TypeEffet.Clairvoyance:
                ModifState.Clairvoyance = valueToChange;
                break;
            case TypeEffet.Volonte:
                ModifState.Volonter += valueToChange;
                break;
            case TypeEffet.VolonteMax:
                ModifState.VolonterMax += valueToChange;
                break;
            case TypeEffet.Conscience:
                ModifState.Conscience += valueToChange;
                break;
            case TypeEffet.ConscienceMax:
                ModifState.ConscienceMax += valueToChange;
                break;
            default:
                break;
        }
        return ModifState;
    }

    private CharacterStat ResultEffetBase(CharacterStat Caster, int LastDamageTaken = 0, JoueurStat Cible = null)
    {
        int valueToChange = ValeurBrut * NbAttaque;
        CharacterStat ModifState = ScriptableObject.CreateInstance("CharacterStat") as CharacterStat;
        switch (this.TypeEffet)
        {
            case TypeEffet.DegatsForceAme:
                ModifState.Radiance += (Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * Caster.ForceAme;
                break;
            case TypeEffet.DegatsBrut:
                ModifState.Radiance += valueToChange;
                break;
            case TypeEffet.Conviction:
                ModifState.Conviction += valueToChange;
                break;
            case TypeEffet.AugmentationPourcentageFA:
                ModifState.ForceAme += (Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * Caster.ForceAme;
                break;
            case TypeEffet.AugmentationBrutFA:
                ModifState.ForceAme += valueToChange;
                break;
            case TypeEffet.RadianceMax:
                ModifState.RadianceMax += valueToChange;
                break;
            case TypeEffet.AugmentFADernierDegatsSubi:
                ModifState.ForceAme += LastDamageTaken;
                break;
            case TypeEffet.Vitesse:
                ModifState.Vitesse += valueToChange;
                break;
            case TypeEffet.Resilience:
                ModifState.Resilience += valueToChange;
                break;
            case TypeEffet.TensionStep:
                ModifState.PalierChangement += valueToChange;
                break;
            case TypeEffet.TensionValue:
                ModifState.Tension += valueToChange;
                break;
            case TypeEffet.TensionGainAttaqueValue:
                ModifState.TensionAttaque += valueToChange;
                break;
            case TypeEffet.TensionGainDebuffValue:
                ModifState.TensionDebuff += valueToChange;
                break;
            case TypeEffet.TensionGainSoinValue:
                ModifState.TensionSoin += valueToChange;
                break;
            case TypeEffet.TensionGainDotValue:
                ModifState.TensionDot += valueToChange;
                break;
            case TypeEffet.MultiplDef:
                ModifState.MultiplDef += valueToChange;
                break;
            case TypeEffet.MultiplDegat:
                ModifState.MultiplDegat += valueToChange;
                break;
            case TypeEffet.MultiplSoin:
                ModifState.MultiplSoin += valueToChange;
                break;
            case TypeEffet.DegatPVMax:
                ModifState.Radiance += (Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * Cible.RadianceMax;
                break;
            case TypeEffet.Soin:
                ModifState.Radiance += valueToChange;
                break;
            case TypeEffet.SoinFA:
                ModifState.Radiance += (Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * Caster.ForceAme;
                break;
            case TypeEffet.SoinRadianceMax:
                ModifState.Radiance += (Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * Cible.RadianceMax;
                break;
            case TypeEffet.SoinRadianceActuelle:
                ModifState.Radiance += (Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * Cible.Radiance;
                break;
            case TypeEffet.RandomAttaque:
                ModifState.Radiance += (Mathf.FloorToInt(Pourcentage / 100f) * Random.Range(RandomX, RandomY + 1)) * Caster.ForceAme;
                break;
            case TypeEffet.AugmentationFaRadianceActuelle:
                ModifState.ForceAme += (Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * Caster.Radiance;
                break;
            default:
                break;
        }
        return ModifState;
    }
}
