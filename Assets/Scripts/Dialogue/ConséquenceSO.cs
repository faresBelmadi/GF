using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum CategoryConsequence
{
    InGame,
    InCombat
}

[System.Serializable]
public enum TypeConsequence
{
    Effect,
    Debuff,
    Both
}

[CreateAssetMenu(fileName = "New conséquence", menuName = "Dialogue/Create New Conséquence", order = 11)]
[System.Serializable]
public class ConséquenceSO : ScriptableObject
{
    public Texture2D Icon;
    //public CategoryConsequence catConsequence;
    public TypeConsequence typConsequence;

    public List<BuffDebuff> Buffs;

    public List<Effect> Effects;

    [Header("Self is always the player")]
    public Cible target; 
    public List<int> IdTarget;
}
