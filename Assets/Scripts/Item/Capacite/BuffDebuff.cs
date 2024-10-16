﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff/debuff", menuName = "Capacité/Create New BuffDebuff", order = 11)]
public class BuffDebuff : ScriptableObject
{
    public string Nom;
    public string idTradName;
    public string Description;
    public string idTradDescription;
    public List<Effet> Effet;
    public bool IsConsomable;
    public int TimingConsomationMinimum = 1;
    public List<BuffDebuff> Consomation;
    public bool IsDebuff;
    public Cible CibleApplication;
    public int IDCombatOrigine;
    public Decompte Decompte;
    public TimerApplication timerApplication;
    public int Temps;
    public Sprite Icon;
    public GameObject SpawnObject;
    public bool DirectApplication = true;
}
