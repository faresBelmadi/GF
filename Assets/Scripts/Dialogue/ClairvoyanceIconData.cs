using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new ClairvoyanceIconData", menuName ="ClairvoyanceIcon/Create New ClairvoyanceIcon")]
public class ClairvoyanceIconData : ScriptableObject
{
    [SerializeField] 
    private Sprite _intentionAtk;
    [SerializeField]
    private Sprite _intentionBuff;
    [SerializeField]
    private Sprite _intendionDebuff;
    [SerializeField]
    private Sprite _statCalmeUp;
    [SerializeField]
    private Sprite _statCalmeDown;
    [SerializeField]
    private Sprite _statClairvoyanceUp;
    [SerializeField]
    private Sprite _statClairvoyanceDown;
    [SerializeField]
    private Sprite _statConscienceUp;
    [SerializeField]
    private Sprite _statConscienceDown;
    [SerializeField]
    private Sprite _statConscienceDroiteUp;
    [SerializeField]
    private Sprite _statConscienceDroiteDown;
    [SerializeField]
    private Sprite _statConscienceGaucheUp;
    [SerializeField]
    private Sprite _statConscienceGaucheDown;
    [SerializeField]
    private Sprite _statConvictionUp;
    [SerializeField]
    private Sprite _statConvictionDown;
    [SerializeField]
    private Sprite _statForceDameUp;
    [SerializeField]
    private Sprite _statForceDameDown;
    [SerializeField]
    private Sprite _statRadianceUp;
    [SerializeField]
    private Sprite _statRadianceDown;
    [SerializeField]
    private Sprite _statResilienceUp;
    [SerializeField]
    private Sprite _statResilienceDown;
    [SerializeField]
    private Sprite _statTensionUp;
    [SerializeField]
    private Sprite _statTensionDown;
    [SerializeField]
    private Sprite _statVitesseUp;
    [SerializeField]
    private Sprite _statVitesseDown;
    [SerializeField]
    private Sprite _statVolonteUp;
    [SerializeField]
    private Sprite _statVolonteDown;

    public Sprite IntentionAtk { get => _intentionAtk; }
    public Sprite IntentionBuff { get => _intendionDebuff; }
    public Sprite IntentionDebuff { get => _intendionDebuff; }
    public Sprite StatCalmeUp { get => _statCalmeUp; }
    public Sprite StatCalmeDown { get => _statCalmeDown; }
    public Sprite StatClairvoyanceUp { get => _statClairvoyanceUp; }
    public Sprite StatClairvoyanceDown { get => _statClairvoyanceDown; }
    public Sprite StatConscienceUp { get => _statConscienceUp; }               
    public Sprite StatConscienceDown { get => _statConscienceDown; }               
    public Sprite StatConscienceDroiteUp { get => _statConscienceDroiteUp; }                 
    public Sprite StatConscienceDroiteDown { get => _statConscienceDroiteDown; }                 
    public Sprite StatConscienceGaucheUp { get => _statConscienceGaucheUp; }                 
    public Sprite StatConscienceGaucheDown { get => _statConscienceGaucheDown; }                 
    public Sprite StatConvictionUp { get => _statConvictionUp; }                  
    public Sprite StatConvictionDown { get => _statConvictionDown; }                  
    public Sprite StatForceDameUp { get => _statForceDameUp; }                 
    public Sprite StatForceDameDown { get => _statForceDameDown; }                 
    public Sprite StatRadianceUp { get => _statRadianceUp; }                 
    public Sprite StatRadianceDown { get => _statRadianceDown; }                 
    public Sprite StatResilienceUp { get => _statResilienceUp; }
    public Sprite StatResilienceDown { get => _statResilienceDown; }
    public Sprite StatTensionUp { get => _statTensionUp; }
    public Sprite StatTensionDown { get => _statTensionDown; }
    public Sprite StatVitesseUp { get => _statVitesseUp; }
    public Sprite StatVitesseDown { get => _statVitesseDown; }
    public Sprite StatVolonteUp { get => _statVolonteUp; }
    public Sprite StatVolonteDown { get => _statVolonteDown; }
}
