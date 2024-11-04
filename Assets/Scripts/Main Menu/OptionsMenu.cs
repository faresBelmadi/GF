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

    private void Start()
    {
        Debug.Log("Add Listeners");
        _masterVolumeSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetMasterVolume(_masterVolumeSlider.value); });
        _musicVolumeSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetMusicVolume(_musicVolumeSlider.value); });
        _SFXVolumeSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetSFXVolume(_SFXVolumeSlider.value); });

        _masterToggle.onValueChanged.AddListener(delegate { MuteUnmuteMaster(_masterToggle.isOn); });
        _musicToggle.onValueChanged.AddListener(delegate { MuteUnmuteMusic(_musicToggle.isOn); });
        _SFXToggle.onValueChanged.AddListener(delegate { MuteUnmuteSFX(_SFXToggle.isOn); });
    }

    private void OnEnable()
    {
        _masterVolumeSlider.value = AudioManager.instance.MasterVolume;
        _musicVolumeSlider.value = AudioManager.instance.MusicVolume;
        _SFXVolumeSlider.value = AudioManager.instance.SFXVolume;

        _masterToggle.isOn = AudioManager.instance.IsMasterMute;
        //_masterToggle.graphic.color = new Color(1f, 1f, 1f, AudioManager.instance.IsMasterMute ? 0f : 1f);

        _musicToggle.isOn = AudioManager.instance.IsMusicMute;
        //_musicToggle.graphic.color = new Color(1f, 1f, 1f, AudioManager.instance.IsMusicMute ? 0f : 1f);

        _SFXToggle.isOn = AudioManager.instance.IsSFXMute;
        //_SFXToggle.graphic.color = new Color(1f, 1f, 1f, AudioManager.instance.IsSFXMute ? 0f : 1f);
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
