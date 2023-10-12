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

    public BattleManager _refBattleMan;
    public bool IsTurn;

    #region Divers start & fin


    public void StartUp()
    {
        
        Stat.RadianceMaxOriginal = Stat.RadianceMax;
        Stat.VitesseOriginal = Stat.Vitesse;
        Stat.ClairvoyanceOriginal = Stat.Clairvoyance;
        Stat.ResilienceOriginal = Stat.Resilience - (int)Stat.ResiliencePassif;
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
            StatClairvoyanceText.color = Color.black;


        StatForceAmeText.text = Stat.ForceAme + "";

        if (Stat.ForceAme > Stat.ForceAmeOriginal)
            StatForceAmeText.color = new Color(147, 250, 165);
        else if (Stat.ForceAme < Stat.ForceAmeOriginal)
            StatForceAmeText.color = red;
        else
            StatForceAmeText.color = Color.black;

        StatSpeedText.text = Stat.Vitesse + "";

        if (Stat.Vitesse > Stat.VitesseOriginal)
            StatSpeedText.color = green;
        else if (Stat.Vitesse < Stat.VitesseOriginal)
            StatSpeedText.color = red;
        else
            StatSpeedText.color = Color.black;

        StatConvictionText.text = Stat.Conviction + "";

        if (Stat.Conviction > Stat.ConvictionOriginal)
            StatConvictionText.color = green;
        else if (Stat.Conviction < Stat.ConvictionOriginal)
            StatConvictionText.color = red;
        else
            StatConvictionText.color = Color.black;

        StatResilienceText.text = Stat.Resilience + "";

        if (Stat.Resilience > Stat.ResilienceOriginal)
            StatResilienceText.color = green;
        else if (Stat.Resilience < Stat.ResilienceOriginal)
            StatResilienceText.color = red;
        else
            StatResilienceText.color = Color.black;
        StatForceAmeText.SetAllDirty();
    }

    public void StartPhase()
    {
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
        if (Stat.isStun)
        {
            Debug.Log("is stuned");
            Stat.isStun = false;
            EndTurn();
        }
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

    public void FinCombat()
    {
        ResetStat();
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
        bool can = false;
        var maxTension = (Stat.ValeurPalier * Stat.NbPalier);
        if (Stat.Tension >= maxTension)
            can = true;
        return can;
    }

    #endregion Tension

    #region Spell

    private void DoAction(Spell toDo)
    {
        SelectedSpell = toDo;
        foreach(GameObject spell in Spells)
        {
            var temp = spell.GetComponent<SpellCombat>();
            if(temp.Action == toDo)
            {
                temp.selectedSpell.SetActive(true);
            }
            else
            {
                temp.selectedSpell.SetActive(false);
            }
        }
        bool isSelf = true;
        foreach(var effet in SelectedSpell.ActionEffet)
        {
            if(effet.Cible != Cible.Self && effet.Cible != Cible.joueur)
            {
                isSelf = false;
            }
        }        
        foreach(var buff in SelectedSpell.ActionBuffDebuff)
        {
            if(buff.CibleApplication != Cible.Self && buff.CibleApplication != Cible.joueur)
            {
                isSelf = false;
            }
        }
        if(!isSelf)
            TakeTarget();
        else
        {
            _refBattleMan.idTarget = 0;
            SendSpell(false);
        }
    }

    public void DesactivateSpells()
    {
        foreach (var item in Spells)
        {
            item.GetComponent<SpellCombat>().isTurn = false;
        }

        foreach(GameObject spell in Spells)
        {
            spell.GetComponent<SpellCombat>().selectedSpell.SetActive(false);
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

    public void SendSpell(bool attack)
    {
        Costs();
        DesactivateSpells();

        if(attack)
            AnimationController.StartAttack(AfterAnim);
        else
            AnimationController.SendAnimBuff(AfterAnim);

        _refBattleMan.LaunchAnimAttacked();
    }

    private void AfterAnim()
    {
        //A Mettre une fois les combats terminer
        _refBattleMan.LaunchSpellJoueur(SelectedSpell);
        ActivateSpells();
        _refBattleMan.PassifManager.CurrentEvent = TimerPassif.FinAction;
        _refBattleMan.PassifManager.ResolvePassifs();
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
            var buff = Instantiate(toAdd);
            buff.Effet = new List<Effet>();
            foreach (var item in toAdd.Effet)
            {
                buff.Effet.Add(Instantiate(item));
            }

            Stat.ListBuffDebuff.Add(buff);
            base.AddBuffDebuff(toAdd, Stat);
            ApplicationBuffDebuff(Timer, buff);
        }
       

        //DecompteDebuffJoueur(Decompte, Timer);
        
        UpdateUI();
    }

    private void DecompteDebuffJoueur(Decompte Decompte, TimerApplication Timer)
    {
        Stat.ListBuffDebuff = DecompteDebuff(Stat.ListBuffDebuff, Decompte,this.Stat);
        
        foreach(var item in Stat.ListBuffDebuff)
        {
            if(item.timerApplication == Timer)
                ApplicationBuffDebuff(Timer,item);
        }

        UpdateUI();
    }

    public void ApplicationBuffDebuff(TimerApplication Timer, BuffDebuff toApply)
    {
        //ResetStat();
        //foreach (var item in Stat.ListBuffDebuff)
        //{
        if(toApply.IsConsomable && toApply.TimingConsomationMinimum <= 1)
        {
            if (toApply.timerApplication == Timer || toApply.timerApplication == TimerApplication.Persistant || toApply.DirectApplication)
            {
                foreach (var effet in toApply.Effet)
                {
                    _refBattleMan.PassageEffet(effet, toApply.IDCombatOrigine, 0, SourceEffet.BuffDebuff);
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
                if (toApply.IsConsomable == true && !toApply.DirectApplication)
                {
                    toApply.Temps = 0;
                    foreach (var ToAdd in toApply.Consomation)
                    {
                        AddDebuff(ToAdd, Decompte.none, TimerApplication.Persistant);
                    }
                }
                if (toApply.DirectApplication)
                    toApply.DirectApplication = false;
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
                ModifStat = effet.ResultEffet(caster.Stat, LastDamageTaken,this.Stat);
            else
                ModifStat = effet.ResultEffet(Stat, LastDamageTaken, Cible:Stat);
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
            getAttacked();
        }

        if(effet.IsFirstApplication && effet.TypeEffet == TypeEffet.RadianceMax)
        {
            effet.IsFirstApplication = false;
            ModifStat.Radiance += ModifStat.RadianceMax;
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

        if(effet.IsAttaqueEffet)
        {
            foreach (var item in Stat.ListBuffDebuff)
            {
                if (item.timerApplication == TimerApplication.Attaque)
                    ApplicationBuffDebuff(TimerApplication.Attaque, item);
            }
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

    public void getAttacked()
    {
        AnimationController.GetAttacked();
    }
    
    public void endHurtAnim()
    {
        AnimationController.EndAnimAttack();
    }
}
