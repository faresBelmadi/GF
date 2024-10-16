﻿using System.Linq;
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
    public int ValeurParBuffDebuff;
    private int TimeAlive = 1;
    public bool IsAttaqueEffet;
    public bool IsFirstApplication = true;
    public BuffDebuff AfterEffectToApply;

    [SerializeField]
    public JoueurStat modifstate;

    [System.NonSerialized] public int nbProcAfterEffect;

    public JoueurStat ResultEffet(JoueurStat Caster, int LastDamageTake = 0, JoueurStat Cible = null)
    {
        //Debug.Log($"Trigger Effect: {this.TypeEffet} from {Caster} to {Cible}");
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
                ModifState = ResultEffetCommun(Caster, LastDamageTake,Cible);
                break;
        }
        modifstate = ModifState;
        return ModifState;
    }

    public JoueurStat ResultEffet(EnnemiStat Caster, int LastDamageTaken = 0, JoueurStat Cible = null)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = ResultEffetCommun(Caster, LastDamageTaken, Cible);
        modifstate = ModifState;
        return ModifState;
    }

    public JoueurStat ResultEffet(JoueurStat Caster, int LastDamageTaken, EnnemiStat CibleEnnemi = null, int NbEnnemies = 1)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = ResultEffetCommun(Caster, LastDamageTaken, CibleEnnemi, NbEnnemies);
        modifstate = ModifState;
        return ModifState;
    }
    public JoueurStat ResultEffet(EnnemiStat Caster, int LastDamageTaken, EnnemiStat CibleEnnemi = null, int NbEnnemies = 1)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = ResultEffetCommun(Caster, LastDamageTaken, CibleEnnemi, NbEnnemies);
        modifstate = ModifState;
        return ModifState;
    }
    private JoueurStat ResultEffetCommun(CharacterStat Caster, int LastDamageTaken = 0, CharacterStat Cible = null, int NbEnnemies = 1)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = JoueurStat.CreateFromCharacter(ResultEffetBase(Caster, LastDamageTaken, Cible, NbEnnemies));
        modifstate = ModifState;

        return ModifState;
    }

    public bool VisualizeAttack(CharacterStat caster, CharacterStat cible, out int damageAmount, int NbEnnemies = 1)
    {
        int percent;
        int nbProcDamage;
        damageAmount = 0;
        int valueToChange = ValeurBrut * NbAttaque;
        switch (this.TypeEffet)
        {
            case TypeEffet.DegatsForceAme:
                damageAmount +=
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * caster.ForceAme) * caster.MultiplDegat);
                break;
            case TypeEffet.DegatsBrut:
                damageAmount += Mathf.FloorToInt(valueToChange * caster .MultiplDegat);
                break;
            case TypeEffet.RadianceMax:

                //damageAmount = Mathf.FloorToInt((Pourcentage / 100f) * caster.RadianceMaxOriginal); ;//Mathf.FloorToInt((Pourcentage / 100f) * ((cible.Radiance / cible.RadianceMax) * cible.RadianceMaxOriginal));
                if (Cible == null)
                {
                    int addAmount = (int)(caster.RadianceMax * Pourcentage * .01f);
                    damageAmount = caster.RadianceMax + addAmount;
                }
                else
                {
                    int addAmount = (int)(cible.RadianceMax * Pourcentage * .01f);
                    damageAmount = cible.RadianceMax + addAmount;
                }
                break;
           
            case TypeEffet.DegatPVMax:
                damageAmount += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * cible.RadianceMax);
                break;
            case TypeEffet.Soin:
                damageAmount += valueToChange;
                break;
           
            case TypeEffet.RandomAttaque:
                damageAmount +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * Random.Range(RandomX, RandomY + 1) * caster.ForceAme));
                break;
          
            case TypeEffet.AttaqueStackAmant:
                damageAmount +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * cible.ListBuffDebuff.Count(c => c.Nom == "Amant")) *
                                     caster.ForceAme);
                break;
            case TypeEffet.AttaqueFADebuff:
                var tempListAFAD = cible.ListBuffDebuff.Where(c => c.IsDebuff).ToList();

                damageAmount +=
                    Mathf.FloorToInt(((Pourcentage + (ValeurBrut * tempListAFAD.Count()) / 100f) * NbAttaque) *
                                     caster.ForceAme);
                break;
            case TypeEffet.DamageLastPhase:
                damageAmount += -GameManager.Instance.BattleMan.LastPhaseDamage;
                break;
            case TypeEffet.DegatsBrutConsequence:
                damageAmount += valueToChange;
                break;
            case TypeEffet.DamageUpTargetLowRadiance:
                percent = Pourcentage;
                if (((float)cible.Radiance / (float)cible.RadianceMax) <= 0.25f)
                    percent *= 3;
                damageAmount +=
                    Mathf.FloorToInt((((percent / 100f) * NbAttaque) * caster.ForceAme) * caster.MultiplDegat);
                break;
            case TypeEffet.UntilDeath:
                damageAmount +=
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * caster.ForceAme) * caster.MultiplDegat);
                //if (damageAmount < Cible.Radiance)
                //    Caster.Radiance -= Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Cible.ForceAme) * Cible.MultiplDegat * Caster.MultiplDef);
                break;
            case TypeEffet.DegatsRetourSurAttaque:
                damageAmount += Mathf.FloorToInt(Pourcentage / 100f * caster.ForceAme);
                break;

            case TypeEffet.CancelPourcentageDamage:
                damageAmount += Mathf.FloorToInt(valueToChange - ((Pourcentage / 100) * valueToChange));
                break;
            case TypeEffet.DamageFaBuff:
                var nbBuffCaster = caster.ListBuffDebuff.Count(x => !x.IsDebuff);
                damageAmount += Mathf.FloorToInt(((Pourcentage / 100f) * caster.ForceAme) * nbBuffCaster);
                break;

            case TypeEffet.DamageDebuffCible:
                var nbDebuffCibleDamage = cible.ListBuffDebuff.Count(x => x.IsDebuff);
                var percentDamages = Pourcentage + (ValeurParBuffDebuff * nbDebuffCibleDamage);
                damageAmount += Mathf.FloorToInt(((percentDamages / 100f) * caster.ForceAme));
                break;
            //case TypeEffet.RemoveAllTensionProcDamage:
            //    ModifState.Tension = 0;
            //    break;
           

            case TypeEffet.RemoveAllDebuffProcDamage:
                nbProcDamage = RemoveBuffOrDebuffFromList(cible, true, false);
                damageAmount += valueToChange * nbProcDamage;
                break;
            case TypeEffet.RemoveAllDebuffSelfProcDamage:
                nbProcDamage = RemoveBuffOrDebuffFromList(cible, true, false);
                damageAmount += valueToChange * nbProcDamage;
                break;
            case TypeEffet.RemoveAllBuffProcDamage:
                nbProcDamage = RemoveBuffOrDebuffFromList(cible, false, false);
                damageAmount += valueToChange * nbProcDamage;
                break;
            case TypeEffet.Ponction:
                var amountPonction =
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * cible.Radiance) *
                                     caster.MultiplDegat); //checker le multipl degat

                damageAmount += -amountPonction;
                break;
            case TypeEffet.DamageAllEvenly:

                damageAmount +=
                    Mathf.FloorToInt((((((float)Pourcentage / NbEnnemies) / 100f) * NbAttaque) * caster.ForceAme) * caster.MultiplDegat);
                break;
            case TypeEffet.DegatsFaRadianceManquanteCible:
                damageAmount +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * caster.ForceAme) * (cible.Radiance * 100f / cible.RadianceMax));
                break;
            case TypeEffet.DegatsFaRadianceManquanteCaster:
                damageAmount +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * caster.ForceAme) * (caster.Radiance * 100f / caster.RadianceMax));
                break;
            case TypeEffet.DamageFaBuffCible:
                var nbBuff = cible.ListBuffDebuff.Count(x => !x.IsDebuff);
                var percentDamage = Pourcentage + (ValeurParBuffDebuff * nbBuff);
                damageAmount += Mathf.FloorToInt(((percentDamage / 100f) * caster.ForceAme));
                break;
            case TypeEffet.PonctionForceAme:
                var amountPonctionFA =
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * caster.ForceAme) *
                                     caster.MultiplDegat); //checker le multipl degat
                damageAmount += -amountPonctionFA;
                break;
            case TypeEffet.PremiereAttaqueJeanne:
                var JeanneStat = (EnnemiStat)caster;
                int percentageFa = 0;
                if (JeanneStat.Divin <= 25)
                {
                    percentageFa = 0;
                    nbProcAfterEffect = 1;
                }
                else if (JeanneStat.Divin > 25 && JeanneStat.Divin <= 50)
                {
                    percentageFa = 20;
                    nbProcAfterEffect = 2;
                }
                else if (JeanneStat.Divin > 50)
                {
                    percentageFa = 40;
                    nbProcAfterEffect = 4;
                }
                var totalPercentage = -(percentageFa + (Pourcentage * -1));
                damageAmount +=
                    Mathf.FloorToInt(((totalPercentage / 100f) * caster.ForceAme) * caster.MultiplDegat);
                //Application de Hérétique nb dépende du truc 
                break;
            case TypeEffet.DeuxiemeAttaqueJeanne:
                var JeanneStat2 = (EnnemiStat)caster;
                var divin = JeanneStat2.Divin > 0 ? JeanneStat2.Divin : JeanneStat2.Divin * -1;
                var TotalPercentage = -(divin + (Pourcentage * -1));
                damageAmount += Mathf.FloorToInt(((TotalPercentage / 100f) * caster.ForceAme) * caster.MultiplDegat);
                break;
            case TypeEffet.SupportJeanne:
                var JeanneStat3 = (EnnemiStat)caster;
                JeanneStat3.Divin -= 20;
                damageAmount += Mathf.FloorToInt((Pourcentage / 100f) * caster.RadianceMax);
                break;
            case TypeEffet.UltimeJeanne:
                var JeanneStat4 = (EnnemiStat)caster;
                damageAmount += Mathf.FloorToInt(-JeanneStat4.Divin / 100f * caster.ForceAme);
                JeanneStat4.Divin = -30;
                break;
            default:
                return false;
        }
        return true;

    }

    private CharacterStat ResultEffetBase(CharacterStat Caster, int LastDamageTaken = 0, CharacterStat Cible = null, int NbEnnemies = 1)
    {
        //Debug.Log($"Trigger Effect Base: {this.TypeEffet} from {Caster} to {Cible}");
        int valueToChange = ValeurBrut * NbAttaque;
        CharacterStat ModifState = ScriptableObject.CreateInstance("CharacterStat") as CharacterStat;
        int percent;
        int nbProcDamage;
        switch (this.TypeEffet)
        {
            case TypeEffet.DegatsForceAme:
                ModifState.Radiance +=
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme) * Caster.MultiplDegat);
                break;
            case TypeEffet.DegatsBrut:
                ModifState.Radiance += Mathf.FloorToInt(valueToChange * Caster.MultiplDegat);
                break;
            case TypeEffet.Conviction:
                ModifState.Conviction += ValeurBrut;
                break;
            case TypeEffet.AugmentationPourcentageFA:
                ModifState.ForceAme += (Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme));
                break;            
            case TypeEffet.AugmentationPourcentageFACible:
                ModifState.ForceAme += (Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Cible.ForceAme));
                break;
            case TypeEffet.AugmentationBrutFA:
                ModifState.ForceAme += valueToChange;
                break;
            case TypeEffet.RadianceMax:
                if (Cible == null)
                {

                    //var radianceModifier = Mathf.FloorToInt((Pourcentage / 100f) * Caster.RadianceMaxOriginal);
                    //var radianceActModifier = Mathf.FloorToInt((Pourcentage / 100f) * ((Caster.Radiance / Caster.RadianceMax) * Caster.RadianceMaxOriginal));
                    //ModifState.RadianceMax += radianceModifier;
                    //ModifState.Radiance += radianceActModifier;
                    int addAmount = Mathf.FloorToInt(Caster.RadianceMax * Pourcentage * .01f);
                    ModifState.RadianceMax += addAmount;
                    ModifState.Radiance += addAmount;
                }
                else
                {
                    //var radianceModifier = Mathf.FloorToInt((Pourcentage / 100f) * Cible.RadianceMaxOriginal);
                    //var radianceActModifier = Mathf.FloorToInt((Pourcentage / 100f) * ((Cible.Radiance / Cible.RadianceMax) * Cible.RadianceMaxOriginal));
                    //ModifState.RadianceMax += radianceModifier;
                    //ModifState.Radiance += radianceModifier;//radianceActModifier;
                    int addAmount = Mathf.FloorToInt(Cible.RadianceMax * Pourcentage * .01f);
                    ModifState.RadianceMax += addAmount;
                    ModifState.Radiance += addAmount;
                }
                break;
            case TypeEffet.AugmentFADernierDegatsSubi:
                ModifState.ForceAme += Mathf.FloorToInt((Pourcentage / 100f) * LastDamageTaken);
                break;
            case TypeEffet.Vitesse:
                ModifState.Vitesse += valueToChange;
                break;
            case TypeEffet.Resilience:
                ModifState._resilience += valueToChange;
                break;
            case TypeEffet.TensionStep:
                ModifState.PalierChangement += valueToChange;
                //ModifState.ValeurPalier += Cible.ValeurPalier;
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
                ModifState.Radiance +=
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme) * Caster.MultiplSoin);
                break;
            case TypeEffet.SoinFANbEnnemi:
                ModifState.Radiance += (Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme) *
                                        GameManager.Instance.BattleMan.EnemyScripts.Count);
                break;
            case TypeEffet.SoinRadianceMax:
                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Caster.RadianceMax);
                break;
            case TypeEffet.SoinRadianceActuelle:
                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Cible.Radiance);
                break;
            case TypeEffet.RandomAttaque:
                ModifState.Radiance +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * Random.Range(RandomX, RandomY + 1) * Caster.ForceAme));
                break;
            case TypeEffet.AugmentationFaRadianceActuelle:
                ModifState.ForceAme += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Caster.Radiance);
                break;
            case TypeEffet.ConsommeTensionAugmentationFA:
                ModifState.Tension += -Cible.Tension;
                var toAdd = AfterEffectToApply;
                toAdd.Effet.First().ValeurBrut = (int)Cible.Tension * ValeurBrut;
                GameManager.Instance.BattleMan.EnemyScripts.Find(c => c.Stat == Cible).AddDebuff(toAdd,toAdd.Decompte,toAdd.timerApplication);
                toAdd.Effet.First().ValeurBrut = 0;
                break;
            case TypeEffet.RemoveDebuff:
                var tempListRD = Cible.ListBuffDebuff.Where(c => c.IsDebuff).ToList();
                if(tempListRD.Count > 0)
                    tempListRD.RemoveAt(Random.Range(0, tempListRD.Count));
                break;
            case TypeEffet.AttaqueStackAmant:
                ModifState.Radiance +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * Cible.ListBuffDebuff.Count(c => c.Nom == "Amant")) *
                                     Caster.ForceAme);
                break;
            case TypeEffet.AttaqueFADebuff:
                var tempListAFAD = Cible.ListBuffDebuff.Where(c => c.IsDebuff).ToList();

                ModifState.Radiance +=
                    Mathf.FloorToInt(((Pourcentage + (ValeurBrut * tempListAFAD.Count()) / 100f) * NbAttaque) *
                                     Caster.ForceAme);
                break;
            case TypeEffet.GainResilienceIncrementale:
                ModifState._resilience += ValeurBrut * TimeAlive;
                TimeAlive++;
                break;
            case TypeEffet.DamageLastPhase:
                ModifState.Radiance += -GameManager.Instance.BattleMan.LastPhaseDamage;
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
                if (((float)Cible.Radiance / (float)Cible.RadianceMax) <= 0.25f)
                    percent *= 3;
                ModifState.Radiance +=
                    Mathf.FloorToInt((((percent / 100f) * NbAttaque) * Caster.ForceAme) * Caster.MultiplDegat);
                break;
            case TypeEffet.UntilDeath:
                ModifState.Radiance +=
                    -Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme) * Caster.MultiplDegat);
                if (ModifState.Radiance + Cible.Radiance > 0)
                {
                    Caster.Radiance -= Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Cible.ForceAme) * Cible.MultiplDegat * Caster.MultiplDef);
                }
                break;
            case TypeEffet.DegatsRetourSurAttaque:
                ModifState.Radiance += Mathf.FloorToInt(Pourcentage / 100f * Caster.ForceAme);
                break;

            case TypeEffet.BuffResilienceCoupRecu:
                ModifState._resilience += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque));
                break;
            case TypeEffet.CancelPourcentageDamage:
                ModifState.Radiance += Mathf.FloorToInt(valueToChange - ((Pourcentage / 100) * valueToChange));
                break;
            case TypeEffet.AugmentationFARadianceManquante:
                int faBonnus = Mathf.FloorToInt(ValeurBrut * ((1f - (Caster.Radiance*1f) / Caster.RadianceMax) * 100f) * NbAttaque);
                ModifState.ForceAme += faBonnus;
                Debug.Log($"Adding Mathf.FloorToInt({ValeurBrut} * ((1f - {Caster.Radiance} / {Caster.RadianceMax}) * 100f) * {NbAttaque}) = {faBonnus} FA");
                break;

            case TypeEffet.DamageFaBuff:
                var nbBuffCaster = Caster.ListBuffDebuff.Count(x => !x.IsDebuff);
                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage / 100f) * Caster.ForceAme) * nbBuffCaster);
                break;

            case TypeEffet.DamageDebuffCible:
                var nbDebuffCibleDamage = Cible.ListBuffDebuff.Count(x => x.IsDebuff);
                var percentDamages = Pourcentage + (ValeurParBuffDebuff*nbDebuffCibleDamage);
                ModifState.Radiance += Mathf.FloorToInt(((percentDamages / 100f) * Caster.ForceAme));
                break;
            //case TypeEffet.RemoveAllTensionProcDamage:
            //    ModifState.Tension = 0;
            //    break;
            case TypeEffet.RemoveAllTensionProcBuffDebuff:
                nbProcAfterEffect = Mathf.RoundToInt(Caster.Tension / Caster.ValeurPalier);
                Caster.Tension = 0;
                break;
            case TypeEffet.RemoveAllDebuffProcBuffDebuf:
                nbProcAfterEffect = RemoveBuffOrDebuffFromList(Caster, true);
                break;
            case TypeEffet.RemoveAllDebuffSelfProcBuffDebuf:
                nbProcAfterEffect = RemoveBuffOrDebuffFromList(Cible, true);
                break;
            case TypeEffet.RemoveAllBuffProcBuffDebuf:
                nbProcAfterEffect = RemoveBuffOrDebuffFromList(Caster, false);
                break;

            case TypeEffet.RemoveAllDebuffProcDamage:
                nbProcDamage = RemoveBuffOrDebuffFromList(Cible, true);
                ModifState.Radiance += valueToChange * nbProcDamage;
                break;
            case TypeEffet.RemoveAllDebuffSelfProcDamage:
                nbProcDamage = RemoveBuffOrDebuffFromList(Cible, true);
                ModifState.Radiance += valueToChange * nbProcDamage;
                break;
            case TypeEffet.RemoveAllBuffProcDamage:
                nbProcDamage = RemoveBuffOrDebuffFromList(Cible, false);
                ModifState.Radiance += valueToChange * nbProcDamage;
                break;
            case TypeEffet.GainFaBuffCible:
                var nbBuffCible = Cible.ListBuffDebuff.Count(x => !x.IsDebuff);
                Caster.ForceAme +=
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme) * nbBuffCible);
                break;
            case TypeEffet.GainFaDebuffCible:
                var nbDebuffCible = Cible.ListBuffDebuff.Count(x => x.IsDebuff);
                Caster.ForceAme +=
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme) * nbDebuffCible);
                break;
            case TypeEffet.Ponction:
                var amountPonction =
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Cible.Radiance) *
                                     Caster.MultiplDegat); //checker le multipl degat
                Caster.Radiance += amountPonction;
                if (Caster.Radiance > Caster.RadianceMax)
                    Caster.Radiance = Caster.RadianceMax;
                ModifState.Radiance += -amountPonction;
                break;
            case TypeEffet.DamageAllEvenly:

                ModifState.Radiance +=
                    Mathf.FloorToInt((((((float)Pourcentage / NbEnnemies) / 100f) * NbAttaque) * Caster.ForceAme) * Caster.MultiplDegat);
                break;
            case TypeEffet.OnKillStunAll:
                ModifState.isStun = true;
                break;
                case TypeEffet.DegatsFaRadianceManquanteCible:
                ModifState.Radiance +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * Caster.ForceAme) * (Cible.Radiance * 100f / Cible.RadianceMax));
                    break;
                case TypeEffet.DegatsFaRadianceManquanteCaster:
                ModifState.Radiance +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * Caster.ForceAme) * (Caster.Radiance * 100f / Caster.RadianceMax));
                    break;
                case TypeEffet.DamageFaBuffCible:
                    var nbBuff = Cible.ListBuffDebuff.Count(x => !x.IsDebuff);
                    var percentDamage = Pourcentage + (ValeurParBuffDebuff*nbBuff);
                    ModifState.Radiance += Mathf.FloorToInt(((percentDamage / 100f) * Caster.ForceAme));
                    break;
            case TypeEffet.PonctionForceAme:
                var amountPonctionFA =
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme) *
                                     Caster.MultiplDegat); //checker le multipl degat
                Caster.Radiance += amountPonctionFA;
                if (Caster.Radiance > Caster.RadianceMax)
                    Caster.Radiance = Caster.RadianceMax;
                ModifState.Radiance += -amountPonctionFA;
                break;
            case TypeEffet.PremiereAttaqueJeanne:
                var JeanneStat = (EnnemiStat) Caster;
                int percentageFa = 0;
                if (JeanneStat.Divin <= 25)
                {
                    percentageFa = 0;
                    nbProcAfterEffect = 1;
                }
                else if (JeanneStat.Divin > 25 && JeanneStat.Divin <= 50)
                {
                    percentageFa = 20;
                    nbProcAfterEffect = 2;
                }
                else if (JeanneStat.Divin > 50)
                {
                    percentageFa = 40;
                    nbProcAfterEffect = 4;
                }
                var totalPercentage = -(percentageFa + (Pourcentage * -1));
                ModifState.Radiance +=
                    Mathf.FloorToInt(((totalPercentage / 100f)  * Caster.ForceAme) * Caster.MultiplDegat);
                //Application de Hérétique nb dépende du truc 
                break;
            case TypeEffet.DeuxiemeAttaqueJeanne:
                var JeanneStat2 = (EnnemiStat)Caster;
                var divin = JeanneStat2.Divin > 0? JeanneStat2.Divin : JeanneStat2.Divin * -1;
                var TotalPercentage = -(divin + (Pourcentage * -1));
                ModifState.Radiance += Mathf.FloorToInt(((TotalPercentage / 100f) * Caster.ForceAme) * Caster.MultiplDegat);
                break;
            case TypeEffet.SupportJeanne:
                var JeanneStat3 = (EnnemiStat)Caster;
                JeanneStat3.Divin -= 20;
                ModifState.Radiance += Mathf.FloorToInt((Pourcentage / 100f) * Caster.RadianceMax);
                break;
            case TypeEffet.UltimeJeanne:
                var JeanneStat4 = (EnnemiStat) Caster;
                ModifState.Radiance += Mathf.FloorToInt(-JeanneStat4.Divin / 100f * Caster.ForceAme);
                JeanneStat4.Divin = -30;
                break;
            default:
                break;
        }

        modifstate = ModifState as JoueurStat;
        return ModifState;
    }

    private int RemoveBuffOrDebuffFromList(CharacterStat Cible, bool isDebuff, bool removeBuff=true)
    {
        int nbBuffDebuffRemoved = 0;
        if (Cible.ListBuffDebuff != null)
        {
            var tempListBuffDebuff = Cible.ListBuffDebuff.Where(x => x.IsDebuff == isDebuff);
            nbBuffDebuffRemoved = tempListBuffDebuff.Count();
            if (removeBuff)
            {
                foreach (var buffDebuff in tempListBuffDebuff)
                {
                    buffDebuff.Temps = -1;
                }
            }
        }
        return nbBuffDebuffRemoved;
    }
}