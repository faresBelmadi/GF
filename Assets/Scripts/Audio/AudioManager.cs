using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private MusicData _musicData;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

  

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError($"Missing audio source in audio manager : {gameObject.name}");
        }

        //On commence par jouer la musique du menu principal
        PlayMusic(MusicType.MainMenuMusic);
    }

   
    public void PlayMusic(MusicType musicType)
    {
        _audioSource.Stop();

        AudioClip clip = _musicData.Clip(musicType);
        _audioSource.clip = clip;

        _audioSource.Play();
    }
}
