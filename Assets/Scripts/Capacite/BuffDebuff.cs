using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue", menuName = "Remake/Capacité/Create New BuffDebuff", order = 11)]
public class BuffDebuff : ScriptableObject
{
    public string Nom;
    public string Description;
    public List<Effet> Effet;
    public bool IsDebuff;
    public Cible CibleApplication;
    public int IDCombatOrigine;
    public Decompte Decompte;
    public TimerApplication timerApplication;
    public int Temps;
    public bool Activate;
    public Sprite Icon;
    public GameObject SpawnObject;
}
