using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuInterfaceOption : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggleAnimation;
    private void OnEnable()
    {
        _toggleAnimation.isOn = GameManager.Instance.BattleMan.IsAnimationSpeedUp;
    }
    public void SetFrenchLanguage()
    {
        TradManager.instance.SetLanguage(TradManager.SUPPORTEDLANGUAGES.FR);
    }
    public void SetEnglishLanguage()
    {
        TradManager.instance.SetLanguage(TradManager.SUPPORTEDLANGUAGES.EN);
    }
    public void SetChineseLanguage()
    {
        TradManager.instance.SetLanguage(TradManager.SUPPORTEDLANGUAGES.ZH);
    }
    public void SetAnimationSpeedMultiplier(bool isChecked)
    {
        
        GameManager.Instance.BattleMan.SetAnimationSpeedMultiplier(isChecked);
    }

}
