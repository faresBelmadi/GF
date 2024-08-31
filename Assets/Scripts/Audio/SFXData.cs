using UnityEngine;

[CreateAssetMenu(fileName = "new SFXData", menuName = "Audio/SFXData")]
public class SFXData : ScriptableObject
{
    [SerializeField]
    private AudioClip _startTurn;
    [SerializeField]
    private AudioClip _startPhase;
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
    #endregion

}
