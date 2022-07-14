using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecompteRemake
{
    phase,
    tour,
    combat
}

public enum TimerApplication
{
    DebutPhase,
    DebutTour,
    Attaque,
    Persistant
}

public enum CibleRemake
{
    joueur,
    ennemi,
    allEnnemi,
    All,
    Ally,
    allAllies
}

public enum SpellStatusRemake
{
    locked,
    unlocked,
    bought,
}

public enum TypeCostSpellRemake
{
    radiance,
    volonte,
    conscience
}

public enum TypeEffetRemake
{
    DegatsForceAme,
    DegatsBrut,
    Clairvoyance,
    Colere
}
