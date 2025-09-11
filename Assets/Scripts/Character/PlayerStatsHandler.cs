using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

[Serializable]
public class PlayerStatsHandler : StatsHandler<JoueurStat>
{
    private class PercentIncrease
    {
        public StatEnum StatEnum;
        public float value;
    }
    #region BASE STATS
    public int BaseConscienceMax => _baseStat.ConscienceMax;
    public int BaseClairvoyance => _baseStat.ClairvoyanceOriginal;
    #endregion

    #region STATS PROPERTY
    [field: SerializeField, ReadOnly] public int Lvl { get; private set; }
    [field: SerializeField, ReadOnly] public int Volonte { get; set; }
    [field: SerializeField, ReadOnly] public int VolonteMax { get; set; }
    [field: SerializeField, ReadOnly] public int Conscience { get; set; }
    [field: SerializeField, ReadOnly] public int ConscienceMax { get; private set; }
    [field: SerializeField, ReadOnly] public int Clairvoyance { get; private set; }
    [field: SerializeField, ReadOnly] public List<Spell> ListSpell { get; set; } = new List<Spell>();
    [field: SerializeField, ReadOnly] public int SlotsSouvenir { get; set; }
    [field: SerializeField, ReadOnly] public List<Souvenir> ListSouvenir { get; set; } = new List<Souvenir>();
    #endregion

    private List<PercentIncrease> _percentIncreaseList = new List<PercentIncrease>();
    #region EVENTS
    public event Action OnConscienceIncrease;
    public event Action OnConscienceDecrease;
    #endregion
    public PlayerStatsHandler(PlayerStatsHandler playerStat) : base(playerStat)
    {
        Lvl = playerStat.Lvl;
        Volonte = playerStat.Volonte;
        VolonteMax = playerStat.VolonteMax;
        Conscience = playerStat.Conscience;
        ConscienceMax = playerStat.ConscienceMax;
        Clairvoyance = playerStat.Clairvoyance;

        ListSpell = new List<Spell>(playerStat.ListSpell);
        SlotsSouvenir = playerStat.SlotsSouvenir;
        ListSouvenir = new List<Souvenir>(playerStat.ListSouvenir);
        _baseStat = ScriptableObject.Instantiate<JoueurStat>(playerStat.BaseStat);
    }
    public PlayerStatsHandler(JoueurStat charStat) : base(charStat)
    {
        Lvl = charStat.Lvl;
        Volonte = charStat.Volonter;
        VolonteMax = charStat.VolonterMax;
        Conscience = charStat.Conscience;
        ConscienceMax = charStat.ConscienceMax;
        Clairvoyance = charStat.Clairvoyance;

        ListSpell = new List<Spell>(charStat.ListSpell);
        SlotsSouvenir = charStat.SlotsSouvenir;
        ListSouvenir = new List<Souvenir>(charStat.ListSouvenir);

        _baseStat = charStat;
    }
    private void UpdatePercentIncrease()
    {
        foreach (var souv in ListSouvenir )
        {
            foreach (var modif in souv.ModificationStat)
            {
                if (modif.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                {
                    modif.ParametreModifStat.ValeurModifier = Mathf.FloorToInt(GetPercentValue(modif.StatModif, modif.ParametreModifStat.Valeur));
                }
                
            }
        }
    }
    public override void UpdateBaseStat(JoueurStat modifStat)
    {
        Debug.Log($"Player Base Stat Modification for player {_baseStat.name}");
        //Base Stat Modification
        _baseStat.ModifStateAll(modifStat);
        //Current Stat modification;
        UpdateStat(modifStat);
    }
    public void UpdateStat(JoueurStat charStatModifier)
    {
        base.UpdateStat(charStatModifier);

        Volonte += charStatModifier.Volonter;
        VolonteMax += charStatModifier.VolonterMax;

        Conscience += charStatModifier.Conscience;
        ConscienceMax += charStatModifier.ConscienceMax;
        Clairvoyance += charStatModifier.Clairvoyance;


       
        RectificationStat();

        //La conscience a été modif, on notifie
        if (charStatModifier.Conscience > 0 || charStatModifier.ConscienceMax > 0)
        {
            OnConscienceIncrease?.Invoke();
        }
        else if (charStatModifier.Conscience < 0 || charStatModifier.ConscienceMax < 0)
        {
            OnConscienceDecrease?.Invoke();
        }
    }

    public override void RectificationStat()
    {
        if (Volonte > VolonteMax)
        {
            Volonte = VolonteMax;
        }
        if (Conscience > ConscienceMax)
        {
            Conscience = ConscienceMax;
        }
        if (Conscience < 0)
        {
            Conscience = 0;
        }
        base.RectificationStat();
    }
    public override void SetZero()
    {
        Volonte = 0;
        Conscience = 0;
        ConscienceMax = 0;
        Clairvoyance = 0;
        VolonteMax = 0;

        base.SetZero();
    }
    public override void ResetStat()
    {
        Clairvoyance = BaseClairvoyance;
        base.ResetStat();
    }
}