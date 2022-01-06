using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Create New Enemy", order = 6)]
public class Enemy : ScriptableObject
{
    [Header("Stats")]
    public int HP;
    public int MaxHP;
    public int MaxHPOriginal;
    public int ForceAme;
    public int Bouclier;
    public int Speed;
    public int SpeedOriginal;
    public int Calme;
    public int Dissimulation;
    public int DissimulationOriginal;
    public int résilience;
    public int résilienceOriginal;

    [Header("Stats Tension")]
    public int GainAttaque = 4;
    public int GainDebuff = 2;
    public int GainDot = 1;
    public int GainHeal = -3;
    public int NbPalier = 3;

    [Header("Drop")]
    public int EssenceDrop;
    

    [Header("Spells")]
    public EnemySpell Att1;
    public EnemySpell Att2;
    public EnemySpell Buff;
    public EnemySpell Debuff;

    [Header("Objet InGame")]
    public GameObject Spawnable;

    
   
}
