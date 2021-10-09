using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSpell
{
    int hpToRemove;
    public List<BuffDebuff> buffs;
}


[CreateAssetMenu(fileName = "New Spell", menuName = "Create New Spell", order = 1)]
public class Spell : ScriptableObject
{
    public string Name;
    public int ID;
    public List<int> idChildren;
    public SpellStatus Status;
    public bool isAvailable;
    public int CostUnlocked;
    public List<Cost> Costs;
    public List<Effect> Effet;
    public List<BuffDebuff> debuffsBuffs;
}

