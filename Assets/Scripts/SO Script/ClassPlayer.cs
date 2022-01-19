using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewClass", menuName = "Create New Playable Class", order = 2)]
public class ClassPlayer : ScriptableObject {

    public int ID;
    public string nameClass;
    
    public Stat PlayerStat;

    public List<Spell> spellClass;
    
    public List<string> Passif;
}
