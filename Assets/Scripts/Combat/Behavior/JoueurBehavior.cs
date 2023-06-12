using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

    private BattleManager _refBattleMan;
    public bool IsTurn;

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

        var green = new Color(147, 250, 165);
        var red = new Color(254, 121, 104);

        if (Stat.Clairvoyance > Stat.ClairvoyanceOriginal)
            StatClairvoyanceText.color = green;
        else if (Stat.Clairvoyance < Stat.ClairvoyanceOriginal)
            StatClairvoyanceText.color = red;
        else
            StatClairvoyanceText.color = Color.white;


        StatForceAmeText.text = Stat.ForceAme + "";

        if (Stat.ForceAme > Stat.ForceAmeOriginal)
            StatForceAmeText.color = green;
        else if (Stat.ForceAme < Stat.ForceAmeOriginal)
            StatForceAmeText.color = red;
        else
            StatForceAmeText.color = Color.white;

        StatSpeedText.text = Stat.Vitesse + "";

        if (Stat.Vitesse > Stat.VitesseOriginal)
            StatSpeedText.color = green;
        else if (Stat.Vitesse < Stat.VitesseOriginal)
            StatSpeedText.color = red;
        else
            StatSpeedText.color = Color.white;

        StatConvictionText.text = Stat.Conviction + "";

        if (Stat.Conviction > Stat.ConvictionOriginal)
            StatConvictionText.color = green;
        else if (Stat.Conviction < Stat.ConvictionOriginal)
            StatConvictionText.color = red;
        else
            StatConvictionText.color = Color.white;

        StatResilienceText.text = Stat.Resilience + "";

        if (Stat.Resilience > Stat.ResilienceOriginal)
            StatResilienceText.color = green;
        else if (Stat.Resilience < Stat.ResilienceOriginal)
            StatResilienceText.color = red;
        else
            StatResilienceText.color = Color.white;

    }

    public void StartPhase()
    {
        _refBattleMan = GameManager.instance.BattleMan;
        //ResetStat();
        DecompteDebuffJoueur(Decompte.phase, TimerApplication.DebutPhase);
        UpdateUI();
    }

    public void StartTurn()
    {
        IsTurn = true;
        DecompteDebuffJoueur(Decompte.tour, TimerApplication.DebutTour);
        _refBattleMan.PassifManager.CurrentEvent = TimerPassif.DebutTour;
        _refBattleMan.PassifManager.ResolvePassifs();
        ActivateSpells();
        Stat.Volonter = Stat.VolonterMax;
        if(!gainedTension)
        {
            ApaisementTension();
        }
        gainedTension = false;
        UpdateUI();
    }

    public void EndTurn()
    {
        _refBattleMan.PassifManager.CurrentEvent = TimerPassif.FinTour;
        _refBattleMan.PassifManager.ResolvePassifs();
        IsTurn = false;
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
        _refBattleMan.DeadPlayer();
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
                gainedTension = true;
                break;
            case Source.Dot:
                Stat.Tension += Stat.TensionDot;
                gainedTension = true;
                break;
            case Source.Buff:
                Stat.Tension += Stat.TensionDebuff;
                gainedTension = true;
                break;
            case Source.Soin:
                Stat.Tension += Stat.TensionSoin;
                gainedTension = true;
                break;
        }
        
        if (Stat.Tension >= Stat.ValeurPalier * Stat.NbPalier)
            Stat.Tension = Stat.ValeurPalier * Stat.NbPalier;
        if(Stat.Tension < 0)
            Stat.Tension = 0;
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
        _refBattleMan.StartTargeting();
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
        _refBattleMan.LaunchAnimAttacked();
    }

    private void AfterAnim()
    {
        //A Mettre une fois les combats terminer
        _refBattleMan.LaunchSpellJoueur(SelectedSpell);
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
       

        //DecompteDebuffJoueur(Decompte, Timer);
        
        ApplicationBuffDebuff(Timer);

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
            if(item.timerApplication == Timer || item.timerApplication == TimerApplication.Persistant || item.DirectApplication)
            {
                foreach (var effet in item.Effet)
                {
                    _refBattleMan.PassageEffet(effet, item.IDCombatOrigine, 0, SourceEffet.BuffDebuff);
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
                if (item.DirectApplication)
                    item.DirectApplication = false;
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
            var caster = _refBattleMan.EnemyScripts.Where(x => x.combatID == idCaster).FirstOrDefault();
            if (caster != null)
                ModifStat = effet.ResultEffet(caster.Stat, LastDamageTaken);
            else
                ModifStat = effet.ResultEffet(Stat, LastDamageTaken, Cible:null);
        }
        else
        {
            ModifStat = effet.ResultEffet(Caster, LastDamageTaken,Stat);
        }

        if (ModifStat.Radiance < 0)
        {
            var toRemove = Mathf.FloorToInt(ModifStat.Radiance / Stat.MultiplDef);
            toRemove -= Mathf.FloorToInt(((Stat.Resilience * 3) / 100f) * toRemove);
            ModifStat.Radiance = toRemove;
        }

        Stat.ModifStateAll(ModifStat);
        if (ModifStat.PalierChangement > 0)
            EnervementTension();
        else if (ModifStat.PalierChangement < 0)
            ApaisementTension();

        if (ModifStat.Radiance < 0)
        {
            LastDamageTaken = -ModifStat.Radiance;
            _refBattleMan.CurrentPhaseDamage += LastDamageTaken;

            if (LastDamageTaken > _refBattleMan.MostDamage)
            {
                _refBattleMan.MostDamage = LastDamageTaken;
                _refBattleMan.MostDamageID = idCaster;
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

        _refBattleMan.PassifManager.CurrentEvent = TimerPassif.FinAction;
        _refBattleMan.PassifManager.ResolvePassifs();

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
