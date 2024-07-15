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

    private void OnEnable()
    {
        Debug.Log("Enable");
        _masterVolumeSlider.value = AudioManager.instance.MasterVolume;
        _musicVolumeSlider.value = AudioManager.instance.MusicVolume;
        _SFXVolumeSlider.value = AudioManager.instance.SFXVolume;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
