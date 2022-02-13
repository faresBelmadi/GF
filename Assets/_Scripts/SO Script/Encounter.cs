using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Create New Encounter", order = 5)]
public class Encounter : ScriptableObject
{
    [Header("Dialogue")]
    public DialogueSO DialogueRencontre;
    [Header("Ennemy")]
    public List<Enemy> ToFight;
    
    
   
}
