using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new ClairvoyanceIconData", menuName ="ClairvoyanceIcon/Create New ClairvoyanceIcon")]
public class ClairvoyanceIconData : ScriptableObject
{
    [SerializeField]
    private Sprite _statCalme;
    [SerializeField]
    private Sprite _statCalmeDown;
    [SerializeField]
    private Sprite _statCalmeUp;

    [SerializeField]
    private Sprite _statClairvoyance;
    [SerializeField]
    private Sprite _statClairvoyanceDown;
    [SerializeField]
    private Sprite _statClairvoyanceUp;

    [SerializeField]
    private Sprite _statForceDame;
    [SerializeField]
    private Sprite _statForceDameDown;
    [SerializeField]
    private Sprite _statForceDameUp;

    [SerializeField]
    private Sprite _statRadiance;
    [SerializeField]
    private Sprite _statRadianceDown;
    [SerializeField]
    private Sprite _statRadianceUp;

    [SerializeField]
    private Sprite _statTension;
    [SerializeField]
    private Sprite _statTensionUp;
    [SerializeField]
    private Sprite _statTensionDown;

    [SerializeField]
    private Sprite _statConscience;
    [SerializeField]
    private Sprite _statConscienceDown;
    [SerializeField]
    private Sprite _statConscienceUp;
    [SerializeField]
    private Sprite _statConscienceDroite;
    [SerializeField]
    private Sprite _statConscienceDroiteDown;
    [SerializeField]
    private Sprite _statConscienceDroiteUp;
    [SerializeField]
    private Sprite _statConscienceGauche;
    [SerializeField]
    private Sprite _statConscienceGaucheDown;
    [SerializeField]
    private Sprite _statConscienceGaucheUp;

    [SerializeField]
    private Sprite _statResilience;
    [SerializeField]
    private Sprite _statResilienceDown;
    [SerializeField]
    private Sprite _statResilienceUp;

    [SerializeField]
    private Sprite _statVitesse;
    [SerializeField]
    private Sprite _statVitesseDown;
    [SerializeField]
    private Sprite _statVitesseUp;

    [SerializeField]
    private Sprite _statConviction;
    [SerializeField]
    private Sprite _statConvictionDown;
    [SerializeField]
    private Sprite _statConvictionUp;

    [SerializeField]
    private Sprite _intendionDebuff;
    [SerializeField]
    private Sprite _intentionBuff;
    [SerializeField]
    private Sprite _damage;

    [SerializeField]
    private Sprite _statVolonte;
    [SerializeField]
    private Sprite _statVolonteUp;
    [SerializeField]
    private Sprite _statVolonteDown;

    [SerializeField] 
    private Sprite _intentionAtk;
    [SerializeField]
    private Sprite _intentionHeavyAtk;
    [SerializeField]
    private Sprite _hiddenIntention;
    
    [SerializeField]
    private Sprite _increaseAtk;
    [SerializeField]
    private Sprite _decreaseAtk;

    [SerializeField]
    private Sprite _increaseDef;
    [SerializeField]
    private Sprite _decreaseDef;
    
    [SerializeField]
    private Sprite _increaseHeal;
    [SerializeField]
    private Sprite _decreaseHeal;

    [SerializeField]
    private Sprite _wrath;
    [SerializeField]
    private Sprite _wrathDown;
    [SerializeField]
    private Sprite _wrathUp;
    [field: SerializeField]
    public Sprite Stun;
    [field: SerializeField]
    public Sprite StunDown;
    [field: SerializeField]
    public Sprite StunUp;
    [field: SerializeField]
    public Sprite Essence;
    [field: SerializeField]
    public Sprite EssenceDown;
    [field: SerializeField]
    public Sprite EssenceUp;
    [field: SerializeField]
    public Sprite Divin;


