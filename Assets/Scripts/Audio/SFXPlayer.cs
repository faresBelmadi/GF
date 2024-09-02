using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField]
    private SFXData _sfxData;
    [SerializeField]
    private AudioSource _sfxSource;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private AudioClip GetClip(SFXType type)
    {
        switch (type)
        {
            case SFXType.StartPhaseSFX:
                return _sfxData.StartPhase;
            case SFXType.StartTurnSFX:
                return _sfxData.StartTurn;
            case SFXType.StartEnnymaAnimalTurnDefaultSFX:
                return _sfxData.StartAnimalTurnDefaultSFX;
            case SFXType.EnnemyFullTensionDefaultSFX:
                return _sfxData.EnnemyTensionFullDefault;
            case SFXType.PlayerFullTensionDefaultSFX:
                return _sfxData.PlayerTensionFullDefault;
            case SFXType.PlayerDamageTakenDefaultSFX:
                return _sfxData.DamageTakenDefault;
            case SFXType.EnnemyDamageTakenDefaultSFX:
                return _sfxData.DamageTakenDefault;
            case SFXType.PlayerDeathDefaultSFX:
                return _sfxData.DeathDefault;
            case SFXType.EnnemyDeathDefaultSFX:
                return _sfxData.DeathDefault;
            case SFXType.EssenceConsuptionSFX:
                return _sfxData.EssenceConsumption;
            case SFXType.PlayerSpellDefaultSFX:
                return _sfxData.PlayerSpellDefault;
            case SFXType.EnnemySpellDefaultSFX:
                return _sfxData.EnnemySpellDefault;
            case SFXType.BuffTriggerSFX:
                return _sfxData.BuffTrigger;
            case SFXType.BuffDisapearSFX:
                return _sfxData.BuffDisapear;
            case SFXType.ClicButtonSFX:
                return _sfxData.ButtonClics;
            case SFXType.DialogueButtonSFX:
                return _sfxData.ButtonDialogue;
            case SFXType.DialogueSFX:
                return _sfxData.DialogueVoice;
            default:
                return null;
        }
    }
    public void PlaySFXClip(SFXType type, float volume = 1)
    {
        PlayClip(GetClip(type), volume);
    }
    public void PlaySFXClip(SFXType type, AudioClip clipToPlay, float volume = 1)
    {
        //Play the default SFX for this type
        if (clipToPlay == null)
        {
            PlaySFXClip(type, volume);
        }
        else
        {
            PlayClip(clipToPlay, volume);
        }
    }
    private void PlayClip(AudioClip clipToPlay, float volume)
    {
        _sfxSource.PlayOneShot(clipToPlay, volume);
    }

}
