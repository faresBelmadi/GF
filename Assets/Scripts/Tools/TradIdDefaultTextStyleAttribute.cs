using UnityEngine;

public enum TradIdDefaultTextStyles
{
    TextField,
    TextArea
}

public class DefaultTextStyleAttribute : PropertyAttribute
{
    public TradIdDefaultTextStyles Style { get; }
    public DefaultTextStyleAttribute(TradIdDefaultTextStyles style = TradIdDefaultTextStyles.TextField)
    {
        Style = style;
    }
}