    public Sprite Damage { get => _damage; }
    public Sprite IntentionAtk { get => _intentionAtk; }
    public Sprite IntentionHeavyAtk { get => _intentionHeavyAtk; }
    public Sprite HiddenIntention { get => _hiddenIntention; }
    public Sprite IntentionBuff { get => _intendionDebuff; }
    public Sprite IntentionDebuff { get => _intendionDebuff; }
    public Sprite StatCalme { get => _statCalme; }
    public Sprite StatCalmeUp { get => _statCalmeUp; }
    public Sprite StatCalmeDown { get => _statCalmeDown; }
    public Sprite StatClairvoyance { get => _statClairvoyance; }
    public Sprite StatClairvoyanceUp { get => _statClairvoyanceUp; }
    public Sprite StatClairvoyanceDown { get => _statClairvoyanceDown; }
    public Sprite StatConscience { get => _statConscience; }
    public Sprite StatConscienceUp { get => _statConscienceUp; }               
    public Sprite StatConscienceDown { get => _statConscienceDown; }               
    public Sprite StatConscienceDroite { get => _statConscienceDroite; }                 
    public Sprite StatConscienceDroiteUp { get => _statConscienceDroiteUp; }                 
    public Sprite StatConscienceDroiteDown { get => _statConscienceDroiteDown; }                 
    public Sprite StatConscienceGauche { get => _statConscienceGauche; }                 
    public Sprite StatConscienceGaucheUp { get => _statConscienceGaucheUp; }                 
    public Sprite StatConscienceGaucheDown { get => _statConscienceGaucheDown; }                 
    public Sprite StatConviction { get => _statConviction; }                  
    public Sprite StatConvictionUp { get => _statConvictionUp; }                  
    public Sprite StatConvictionDown { get => _statConvictionDown; }                  
    public Sprite StatForceDame { get => _statForceDame; }                 
    public Sprite StatForceDameUp { get => _statForceDameUp; }                 
    public Sprite StatForceDameDown { get => _statForceDameDown; }                 
    public Sprite StatRadiance { get => _statRadiance; }                 
    public Sprite StatRadianceUp { get => _statRadianceUp; }                 
    public Sprite StatRadianceDown { get => _statRadianceDown; }                 
    public Sprite StatResilience { get => _statResilience; }
    public Sprite StatResilienceUp { get => _statResilienceUp; }
    public Sprite StatResilienceDown { get => _statResilienceDown; }
    public Sprite StatTension { get => _statTension; }
    public Sprite StatTensionUp { get => _statTensionUp; }
    public Sprite StatTensionDown { get => _statTensionDown; }
    public Sprite StatVitesse { get => _statVitesse; }
    public Sprite StatVitesseUp { get => _statVitesseUp; }
    public Sprite StatVitesseDown { get => _statVitesseDown; }
    public Sprite StatVolonte { get => _statVolonte; }
    public Sprite StatVolonteUp { get => _statVolonteUp; }
    public Sprite StatVolonteDown { get => _statVolonteDown; }
    public Sprite IncreaseAtk { get => _increaseAtk; }
    public Sprite DecreaseAtk { get => _decreaseAtk; }
    public Sprite IncreaseDef { get => _increaseDef; }
    public Sprite DecreaseDef { get => _decreaseDef; }
    public Sprite IncreaseHeal { get => _increaseHeal; }
    public Sprite DecreaseHeal { get => _decreaseHeal; }
    public Sprite Wrath { get => _wrath; }
    public Sprite WrathDown { get => _wrathDown; }
    public Sprite WrathUp { get => _wrathUp; }


    #region TMP INSERT

