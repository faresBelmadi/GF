using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuGraphicsOption : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _resolutionDropdown;
    [SerializeField]
    private Toggle _fullscreenToggle;

    private void OnEnable()
    {
        //TODO: Temporary FIX
        var option = _resolutionDropdown.options[0];
        _resolutionDropdown.ClearOptions();
        _resolutionDropdown.options.Add(option);
    }
    public void ApplyResolution()
    {
        int width, height;
        switch (_resolutionDropdown.value)
        {
            case 0: // 1920 x 1080
                width = 1920;
                height = 1080;
                break;
            case 1: // 1280 x 720
                width = 1280;
                height = 720;
                break;
            default:
                width = 1920;
                height = 1080;
                break;
        }
        Debug.Log("Set resolution to" + width + " x " + height + " in " + (_fullscreenToggle.isOn ? "fullscreen" : "windowed"));
        Screen.SetResolution(width, height, _fullscreenToggle.isOn);
    }
}
