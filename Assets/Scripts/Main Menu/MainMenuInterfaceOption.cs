using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuInterfaceOption : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggleAnimation;

    private void OnEnable()
    {
        _toggleAnimation.isOn = PlayerPrefs.GetInt("AnimationSpeedUp") == 1;
    }

    public void SetAnimationSpeedUp(bool isChecked)
    {
        PlayerPrefs.SetInt("AnimationSpeedUp", isChecked ? 1 : 0);
    }
}
