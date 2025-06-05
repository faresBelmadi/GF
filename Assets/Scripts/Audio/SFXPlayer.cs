using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField]
    private bool _debugMode = false;
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
            case SFXType.EnnemyFullTensionSFX:
                return _sfxData.EnnemyTensionFullDefault;
            case SFXType.PlayerFullTensionSFX:
                return _sfxData.PlayerTensionFullDefault;
            case SFXType.PlayerDamageTakenSFX:
                return _sfxData.DamageTakenDefault;
            case SFXType.EnnemyDamageTakenSFX:
                return _sfxData.DamageTakenDefault;
            case SFXType.PlayerDeathSFX:
                return _sfxData.DeathDefault;
            case SFXType.EnnemyDeathSFX:
                return _sfxData.DeathDefault;
            case SFXType.EssenceConsuptionSFX:
                return _sfxData.EssenceConsumption;
            case SFXType.PlayerSpellSFX:
                return _sfxData.PlayerSpellDefault;
            case SFXType.EnnemySpellSFX:
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
            case SFXType.MapAutelSFX:
                return _sfxData.MapAutel;
            case SFXType.MapBattleSFX:
                return _sfxData.MapBattle;
            case SFXType.MapSFX:
                return _sfxData.MapAlea;
            default:
                return null;
        }
    }

    private float GetVolumeMultiplier(SFXType type) => type switch
    {
        SFXType.StartPhaseSFX                   => _sfxData.StartPhaseVolume,
        SFXType.StartTurnSFX                    => _sfxData.StartTurnVolume,
        SFXType.StartEnnymaAnimalTurnDefaultSFX => _sfxData.StartAnimalTurnVolume,
        SFXType.EnnemyFullTensionSFX            => _sfxData.EnnemyTensionFullVolume,
        SFXType.PlayerFullTensionSFX            => _sfxData.PlayerTensionFullVolume,
        SFXType.PlayerDamageTakenSFX            => _sfxData.DamageTakenVolume,
        SFXType.EnnemyDamageTakenSFX            => _sfxData.DamageTakenVolume,
        SFXType.PlayerDeathSFX                  => _sfxData.DeathDefaultVolume,
        SFXType.EnnemyDeathSFX                  => _sfxData.DeathDefaultVolume,
        SFXType.EssenceConsuptionSFX            => _sfxData.EssenceComsumptionVolume,
        SFXType.PlayerSpellSFX                  => _sfxData.PlayerSpellVolume,
        SFXType.EnnemySpellSFX                  => _sfxData.EnemySpellVolume,
        SFXType.BuffTriggerSFX                  => _sfxData.BuffTriggerVolume,
        SFXType.BuffDisapearSFX                 => _sfxData.BuffDisappearVolume,
        SFXType.ClicButtonSFX                   => _sfxData.ButtonCLicVolume,
        SFXType.DialogueButtonSFX               => _sfxData.ButtonDialogueVolume,
        SFXType.DialogueSFX                     => _sfxData.DialogueVoiceVolume,
        SFXType.MapAutelSFX                     => _sfxData.MapAutelVolume,
        SFXType.MapBattleSFX                    => _sfxData.MapBattleVolume,
        SFXType.MapSFX                          => _sfxData.MapAleaVolume,
        _                                       => 1,
    };
    public void PlaySFXClip(SFXType type, float volume = 1)
    {
        
        if (_debugMode)
        {
            Debug.Log($"Playing SFX (type : {type})");
        }
        PlayClip(GetClip(type), volume * GetVolumeMultiplier(type));
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
        if (_debugMode)
        {
            Debug.Log($"Playing SFX Clip (type : {clipToPlay.name})");
        }
        _sfxSource.PlayOneShot(clipToPlay, volume);
    }
    public void StopPlaying()
    {
        _sfxSource.Stop();
    }

}
