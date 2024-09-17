using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextComponent : MonoBehaviour
{
    [SerializeField]
    private string _idLabel;
    [SerializeField]
    private string _defaultText;
    [SerializeField]
    private TMP_Text _textTMPObject;
    [SerializeField]
    private Text _textObject;

    void Start()
    {
        if (_textTMPObject != null)
        {
            _textTMPObject.text = TradManager.instance.GetTranslation(_idLabel, _defaultText);
        }
        if (_textObject != null)
        {
            _textObject.text = TradManager.instance.GetTranslation(_idLabel, _defaultText);
        }
    }

}
