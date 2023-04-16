﻿public enum Decompte
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
    FinAction,
    FinCombat,
    Death
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
    AllAllyExceptSelf,
    Self
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
    DegatsForceAme=0,
    DegatsBrut=1,
    Clairvoyance=2,
    Colere=3,
    Conviction=4,
    AugmentationPourcentageFA=5,
    AugmentationBrutFA=6,
    RadianceMax=7,
    AugmentFADernierDegatsSubi=8,
    MultiplDegat=9,
    MultiplSoin=10,
    MultiplDef=11,
    Vitesse=12,
    Volonte=13,
    VolonteMax=14,
    Resilience=15,
    TensionStep=16,
    TensionValue=17,
    TensionGainAttaqueValue=18,
    TensionGainDebuffValue=19,
    TensionGainSoinValue=20,
    TensionGainDotValue=21,
    Conscience=22,
    ConscienceMax=23,
    Soin=24,
    SoinFA=25,
    SoinFANbEnnemi=26,
    SoinRadianceMax=27,
    SoinRadianceActuelle=28,
    DegatPVMax=29,
    RandomAttaque=30,
    AugmentationFaRadianceActuelle=31,
    ConsommeTensionAugmentationFA=32,
    RemoveDebuff=33,
    AttaqueStackAmant=34,
    AttaqueFADebuff=35,
    GainResilienceIncrementale=36,
    DamageLastPhase=37,
    NoEssence=38,
    DoubleBuffDebuff=39,
    AugmentationRadianceMaxPourcentage=40,
    //todo Github a fait un rollback ce qui nous a fait perdre tout ces effets dans le effet.cs
    BuffFaCoupRecu=41,
    BuffResilienceCoupRecu=42,
    ConsommeTensionDmgAllExceptCaster=43,
    Provocation=44,
    VolEssence=45,
    RandomChanceCastSpellSelf=46,
    SwapMostLeastBuffDebuff=47,
    RadianceRepartition=48,
    RandomAttaqueDebuff=49,
    DegatsRetourSurAttaque=50,
    RedirectionDegatsOnCasteur=51,
    CancelPourcentageDamage=52,
    RedirectionCancel=53,
    DispellBuffJoueurDamage=54,
    DispellDebuffCasterDamage=55,
    DamageAllEvenly=56,
    DamageUpTargetLowRadiance=57,
    OnKillStunAll=58,
    AugmentationFARadianceManquante=59,
    DamageFaBuff=60,
    RemoveAllTensionProcDamage=61,
    RemoveAllDebuffProcBuffDebuf=62,
    RemoveAllDebuffSelfProcBuffDebuf=63,
    RemoveAllBuffProcBuffDebuf=64,
    RemoveAllDebuffProcDamage=65, 
    RemoveAllDebuffSelfProcDamage=66, 
    RemoveAllBuffProcDamage=67,
    NoCapaPossible=68,
    ConsommeTensionReduitFa=69,
    AugmentationDegatsHitJoueur=70,
    GainFaBuffCible=71,
    GainFaDebuffCible=72,
    Ponction=73,
    DegatsBrutConsequence=74

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
    PassifJeanne,
    PassifPapa,
    PassifPapy,
    PassifClou
}
