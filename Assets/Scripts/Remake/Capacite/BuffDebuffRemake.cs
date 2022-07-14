using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue", menuName = "Remake/Capacité/Create New BuffDebuff", order = 11)]
public class BuffDebuffRemake : ScriptableObject
{
    public string Nom;
    public string Description;
    public List<EffetRemake> Effet;
    public bool IsDebuff;
    public CibleRemake CibleApplication;
    public int IDCombatOrigine;
    public DecompteRemake Decompte;
    public TimerApplication timerApplication;
    public int Temps;
    public bool Activate;
    public Sprite Icon;
    public GameObject SpawnObject;
}
