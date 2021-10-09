using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Create New Player Stat", order = 4)]
public class Stat : ScriptableObject
{
    public int HP;
    public int Volonté;
    public int Conscience;
    public int ConscienceMax;
    public int Essence;
    public int dmg;

    [Range(-10,10)]
    public int Résilience;
    
    [Range(-10,10)]
    public int Conviction;
    public int Clairvoyance;
    public int Speed;
    public int Calme;
   
}
