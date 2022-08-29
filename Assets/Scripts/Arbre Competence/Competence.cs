using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Competence
{
    public int IDLvl;
    public Spell Spell;
    public List<ModifStat> ModifStat;
    public int EssenceOriginal, Essence;
    public List<int> IDLier;
    public bool Bought;
    public bool Equiped;
}
