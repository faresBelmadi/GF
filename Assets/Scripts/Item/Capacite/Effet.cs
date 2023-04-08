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
                ModifState = ResultEffetCommun(Caster);
                break;
        }
        return ModifState;
    }

    public JoueurStat ResultEffet(EnnemiStat Caster, int LastDamageTaken = 0, JoueurStat Cible = null)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = ResultEffetCommun(Caster, LastDamageTaken,Cible);
        return ModifState;
    }

    public JoueurStat ResultEffet(JoueurStat Caster, int LastDamageTaken, EnnemiStat CibleEnnemi = null)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = ResultEffetCommun(Caster, LastDamageTaken, CibleEnnemi);
        return ModifState;
    }
    private JoueurStat ResultEffetCommun(CharacterStat Caster, int LastDamageTaken = 0, CharacterStat Cible = null)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = JoueurStat.CreateFromCharacter(ResultEffetBase(Caster, LastDamageTaken,Cible));

        return ModifState;
    }

    private CharacterStat ResultEffetBase(CharacterStat Caster, int LastDamageTaken = 0, CharacterStat Cible = null)
    {
        int valueToChange = ValeurBrut * NbAttaque;
        CharacterStat ModifState = ScriptableObject.CreateInstance("CharacterStat") as CharacterStat;
        int percent;
        switch (this.TypeEffet)
        {
            case TypeEffet.DegatsForceAme:
                ModifState.Radiance += Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme)*Caster.MultiplDegat);
                break;
            case TypeEffet.DegatsBrut:
                ModifState.Radiance += Mathf.FloorToInt(valueToChange*Caster.MultiplDegat);
                break;
            case TypeEffet.Conviction:
                ModifState.Conviction += valueToChange;
                break;
            case TypeEffet.AugmentationPourcentageFA:
                ModifState.ForceAme += (Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme));
                break;
            case TypeEffet.AugmentationBrutFA:
                ModifState.ForceAme += valueToChange;
                break;
            case TypeEffet.RadianceMax:
                ModifState.RadianceMax += valueToChange;
                ModifState.Radiance += valueToChange;
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
                ModifState.MultiplDef += (Pourcentage / 100f) * NbAttaque;
                break;
            case TypeEffet.MultiplDegat:
                ModifState.MultiplDegat += (Pourcentage / 100f) * NbAttaque;
                break;
            case TypeEffet.MultiplSoin:
                ModifState.MultiplSoin += (Pourcentage / 100f) * NbAttaque;
                break;
            case TypeEffet.DegatPVMax:
                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Cible.RadianceMax);
                break;
            case TypeEffet.Soin:
                ModifState.Radiance += valueToChange;
                break;
            case TypeEffet.SoinFA:
                ModifState.Radiance += Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme)*Caster.MultiplSoin);
                break;
            case TypeEffet.SoinFANbEnnemi:
                ModifState.Radiance += (Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme) * GameManager.instance.BattleMan.EnemyScripts.Count);
                break;
            case TypeEffet.SoinRadianceMax:
                ModifState.Radiance +=Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Cible.RadianceMax);
                break;
            case TypeEffet.SoinRadianceActuelle:
                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Cible.Radiance);
                break;
            case TypeEffet.RandomAttaque:
                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage / 100f) * Random.Range(RandomX, RandomY + 1) * Caster.ForceAme));
                break;
            case TypeEffet.AugmentationFaRadianceActuelle:
                ModifState.ForceAme += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Caster.Radiance);
                break;
            case TypeEffet.ConsommeTensionAugmentationFA:
                ModifState.Tension += -Cible.Tension;
                var toAdd = Instantiate(FindObjectsOfType<BuffDebuff>().First(c => c.Nom == "Tension convertis en FA"));
                toAdd.Effet.First().Pourcentage = (int)Cible.Tension;
                Caster.ListBuffDebuff.Add(toAdd);
                break;
            case TypeEffet.RemoveDebuff:
                var tempListRD = Cible.ListBuffDebuff.Where(c => c.IsDebuff).ToList();
                tempListRD.RemoveAt(Random.Range  (0, tempListRD.Count));
                break;
            case TypeEffet.AttaqueStackAmant:
                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage / 100f) * Cible.ListBuffDebuff.Count(c => c.Nom == "Amant")) * Caster.ForceAme);
                break;
            case TypeEffet.AttaqueFADebuff:
                var tempListAFAD = Cible.ListBuffDebuff.Where(c => c.IsDebuff).ToList();

                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage + (ValeurBrut * tempListAFAD.Count()) / 100f) * NbAttaque) * Caster.ForceAme);
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
            case TypeEffet.AugmentationRadianceMaxPourcentage:
                ModifState.RadianceMax += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Cible.RadianceMax);
                break;
            case TypeEffet.BuffFaCoupRecu:
                ModifState.ForceAme += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque));
                break;
            case TypeEffet.DegatsBrutConsequence:
                ModifState.Radiance += valueToChange;
                break;
            case TypeEffet.DamageUpTargetLowRadiance:
                percent = Pourcentage;
                if (((Cible.Radiance / Cible.RadianceMax) * 100) <= 25)
                    percent *= 3;
                ModifState.Radiance += Mathf.FloorToInt((((percent / 100f) * NbAttaque) * Caster.ForceAme) * Caster.MultiplDegat);
                break;

            case TypeEffet.DegatsRetourSurAttaque: //S2 manequin utiliser le nouveau truc pour le buff/ var dans l'enemi last damage taken pour appliquer ici
                //TODO
                break;

            case TypeEffet.BuffResilienceCoupRecu:
                ModifState.Resilience += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque));
                break;
            case TypeEffet.CancelPourcentageDamage:
                ModifState.Radiance += Mathf.FloorToInt(valueToChange - ((Pourcentage / 100) * valueToChange));
                break;
            case TypeEffet.AugmentationFARadianceManquante: //martyr elite 
                ModifState.ForceAme += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * (Caster.RadianceMax - Caster.Radiance));
                break;

            //case TypeEffet.DamageFaBuff: //martyr capa 3 Inflige x% de Force d'ame au joueur, par buff sur lui 
            //    //TODO
            //    break;
            //case TypeEffet.AugmentationFaNbDebuff: // buff FA en fct nb attaque
            //    //TODO
            //    break;

                // rajouter buff debuff et dans les remove on check et on applique le buff/debuff en fct du truc
                //separer proc damage et proc effect proc damage =W applique direct
            case TypeEffet.RemoveAllTensionProcDamage:
                var tempListBuffDebuffToRemoveProcDamage = Cible.ListBuffDebuff.Where(x => x.IsDebuff && x.CibleApplication == global::Cible.All);
                foreach (var buffDebuff in tempListBuffDebuffToRemoveProcDamage)
                {
                    if (Cible.ListBuffDebuff.Contains(buffDebuff))//pourcentage FA
                        Cible.ListBuffDebuff.Remove(buffDebuff);
                }
                break;
            case TypeEffet.RemoveAllDebuffProcEffect:
                var tempListDebuffToRemoveProcEffect = Cible.ListBuffDebuff.Where(x => x.IsDebuff
                    && x.CibleApplication == global::Cible.joueur);
                foreach (var buffDebuff in tempListDebuffToRemoveProcEffect)
                {
                    if (Cible.ListBuffDebuff.Contains(buffDebuff))
                        Cible.ListBuffDebuff.Remove(buffDebuff);
                }
                break;
            case TypeEffet.RemoveAllDebuffSelfProcEffect:
                var tempListDebuffToRemoveSelfProcEffect = Cible.ListBuffDebuff.Where(x => x.IsDebuff
                    && x.CibleApplication == global::Cible.joueur);
                foreach (var buffDebuff in tempListDebuffToRemoveSelfProcEffect)
                {
                    if (Cible.ListBuffDebuff.Contains(buffDebuff))
                        Cible.ListBuffDebuff.Remove(buffDebuff);
                }
                break;
            case TypeEffet.RemoveAllBuffProcEffect:
                var tempListBuffToRemoveProcEffect = Cible.ListBuffDebuff.Where(x => !x.IsDebuff
                    && x.CibleApplication == global::Cible.joueur);
                foreach (var buffDebuff in tempListBuffToRemoveProcEffect)
                {
                    if (Cible.ListBuffDebuff.Contains(buffDebuff))
                        Cible.ListBuffDebuff.Remove(buffDebuff);
                }
                break;
            case TypeEffet.GainFaBuffCible:
                ModifState.ForceAme += Cible.ListBuffDebuff.Count(x => !x.IsDebuff) * valueToChange;
                break;
            case TypeEffet.GainFaDebuffCible:
                ModifState.ForceAme += Cible.ListBuffDebuff.Count(x => x.IsDebuff) * valueToChange;
                break;
            case TypeEffet.Ponction:
                ModifState.Radiance += valueToChange;//porcentage de la radiance de la cible // heal caster et le modif state == target en gros
                break;
            default:
                break;
        }
        return ModifState;
    }
}