using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuVolumeOption : MonoBehaviour
{
    [SerializeField]
    private Slider _masterSlider;
    [SerializeField]
    private Slider _musicSlider;
    [SerializeField]
    private Slider _sfxSlider;
    private void OnEnable()
    {
        _masterSlider.value = AudioManager.instance.MasterVolume;
        _musicSlider.value = AudioManager.instance.MusicVolume;
        _sfxSlider.value = AudioManager.instance.SFXVolume;
    }
    // Start is called before the first frame update
    void Start()
    {
        _masterSlider.value = AudioManager.instance.MasterVolume;
        _musicSlider.value = AudioManager.instance.MusicVolume;
        _sfxSlider.value = AudioManager.instance.SFXVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterVolume(float volume)
    {
        AudioManager.instance.SetMasterVolume(volume);
    }
    public void SetMusicVolume(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
    }
    public void SetSFXVolume(float volume)
    {
        AudioManager.instance.SetSFXVolume(volume);
    }
        
}
