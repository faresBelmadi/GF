﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue", menuName = "Remake/Character/Create New Encounter", order = 11)]
public class EncounterRemake : ScriptableObject
{
    [Header("Dialogue")]
    public DialogueSO DialogueRencontre;
    [Header("Ennemy")]
    public List<EnnemiStatRemake> ToFight;



}
