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

    public string IDLabel { get => _idLabel; }

    private void OnEnable()
    {
        TradManager.OnRefreshTranslation += RefreshText;
        
        RefreshText();
    }
    private void OnDisable()
    {
        TradManager.OnRefreshTranslation -= RefreshText;
    }
    void Start()
    {
        RefreshText();
    }
    public void RefreshText()
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
