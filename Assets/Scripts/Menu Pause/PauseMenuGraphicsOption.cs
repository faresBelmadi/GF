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
    [SerializeField]
    private Toggle _vsyncToggle;

    private void OnEnable()
    {
        int height = PlayerPrefs.GetInt("ScreenHeight", 1080);
        int width = PlayerPrefs.GetInt("ScreenWidth", 1920);
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        bool vsync = PlayerPrefs.GetInt("Vsync", 0) == 1;

        _vsyncToggle.isOn = vsync;
        _fullscreenToggle.isOn = fullscreen;
        int valueDropdown = GetResolutionDropdownValue(width, height);
        _resolutionDropdown.value = valueDropdown;
       
    }
    private int GetResolutionDropdownValue(int width, int height)
    {
        if (width == 1280)
        {
            if (height == 720)
                return 1;           // 1280 x 720
            else
                return 3;           // 1280 x 800
        }
        else if (width ==1366)
        {
            return 2;               // 1366 x 768
        }
        else if (width == 1440)
        {
            return 4;               // 1440 x 960
        }
        else if (width == 2560)
        {
            return 5;               // 2560 x 1440
        }
        else
        {
            return 0;               // 1920 x 1080
        }
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
            case 2: // 1366 x 768
                width = 1366;
                height = 768;
                break;
            case 3: // 1280 x 800
                width = 1280;
                height = 800;
                break;
            case 4: // 1440 x 960
                width = 1440;
                height = 960;
                break;
            case 5: // 2560 x 1440
                width = 2560;
                height = 1440;
                break;
            default:
                width = 1920;
                height = 1080;
                break;
        }
        Debug.Log("Video Settings : ");
        Debug.Log("Set resolution to" + width + " x " + height + " in " + (_fullscreenToggle.isOn ? "fullscreen" : "windowed"));
        Debug.Log("VSync : " + (_vsyncToggle.isOn ? "on" : "off"));
        Screen.SetResolution(width, height, _fullscreenToggle.isOn);
        QualitySettings.vSyncCount = _vsyncToggle.isOn ? 1 : 0;

        PlayerPrefs.SetInt("ScreenHeight", height);
        PlayerPrefs.SetInt("ScreenWidth", width);
        PlayerPrefs.SetInt("Fullscreen", _fullscreenToggle.isOn?1:0);
        PlayerPrefs.SetInt("Vsync", _vsyncToggle?1:0);
    }
}
