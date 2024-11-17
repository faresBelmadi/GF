using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuInterfaceOption : MonoBehaviour
{

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

}
