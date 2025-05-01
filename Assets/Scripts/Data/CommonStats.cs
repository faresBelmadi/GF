using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " new CommonStats", menuName = "CommonData/Create New CommonStats")]
public class CommonStats : ScriptableObject
{
    [SerializeField]
    int _convictionValue;
    [SerializeField]
    int _convictionPalier1;
    [SerializeField]
    int _convictionPalier2;
    [SerializeField]
    int _convictionPalierValue;
    [SerializeField]
    int _convictionMin;
    [SerializeField]
    int _convictionMax;
    [SerializeField]
    int _resilienceValue;
    [SerializeField]
    int _resilienceMin;
    [SerializeField]
    int _resilienceMax;
    [SerializeField]
    int _gainTensionAttaque;
    [SerializeField]
    int _gainTensionDebuff;
    [SerializeField]
    int _gainTensionSoin;
    [SerializeField]
    int _gainTensionDot;
    [SerializeField]
    int _nbPalier;

    public int ConvictionValue { get => _convictionValue; }
    public int ConvictionPalier1 { get => _convictionPalier1;}
    public int ConvictionPalier2 { get => _convictionPalier2;}
    public int ConvictionMin { get => _convictionMin;}
    public int ConvictionMax { get => _convictionMax;}
    public int ResilienceValue { get => _resilienceValue; }
    public int ResilienceMin { get => _resilienceMin; }
    public int ResilienceMax { get => _resilienceMax; }
    public int GainTensionAttaque { get => _gainTensionAttaque; }
    public int GainTensionDebuff { get => _gainTensionDebuff; }
    public int GainTensionSoin { get => _gainTensionSoin; }
    public int GainTensionDot { get => _gainTensionDot; }
    public int NbPalier { get => _nbPalier; }
    public int ConvictionPalierValue { get => _convictionPalierValue; }
}