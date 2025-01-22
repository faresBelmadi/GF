using UnityEngine;

[CreateAssetMenu(fileName = " new CommonDescData", menuName = "CommonData/Create New CommonDescData")]
public class CommonDescData : ScriptableObject
{
    [SerializeField]
    private string _idTradAttLegere;
    [SerializeField]
    private string _idTradAttLourde;
    [SerializeField]
    private string _idTradBuff;
    [SerializeField]
    private string _idTradDebuff;
    [SerializeField]
    private string _idTradBuffMultAtt;
    [SerializeField]
    private string _idTradDebuffMultAtt;
    [SerializeField]
    private string _idTradBuffMultDef;
    [SerializeField]
    private string _idTradDebuffMultDef;
    [SerializeField]
    private string _idTradBuffMultHeal;
    [SerializeField]
    private string _idTradDebuffMultHeal;
    [SerializeField]
    private string _idTradDegat;
    [SerializeField]
    private string _idTradColere;


    public string IdTradAttLegere => TradManager.instance.GetTranslation(_idTradAttLegere, "A light attack will be performed.");
    public string IdTradAttLourde => TradManager.instance.GetTranslation(_idTradAttLourde, "A heavy attack will be performed.");
    public string IdTradBuff => TradManager.instance.GetTranslation(_idTradBuff, "A good effect will be applied.");
    public string IdTradDebuff => TradManager.instance.GetTranslation(_idTradDebuff, "A bad effect will be applied.");
    public string IdTradBuffMultAtt => TradManager.instance.GetTranslation(_idTradBuffMultAtt, "A bonus/malus multiplier on attack.");
    public string IdTradDebuffMultAtt => TradManager.instance.GetTranslation(_idTradDebuffMultAtt, "A bonus/malus multiplier on attack.");
    public string IdTradBuffMultDef => TradManager.instance.GetTranslation(_idTradBuffMultDef, "A bonus/malus multiplier on defense.");
    public string IdTradDebuffMultDef => TradManager.instance.GetTranslation(_idTradDebuffMultDef, "A bonus/malus multiplier on defense.");
    public string IdTradBuffMultHeal => TradManager.instance.GetTranslation(_idTradBuffMultHeal, "A bonus/malus multiplier on healing.");
    public string IdTradDebuffMultHeal => TradManager.instance.GetTranslation(_idTradDebuffMultHeal, "A bonus/malus multiplier on healing.");
    public string IdTradDegat => TradManager.instance.GetTranslation(_idTradDegat, "Damage will be inflicted.");
    public string IdTradColere => TradManager.instance.GetTranslation(_idTradColere, "Wrath provides a chance to deal an offensive attack.");
}
