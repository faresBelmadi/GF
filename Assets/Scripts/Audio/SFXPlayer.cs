using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField]
    private SFXData _sfxData;
    [SerializeField]
    private AudioSource _sfxSoundPrefab;


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
            case SFXType.EssenceConsuptionSFX :
                return _sfxData.EssenceConsumption;
            case SFXType.StartPhaseSFX:
                return _sfxData.StartPhase;
            case SFXType.StartTurnSFX:
                return _sfxData.StartTurn;
            case SFXType.BuffTriggerSFX:
                return _sfxData.BuffTrigger;
            case SFXType.BuffDisapearSFX:
                return _sfxData.BuffDisapear;
            case SFXType.ClicButtonSFX:
                return _sfxData.ButtonClics;
            case SFXType.DialogueButtonSFX:
                return _sfxData.ButtonDialogue;
            default:
                return null;
        }
    }
    public void PlaySFXClip(SFXType type, float volume = 1)
    {
        AudioSource sfxSource = Instantiate(_sfxSoundPrefab);
        
        sfxSource.clip = GetClip(type);        
        sfxSource.volume = volume;
        sfxSource.Play();
        Destroy(sfxSource, sfxSource.clip.length);
        
    }
    public void PlaySFXClip()
    {
        AudioSource sfxSource = Instantiate(_sfxSoundPrefab);

        //sfxSource.clip = GetClip(type);
        sfxSource.Play();
        Destroy(sfxSource, sfxSource.clip.length);

    }

}