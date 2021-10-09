using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Emotion
{
    joie,
    honte,
}

[System.Serializable]
public struct RéponseSO
{
    public bool IsEmotion;
    public Emotion whichEmotion;
    public int SeuilClairvoyance;
    public int IDNextQuestion;
    [TextArea(10,10)]
    public string TexteRéponse;
    //possibilité de devoir changer liste Conséquences et Icon en un seul SO, a voir en fonction des GD
    public List<ConséquenceSO> conséquences;
}
