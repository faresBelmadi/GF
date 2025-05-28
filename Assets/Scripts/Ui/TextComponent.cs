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
    [Tooltip("Line indent, in percent")]
    [SerializeField]
    private float _lineIndent = 0;

    public string IDLabel { get => _idLabel; }

    private void OnEnable()
    {
        TradManager.OnRefreshTranslation += RefreshText;
        if (TradManager.instance != null )
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
    public void InitTextComponent(string idLabel, string defaultText)
    {
        _idLabel = idLabel;
        _defaultText = defaultText;
        RefreshText();
    }
    public void RefreshText()
    {
        string text = $"<line-indent={_lineIndent}%>{TradManager.instance.GetTranslation(_idLabel, _defaultText)}";
        if (_textTMPObject != null)
        {
            _textTMPObject.text = text;
        }
        if (_textObject != null)
        {
            _textObject.text = text;
        }
    }

}
