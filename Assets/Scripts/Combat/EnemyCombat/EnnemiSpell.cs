using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Spell", menuName = "Remake/Capacité/Create New Enemy Spell", order = 1)]
public class EnnemiSpell : ScriptableObject
{
    public string Name;
    public int ID;
    public bool IsAttaque;
    public Sprite ImageIntentionSpell;

    [Tooltip("Poids de départ du sort, plus il est bas, plus le sort aura de chance d'etre lancé en premier")]
    [Min(10)]
    public int Weight = 10;

    [Tooltip("Poids ajouté au sort, plus il est bas, plus le sort aura de chance d'etre lancé")]
    public int AddedWeight;

    public List<Effet> Effet;
    public List<BuffDebuff> debuffsBuffs;
}
