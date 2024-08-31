using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
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
                break;
            default:
                return null;
        }
        return null;
    }
    public void PlaySFXClip(SFXType type, float volume = 1)
    {
        AudioSource sfxSource = Instantiate(_sfxSoundPrefab);
    }

}
