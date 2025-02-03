using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private List<TabOptionButton> _tabs;
    [SerializeField]
    private List<GameObject> _optionPanel;

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
        //_masterVolumeSlider.value = AudioManager.instance.MasterVolume;
        //_musicVolumeSlider.value = AudioManager.instance.MusicVolume;
        //_SFXVolumeSlider.value = AudioManager.instance.SFXVolume;

        foreach(var tabButton in _tabs)
        {
            tabButton.Unselect();
        }
        foreach (var panel in _optionPanel)
        {
            panel.SetActive(false);
        }
        _tabs[0].Select();
        _optionPanel[0].SetActive(true);
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

    public void ShowGiventTab(int indiceTab)
    {
        foreach (var tabButton in _tabs)
        {
            tabButton.Unselect();
        }
        foreach (var panel in _optionPanel)
        {
            panel.SetActive(false);
        }
        _tabs[indiceTab].Select();
        _optionPanel[indiceTab].SetActive(true);
    }
  
}
