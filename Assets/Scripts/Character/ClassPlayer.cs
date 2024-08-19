using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewClass", menuName = "Create New Playable Class", order = 2)]
public class ClassPlayer : ScriptableObject
{
    public int ID;
    public string nameClass;

    public JoueurStat PlayerStat;

    public List<Spell> spellClass;
    public int NbMaxSpell;

    public List<Competence> Competences;
}
