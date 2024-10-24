﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Encounter/Create New Encounter", order = 11)]
public class Encounter : ScriptableObject
{
    [Header("Dialogue")]
    public DialogueSO DialogueRencontre;
    [Header("Ennemy")]
    public int idMainMob = -1;
    public List<EnnemiStat> ToFight;
    public int PourcentageLootSouvenir;
    public List<LootRarity> LootRarity;
    [Header("EncouterOptions")]
    public List<EncounterOption> forcedOrder;
}
