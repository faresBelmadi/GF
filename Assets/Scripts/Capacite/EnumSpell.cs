using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Decompte
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

public enum Cible
{
    joueur,
    ennemi,
    allEnnemi,
    All,
    Ally,
    allAllies
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
    Colere
}