    public string DamageSpriteTMP { get => FormatForTMP(_damage); }
    public string IntentionAtkSpriteTMP { get => FormatForTMP(_intentionAtk); }
    public string IntentionHeavyAtkSpriteTMP { get => FormatForTMP(_intentionHeavyAtk); }
    public string HiddenIntentionSpriteTMP { get => FormatForTMP(_hiddenIntention); }
    public string IntentionBuffSpriteTMP { get => FormatForTMP(_intendionDebuff); }
    public string IntentionDebuffSpriteTMP { get => FormatForTMP(_intendionDebuff); }
    public string StatCalmeSpriteTMP { get => FormatForTMP(_statCalme); }
    public string StatCalmeUpSpriteTMP { get => FormatForTMP(_statCalmeUp); }
    public string StatCalmeDownSpriteTMP { get => FormatForTMP(_statCalmeDown); }
    public string StatClairvoyanceSpriteTMP { get => FormatForTMP(_statClairvoyance); }
    public string StatClairvoyanceUpSpriteTMP { get => FormatForTMP(_statClairvoyanceUp); }
    public string StatClairvoyanceDownSpriteTMP { get => FormatForTMP(_statClairvoyanceDown); }
    public string StatConscienceSpriteTMP { get => FormatForTMP(_statConscience); }
    public string StatConscienceUpSpriteTMP { get => FormatForTMP(_statConscienceUp); }
    public string StatConscienceDownSpriteTMP { get => FormatForTMP(_statConscienceDown); }
    public string StatConscienceDroiteSpriteTMP { get => FormatForTMP(_statConscienceDroite); }
    public string StatConscienceDroiteUpSpriteTMP { get => FormatForTMP(_statConscienceDroiteUp); }
    public string StatConscienceDroiteDownSpriteTMP { get => FormatForTMP(_statConscienceDroiteDown); }
    public string StatConscienceGaucheSpriteTMP { get => FormatForTMP(_statConscienceGauche); }
    public string StatConscienceGaucheUpSpriteTMP { get => FormatForTMP(_statConscienceGaucheUp); }
    public string StatConscienceGaucheDownSpriteTMP { get => FormatForTMP(_statConscienceGaucheDown); }
    public string StatConvictionSpriteTMP { get => FormatForTMP(_statConviction); }
    public string StatConvictionUpSpriteTMP { get => FormatForTMP(_statConvictionUp); }
    public string StatConvictionDownSpriteTMP { get => FormatForTMP(_statConvictionDown); }
    public string StatForceDameSpriteTMP { get => FormatForTMP(_statForceDame); }
    public string StatForceDameUpSpriteTMP { get => FormatForTMP(_statForceDameUp); }
    public string StatForceDameDownSpriteTMP { get => FormatForTMP(_statForceDameDown); }
    public string StatRadianceSpriteTMP { get => FormatForTMP(_statRadiance); }
    public string StatRadianceUpSpriteTMP { get => FormatForTMP(_statRadianceUp); }
    public string StatRadianceDownSpriteTMP { get => FormatForTMP(_statRadianceDown); }
    public string StatResilienceSpriteTMP { get => FormatForTMP(_statResilience); }
    public string StatResilienceUpSpriteTMP { get => FormatForTMP(_statResilienceUp); }
    public string StatResilienceDownSpriteTMP { get => FormatForTMP(_statResilienceDown); }
    public string StatTensionSpriteTMP { get => FormatForTMP(_statTension); }
    public string StatTensionUpSpriteTMP { get => FormatForTMP(_statTensionUp); }
    public string StatTensionDownSpriteTMP { get => FormatForTMP(_statTensionDown); }
    public string StatVitesseSpriteTMP { get => FormatForTMP(_statVitesse); }
    public string StatVitesseUpSpriteTMP { get => FormatForTMP(_statVitesseUp); }
    public string StatVitesseDownSpriteTMP { get => FormatForTMP(_statVitesseDown); }
    public string StatVolonteSpriteTMP { get => FormatForTMP(_statVolonte); }
    public string StatVolonteUpSpriteTMP { get => FormatForTMP(_statVolonteUp); }
    public string StatVolonteDownSpriteTMP { get => FormatForTMP(_statVolonteDown); }
    public string IncreaseAtkSpriteTMP { get => FormatForTMP(_increaseAtk); }
    public string DecreaseAtkSpriteTMP { get => FormatForTMP(_decreaseAtk); }
    public string IncreaseDefSpriteTMP { get => FormatForTMP(_increaseDef); }
    public string DecreaseDefSpriteTMP { get => FormatForTMP(_decreaseDef); }
    public string IncreaseHealSpriteTMP { get => FormatForTMP(_increaseHeal); }
    public string DecreaseHealSpriteTMP { get => FormatForTMP(_decreaseHeal); }
    public string WrathSpriteTMP { get => FormatForTMP(_wrath); }
    public string WrathDownSpriteTMP { get => FormatForTMP(_wrathDown); }
    public string WrathUpSpriteTMP { get => FormatForTMP(_wrathUp); }
    public string StunSpriteTMP { get => FormatForTMP(Stun); }
    public string StunDownSpriteTMP { get => FormatForTMP(StunDown); }
    public string StunUpSpriteTMP { get => FormatForTMP(StunUp); }
    public string EssenceSpriteTMP { get => FormatForTMP(Essence); }
    public string EssenceDownSpriteTMP { get => FormatForTMP(EssenceDown); }
    public string EssenceUpSpriteTMP { get => FormatForTMP(EssenceUp); }
    public string DivinSpriteTMP { get => FormatForTMP(Divin); }



    private string FormatForTMP(Sprite sprite) => $"<sprite name=\"{sprite.name}\">";
    

    #endregion

}
