using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Competence
{
    public int IDLvl;
    public Spell Spell;
    public List<ModifStat> ModifStat;
    public int EssenceCostOriginal, EssenceCost;
    //public List<int> IDLier;
    public bool Bought;
    public bool Equiped;
}
