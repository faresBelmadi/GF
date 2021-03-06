using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Create New Buff-Debuff", order = 1)]
[System.Serializable]
public class BuffDebuff : ScriptableObject
{
    
    public string NomDebuff;
    
    [Multiline(5)]
    public string Description;
    public bool IsDebuff;
    public EffetTypeDecompte Decompte;
    public Cible target;
    public int nbTemps;
    public Sprite Icon;
    public List<Effect> effects; 

    public GameObject SpawnedObject;
}