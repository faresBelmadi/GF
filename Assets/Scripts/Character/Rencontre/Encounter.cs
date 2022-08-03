using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue", menuName = "Remake/Character/Create New Encounter", order = 11)]
public class Encounter : ScriptableObject
{
    [Header("Dialogue")]
    public DialogueSO DialogueRencontre;
    [Header("Ennemy")]
    public List<EnnemiStat> ToFight;
    public int PourcentageLootSouvenir;
    public List<LootRarity> LootRarity;
}
