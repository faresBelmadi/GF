﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoueurBehavior : CombatBehavior
{
    public JoueurStatRemake Stat;

    //public List<BuffDebuff> debuffs = new List<BuffDebuff>();
    //public List<GameObject> ListBuff = new List<GameObject>();

    public List<GameObject> Spells;
    public Transform DamageSpawn;
    public GameObject DamagePrefab;
    public GameObject SoinPrefab;
    public GameObject SpellPrefab;
    public GameObject SpellsSpawn;
    public Button EndTurnButton;

    public Slider RadianceSlider;
    public Slider VolonteSlider;
    public Slider TensionSlider;
    public Slider ConscienceSlider;

    public TextMeshProUGUI TensionText;
    public TextMeshProUGUI HpText;
    public TextMeshProUGUI VolontéText;
    public TextMeshProUGUI ConscienceText;
    public TextMeshProUGUI StatSpeedText;
    public TextMeshProUGUI StatForceAmeText;
    public TextMeshProUGUI StatResilienceText;
    public TextMeshProUGUI StatClairvoyanceText;
    public TextMeshProUGUI StatConvictionText;

    public SpellRemake SelectedSpell;

    public AnimationControllerAttack AnimationController;

    #region Divers start & fin

    public void StartUp()
    {
        Stat.RadianceMaxOriginal = Stat.RadianceMax;
        Stat.VitesseOriginal = Stat.Vitesse;
        Stat.ClairvoyanceOriginal = Stat.Clairvoyance;
        Stat.ResilienceOriginal = Stat.Resilience;
        Stat.ConvictionOriginal = Stat.Conviction;
        Stat.ForceAmeOriginal = Stat.ForceAme;
        foreach (var item in Stat.ListSpell)
        {
            var temp = Instantiate(SpellPrefab, SpellsSpawn.transform);
            temp.GetComponent<SpellCombatRemake>().Action = item;
            temp.GetComponent<SpellCombatRemake>().Act = DoAction;
            temp.GetComponent<SpellCombatRemake>().StartUp();

            Spells.Add(temp);
        }
    }

    public void UpdateUI()
    {
        RadianceSlider.value = Stat.Radiance;
        RadianceSlider.minValue = 0;
        RadianceSlider.maxValue = Stat.RadianceMax;
        TensionSlider.value = Mathf.FloorToInt((Tension * NbPalier) / TensionMax);
        TensionSlider.maxValue = NbPalier;
        VolonteSlider.value = Stat.Volonter;
        VolonteSlider.maxValue = Stat.VolonterMax;
        ConscienceSlider.value = Stat.Conscience;
        ConscienceSlider.maxValue = Stat.ConscienceMax;

        HpText.text = Stat.Radiance + "/" + Stat.RadianceMax;

        ConscienceText.text = Stat.Conscience + "/" + Stat.ConscienceMax;

        StatClairvoyanceText.text = Stat.Clairvoyance + "";

        if (Stat.Clairvoyance > Stat.ClairvoyanceOriginal)
            StatClairvoyanceText.color = Color.green;
        else if (Stat.Clairvoyance < Stat.ClairvoyanceOriginal)
            StatClairvoyanceText.color = Color.red;
        else
            StatClairvoyanceText.color = Color.black;


        StatForceAmeText.text = Stat.ForceAme + "";

        if (Stat.ForceAme > Stat.ForceAmeOriginal)
            StatForceAmeText.color = Color.green;
        else if (Stat.ForceAme < Stat.ForceAmeOriginal)
            StatForceAmeText.color = Color.red;
        else
            StatForceAmeText.color = Color.black;

        StatSpeedText.text = Stat.Vitesse + "";

        if (Stat.Vitesse > Stat.VitesseOriginal)
            StatSpeedText.color = Color.green;
        else if (Stat.Vitesse < Stat.VitesseOriginal)
            StatSpeedText.color = Color.red;
        else
            StatSpeedText.color = Color.black;

        StatConvictionText.text = Stat.Conviction + "";

        if (Stat.Conviction > Stat.ConvictionOriginal)
            StatConvictionText.color = Color.green;
        else if (Stat.Conviction < Stat.ConvictionOriginal)
            StatConvictionText.color = Color.red;
        else
            StatConvictionText.color = Color.black;

        StatResilienceText.text = Stat.Resilience + "";

        if (Stat.Resilience > Stat.ResilienceOriginal)
            StatResilienceText.color = Color.green;
        else if (Stat.Resilience < Stat.ResilienceOriginal)
            StatResilienceText.color = Color.red;
        else
            StatResilienceText.color = Color.black;

    }

    public void StartPhase()
    {
        DecompteDebuffJoueur(DecompteRemake.phase, TimerApplication.DebutPhase);
    }

    public void StartTurn()
    {
        DecompteDebuffJoueur(DecompteRemake.tour, TimerApplication.DebutTour);
        ActivateSpells();
        Stat.Volonter = Stat.VolonterMax;
        UpdateUI();
    }

    public void EndTurn()
    {
        DesactivateSpells();
        EndTurnBM();
    }

    public void ResetStat()
    {
        Stat.RadianceMax = Stat.RadianceMaxOriginal;
        Stat.Vitesse = Stat.VitesseOriginal;
        Stat.Clairvoyance = Stat.ClairvoyanceOriginal;
        Stat.Resilience = Stat.ResilienceOriginal;
        Stat.ForceAme = Stat.ForceAmeOriginal;
        Stat.Conviction = Stat.ConvictionOriginal;
    }

    void Dead()
    {
        GameManager.instance.BattleMan.DeadPlayer();
    }

    #endregion Divers start & fin

    #region Tension

    public void EnervementTension()
    {
        var t = (int)((Tension / (NbPalier * ValeurPalier)) * NbPalier);
        if (t >= NbPalier)
            t = NbPalier;
        else
            t++;

        Tension = t * ValeurPalier;
    }

    public void ApaisementTension()
    {

        var t = (int)((Tension / (NbPalier * ValeurPalier)) * NbPalier);
        if (t <= 0)
            t = 0;
        else
            t--;

        Tension = t * ValeurPalier;
    }

    public void ReceiveTension(Source sourceDamage)
    {
        switch (sourceDamage)
        {
            case Source.Attaque:
                Tension += TensionAttaque;
                break;
            case Source.Dot:
                Tension += TensionDot;
                break;
            case Source.Buff:
                Tension += TensionDebuff;
                break;
            case Source.Soin:
                Tension += TensionSoin;
                break;
        }
    }

    public bool CanHaveAnotherTurn()
    {
        if (Tension >= ValeurPalier * NbPalier)
        {
            Tension = ValeurPalier * NbPalier;
            return true;
        }

        return false;
    }

    #endregion Tension

    #region Spell

    private void DoAction(SpellRemake toDo)
    {
        SelectedSpell = toDo;
        TakeTarget();
    }

    public void DesactivateSpells()
    {
        foreach (var item in Spells)
        {
            item.GetComponent<SpellCombatRemake>().isTurn = false;
        }
        EndTurnButton.interactable = false;
    }

    public void ActivateSpells()
    {
        foreach (var item in Spells)
        {
            item.GetComponent<SpellCombatRemake>().isTurn = true;
        }
        EndTurnButton.interactable = true;
    }

    private void TakeTarget()
    {
        GameManager.instance.BattleMan.StartTargeting();
    }

    public void Costs()
    {
        foreach (var price in SelectedSpell.Costs)
        {
            switch (price.typeCost)
            {
                case TypeCostSpellRemake.conscience:
                    Stat.Conscience -= price.Value;
                    break;
                case TypeCostSpellRemake.radiance:
                    Stat.Radiance -= price.Value;
                    break;
                case TypeCostSpellRemake.volonte:
                    Stat.Volonter -= price.Value;
                    break;
            }
        }
    }

    public void SendSpell()
    {
        Costs();
        DesactivateSpells();
        AnimationController.StartAttack(AfterAnim);
        GameManager.instance.BattleMan.LaunchAnimAttacked();
    }

    void AfterAnim()
    {
        //A Mettre une fois les combats terminer
        //GameManager.instance.BattleMan.LaunchSpell(SelectedSpell);
        ActivateSpells();
        UpdateUI();
    }

    #endregion Spell

    #region BuffDebuff

    public void AddDebuff(BuffDebuffRemake toAdd, DecompteRemake Decompte, TimerApplication Timer)
    {
        if(toAdd.IsDebuff)
        {
            ReceiveTension(Source.Buff);
        }
        Stat.ListBuffDebuff.Add(Instantiate(toAdd));
        base.AddBuffDebuff(toAdd);

        DecompteDebuffJoueur(Decompte, Timer);

        UpdateUI();
    }

    private void DecompteDebuffJoueur(DecompteRemake Decompte, TimerApplication Timer)
    {
        Stat.ListBuffDebuff = DecompteDebuff(Stat.ListBuffDebuff, Decompte);

        ApplicationBuffDebuff(Timer);

        UpdateUI();
    }

    public void ApplicationBuffDebuff(TimerApplication Timer)
    {
        ResetStat();
        foreach (var item in Stat.ListBuffDebuff)
        {
            if(item.Activate == true && (item.timerApplication == Timer || item.timerApplication == TimerApplication.Persistant))
            {
                foreach (var effet in item.Effet)
                {
                    if(item.CibleApplication == effet.Cible)
                    {
                        ApplicationEffet(effet);
                    }
                    else
                    {
                        //A Mettre une fois les combats terminer
                        //GameManager.instance.BattleMan.PassageEffet(effet, item.IDCombatOrigine);
                    }
                }
            }
            else
            {
                item.Activate = true;
            }
        }
    }

    #endregion BuffDebuff

    #region Effet

    public void ApplicationEffet(EffetRemake effet)
    {
        var ModifStat = effet.ResultEffet(Stat);
        Stat.ModifStateAll(ModifStat);
    }

    #endregion Effet
}
