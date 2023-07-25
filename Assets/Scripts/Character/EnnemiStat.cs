using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Stat Ennemi", menuName = "Character/Create New Ennemi", order = 11)]
public class EnnemiStat : CharacterStat
{
    public string Nom;
    public int Dissimulation;
    public int DissimulationOriginal;
    public bool NoTension = false;
    public GameObject Spawnable;
    public EnnemiSpell Att1;
    public EnnemiSpell Att2;
    public EnnemiSpell Buff;
    public EnnemiSpell Debuff;
}
