using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    DégatsForceAme,
    DégatsBrut,
    Enervement,
    Apaisement,
    Att,
    AttBrut,
    Def,
    DefBrut,
    //Doute = chance de skip tour
    Doute,
    //Peur = chance de leave le combat #dead sans leave l'essence
    Peur,
    //Colere = chance d'attaquer pas de buff ou debuff
    Colère,
    Passif,
    Soin,
    VitesseBrut,
    Vitesse,
    Résilience,
    Conviction,
    Dissimulation,
    Clairvoyance,
    PVMax,
    Stun,
    //TODO
    //Soif de vengeance = att up for each dmg taken
    AttUpLastDmgTaken,
    SoinRadianceMax,
    //on hold 
    AttUpPVMiss,
    AttUpPVMissSelf,
    Ponction,
    DmgPVMax,
    EpineForceAme,
    DeathTrigger,
    RandNBAttack,
    AttUpBuff,
    AttUpDebuff,
    Conscience,
    Volonté,
    SkipCombat
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