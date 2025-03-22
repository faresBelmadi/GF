using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Stat Ennemi", menuName = "Character/Create New Ennemi", order = 11)]
public class CommonStats : ScriptableObject
{
    [SerializeField]
    int ConvictionValue;
    [SerializeField]
    int ConvictionMin;
    [SerializeField]
    int ConvictionMax;
    [SerializeField]
    int ResilienceValue;
    [SerializeField]
    int ResilienceMin;
    [SerializeField]
    int ResilienceMax;
    [SerializeField]
    int GainTensionAttaque;
    [SerializeField]
    int GainTensionDebuff;
    [SerializeField]
    int GainTensionSoin;
    [SerializeField]
    int GainTensionDot;
    [SerializeField]
    int NbPalier;
}