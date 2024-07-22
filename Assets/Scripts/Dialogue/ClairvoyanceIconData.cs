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
    private Sprite _statCalme;
    [SerializeField]
    private Sprite _statClairvoyance;
    [SerializeField]
    private Sprite _statConscience;
    [SerializeField]
    private Sprite _statConscienceDroite;
    [SerializeField]
    private Sprite _statConscienceGauche;
    [SerializeField]
    private Sprite _statConviction;
    [SerializeField]
    private Sprite _statForceDame;
    [SerializeField]
    private Sprite _statRadiance;
    [SerializeField]
    private Sprite _statResilience;
    [SerializeField]
    private Sprite _statTension;
    [SerializeField]
    private Sprite _statVitesse;
    [SerializeField]
    private Sprite _volonte;

    public Sprite IntentionAtk { get => _intentionAtk; }
    public Sprite IntentionBuff { get => _intendionDebuff; }
    public Sprite IntentionDebuff { get => _intendionDebuff; }
    public Sprite StatCalme { get => _statCalme; }
    public Sprite StatClairvoyance { get => _statClairvoyance; }                
    public Sprite StatConscience { get => _statConscience; }               
    public Sprite StatConscienceDroite { get => _statConscienceDroite; }                 
    public Sprite StatConscienceGauche { get => _statConscienceGauche; }                 
    public Sprite StatConviction { get => _statConviction; }                  
    public Sprite StatForceDame { get => _statForceDame; }                 
    public Sprite StatRadiance { get => _statRadiance; }                 
    public Sprite StatResilience { get => _statResilience; }
    public Sprite StatTension { get => _statTension; }
    public Sprite StatVitesse { get => _statVitesse; }
    public Sprite Volonte { get => _volonte; }
}
