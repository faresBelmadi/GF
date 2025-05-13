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
public class CustomStatPassiv : AbstractPassive, IUpdateStatPassive, IStartTurnPassive
{
    [Space]
    [Header("CustomStatPassiv")]
    [SerializeField]
    private List<PalierAction> _palierActions;
    [Tooltip("Chaque point de la custom stat donnera autant de points de stat que le ratio indique")]
    [SerializeField]
    private List<StatToModif> _conversionStat;
    private EnnemiStat _ennemiStat;
    public void Apply(CharacterStat charStat)
    {
        int indPalier = UnityEngine.Random.Range(0, _palierActions.Count);

        switch (_palierActions[indPalier].CustomAction)
        {
            case CustomStatPalierAction.GainStat:
                Debug.Log("Gain Stat : " + _palierActions[indPalier].Value);
                ((EnnemiStat)charStat).CustomStat += _palierActions[indPalier].Value;
                break;
            case CustomStatPalierAction.ResetAndRefreshTension:
                Debug.Log("Reset");
                charStat.Tension += Mathf.RoundToInt(((EnnemiStat)charStat).CustomStat * _palierActions[indPalier].Value);
                ((EnnemiStat)charStat).CustomStat = 0;
                break;
        }
    }

    public void InitPassif(CharacterStat stat)
    {
        _ennemiStat = stat as EnnemiStat;
        _ennemiStat.OnCustomStatModification += UpdateStat;
        
    }
    public void Clear()
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
                    _ennemiStat.RadianceMax = _ennemiStat.RadianceMaxOriginal;
                    _ennemiStat.RadianceMax += _ennemiStat.CustomStat * 10;
                    _ennemiStat.Radiance = Mathf.FloorToInt(pourcentagePVActuel / 100 * _ennemiStat.RadianceMax);
                    break;
                case BaseStats.ForceAme:
                    _ennemiStat.ForceAme = _ennemiStat.ForceAmeOriginal;
                    _ennemiStat.ForceAme += _ennemiStat.CustomStat * 1;
                    break;
               
            }
        }
    }
}
