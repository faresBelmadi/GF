using System;
using System.Collections.Generic;
using UnityEngine;

public enum  CustomStatPalierAction
{
    GainStat,
    ResetAndRefreshTension
}

[Serializable]
public class PalierAction
{
    public CustomStatPalierAction CustomAction;
    public int Value;
}


[CreateAssetMenu(fileName = "New Custom Stat Passiv", menuName = "PassiveEffect/New Custom Stat¨Passiv")]
public class CustomStatPassiv : AbstractPassive, IEffectOnStat<EnemyStatsHandler>, IDynamicEventPassive<EnemyStatsHandler>, IStartTurnPassive
{
    [Space]
    [Header("CustomStatPassiv")]
    [SerializeField]
    private List<PalierAction> _palierActions;
    [Tooltip("Chaque point de la custom stat donnera autant de points de stat que le ratio indique")]
    [SerializeField]
    private List<StatToModif> _conversionStat;
    private EnemyStatsHandler _ennemiStat;
    public void Apply(EnemyStatsHandler charStat)
    {
        int indPalier = UnityEngine.Random.Range(0, _palierActions.Count);

        switch (_palierActions[indPalier].CustomAction)
        {
            case CustomStatPalierAction.GainStat:
                Debug.Log("Gain Stat : " + _palierActions[indPalier].Value);
                charStat.CustomStat += _palierActions[indPalier].Value;
                break;
            case CustomStatPalierAction.ResetAndRefreshTension:
                Debug.Log("Reset");
                charStat.ChangeTension(Mathf.RoundToInt(charStat.CustomStat * _palierActions[indPalier].Value));
                charStat.CustomStat = 0;
                break;
        }
    }

    public void SubscribeEvents(EnemyStatsHandler stat)
    {
        _ennemiStat = (EnemyStatsHandler) stat;
        _ennemiStat.OnCustomStatModification += UpdateStat;
        
    }
    public void UnsubscribeEvents()
    {
        _ennemiStat.OnCustomStatModification -= UpdateStat;
    }

    public void UpdateStat()
    {
        foreach (var item in _conversionStat)
        {
            switch (item.Stat)
            {
                case BaseStats.RadianceMax:
                    var pourcentagePVActuel = (float)_ennemiStat.Radiance / (float)_ennemiStat.RadianceMax * 100f;
                    CharacterStat modifStat = ScriptableObject.CreateInstance<CharacterStat>();
                    modifStat.RadianceMax = _ennemiStat.BaseRadianceMax;
                    modifStat.RadianceMax += _ennemiStat.CustomStat * 10;
                    modifStat.Radiance = Mathf.FloorToInt(pourcentagePVActuel / 100 * _ennemiStat.RadianceMax); //TODO CHANGE RADIANCE
                    break;
                case BaseStats.ForceAme:
                    _ennemiStat.ChangeForceDame(_ennemiStat.BaseForceDame + _ennemiStat.CustomStat * 1);
                    break;
               
            }
        }
    }

}
