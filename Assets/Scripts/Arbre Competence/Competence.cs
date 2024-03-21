﻿using System.Collections.Generic;

[System.Serializable]
public class Competence
{
    public int IDLvl;
    public Spell Spell;
    public List<ModifStat> ModifStat;
    public int EssenceCostOriginal, EssenceCost;
    public List<int> IDLier;
    public int lvlCapa;
    public bool Bought;
    public bool Equiped;
    public bool isBuyable;
}
