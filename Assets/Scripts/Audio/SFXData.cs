using UnityEngine;

[CreateAssetMenu(fileName = "new SFXData", menuName = "Audio/SFXData")]
public class SFXData : ScriptableObject
{
    [Header("Battle Phase SXF")]
    [SerializeField]
    private AudioClip _startTurn;
    [field:SerializeField, Range(0,1)]
    public float StartTurnVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _startPhase;
    [field: SerializeField, Range(0, 1)]
    public float StartPhaseVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _startAnimalTurnDefault;
    [field: SerializeField, Range(0, 1)]
    public float StartAnimalTurnVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _ennemyTensionFullDefault;
    [field: SerializeField, Range(0, 1)]
    public float EnnemyTensionFullVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _playerTensionFullDefault;
    [field: SerializeField, Range(0, 1)]
    public float PlayerTensionFullVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _damagetakenDefault;
    [field: SerializeField, Range(0, 1)]
    public float DamageTakenVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _deathDefault;
    [field: SerializeField, Range(0, 1)]
    public float DeathDefaultVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _essenceConsumption;
    [field: SerializeField, Range(0, 1)]
    public float EssenceComsumptionVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _buffDisappear;
    [field: SerializeField, Range(0, 1)]
    public float BuffDisappearVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _buffTrigger;
    [field: SerializeField, Range(0, 1)]
    public float BuffTriggerVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _playerSpellDefault;
    [field: SerializeField, Range(0, 1)]
    public float PlayerSpellVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _ennemySpellDefault;
    [field: SerializeField, Range(0, 1)]
    public float EnemySpellVolume { get; private set; } = 1f;


    [Space]
    [Header("Map")]
    [SerializeField]
    private AudioClip _mapBattle;
    [field: SerializeField, Range(0, 1)]
    public float MapBattleVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _mapAlea;
    [field: SerializeField, Range(0, 1)]
    public float MapAleaVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _mapAutel;
    [field: SerializeField, Range(0, 1)]
    public float MapAutelVolume { get; private set; } = 1f;


    [Space]
    [Header("UI SFX")]
    [SerializeField]
    private AudioClip _buttonClics;
    [field: SerializeField, Range(0, 1)]
    public float ButtonCLicVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _buttonDialogue;
    [field: SerializeField, Range(0, 1)]
    public float ButtonDialogueVolume { get; private set; } = 1f;

    [SerializeField]
    private AudioClip _dialogueVoice;
    [field: SerializeField, Range(0, 1)]
    public float DialogueVoiceVolume { get; private set; } = 1f;


    #region Getters
    public AudioClip StartTurn => _startTurn;
    public AudioClip StartPhase => _startPhase;
    public AudioClip EnnemyTensionFullDefault => _ennemyTensionFullDefault;
    public AudioClip PlayerTensionFullDefault => _playerTensionFullDefault;
    public AudioClip DamageTakenDefault => _damagetakenDefault;
    public AudioClip DeathDefault => _deathDefault;
    public AudioClip EssenceConsumption => _essenceConsumption;
    public AudioClip BuffDisapear => _buffDisappear;
    public AudioClip BuffTrigger => _buffTrigger;
    public AudioClip ButtonClics => _buttonClics;
    public AudioClip ButtonDialogue => _buttonDialogue;
    public AudioClip DialogueVoice => _dialogueVoice;
    public AudioClip StartAnimalTurnDefaultSFX => _startAnimalTurnDefault;
    public AudioClip PlayerSpellDefault => _playerSpellDefault;
    public AudioClip EnnemySpellDefault => _ennemySpellDefault;
    public AudioClip MapBattle => _mapBattle;
    public AudioClip MapAlea => _mapAlea;
    public AudioClip MapAutel => _mapAutel;
    #endregion

}
