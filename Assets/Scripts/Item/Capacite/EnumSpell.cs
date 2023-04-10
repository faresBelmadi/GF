public enum Decompte
{
    phase,
    tour,
    combat,
    none
}

public enum TimerApplication
{
    DebutPhase,
    DebutTour,
    Attaque,
    Persistant
}
public enum TimerPassif
{
    DebutPhase,
    DebutTour,
    FinPhase,
    FinTour,
    ChangementStat,
    SourceDegats,
    PersonnageMeurt,
    AjoutBuffDebuff,
    SuppBuffDebuff
}

public enum Cible
{
    joueur,
    ennemi,
    allEnnemi,
    All,
    Ally,
    AllExceptSelf,
    MostDamage,
    LastAttacker,
    AllAllyExceptSelf
}

public enum SpellStatus
{
    locked,
    unlocked,
    bought,
}

public enum TypeCostSpell
{
    radiance,
    volonte,
    conscience
}

public enum TypeEffet
{
    DegatsForceAme,
    DegatsBrut,
    Clairvoyance,
    Colere,
    Conviction,
    AugmentationPourcentageFA,
    AugmentationBrutFA,
    RadianceMax,
    AugmentFADernierDegatsSubi,
    MultiplDegat,
    MultiplSoin,
    MultiplDef,
    Vitesse,
    Volonte,
    VolonteMax,
    Resilience,
    TensionStep,
    TensionValue,
    TensionGainAttaqueValue,
    TensionGainDebuffValue,
    TensionGainSoinValue,
    TensionGainDotValue,
    Conscience,
    ConscienceMax,
    Soin,
    SoinFA,
    SoinFANbEnnemi,
    SoinRadianceMax,
    SoinRadianceActuelle,
    DegatPVMax,
    RandomAttaque,
    AugmentationFaRadianceActuelle,
    ConsommeTensionAugmentationFA,
    RemoveDebuff,
    AttaqueStackAmant,
    AttaqueFADebuff,
    GainResilienceIncrementale,
    DamageLastPhase,
    NoEssence,
    DoubleBuffDebuff,
    AugmentationRadianceMaxPourcentage,
    //todo Github a fait un rollback ce qui nous a fait perdre tout ces effets dans le effet.cs
    BuffFaCoupRecu,
    BuffResilienceCoupRecu,
    ConsommeTensionDmgAllExceptCaster,
    Provocation,
    VolEssence,
    RandomChanceCastSpellSelf,
    SwapMostLeastBuffDebuff,
    RadianceRepartition,
    RandomAttaqueDebuff,
    DegatsRetourSurAttaque,
    RedirectionDegatsOnCasteur,
    CancelPourcentageDamage,
    RedirectionCancel,
    DispellBuffJoueurDamage,
    DispellDebuffCasterDamage,
    DamageAllEvenly,
    DamageUpTargetLowRadiance,
    OnKillStunAll,
    AugmentationFARadianceManquante,
    DamageFaBuff,
    RemoveAllTensionProcDamage,
    RemoveAllDebuffProcBuffDebuf,
    RemoveAllDebuffSelfProcBuffDebuf,
    RemoveAllBuffProcBuffDebuf,
    RemoveAllDebuffProcDamage, 
    RemoveAllDebuffSelfProcDamage, 
    RemoveAllBuffProcDamage,
    NoCapaPossible,
    ConsommeTensionReduitFa,
    AugmentationDegatsHitJoueur,
    GainFaBuffCible,
    GainFaDebuffCible,
    Ponction,
    DegatsBrutConsequence

}

public enum TypePassif
{
    None,
    PassifGuerrier1,
    PassifGuerrier2,
    PassifMartyr,
    PassifCameleon,
    PassifCultiste,
    PassifChefCultiste,
    PassiJeanne,
    PassifPapa,
    PassifPapy,
}
