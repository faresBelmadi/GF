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
    public int NbAttaque = 1;
    public int ValeurParBuffDebuff;
    private int TimeAlive = 1;
    public bool IsAttaqueEffet;
    public bool IsFirstApplication = true;
    public BuffDebuff AfterEffectToApply;

    [SerializeField]
    public JoueurStat modifstateOutput;

    [System.NonSerialized] public int nbProcAfterEffect;

    public JoueurStat ResultEffet(PlayerStatsHandler Caster, int LastDamageTake = 0, PlayerStatsHandler Cible = null)
    {
        //Debug.Log($"Trigger Effect: {this.TypeEffet} from {Caster} to {Cible}");
        int valueToChange = ValeurBrut * NbAttaque;
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        switch (this.TypeEffet)
        {
            case TypeEffet.Clairvoyance:
                ModifState.Clairvoyance += valueToChange;
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
        modifstateOutput = ModifState;
        return ModifState;
    }

    public JoueurStat ResultEffet(EnemyStatsHandler Caster, int LastDamageTaken = 0, PlayerStatsHandler Cible = null)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = ResultEffetCommun(Caster, LastDamageTaken, Cible);
        modifstateOutput = ModifState;
        return ModifState;
    }

    public JoueurStat ResultEffet(PlayerStatsHandler Caster, int LastDamageTaken, EnemyStatsHandler CibleEnnemi = null, int NbEnnemies = 1)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = ResultEffetCommun(Caster, LastDamageTaken, CibleEnnemi, NbEnnemies);
        modifstateOutput = ModifState;
        return ModifState;
    }
    public JoueurStat ResultEffet(EnemyStatsHandler Caster, int LastDamageTaken, EnemyStatsHandler CibleEnnemi = null, int NbEnnemies = 1)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = ResultEffetCommun(Caster, LastDamageTaken, CibleEnnemi, NbEnnemies);
        modifstateOutput = ModifState;
        return ModifState;
    }
    private JoueurStat ResultEffetCommun(AbstractStatsHandler Caster, int LastDamageTaken = 0, AbstractStatsHandler Cible = null, int NbEnnemies = 1)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = JoueurStat.CreateFromCharacter(ResultEffetBase(Caster, LastDamageTaken, Cible, NbEnnemies));
        modifstateOutput = ModifState;

        return ModifState;
    }

    public bool VisualizeAttack(AbstractStatsHandler caster, AbstractStatsHandler cible, out int damageAmount, out int returnedDamages, int NbEnnemies = 1)
    {
        int percent;
        int nbProcDamage;
        damageAmount = 0;
        returnedDamages = 0;
        int valueToChange = ValeurBrut * NbAttaque;
        switch (this.TypeEffet)
        {
            case TypeEffet.DegatsForceAme:
                damageAmount +=
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * caster.ForceDameTotal) * caster.MultiplDegat);
                break;
            case TypeEffet.DegatsBrut:
                damageAmount += Mathf.FloorToInt(valueToChange * caster .MultiplDegat);
                break;
            case TypeEffet.RadianceMax:
                int radianceMax = cible?.RadianceMaxTotal ?? caster.RadianceMaxTotal;
                int addAmount = (int)(radianceMax * Pourcentage * .01f);
                damageAmount = radianceMax + addAmount;
                break;
           
            case TypeEffet.DegatPVMax:
                damageAmount += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * cible.RadianceMaxTotal);
                break;
            case TypeEffet.Soin:
                damageAmount += valueToChange;
                break;
           
            case TypeEffet.RandomAttaque:
                damageAmount +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * Random.Range(RandomX, RandomY + 1) * caster.ForceDameTotal));
                break;
          
            case TypeEffet.AttaqueStackAmant:
                damageAmount +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * cible.ListBuffDebuff.Count(c => c.Nom == "Amant")) *
                                     caster.ForceDameTotal);
                break;
            case TypeEffet.AttaqueFADebuff:
                var tempListAFAD = cible.ListBuffDebuff.Where(c => c.IsDebuff).ToList();

                damageAmount +=
                    Mathf.FloorToInt(((Pourcentage + (ValeurBrut * tempListAFAD.Count()) / 100f) * NbAttaque) *
                                     caster.ForceDameTotal);
                break;
            case TypeEffet.DamageLastPhase:
                damageAmount += -GameManager.Instance.BattleMan.LastPhaseDamage;
                break;
            case TypeEffet.DegatsBrutConsequence:
                damageAmount += valueToChange;
                break;
            case TypeEffet.DamageUpTargetLowRadiance:
                percent = Pourcentage;
                if (((float)cible.Radiance / (float)cible.RadianceMaxTotal) <= 0.25f)
                    percent *= 3;
                damageAmount +=
                    Mathf.FloorToInt((((percent / 100f) * NbAttaque) * caster.ForceDameTotal) * caster.MultiplDegat);
                break;
            case TypeEffet.UntilDeath:
                damageAmount += Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * caster.ForceDameTotal) * caster.MultiplDegat);
                if (damageAmount + cible.Radiance > 0)
                {
                    returnedDamages += Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * cible.ForceDameTotal) * cible.MultiplDegat * caster.MultiplDef);
                }
                //if (damageAmount < Cible.Radiance)
                //    Caster.Radiance -= Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Cible.ForceAme) * Cible.MultiplDegat * Caster.MultiplDef);
                //returnedDamages
                break;
            case TypeEffet.DegatsRetourSurAttaque:
                damageAmount += Mathf.FloorToInt(Pourcentage / 100f * caster.ForceDameTotal);
                break;

            case TypeEffet.CancelPourcentageDamage:
                damageAmount += Mathf.FloorToInt(valueToChange - ((Pourcentage / 100) * valueToChange));
                break;
            case TypeEffet.DamageFaBuff:
                var nbBuffCaster = caster.ListBuffDebuff.Count(x => !x.IsDebuff);
                damageAmount += Mathf.FloorToInt(((Pourcentage / 100f) * caster.ForceDameTotal) * nbBuffCaster);
                break;

            case TypeEffet.DamageDebuffCible:
                var nbDebuffCibleDamage = cible.ListBuffDebuff.Count(x => x.IsDebuff);
                var percentDamages = Pourcentage + (ValeurParBuffDebuff * nbDebuffCibleDamage);
                damageAmount += Mathf.FloorToInt(((percentDamages / 100f) * caster.ForceDameTotal));
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
                    Mathf.FloorToInt((((((float)Pourcentage / NbEnnemies) / 100f) * NbAttaque) * caster.ForceDameTotal) * caster.MultiplDegat);
                break;
            case TypeEffet.DegatsFaRadianceManquanteCible:
                damageAmount +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * caster.ForceDameTotal) * (cible.Radiance * 100f / cible.RadianceMaxTotal));
                break;
            case TypeEffet.DegatsFaRadianceManquanteCaster:
                damageAmount +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * caster.ForceDameTotal) * (caster.Radiance * 100f / caster.RadianceMaxTotal));
                break;
            case TypeEffet.DamageFaBuffCible:
                var nbBuff = cible.ListBuffDebuff.Count(x => !x.IsDebuff);
                var percentDamage = Pourcentage + (ValeurParBuffDebuff * nbBuff);
                damageAmount += Mathf.FloorToInt(((percentDamage / 100f) * caster.ForceDameTotal));
                break;
            case TypeEffet.PonctionForceAme:
                var amountPonctionFA =
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * caster.ForceDameTotal) *
                                     caster.MultiplDegat); //checker le multipl degat
                damageAmount += -amountPonctionFA;
                break;
            case TypeEffet.PremiereAttaqueJeanne:
                var JeanneStat = (EnemyStatsHandler)caster;
                int percentageFa = 0;
                if (JeanneStat.CustomStat <= 25)
                {
                    percentageFa = 0;
                    nbProcAfterEffect = 1;
                }
                else if (JeanneStat.CustomStat > 25 && JeanneStat.CustomStat <= 50)
                {
                    percentageFa = 20;
                    nbProcAfterEffect = 2;
                }
                else if (JeanneStat.CustomStat > 50)
                {
                    percentageFa = 40;
                    nbProcAfterEffect = 4;
                }
                var totalPercentage = -(percentageFa + (Pourcentage * -1));
                damageAmount +=
                    Mathf.FloorToInt(((totalPercentage / 100f) * caster.ForceDameTotal) * caster.MultiplDegat);
                //Application de Hérétique nb dépende du truc 
                break;
            case TypeEffet.DeuxiemeAttaqueJeanne:
                var JeanneStat2 = (EnemyStatsHandler)caster;
                var divin = JeanneStat2.CustomStat > 0 ? JeanneStat2.CustomStat : JeanneStat2.CustomStat * -1;
                var TotalPercentage = -(divin + (Pourcentage * -1));
                damageAmount += Mathf.FloorToInt(((TotalPercentage / 100f) * caster.ForceDameTotal) * caster.MultiplDegat);
                break;
            case TypeEffet.SupportJeanne:
                var JeanneStat3 = (EnemyStatsHandler)caster;
                JeanneStat3.CustomStat -= 20;
                damageAmount += Mathf.FloorToInt((Pourcentage / 100f) * caster.RadianceMaxTotal);
                break;
            case TypeEffet.UltimeJeanne:
                var JeanneStat4 = (EnemyStatsHandler)caster;
                damageAmount += Mathf.FloorToInt(-JeanneStat4.CustomStat / 100f * caster.ForceDameTotal);
                JeanneStat4.CustomStat = -30;
                break;
            default:
                return false;
        }
        return true;

    }

    private CharacterStat ResultEffetBase(AbstractStatsHandler Caster, int LastDamageTaken = 0, AbstractStatsHandler Cible = null, int NbEnnemies = 1)
    {
        Debug.Log($"Trigger Effect Base: {this.name} - {this.TypeEffet} from {Caster} to {Cible}");
        int valueToChange = ValeurBrut * NbAttaque;
        CharacterStat ModifState = ScriptableObject.CreateInstance("CharacterStat") as CharacterStat;
        int percent;
        int nbProcDamage;
        switch (this.TypeEffet)
        {
            case TypeEffet.DegatsForceAme:
                ModifState.Radiance +=
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceDameTotal) * Caster.MultiplDegat);
                break;
            case TypeEffet.DegatsBrut:
                ModifState.Radiance += Mathf.FloorToInt(valueToChange * Caster.MultiplDegat);
                break;
            case TypeEffet.Conviction:
                ModifState.Conviction += ValeurBrut;
                break;
            case TypeEffet.AugmentationPourcentageFACaster:
                ModifState.ForceAme += (Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Caster.ForceDameTotal));
                break;            
            case TypeEffet.AugmentationPourcentageFACible:
                ModifState.ForceAme += (Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Cible.ForceDameTotal));
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
                    int radAmount = Caster.GetPercentValue(StatEnum.Radiance, Pourcentage);
                    int radMaxAmount = Caster.GetPercentValue(StatEnum.RadianceMax, Pourcentage);
                    //int addAmount = Mathf.FloorToInt(Caster.RadianceMaxTotal * Pourcentage * .01f);
                    ModifState.RadianceMax += radMaxAmount;
                    ModifState.Radiance += radAmount;
                }
                else
                {
                    //var radianceModifier = Mathf.FloorToInt((Pourcentage / 100f) * Cible.RadianceMaxOriginal);
                    //var radianceActModifier = Mathf.FloorToInt((Pourcentage / 100f) * ((Cible.Radiance / Cible.RadianceMax) * Cible.RadianceMaxOriginal));
                    //ModifState.RadianceMax += radianceModifier;
                    //ModifState.Radiance += radianceModifier;//radianceActModifier;
                    float radianceProportion = (float)Cible.Radiance / (float)Cible.RadianceMaxTotal;
                    int radAmount = Cible.GetPercentValue(StatEnum.Radiance, Pourcentage);
                    int radMaxAmount = Cible.GetPercentValue(StatEnum.RadianceMax, Pourcentage);
                    int rad = (int) ((Cible.RadianceMaxTotal + radMaxAmount) * radianceProportion);
                    ModifState.RadianceMax += radMaxAmount;
                    Cible.SetRadiance(rad);
                    //ModifState.Radiance += radAmount;
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
            //case TypeEffet.TensionGainAttaqueValue:
            //    ModifState.TensionAttaque += valueToChange;
            //    break;
            //case TypeEffet.TensionGainDebuffValue:
            //    ModifState.TensionDebuff += valueToChange;
            //    break;
            //case TypeEffet.TensionGainSoinValue:
            //    ModifState.TensionSoin += valueToChange;
            //    break;
            //case TypeEffet.TensionGainDotValue:
            //    ModifState.TensionDot += valueToChange;
            //    break;
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
                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Cible.RadianceMaxTotal);
                break;
            case TypeEffet.Soin:
                ModifState.Radiance += valueToChange;
                break;
            case TypeEffet.SoinFA:
                ModifState.Radiance +=
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceDameTotal) * Caster.MultiplSoin);
                break;
            case TypeEffet.SoinFANbEnnemi:
                ModifState.Radiance += (Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Caster.ForceDameTotal) *
                                        GameManager.Instance.BattleMan.EnemyScripts.Count);
                break;
            case TypeEffet.SoinRadianceMax:
                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Caster.RadianceMaxTotal);
                break;
            case TypeEffet.SoinRadianceActuelle:
                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Cible.Radiance);
                break;
            case TypeEffet.RandomAttaque:
                ModifState.Radiance +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * Random.Range(RandomX, RandomY + 1) * Caster.ForceDameTotal));
                break;
            case TypeEffet.AugmentationFaRadianceActuelle:
                ModifState.ForceAme += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Caster.Radiance);
                break;
            case TypeEffet.ConsommeTensionAugmentationFA:
                ModifState.Tension += -Cible.Tension;
                var toAdd = AfterEffectToApply;
                toAdd.Effet.First().NbAttaque = 1;
                toAdd.Effet.First().Pourcentage = (int)Cible.Tension * ValeurBrut;
                GameManager.Instance.BattleMan.EnemyScripts.Find(c => c.Stat == Cible).AddDebuff(toAdd,toAdd.timerApplication);
                toAdd.Effet.First().Pourcentage = 0;
                toAdd.Effet.First().NbAttaque = 0;
                break;
            case TypeEffet.RemoveDebuff:
                var tempListRD = Cible.ListBuffDebuff.Where(c => c.IsDebuff).ToList();
                if(tempListRD.Count > 0)
                    tempListRD.RemoveAt(Random.Range(0, tempListRD.Count));
                break;
            case TypeEffet.AttaqueStackAmant:
                ModifState.Radiance +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * Cible.ListBuffDebuff.Count(c => c.Nom == "Amant")) *
                                     Caster.ForceDameTotal);
                break;
            case TypeEffet.AttaqueFADebuff:
                var tempListAFAD = Cible.ListBuffDebuff.Where(c => c.IsDebuff).ToList();

                ModifState.Radiance +=
                    Mathf.FloorToInt(((Pourcentage + (ValeurBrut * tempListAFAD.Count()) / 100f) * NbAttaque) *
                                     Caster.ForceDameTotal);
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
                ModifState.RadianceMax += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque) * Cible.RadianceMaxTotal);
                break;
            case TypeEffet.BuffFaCoupRecu:
                ModifState.ForceAme += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque));
                break;
            case TypeEffet.DegatsBrutConsequence:
                ModifState.Radiance += valueToChange;
                break;
            case TypeEffet.DamageUpTargetLowRadiance:
                percent = Pourcentage;
                if (((float)Cible.Radiance / (float)Cible.RadianceMaxTotal) <= 0.25f)
                    percent *= 3;
                ModifState.Radiance +=
                    Mathf.FloorToInt((((percent / 100f) * NbAttaque) * Caster.ForceDameTotal) * Caster.MultiplDegat);

                if(-ModifState.Radiance  >= (float)Cible.Radiance)
                    foreach (var item in GameManager.Instance.BattleMan.EnemyScripts)
                    {
                        item.AddDebuff(AfterEffectToApply, AfterEffectToApply.timerApplication);
                    }
                break;
            case TypeEffet.UntilDeath:
                ModifState.Radiance += Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceDameTotal) * Caster.MultiplDegat);
                if (ModifState.Radiance + Cible.Radiance > 0)
                {
                    Caster.ChangeRadiance(Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Cible.ForceDameTotal) * Cible.MultiplDegat * Caster.MultiplDef));
                    /*Caster.Radiance += Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Cible.ForceAme) * Cible.MultiplDegat * Caster.MultiplDef);*/
                }
                break;
            case TypeEffet.DegatsRetourSurAttaque:
                ModifState.Radiance += Mathf.FloorToInt(Pourcentage / 100f * Caster.ForceDameTotal);
                break;

            case TypeEffet.BuffResilienceCoupRecu:
                ModifState._resilience += Mathf.FloorToInt(((Pourcentage / 100f) * NbAttaque));
                break;
            case TypeEffet.CancelPourcentageDamage:
                ModifState.Radiance += Mathf.FloorToInt(valueToChange - ((Pourcentage / 100) * valueToChange));
                break;
            case TypeEffet.AugmentationFARadianceManquante:
                int faBonnus = Mathf.FloorToInt(ValeurBrut * ((1f - (Caster.Radiance*1f) / Caster.RadianceMaxTotal) * 100f) * NbAttaque);
                ModifState.ForceAme += faBonnus;
                Debug.Log($"Adding Mathf.FloorToInt({ValeurBrut} * ((1f - {Caster.Radiance} / {Caster.RadianceMaxTotal}) * 100f) * {NbAttaque}) = {faBonnus} FA");
                break;

            case TypeEffet.DamageFaBuff:
                var nbBuffCaster = Caster.ListBuffDebuff.Count(x => !x.IsDebuff);
                ModifState.Radiance += Mathf.FloorToInt(((Pourcentage / 100f) * Caster.ForceDameTotal) * nbBuffCaster);
                break;

            case TypeEffet.DamageDebuffCible:
                var nbDebuffCibleDamage = Cible.ListBuffDebuff.Count(x => x.IsDebuff);
                var percentDamages = Pourcentage + (ValeurParBuffDebuff*nbDebuffCibleDamage);
                ModifState.Radiance += Mathf.FloorToInt(((percentDamages / 100f) * Caster.ForceDameTotal));
                break;
            //case TypeEffet.RemoveAllTensionProcDamage:
            //    ModifState.Tension = 0;
            //    break;
            case TypeEffet.RemoveAllTensionProcBuffDebuff:
                nbProcAfterEffect = Mathf.RoundToInt(Caster.Tension / Caster.ValeurPalier);
                Caster.ChangeTension(-Caster.Tension);
                /*Caster.Tension = 0;*/
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
                Caster.ChangeForceDame(Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceDameTotal) * nbBuffCible));
                /*Caster.ForceAme +=
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme) * nbBuffCible);*/
                break;
            case TypeEffet.GainFaDebuffCible:
                var nbDebuffCible = Cible.ListBuffDebuff.Count(x => x.IsDebuff);
                Caster.ChangeForceDame(Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceDameTotal) * nbDebuffCible));
                /*Caster.ForceAme +=
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceAme) * nbDebuffCible);*/
                break;
            case TypeEffet.Ponction:
                var amountPonction =
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Cible.Radiance) *
                                     Caster.MultiplDegat); //checker le multipl degat
                Caster.ChangeRadiance(amountPonction);
                ModifState.Radiance += -amountPonction;
                break;
            case TypeEffet.DamageAllEvenly:

                ModifState.Radiance +=
                    Mathf.FloorToInt((((((float)Pourcentage / NbEnnemies) / 100f) * NbAttaque) * Caster.ForceDameTotal) * Caster.MultiplDegat);
                break;
            case TypeEffet.OnKillStunAll:
                ModifState.isStun = true;
                break;
                case TypeEffet.DegatsFaRadianceManquanteCible:
                ModifState.Radiance +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * Caster.ForceDameTotal) * (Cible.Radiance * 100f / Cible.RadianceMaxTotal));
                    break;
                case TypeEffet.DegatsFaRadianceManquanteCaster:
                ModifState.Radiance +=
                    Mathf.FloorToInt(((Pourcentage / 100f) * Caster.ForceDameTotal) * (Caster.Radiance * 100f / Caster.RadianceMaxTotal));
                    break;
                case TypeEffet.DamageFaBuffCible:
                    var nbBuff = Cible.ListBuffDebuff.Count(x => !x.IsDebuff);
                    var percentDamage = Pourcentage + (ValeurParBuffDebuff*nbBuff);
                    ModifState.Radiance += Mathf.FloorToInt(((percentDamage / 100f) * Caster.ForceDameTotal));
                    break;
            case TypeEffet.PonctionForceAme:
                var amountPonctionFA =
                    Mathf.FloorToInt((((Pourcentage / 100f) * NbAttaque) * Caster.ForceDameTotal) *
                                     Caster.MultiplDegat); //checker le multipl degat
                Caster.ChangeRadiance(amountPonctionFA);
                ModifState.Radiance += -amountPonctionFA;
                break;
            case TypeEffet.PremiereAttaqueJeanne:
                var JeanneStat = (EnemyStatsHandler) Caster;
                int percentageFa = 0;
                if (JeanneStat.CustomStat <= 25)
                {
                    percentageFa = 0;
                    nbProcAfterEffect = 1;
                }
                else if (JeanneStat.CustomStat > 25 && JeanneStat.CustomStat <= 50)
                {
                    percentageFa = 20;
                    nbProcAfterEffect = 2;
                }
                else if (JeanneStat.CustomStat > 50)
                {
                    percentageFa = 40;
                    nbProcAfterEffect = 4;
                }
                var totalPercentage = -(percentageFa + (Pourcentage * -1));
                ModifState.Radiance +=
                    Mathf.FloorToInt(((totalPercentage / 100f)  * Caster.ForceDameTotal) * Caster.MultiplDegat);
                //Application de Hérétique nb dépende du truc 
                break;
            case TypeEffet.DeuxiemeAttaqueJeanne:
                var JeanneStat2 = (EnemyStatsHandler)Caster;
                var divin = JeanneStat2.CustomStat > 0? JeanneStat2.CustomStat : JeanneStat2.CustomStat * -1;
                var TotalPercentage = -(divin + (Pourcentage * -1));
                ModifState.Radiance += Mathf.FloorToInt(((TotalPercentage / 100f) * Caster.ForceDameTotal) * Caster.MultiplDegat);
                break;
            case TypeEffet.SupportJeanne:
                var JeanneStat3 = (EnemyStatsHandler)Caster;
                JeanneStat3.CustomStat -= 20;
                ModifState.Radiance += Mathf.FloorToInt((Pourcentage / 100f) * Caster.RadianceMaxTotal);
                break;
            case TypeEffet.UltimeJeanne:
                var JeanneStat4 = (EnemyStatsHandler) Caster;
                ModifState.Radiance += Mathf.FloorToInt(((JeanneStat4.CustomStat * Pourcentage) / 100f * Caster.ForceDameTotal) * Caster.MultiplDegat);
                JeanneStat4.CustomStat = -30;
                break;
            case TypeEffet.MultiplTension:
                ModifState.MultipleTension += (Pourcentage / 100f) * NbAttaque;
                break;
            default:
                break;
        }

        modifstateOutput = ModifState as JoueurStat;
        return ModifState;
    }

    private int RemoveBuffOrDebuffFromList(AbstractStatsHandler Cible, bool isDebuff, bool removeBuff=true)
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
    public Sprite GetSpriteOfEffect()
    {

        switch (TypeEffet)
        {
            case TypeEffet.AugmentationBrutFA:
            case TypeEffet.AttaqueFADebuff:
                if ((Cible == Cible.joueur && ValeurBrut < 0) || (Cible != Cible.joueur && ValeurBrut > 0))
                {
                    return GameManager.Instance.StatIcons.StatForceDameDown;
                }
                else
                {
                    return GameManager.Instance.StatIcons.StatForceDameUp;
                }
                ;
            case TypeEffet.AugmentationPourcentageFACible:
            case TypeEffet.AugmentationPourcentageFACaster:
                if ((Cible == Cible.joueur && Pourcentage < 0) || (Cible != Cible.joueur && Pourcentage > 0))
                {
                    return GameManager.Instance.StatIcons.StatForceDameDown;
                }
                else
                {
                    return GameManager.Instance.StatIcons.StatForceDameUp;
                }
            case TypeEffet.RadianceMax:
                if ((Cible == Cible.joueur && ValeurBrut < 0) || (Cible != Cible.joueur && ValeurBrut > 0))
                {
                     return GameManager.Instance.StatIcons.StatRadianceDown;
                }
                else
                {
                    return GameManager.Instance.StatIcons.StatRadianceUp;
                }
            case TypeEffet.Resilience:
                if ((Cible == Cible.joueur && ValeurBrut < 0) || (Cible != Cible.joueur && ValeurBrut > 0))
                {
                    return GameManager.Instance.StatIcons.StatRadianceDown;
                }
                else
                {
                    return GameManager.Instance.StatIcons.StatResilienceUp;
                }
            case TypeEffet.Clairvoyance:
                if ((Cible == Cible.joueur && ValeurBrut < 0) || (Cible != Cible.joueur && ValeurBrut > 0))
                {
                    return GameManager.Instance.StatIcons.StatClairvoyanceDown;
                }
                else
                {
                    return GameManager.Instance.StatIcons.StatClairvoyanceUp;
                }
            case TypeEffet.Vitesse:
                if ((Cible == Cible.joueur && ValeurBrut < 0) || (Cible != Cible.joueur && ValeurBrut > 0))
                {
                    return GameManager.Instance.StatIcons.StatVitesseDown;
                }
                else
                {
                    return GameManager.Instance.StatIcons.StatVitesseUp;
                }
            case TypeEffet.Conviction:
                if ((Cible == Cible.joueur && ValeurBrut < 0) || (Cible != Cible.joueur && ValeurBrut > 0))
                {
                    return GameManager.Instance.StatIcons.StatConvictionDown;
                }
                else
                {
                    return GameManager.Instance.StatIcons.StatConvictionUp;
                }
            case TypeEffet.Conscience:
            case TypeEffet.ConscienceMax:
                if ((Cible == Cible.joueur && ValeurBrut < 0) || (Cible != Cible.joueur && ValeurBrut > 0))
                {
                    return GameManager.Instance.StatIcons.StatConscienceDown;
                }
                else
                {
                    return GameManager.Instance.StatIcons.StatConscienceUp;
                }
            case TypeEffet.DegatsBrutConsequence:
                if ((Cible == Cible.joueur && ValeurBrut < 0) || (Cible != Cible.joueur && ValeurBrut > 0))
                {
                    return GameManager.Instance.StatIcons.Damage;
                }
                else
                    break;
            case TypeEffet.Volonte:
            case TypeEffet.VolonteMax:
                if ((Cible == Cible.joueur && ValeurBrut < 0) || (Cible != Cible.joueur && ValeurBrut > 0))
                {
                    return GameManager.Instance.StatIcons.StatVolonteDown;
                }
                else
                {
                    return GameManager.Instance.StatIcons.StatVolonteUp;
                }
            case TypeEffet.TensionStep:
            case TypeEffet.TensionValue:
            case TypeEffet.TensionGainAttaqueValue:
            case TypeEffet.TensionGainDebuffValue:
            case TypeEffet.TensionGainSoinValue:
            case TypeEffet.TensionGainDotValue:
                if ((Cible == Cible.joueur && ValeurBrut < 0) || (Cible != Cible.joueur && ValeurBrut > 0))
                {
                    return GameManager.Instance.StatIcons.StatTensionDown;
                }
                else
                {
                    return GameManager.Instance.StatIcons.StatTensionUp;
                }
            case TypeEffet.DegatsForceAme:
                return GameManager.Instance.StatIcons.Damage;
            case TypeEffet.MultiplDegat:
                if ((Cible == Cible.joueur && Pourcentage < 0) || (Cible != Cible.joueur && Pourcentage > 0))
                {
                    return GameManager.Instance.StatIcons.DecreaseAtk;
                }
                else
                {
                    return GameManager.Instance.StatIcons.IncreaseAtk;
                }
            case TypeEffet.MultiplSoin:
                if ((Cible == Cible.joueur && Pourcentage < 0) || (Cible != Cible.joueur && Pourcentage > 0))
                {
                    return GameManager.Instance.StatIcons.DecreaseHeal;
                }
                else
                {
                    return GameManager.Instance.StatIcons.IncreaseHeal;
                }
            case TypeEffet.MultiplDef:
                if ((Cible == Cible.joueur && Pourcentage < 0) || (Cible != Cible.joueur && Pourcentage > 0))
                {
                    return GameManager.Instance.StatIcons.DecreaseDef;
                }
                else
                {
                    return GameManager.Instance.StatIcons.IncreaseDef;
                }
            case TypeEffet.Colere:
                if (Cible == Cible.joueur)
                {
                    return GameManager.Instance.StatIcons.WrathDown;
                }
                else
                {
                    return GameManager.Instance.StatIcons.WrathUp;
                }
            case TypeEffet.AddPassiveStack:
                return GameManager.Instance.StatIcons.Divin;
            case TypeEffet.DegatPVMax:
            case TypeEffet.DegatsBrut:
            case TypeEffet.DegatsRetourSurAttaque:
            case TypeEffet.DamageAllEvenly:
            case TypeEffet.DamageUpTargetLowRadiance:
            case TypeEffet.DamageFaBuff:
            case TypeEffet.DamageFaBuffCible:
            case TypeEffet.DamageDebuffCible:
            case TypeEffet.Ponction:
            case TypeEffet.PonctionForceAme:
            case TypeEffet.DegatsFaRadianceManquanteCible:
            case TypeEffet.DegatsFaRadianceManquanteCaster:
                return GameManager.Instance.StatIcons.Damage;
            case TypeEffet.Soin:
            case TypeEffet.SoinFA:
            case TypeEffet.SoinFANbEnnemi:
            case TypeEffet.SoinRadianceMax:
            case TypeEffet.SoinRadianceActuelle:
                return GameManager.Instance.StatIcons.IncreaseHeal;
            default:
                Debug.Log($"Effet non géré : {TypeEffet})");
                break;
        }
        return null;
    }
    public string GetTargetStat()
    {
        return TypeEffet switch
        {
            TypeEffet.AugmentationBrutFA => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.DegatsForceAme => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.DegatsBrut => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.Clairvoyance => GameManager.Instance.CommonNameData.Clairvoyance,
            TypeEffet.Colere => GameManager.Instance.CommonDescData.IdTradColere,
            TypeEffet.Conviction => GameManager.Instance.CommonNameData.Conviction,
            TypeEffet.AugmentationPourcentageFACaster => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.RadianceMax => GameManager.Instance.CommonNameData.Radiance,
            TypeEffet.AugmentFADernierDegatsSubi => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.AugmentationPourcentageFACible => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.MultiplDegat => GameManager.Instance.CommonDescData.IdTradBuffMultAtt,
            TypeEffet.MultiplSoin => GameManager.Instance.CommonDescData.IdTradBuffMultHeal,
            TypeEffet.MultiplDef => GameManager.Instance.CommonDescData.IdTradBuffMultDef,
            TypeEffet.Vitesse => GameManager.Instance.CommonNameData.Vitesse,
            TypeEffet.Volonte => GameManager.Instance.CommonNameData.Volonte,
            TypeEffet.VolonteMax => GameManager.Instance.CommonNameData.Volonte,
            TypeEffet.Resilience => GameManager.Instance.CommonNameData.Resilience,
            TypeEffet.TensionStep => GameManager.Instance.CommonNameData.Tension,
            TypeEffet.TensionValue => GameManager.Instance.CommonNameData.Tension,
            TypeEffet.TensionGainAttaqueValue => GameManager.Instance.CommonNameData.Tension,
            TypeEffet.TensionGainDebuffValue => GameManager.Instance.CommonNameData.Tension,
            TypeEffet.TensionGainSoinValue => GameManager.Instance.CommonNameData.Tension,
            TypeEffet.TensionGainDotValue => GameManager.Instance.CommonNameData.Tension,
            TypeEffet.Conscience => GameManager.Instance.CommonNameData.Conscience,
            TypeEffet.ConscienceMax => GameManager.Instance.CommonNameData.Conscience,
            TypeEffet.Soin => GameManager.Instance.CommonNameData.Radiance,
            TypeEffet.SoinFA => GameManager.Instance.CommonNameData.Radiance,
            TypeEffet.SoinFANbEnnemi => GameManager.Instance.CommonNameData.Radiance,
            TypeEffet.SoinRadianceMax => GameManager.Instance.CommonNameData.Radiance,
            TypeEffet.SoinRadianceActuelle => GameManager.Instance.CommonNameData.Radiance,
            TypeEffet.DegatPVMax => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.RandomAttaque => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.AugmentationFaRadianceActuelle => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.ConsommeTensionAugmentationFA => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.RemoveDebuff => GameManager.Instance.CommonDescData.IdTradBuff,
            TypeEffet.AttaqueStackAmant => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.AttaqueFADebuff => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.GainResilienceIncrementale => GameManager.Instance.CommonNameData.Resilience,
            TypeEffet.DamageLastPhase => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.NoEssence => GameManager.Instance.CommonNameData.Essence,
            TypeEffet.DoubleBuffDebuff => throw new System.NotImplementedException(),
            TypeEffet.AugmentationRadianceMaxPourcentage => throw new System.NotImplementedException(),
            TypeEffet.BuffFaCoupRecu => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.BuffResilienceCoupRecu => GameManager.Instance.CommonNameData.Resilience,
            TypeEffet.ConsommeTensionDmgAllExceptCaster => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.Provocation => throw new System.NotImplementedException(),
            TypeEffet.VolEssence => GameManager.Instance.CommonNameData.Essence,
            TypeEffet.RandomChanceCastSpellSelf => throw new System.NotImplementedException(),
            TypeEffet.SwapMostLeastBuffDebuff => throw new System.NotImplementedException(),
            TypeEffet.RadianceRepartition => GameManager.Instance.CommonNameData.Radiance,
            TypeEffet.RandomAttaqueDebuff => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.DegatsRetourSurAttaque => throw new System.NotImplementedException(),
            TypeEffet.RedirectionDegatsOnCasteur => throw new System.NotImplementedException(),
            TypeEffet.CancelPourcentageDamage => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.RedirectionCancel => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.DispellBuffJoueurDamage => GameManager.Instance.CommonDescData.IdTradBuff,
            TypeEffet.DispellDebuffCasterDamage => GameManager.Instance.CommonDescData.IdTradDebuff,
            TypeEffet.DamageAllEvenly => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.DamageUpTargetLowRadiance => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.OnKillStunAll => throw new System.NotImplementedException(),
            TypeEffet.UntilDeath => throw new System.NotImplementedException(),
            TypeEffet.AugmentationFARadianceManquante => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.DamageFaBuff => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.DamageFaBuffCible => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.DamageDebuffCible => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.RemoveAllTensionProcDamage => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.RemoveAllTensionProcBuffDebuff => GameManager.Instance.CommonDescData.IdTradBuff,
            TypeEffet.RemoveAllDebuffProcBuffDebuf => GameManager.Instance.CommonDescData.IdTradDebuff,
            TypeEffet.RemoveAllDebuffSelfProcBuffDebuf => throw new System.NotImplementedException(),
            TypeEffet.RemoveAllBuffProcBuffDebuf => throw new System.NotImplementedException(),
            TypeEffet.RemoveAllDebuffProcDamage => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.RemoveAllDebuffSelfProcDamage => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.RemoveAllBuffProcDamage => throw new System.NotImplementedException(),
            TypeEffet.NoCapaPossible => throw new System.NotImplementedException(),
            TypeEffet.ConsommeTensionReduitFa => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.AugmentationDegatsHitJoueur => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.GainFaBuffCible => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.GainFaDebuffCible => GameManager.Instance.CommonNameData.ForceDame,
            TypeEffet.Ponction => GameManager.Instance.CommonNameData.Radiance,
            TypeEffet.PonctionForceAme => GameManager.Instance.CommonNameData.Radiance,
            TypeEffet.DegatsBrutConsequence => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.DegatsFaRadianceManquanteCible => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.DegatsFaRadianceManquanteCaster => GameManager.Instance.CommonDescData.IdTradDegat,
            TypeEffet.PremiereAttaqueJeanne => throw new System.NotImplementedException(),
            TypeEffet.DeuxiemeAttaqueJeanne => throw new System.NotImplementedException(),
            TypeEffet.SupportJeanne => throw new System.NotImplementedException(),
            TypeEffet.UltimeJeanne => throw new System.NotImplementedException(),
            TypeEffet.MultiplTension => GameManager.Instance.CommonNameData.Tension,
            TypeEffet.AddPassiveStack => GameManager.Instance.CommonNameData.Passif,
            _ => throw new System.NotImplementedException(),
        };
    }
}