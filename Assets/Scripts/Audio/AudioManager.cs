using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public enum MixerGroup
    {
        Master,
        Music,
        SFX
    }
    public static AudioManager instance;

    [SerializeField]
    private SFXPlayer _sfx;
    [SerializeField]
    private MusicData _musicData;
    [SerializeField]
    private AudioMixer _audioMixer;

    public float MasterVolume { get; private set; }
    public float SFXVolume { get; private set; }
    public float MusicVolume { get; private set; }
    public SFXPlayer SFX => _sfx;
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
            DontDestroyOnLoad(this.gameObject.transform.parent.gameObject);
            
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume",0);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume",0);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume",0);

        SetVolume(MixerGroup.Master, MasterVolume);
        SetVolume(MixerGroup.Music, MusicVolume);
        SetVolume(MixerGroup.SFX, SFXVolume);

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError($"Missing audio source in audio manager : {gameObject.name}");
        }
        if (_sfx == null)
        {
            Debug.LogError($"Missing sfx player reference in audio manager : {gameObject.name}");
        }

        //On commence par jouer la musique du menu principal
        PlayMusic(MusicType.MainMenuMusic);
    }

    /// <summary>
    /// Joue la musique correspondant au type donné
    /// </summary>
    /// <param name="musicType">Le type de musique qu'on souhaite jouer</param>
    /// <param name="forceRestart">Si il est vrai, on va forcer la musique a se  rejouer depuis le début</param>
    public void PlayMusic(MusicType musicType, bool forceRestart = false)
    {
        AudioClip clip = _musicData.Clip(musicType);
        // Si la musique qu'on souhaite lancer est la meme que l'actuelle, on laisse la possibilité de la laisser se jouer ou de la restart
        if (clip != _audioSource.clip || forceRestart)
        {
            _audioSource.Stop();
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume(MixerGroup.Master, volume);
    }
    public void SetMusicVolume(float volume)
    {
        SetVolume(MixerGroup.Music, volume);
    }
    public void SetSFXVolume(float volume)
    {
        SetVolume(MixerGroup.SFX, volume);
    }

    public void SetVolume(MixerGroup group, float volume)
    {
        string parameter = "";
        if (group == MixerGroup.Master)
        {
            MasterVolume = volume;
            parameter = "MasterVolume";
        }
        else if (group == MixerGroup.Music)
        {
            MusicVolume = volume;
            parameter = "MusicVolume";
        }
        else if (group == MixerGroup.SFX)
        {
            SFXVolume = volume;
            parameter = "SFXVolume";
        }
        _audioMixer.SetFloat(parameter, volume);
        PlayerPrefs.SetFloat(parameter, volume);
    }
}
