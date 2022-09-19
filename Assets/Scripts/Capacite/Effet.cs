using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private int TimeAlive = 1;
    public bool IsAttaqueEffet;

    public JoueurStat ResultEffet(JoueurStat Caster, int LastDamageTake = 0, JoueurStat Cible = null)
    {
        int valueToChange = ValeurBrut * NbAttaque;
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        switch (this.TypeEffet)
        {
            default:
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
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = JoueurStat.CreateFromCharacter(ResultEffetBase(Caster, LastDamageTaken));
        switch (this.TypeEffet)
        {
            
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
            case TypeEffet.SoinFANbEnnemi:
                ModifState.Radiance += ((Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * Caster.ForceAme) * GameManager.instance.BattleMan.EnemyScripts.Count;
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
            case TypeEffet.ConsommeTensionAugmentationFA:
                ModifState.Tension += -Cible.Tension;
                var toAdd = Instantiate(GameObject.FindObjectsOfType<BuffDebuff>().Where(c => c.Nom == "Tension convertis en FA").First());
                toAdd.Effet.First().Pourcentage = (int)Cible.Tension;
                Caster.ListBuffDebuff.Add(toAdd);
                break;
            case TypeEffet.RemoveDebuff:
                var tempListRD = Cible.ListBuffDebuff.Where(c => c.IsDebuff).ToList();
                tempListRD.RemoveAt(Random.Range  (0, tempListRD.Count));
                break;
            case TypeEffet.AttaqueStackAmant:
                ModifState.Radiance += (Mathf.FloorToInt(Pourcentage / 100f) * Cible.ListBuffDebuff.Where(c => c.Nom == "Amant").Count()) * Caster.ForceAme;
                break;
            case TypeEffet.AttaqueFADebuff:
                var tempListAFAD = Cible.ListBuffDebuff.Where(c => c.IsDebuff).ToList();

                ModifState.Radiance += Mathf.FloorToInt((Pourcentage + (ValeurBrut * tempListAFAD.Count()) / 100f) * NbAttaque) * Caster.ForceAme;
                break;
            case TypeEffet.GainResilienceIncrementale:
                ModifState.Resilience += ValeurBrut * TimeAlive;
                TimeAlive++;
                if (ModifState.Resilience > 10)
                    ModifState.Resilience = 10;
                break;
            case TypeEffet.DamageLastPhase:
                ModifState.Radiance += -GameManager.instance.BattleMan.LastPhaseDamage;
                break;
            case TypeEffet.NoEssence:
                ModifState.Essence += -Cible.Essence;
                break;
            case TypeEffet.DoubleBuffDebuff:
                ModifState.MultipleBuffDebuff = ValeurBrut;
                break;
            default:
                break;
        }
        return ModifState;
    }
}
