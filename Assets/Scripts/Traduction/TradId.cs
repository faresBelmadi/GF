using System;

[Serializable]
public class TradId
{
    public string Id;

    public string DefaultText;

    public string Text => TradManager.instance?.GetTranslation(Id) ?? DefaultText;
}
