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
    [SerializeField]
    private Toggle _masterToggle;
    [SerializeField]
    private Toggle _musicToggle;
    [SerializeField]
    private Toggle _SFXToggle;

    private void OnEnable()
    {
        _masterVolumeSlider.value = AudioManager.instance.MasterVolume;
        _musicVolumeSlider.value = AudioManager.instance.MusicVolume;
        _SFXVolumeSlider.value = AudioManager.instance.SFXVolume;

        _masterToggle.isOn = AudioManager.instance.IsMasterMute;
        _musicToggle.isOn = AudioManager.instance.IsMusicMute;
        _SFXToggle.isOn = AudioManager.instance.IsSFXMute;
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

    public void MuteUnmuteMaster(bool isMute)
    {
        _masterVolumeSlider.interactable = !isMute;
        AudioManager.instance.MuteUnmuteMaster(isMute);
    }
    public void MuteUnmuteMusic(bool isMute)
    {
        _musicVolumeSlider.interactable = !isMute;
        AudioManager.instance.MuteUnmuteMusic(isMute);
    }
    public void MuteUnmuteSFX(bool isMute)
    {
        _SFXVolumeSlider.interactable = !isMute;
        AudioManager.instance.MuteUnmuteSFX(isMute);
    }
}
