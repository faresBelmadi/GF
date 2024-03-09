using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RéponseSO
{
    //les réponses possedents des emotions lié au joueur et a ce que la réponse va faire ressentir a l'npc parlant avec le joueur
    public bool IsEmotion;
    public Emotion whichEmotion;
    //la clairvoyance dont le joueur a besoin pour voir qu'elle emotion chaque réponse possede
    public int SeuilClairvoyance;
    //chaque réponse donne vers une question sauf si c'est la dernière question; plusieurs réponse peuvent amener a la meme question;
    public int IDNextQuestion;
    [TextArea(10,10)]
    public string TexteRéponse;
    //possibilité de devoir changer liste Conséquences et Icon en un seul SO, a voir en fonction des GD
    public List<ConséquenceSO> conséquences;
}
