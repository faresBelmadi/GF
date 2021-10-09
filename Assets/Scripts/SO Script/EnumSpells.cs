using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Dégats,
    DégatsBrut,
    Enervement,
    Apaisement,
    Att,
    AttBrut,
    Def,
    DefBrut,
    Doute,
    Découragé,
    Vulnérable,
    Peur,
    Colère,
    PourEnFinir,
    Execution,
    Passif,
    Cible,
    Armure,
    Soin,
    //TODO
    Vitesse,
    Résilience,
    Conviction,
    Clairvoyance,
    PVMax,
    Stun,
    AttUpDamageTaken,
    SoinRadianceMax,
    AttUpPVMiss,
    Ponction,
    DmgPVMax,
    EpineForceAme,
    DeathTrigger,
    RandNBAttack
}

public enum EffetAcharnementHabitude
{
    None,
    Acharnement,
    Habitude
}
public enum FacteurAcharnementHabitude
{
    Additive,
    Soustractive,
    Double
}

public enum EffetTypeDecompte
{
    round,
    tour,
    combat
}

public enum Cible
{
    self,
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

public enum AcharnementType
{
    Dmg,
    NbAtt,
    Crit,
    BuffDebuff
}

public enum TypeCostSpell
{
    Radiance,
    Conscience,
    Volonté
}