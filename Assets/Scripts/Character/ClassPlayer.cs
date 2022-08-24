using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewClass", menuName = "Create New Playable Class Remake", order = 2)]
public class ClassPlayer : ScriptableObject
{

    public int ID;
    public string nameClass;

    public JoueurStat PlayerStat;

    public List<Spell> spellClass;

    public List<string> Passif;

    public List<Competence> Competences;
}
