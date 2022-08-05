using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public enum Cible
{
    joueur,
    ennemi,
    allEnnemi,
    All,
    Ally,
    allAllies,
    MostDamage
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
    SoinRadianceMax,
    SoinRadianceActuelle,
    DegatPVMax,
    RandomAttaque,
    AugmentationFaRadianceActuelle,
}
