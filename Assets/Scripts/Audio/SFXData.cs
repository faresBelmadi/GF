using UnityEngine;

[CreateAssetMenu(fileName = "new SFXData", menuName = "Audio/SFXData")]
public class SFXData : ScriptableObject
{
    [Header("Battle Phase SXF")]
    [SerializeField]
    private AudioClip _startTurn;
    [SerializeField]
    private AudioClip _startPhase;
    [SerializeField]
    private AudioClip _startAnimalTurnDefault;
    [SerializeField]
    private AudioClip _ennemyTensionFullDefault;
    [SerializeField]
    private AudioClip _playerTensionFullDefault;
    [SerializeField]
    private AudioClip _damagetakenDefault;
    [SerializeField]
    private AudioClip _deathDefault;
    [SerializeField]
    private AudioClip _essenceConsumption;
    [SerializeField]
    private AudioClip _buffDisappear;
    [SerializeField]
    private AudioClip _buffTrigger;
    [SerializeField]
    private AudioClip _playerSpellDefault;
    [SerializeField]
    private AudioClip _ennemySpellDefault;

    [Space]
    [Header("Map")]
    [SerializeField]
    private AudioClip _mapBattle;
    [SerializeField]
    private AudioClip _mapAlea;
    [SerializeField]
    private AudioClip _mapAutel;

    [Space]
    [Header("UI SFX")]
    [SerializeField]
    private AudioClip _buttonClics;
    [SerializeField]
    private AudioClip _buttonDialogue;
    [SerializeField]
    private AudioClip _dialogueVoice;

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
