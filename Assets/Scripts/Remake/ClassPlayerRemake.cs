using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewClass", menuName = "Create New Playable Class Remake", order = 2)]
public class ClassPlayerRemake : ScriptableObject
{

    public int ID;
    public string nameClass;

    public JoueurStatRemake PlayerStat;

    public List<SpellRemake> spellClass;

    public List<string> Passif;
}
