using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New dialogue", menuName = "Remake/Character/Create New Ennemi", order = 11)]
public class EnnemiStatRemake : CharacterStatRemake
{
    public int Dissimulation;
    public GameObject Spawnable;
}
