using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoueurBehavior : CombatBehavior
{
    public JoueurStat Stat;

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

    public Spell SelectedSpell;

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
            temp.GetComponent<SpellCombat>().Action = item;
            temp.GetComponent<SpellCombat>().Act = DoAction;
            temp.GetComponent<SpellCombat>().StartUp();

            Spells.Add(temp);
        }
    }

    public void UpdateUI()
    {
        RadianceSlider.value = Stat.Radiance;
        RadianceSlider.minValue = 0;
        RadianceSlider.maxValue = Stat.RadianceMax;
        TensionSlider.value = Mathf.FloorToInt((Stat.Tension * Stat.NbPalier) / Stat.TensionMax);
        TensionSlider.maxValue = Stat.NbPalier;
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
        ResetStat();
        DecompteDebuffJoueur(Decompte.phase, TimerApplication.DebutPhase);
    }

    public void StartTurn()
    {
        DecompteDebuffJoueur(Decompte.tour, TimerApplication.DebutTour);
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
        Stat.MultiplDegat = 1;
        Stat.MultiplDef = 1;
        Stat.MultiplSoin = 1;
        Stat.MultipleBuffDebuff = 1;
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
        var t = (int)((Stat.Tension / (Stat.NbPalier * Stat.ValeurPalier)) * Stat.NbPalier);
        if (t >= Stat.NbPalier)
            t = Stat.NbPalier;
        else
            t++;

        Stat.Tension = t * Stat.ValeurPalier;
    }

    public void ApaisementTension()
    {

        var t = (int)((Stat.Tension / (Stat.NbPalier * Stat.ValeurPalier)) * Stat.NbPalier);
        if (t <= 0)
            t = 0;
        else
            t--;

        Stat.Tension = t * Stat.ValeurPalier;
    }

    public void ReceiveTension(Source sourceDamage)
    {
        switch (sourceDamage)
        {
            case Source.Attaque:
                Stat.Tension += Stat.TensionAttaque;
                break;
            case Source.Dot:
                Stat.Tension += Stat.TensionDot;
                break;
            case Source.Buff:
                Stat.Tension += Stat.TensionDebuff;
                break;
            case Source.Soin:
                Stat.Tension += Stat.TensionSoin;
                break;
        }
    }

    public bool CanHaveAnotherTurn()
    {
        if (Stat.Tension >= Stat.ValeurPalier * Stat.NbPalier)
        {
            Stat.Tension = Stat.ValeurPalier * Stat.NbPalier;
            return true;
        }

        return false;
    }

    #endregion Tension

    #region Spell

    private void DoAction(Spell toDo)
    {
        SelectedSpell = toDo;
        TakeTarget();
    }

    public void DesactivateSpells()
    {
        foreach (var item in Spells)
        {
            item.GetComponent<SpellCombat>().isTurn = false;
        }
        EndTurnButton.interactable = false;
    }

    public void ActivateSpells()
    {
        foreach (var item in Spells)
        {
            item.GetComponent<SpellCombat>().isTurn = true;
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
                case TypeCostSpell.conscience:
                    Stat.Conscience -= price.Value;
                    break;
                case TypeCostSpell.radiance:
                    Stat.Radiance -= price.Value;
                    break;
                case TypeCostSpell.volonte:
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

    private void AfterAnim()
    {
        //A Mettre une fois les combats terminer
        GameManager.instance.BattleMan.LaunchSpellJoueur(SelectedSpell);
        ActivateSpells();
        UpdateUI();
    }

    #endregion Spell

    #region BuffDebuff

    public void AddDebuff(BuffDebuff toAdd, Decompte Decompte, TimerApplication Timer)
    {
        for (int i = 0; i < Stat.MultipleBuffDebuff; i++)
        {
            if (toAdd.IsDebuff)
            {
                ReceiveTension(Source.Buff);
            }
            Stat.ListBuffDebuff.Add(Instantiate(toAdd));
            base.AddBuffDebuff(toAdd);
        }
       

        DecompteDebuffJoueur(Decompte, Timer);

        UpdateUI();
    }

    private void DecompteDebuffJoueur(Decompte Decompte, TimerApplication Timer)
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
            if(item.timerApplication == Timer || Timer == TimerApplication.Persistant)
            {
                foreach (var effet in item.Effet)
                {
                    GameManager.instance.BattleMan.PassageEffet(effet, item.IDCombatOrigine, 0, SourceEffet.BuffDebuff);
                    /*if(item.CibleApplication == effet.Cible)
                    {
                        ApplicationEffet(effet);
                    }
                    else
                    {
                        //A Mettre une fois les combats terminer
                        GameManagerRemake.instance.BattleMan.PassageEffet(effet, item.IDCombatOrigine);
                    }*/
                }
                if (item.IsConsomable == true)
                {
                    item.Temps = 0;
                    foreach (var ToAdd in item.Consomation)
                    {
                        AddDebuff(ToAdd, Decompte.none, TimerApplication.Persistant);
                    }
                }
            }
            
        }
    }

    #endregion BuffDebuff

    #region Effet

    public void ApplicationEffet(Effet effet, EnnemiStat Caster = null, SourceEffet source = SourceEffet.Spell, int idCaster = 0)
    {
        JoueurStat ModifStat;
        if (Caster == null)
        {
            ModifStat = effet.ResultEffet(Stat, LastDamageTaken, Cible:null);
        }
        else
        {
            ModifStat = effet.ResultEffet(Caster, LastDamageTaken,Stat);
        }

        Stat.ModifStateAll(ModifStat);

        if (ModifStat.PalierChangement > 0)
            EnervementTension();
        else if (ModifStat.PalierChangement < 0)
            ApaisementTension();

        if (ModifStat.Radiance < 0)
        {
            LastDamageTaken = ModifStat.Radiance;
            GameManager.instance.BattleMan.CurrentPhaseDamage += LastDamageTaken;

            if (LastDamageTaken < GameManager.instance.BattleMan.MostDamage)
            {
                GameManager.instance.BattleMan.MostDamage = LastDamageTaken;
                GameManager.instance.BattleMan.MostDamageID = idCaster;
            }

            if (source == SourceEffet.Spell)
                ReceiveTension(Source.Attaque);
            else if (source == SourceEffet.BuffDebuff)
                ReceiveTension(Source.Dot);

            var temp = Instantiate(DamagePrefab, DamageSpawn);
            temp.GetComponent<TextAnimDegats>().Value = ModifStat.Radiance;
        }
        else if(ModifStat.Radiance > 0)
        {
            ReceiveTension(Source.Soin);
            var temp = Instantiate(SoinPrefab, DamageSpawn);
            temp.GetComponent<TextAnimDegats>().Value = ModifStat.Radiance;
        }

        UpdateUI();

        if (Stat.Radiance <= 0)
        {
            Dead();
        }
    }

    #endregion Effet

    #region Essence

    public void UseEssence(int Essence, Source source)
    {
        Stat.Radiance += Essence;
        ReceiveTension(source);
    }

    #endregion Essence
}
