using Codice.Client.BaseCommands.Differences;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHolder
{
    private readonly CharacterStat _baseStat;

    public int BaseRadianceMax => _baseStat.RadianceMax;
    public int BaseForceDame => _baseStat.ForceAmeOriginal;
    public int BaseVitesse => _baseStat.VitesseOriginal;
    public int BaseConviction => _baseStat.ConvictionOriginal;
    public int BaseCalme => _baseStat.Calme;
    public int BaseResilience => _baseStat.ResilienceOriginal;
    

    private int     _radiance;
    private int     _radianceMax;
    private int     _forceAme;
    private int     _forceAmeBonus;
    private int     _vitesse;
    private int     _conviction;
    private int     _calme;
    private int     _resilience;
    private int     _resiliencePassif;
    private int     _essence;
    private float   _multiplDef;
    private float   _multiplSoin;
    private float   _multiplDegat;
    private float   _multiplBuffDebuff;
    private float   _multiplTension;
    private float   _tension;
    private float   _tensionMax;
    private int     _valeurPalier;
    private int     _palierChangement;
    private bool    _isStun;


    public event Action OnConvictionChanged;

    public StatsHolder(CharacterStat charStat)
    {
        _forceAme   = charStat.ForceAme;
        _vitesse    = charStat.Vitesse;
        _conviction = charStat.Conviction;
        _calme      = charStat.Calme;
    }

    public void UpdateStat(CharacterStat charStatModifier)
    {
        if (charStatModifier.MultiplDef != 1)
        {
            if (charStatModifier.MultiplDef > 1)
            {
                var multiplicateurDef = charStatModifier.MultiplDef % 1;
                _multiplDef += multiplicateurDef;

            }
            else
            {
                var multiplicateurDef = charStatModifier.MultiplDef - 1;
                _multiplDef += multiplicateurDef;
            }
        }
        if (charStatModifier.MultiplSoin != 1)
        {
            if (charStatModifier.MultiplSoin > 1)
            {
                var multiplicateurSoin = charStatModifier.MultiplSoin % 1;
                _multiplSoin += multiplicateurSoin;
            }
            else
            {
                var multiplicateurSoin = charStatModifier.MultiplSoin - 1;
                _multiplSoin += multiplicateurSoin;
            }
        }
        if (charStatModifier.MultiplDegat != 1)
        {
            if (charStatModifier.MultiplDegat > 1)
            {
                var multiplicateurDegat = charStatModifier.MultiplDegat % 1;
                _multiplDegat += multiplicateurDegat;

            }
            else
            {
                var multiplicateurDegat = charStatModifier.MultiplDegat - 1;
                _multiplDegat += multiplicateurDegat;
            }
        }
        if (charStatModifier.MultipleBuffDebuff != 1)
            _multiplBuffDebuff = charStatModifier.MultipleBuffDebuff;

        _radianceMax += charStatModifier.RadianceMax;
        _forceAme += charStatModifier._forceAme;
        _vitesse += charStatModifier.Vitesse;
        if ((_conviction > 0 && _conviction + charStatModifier.Conviction <= 0) && (_conviction < 0 && _conviction + charStatModifier.Conviction >= 0))
            OnConvictionChanged?.Invoke();
        _conviction += charStatModifier.Conviction;
        this._resilience += charStatModifier._resilience;
        _calme += charStatModifier.Calme;
        _essence += charStatModifier.Essence;
        _tension += charStatModifier.Tension * _multiplTension;
        _palierChangement += charStatModifier.PalierChangement;
        if (charStatModifier.Radiance < 0)
        {
            _radiance += Mathf.FloorToInt(charStatModifier.Radiance * _multiplDef);
        }
        else
        {
            _radiance += Mathf.FloorToInt(charStatModifier.Radiance * _multiplSoin);
        }

        _isStun = charStatModifier.isStun;
    }


    public void RectificationStat()
    {
        if (_radiance > _radianceMax && _radianceMax > 0)
        {
            _radiance = _radianceMax;
        }

        if (_conviction > GameManager.Instance.CommonStatsData.ConvictionMax)
        {
            _conviction = GameManager.Instance.CommonStatsData.ConvictionMax;
        }
        else if (_conviction < GameManager.Instance.CommonStatsData.ConvictionMin)
        {
            _conviction = GameManager.Instance.CommonStatsData.ConvictionMin;
        }

        if (_resilience > GameManager.Instance.CommonStatsData.ResilienceMax)
        {
            _resilience = GameManager.Instance.CommonStatsData.ResilienceMax - _resiliencePassif;
        }
        else if (_resilience < GameManager.Instance.CommonStatsData.ResilienceMin)
        {
            _resilience = GameManager.Instance.CommonStatsData.ResilienceMin + _resiliencePassif;
        }

        if (_forceAme < 0)
            _forceAme = 0;
    }

    internal void setZero()
    {
        _calme = 0;
        _conviction = 0;
        _resilience = 0;
        _essence = 0;
        _forceAme = 0;
        _radiance = 0;
        _resiliencePassif = 0;
        _radianceMax = 0;
    }

    public void removeStat(CharacterStat charStatModifier)
    {
        if (charStatModifier.MultiplDef != 1)
        {
            if (charStatModifier.MultiplDef > 1)
            {
                var multiplicateurDef = charStatModifier.MultiplDef % 1;
                _multiplDef -= multiplicateurDef;

            }
            else
            {
                var multiplicateurDef = charStatModifier.MultiplDef - 1;
                _multiplDef -= multiplicateurDef;
            }
        }

        if (charStatModifier.MultiplSoin != 1)
        {
            if (charStatModifier.MultiplSoin > 1)
            {
                var multiplicateurSoin = charStatModifier.MultiplSoin % 1;
                _multiplSoin -= multiplicateurSoin;

            }
            else
            {
                var multiplicateurSoin = charStatModifier.MultiplSoin - 1;
                _multiplSoin -= multiplicateurSoin;
            }
        }
        if (charStatModifier.MultiplDegat != 1)
        {
            if (charStatModifier.MultiplDegat > 1)
            {
                var multiplicateurAtk = charStatModifier.MultiplDegat % 1;
                _multiplDegat -= multiplicateurAtk;

            }
            else
            {
                var multiplicateurAtk = charStatModifier.MultiplDegat - 1;
                _multiplDegat -= multiplicateurAtk;
            }
        }
        if (charStatModifier.MultipleBuffDebuff != 1)
            _multiplBuffDebuff = charStatModifier.MultipleBuffDebuff;

        _radianceMax -= charStatModifier.RadianceMax;
        _forceAme -= charStatModifier.ForceAme;
        _vitesse -= charStatModifier.Vitesse;
        _conviction -= charStatModifier.Conviction;
        _resilience -= charStatModifier._resilience;
        _calme -= charStatModifier.Calme;
        _essence -= charStatModifier.Essence;
        _tension -= charStatModifier.Tension;

        if (charStatModifier.Radiance < 0)
        {
            _radiance -= Mathf.FloorToInt(charStatModifier.Radiance * _multiplDef);
        }
        else
        {
            _radiance -= Mathf.FloorToInt(charStatModifier.Radiance * _multiplSoin);
        }

        if (charStatModifier.isStun)
            _isStun = !charStatModifier.isStun;
    }

    public void ResetStat()
    {
        _radianceMax = BaseRadianceMax;
        _radiance = BaseRadianceMax;
        _forceAme = BaseForceDame;
        _vitesse = BaseVitesse;
        _conviction = BaseConviction;
        _resilience = BaseResilience;
        _calme = 0;
        _multiplDef = 1;
        _multiplSoin = 1;
        _multiplDegat = 1;
        _multiplBuffDebuff = 1;
        _tension = 0;
        _tensionMax = 0;
        _valeurPalier = 0;
        _palierChangement = 0;
        _isStun = false;

        // TODO
        /*this.nbAttaqueRecu = 0;
        this.ListBuffDebuff.Clear();*/
    }

}
