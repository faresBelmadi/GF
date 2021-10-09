using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct QuestionRéponse
{    
    public int ID;
    public QuestionSO Question;
    public List<RéponseSO> ReponsePossible;
} 

[CreateAssetMenu(fileName = "New dialogue", menuName = "Dialogue/Create New Dialogue", order = 10)]
public class DialogueSO : ScriptableObject
{
    public List<QuestionRéponse> Questions;
}
