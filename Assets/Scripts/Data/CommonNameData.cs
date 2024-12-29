using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = " new CommonNameData", menuName = "CommonNameData/Create New CommonNameData")]

public class CommonNameData : ScriptableObject
{
    [SerializeField]
    private string _idTradRadiance;
    [SerializeField]
    private string _idTradVitesse;
    [SerializeField]
    private string _idTradConviction;
    [SerializeField]
    private string _idTradResilience;
    [SerializeField]
    private string _idTradCalme;
    [SerializeField]
    private string _idTradForceDame;
    [SerializeField]
    private string _idTradConscience;
    [SerializeField]
    private string _idTradClairvoyance;
    [SerializeField]
    private string _idTradVolonte;
    [SerializeField]
    private string _idTradTension;
    [SerializeField]
    private string _idTradEssence;


    public string Radiance => TradManager.instance.GetTranslation(_idTradRadiance, "Radiance");
    public string Vitesse => TradManager.instance.GetTranslation(_idTradVitesse, "Speed");
    public string Conviction => TradManager.instance.GetTranslation(_idTradConviction, "Conviction");
    public string Resilience => TradManager.instance.GetTranslation(_idTradResilience, "Resilience");
    public string Calme => TradManager.instance.GetTranslation(_idTradCalme, "Calm");
    public string ForceDame => TradManager.instance.GetTranslation(_idTradForceDame, "Fortitude");
    public string Conscience => TradManager.instance.GetTranslation(_idTradConscience, "Conscience");
    public string Clairvoyance => TradManager.instance.GetTranslation(_idTradClairvoyance, "Insight");
    public string Volonte => TradManager.instance.GetTranslation(_idTradVolonte, "Willpower");
    public string Tension => TradManager.instance.GetTranslation(_idTradTension, "Tension");
    public string Essence => TradManager.instance.GetTranslation(_idTradEssence, "Essence");
}
