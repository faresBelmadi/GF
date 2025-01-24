using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider _masterVolumeSlider;
    [SerializeField]
    private Slider _musicVolumeSlider;
    [SerializeField]
    private Slider _SFXVolumeSlider;


    private void Start()
    {
        _masterVolumeSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetMasterVolume(_masterVolumeSlider.value); });
        _musicVolumeSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetMusicVolume(_musicVolumeSlider.value); });
        _SFXVolumeSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetSFXVolume(_SFXVolumeSlider.value); });

       
    }

    private void OnEnable()
    {
        _masterVolumeSlider.value = AudioManager.instance.MasterVolume;
        _musicVolumeSlider.value = AudioManager.instance.MusicVolume;
        _SFXVolumeSlider.value = AudioManager.instance.SFXVolume;
    }


    public void SaveMasterVolume()
    {
        AudioManager.instance.SetMasterVolume(_masterVolumeSlider.value);
    }
    public void SaveMusicVolume()
    {
        AudioManager.instance.SetMusicVolume(_musicVolumeSlider.value);
    }
    public void SaveSFXVolume()
    {
        AudioManager.instance.SetSFXVolume(_SFXVolumeSlider.value);
    }

  
}
