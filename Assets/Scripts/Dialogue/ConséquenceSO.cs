using System.Collections.Generic;
using UnityEngine;

/*[System.Serializable]
public enum CategoryConsequence
{
    InGame,
    InCombat
}*/

/*[System.Serializable]
public enum TypeConsequence
{
    Effect,
    Debuff,
    Both
}*/

[CreateAssetMenu(fileName = "New conséquence", menuName = "Dialogue/Create New Conséquence", order = 11)]
[System.Serializable]
public class ConséquenceSO : ScriptableObject
{
    public Texture2D Icon;

    //Consequence peuvent avoir 3 type : effet de spell/application d'un debuff pour le prochain combat ou les deux en meme temps;
    //public TypeConsequence typConsequence;

    public List<BuffDebuff> Buffs;
    public List<Effet> Effects;

    [Header("Self is always the player")]
    public CibleDialogue target;
    public List<int> IdTarget;
}
