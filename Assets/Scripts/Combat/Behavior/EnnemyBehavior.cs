using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnnemyBehavior : CombatBehavior
{
    public EnnemiStat Stat;

    public UIEnnemi UICombat;
    public int TensionUI;

    public int combatID;
    public EnnemiSpell nextAction;
    public GameObject EssencePrefab;
    bool skip;
    public bool IsTurn;
    nextActionEnum nextActionType;
    List<EnnemiSpell> Spells;
    public bool isMainEnemy;
    private BattleManager _refBattleMan;
    List<BuffDebuff> tempAddList = new List<BuffDebuff>();

    private int currentHp = 0;
    private int currentTension = 0;
    #region Divers start & fin

    public void SetUp()
    {
        _refBattleMan = GameManager.instance.BattleMan;
        UICombat = this.GetComponent<UIEnnemi>();
        UpdateUI();

        //assignation des container dans le parent
        base.BuffContainer = UICombat.buffParents;
        base.DebuffContainer = UICombat.debuffParents;

        Stat.VitesseOriginal = Stat.Vitesse;
        Stat.DissimulationOriginal = Stat.Dissimulation;
        Stat.ResilienceOriginal = Stat.Resilience;
        Stat.RadianceMaxOriginal = Stat.RadianceMax;

        if (Stat.Att1 != null)
            Stat.Att1 = Instantiate(Stat.Att1);
        if (Stat.Att2 != null)
            Stat.Att2 = Instantiate(Stat.Att2);
        if (Stat.Buff != null)
            Stat.Buff = Instantiate(Stat.Buff);
        if (Stat.Debuff != null)
            Stat.Debuff = Instantiate(Stat.Debuff);
    }

    public void ResetStat()
    {
        Stat.RadianceMax = Stat.RadianceMaxOriginal;
        Stat.Vitesse = Stat.VitesseOriginal;
        Stat.Dissimulation = Stat.DissimulationOriginal;
        Stat.Resilience = Stat.ResilienceOriginal;
        Stat.MultipleBuffDebuff = 1;

        Stat.MultiplDegat = 1;
        Stat.MultiplDef = 1;
        Stat.MultiplSoin = 1;
        Stat.MultipleBuffDebuff = 1;
        Stat.ForceAme = Stat.ForceAmeOriginal;
        Stat.Conviction = Stat.ConvictionOriginal;
    }

    public void StartPhase()
    {
        DecompteDebuffEnnemi(Decompte.phase, TimerApplication.DebutPhase);
    }

    public void StartTurn() 
    {
        IsTurn = true;
        _refBattleMan.PassifManager.CurrentEvent = TimerPassif.DebutTour;
        _refBattleMan.PassifManager.ResolvePassifs();
        DecompteDebuffEnnemi(Decompte.tour, TimerApplication.DebutTour);

        if (!gainedTension)
        {
            ApaisementTension();
        }
        gainedTension = false;
        if (Stat.isStun)
        {
            Debug.Log("is stuned");
            //Stat.isStun = false;
            EndTurn();
        }
        if (!skip && !Stat.isStun)
        {
            DoAction();
        }
    }

    public void EndTurn()
    {

        _refBattleMan.PassifManager.CurrentEvent = TimerPassif.FinTour;
        _refBattleMan.PassifManager.ResolvePassifs();
        IsTurn = false;
        if (!skip)
            EndAnimBool();
        ChooseNextAction();
        EndTurnBM();

    }

    public void Dead()
    {
        _refBattleMan.PassifManager.CurrentEvent = TimerPassif.Death;
        _refBattleMan.PassifManager.ResolvePassifs();

        foreach (var item in Stat.ListBuffDebuff)
        {
            foreach (var effect in item.Effet)
            {

                /*if (effect.TypeEffet == TypeEffetRemake.DeathTrigger)
                {
                    //to do : change name to official debuff name for debuff intouchable
                    if (item.Nom == "Débuff intouchable")
                    {
                        Stat.Essence = 0;
                    }
                }*/
            }
        }
        if (Stat.Essence != 0)
        {
            var t = Instantiate(EssencePrefab, this.transform.parent);
            t.GetComponent<Essence>().AddEssence(Stat.Essence);
            _refBattleMan.ListEssence.Add(t);
        }
        _refBattleMan.DeadEnemy(combatID);
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

    public bool CanHaveAnotherTurn()
    {
        if (Stat.Tension >= Stat.ValeurPalier * Stat.NbPalier && !Stat.NoTension)
        {
            return true;
        }
        return false;
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
        if (Stat.Tension < 0)
            Stat.Tension = 0;
    }

    #endregion Tension

    #region Update

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if(currentHp!=Stat.Radiance) UICombat.UpdateHp(Stat.Radiance, Stat.RadianceMax);
        currentHp = Stat.Radiance;
        
        TensionUI = Mathf.FloorToInt((Stat.Tension * Stat.NbPalier) / Stat.TensionMax);
        if(currentTension != TensionUI) UICombat.UpdateTension(TensionUI, Stat.NbPalier);
        currentTension = TensionUI;

        string[] t = Stat.Nom.Split('(');
        UICombat.UpdateNom(t[0]);
        UICombat.RaiseEvent = TargetAcquired;
    }

    #endregion Update

    #region IA

    public void ChooseNextAction()
    {
        bool colere = false;
        foreach (var item in Stat.ListBuffDebuff)
        {
            foreach (var effect in item.Effet)
            {
                if (effect.TypeEffet == TypeEffet.Colere)
                {
                    UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
                    var temp = UnityEngine.Random.Range(0, 100);
                    if (temp <= effect.Pourcentage)
                        colere = true;
                }
            }
        }
        if (Spells == null)
            CreateSpellList();

        nextAction = Spells.First();

        foreach (var item in Spells)
        {
            if (nextAction.IsAttaque && colere)
            {

            }
            else if (item.Weight < nextAction.Weight)
            {
                if (item.name.Contains("UltimeJeanne"))
                {
                    if (Stat.Divin >= 70)
                        nextAction = item;
                }
                else
                    nextAction = item;
            }
        }

        nextAction.Weight += nextAction.AddedWeight;
        foreach (var item in Spells)
        {
            if (item != nextAction)
                item.Weight--;
        }
        NextActionType();
        UpdateIntention();
    }

    private void NextActionType()
    {
        if (Stat.Att1 != null)
            if (Stat.Att1 == nextAction)
                nextActionType = nextActionEnum.Attaque;
        if (Stat.Att2 != null)
            if (Stat.Att2 == nextAction)
                nextActionType = nextActionEnum.Attaque2;
        if (Stat.Buff != null)
            if (Stat.Buff == nextAction)
                nextActionType = nextActionEnum.Buff;
        if (Stat.Debuff != null)
            if (Stat.Debuff == nextAction)
                nextActionType = nextActionEnum.Debuff;
    }

    private void UpdateIntention()
    {
        UICombat.ChangeIntention(nextAction.ImageIntentionSpell);
    }

    #endregion IA

    #region Spell

    public void DoAction()
    {
        //A Mettre une fois les combats terminer
        LaunchAnimBool();
        DecompteDebuffEnnemi(Decompte.none, TimerApplication.Attaque);
        _refBattleMan.LaunchSpellEnnemi(nextAction);
    }

    public void EndAttackAnimation()
    {
        Debug.Log("commencement des degats");
        GameManager.instance.BattleMan.LaunchSpellEnnemi(nextAction);
    }

    public void CreateSpellList()
    {
        Spells = new List<EnnemiSpell>();

        if (Stat.Att1 != null)
            Spells.Add(Stat.Att1);
        if (Stat.Att2 != null)
            Spells.Add(Stat.Att2);
        if (Stat.Buff != null)
            Spells.Add(Stat.Buff);
        if (Stat.Debuff != null)
            Spells.Add(Stat.Debuff);

        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        foreach (var item in Spells)
        {
            item.Weight += UnityEngine.Random.Range(0, 4);
        }
    }

    #endregion Spell

    #region BuffDebuff

    public void AddDebuff(BuffDebuff toAdd, Decompte Decompte, TimerApplication Timer)
    {
        for (int i = 0; i < Stat.MultipleBuffDebuff; i++)
        {
            if (toAdd.IsDebuff && !Stat.NoTension)
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

        UpdateUI();
    }

    private void DecompteDebuffEnnemi(Decompte Decompte, TimerApplication Timer)
    {
        DecompteDebuff(Stat.ListBuffDebuff, Decompte,this.Stat);
        foreach(var item in Stat.ListBuffDebuff)
        {
            if(item.timerApplication == Timer)
                ApplicationBuffDebuff(Timer,item);
        }
        foreach (var item in tempAddList)
        {
            AddDebuff(item, Decompte.none, TimerApplication.Persistant);
        }
        Stat.ListBuffDebuff = base.UpdateBuffDebuffGameObject(Stat.ListBuffDebuff, this.Stat);
        tempAddList.Clear();
        UpdateUI();
    }

    public void ApplicationBuffDebuff(TimerApplication Timer, BuffDebuff toApply)
    {
        skip = false;
        //ResetStat();
        //foreach (var item in Stat.ListBuffDebuff)
        //{
        if (toApply.timerApplication == Timer || toApply.timerApplication == TimerApplication.Persistant || toApply.DirectApplication)
        {
            foreach (var effet in toApply.Effet)
            {
                _refBattleMan.PassageEffet(effet, toApply.IDCombatOrigine, combatID, SourceEffet.BuffDebuff);
                /*if (item.CibleApplication == effet.Cible)
                {
                    ApplicationEffet(effet);
                }
                else
                {
                    //A Mettre une fois les combats terminer
                    GameManagerRemake.instance.BattleMan.PassageEffet(effet, item.IDCombatOrigine, combatID);
                }*/
            }
            if (toApply.IsConsomable == true && toApply.TimingConsomationMinimum < 1 && toApply.Temps > 0)
            {
                toApply.Temps = -1;
                foreach (var ToAdd in toApply.Consomation)
                {
                    tempAddList.Add(ToAdd);
                }
            }
            else
                toApply.TimingConsomationMinimum--;
            if (toApply.DirectApplication)
                toApply.DirectApplication = false;
        }
        if (skip)
            EndTurn();
    }

    #endregion BuffDebuff

    #region Effet

    public void ApplicationEffet(Effet effet, JoueurStat Caster = null, SourceEffet source = SourceEffet.Spell, int idCaster = 0, int NbEnnemies = 1)
    {
        JoueurStat ModifStat;
        if (Caster == null)
        {
            if (idCaster == 0)
            {
                Caster = _refBattleMan.player.Stat;
                ModifStat = effet.ResultEffet(Caster, LastDamageTaken, this.Stat, NbEnnemies);
            }
            else
            {
                var caster = _refBattleMan.EnemyScripts.Where(x => x.combatID == idCaster).FirstOrDefault();
                if (caster != null)
                    ModifStat = effet.ResultEffet(caster.Stat, LastDamageTaken,this.Stat);
                else
                    ModifStat = effet.ResultEffet(Stat, LastDamageTaken,null,1);

            }

        }
        else
        {
            ModifStat = effet.ResultEffet(Caster, LastDamageTaken, Stat);
        }

        if (ModifStat.Radiance < 0)
        {
            var toRemove = Mathf.FloorToInt(ModifStat.Radiance / Stat.MultiplDef);
            toRemove -= Mathf.FloorToInt(((Stat.Resilience * 3) / 100f) * toRemove);
            ModifStat.Radiance = toRemove;
            if(source != SourceEffet.BuffDebuff)
                GetAttacked();
        }

        if(effet.IsFirstApplication && effet.TypeEffet == TypeEffet.RadianceMax)
        {
            effet.IsFirstApplication = false;
            ModifStat.Radiance += ModifStat.RadianceMax;
        }

        Stat.ModifStateAll(ModifStat);
        Stat.RectificationStat();

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

            if (source == SourceEffet.Spell && !Stat.NoTension)
                ReceiveTension(Source.Attaque);
            else if (source == SourceEffet.BuffDebuff && !Stat.NoTension)
                ReceiveTension(Source.Dot);

            UICombat.SpawnDegatSoin(ModifStat.Radiance);
        }
        else if (ModifStat.Radiance > 0)
        {
            if(!Stat.NoTension)
                ReceiveTension(Source.Soin);
            UICombat.SpawnDegatSoin(ModifStat.Radiance);
        }

        _refBattleMan.PassifManager.CurrentEvent = TimerPassif.FinAction;
        _refBattleMan.PassifManager.ResolvePassifs();

        UpdateUI();

        if (Stat.Radiance <= 0)
        {
            /*EndTurn();*/ // provoque une fin de tour du joueur a la mort d'un ennemi, est ce que c'est une feature voulu ?
            Dead();
        }
    }

    #endregion Effet

    #region TargetingMode

    public void SetTargetingMode()
    {
        UICombat.TargetingMode = true;
    }

    public void TargetAcquired()
    {
        _refBattleMan.idTarget = combatID;

    }

    public void EndTargetingMode()
    {
        UICombat.TargetingMode = false;
    }

    #endregion

    #region Animation

    public void GetAttacked()
    {
        DecompteDebuffEnnemi(Decompte.none, TimerApplication.Attaque);
        this.GetComponent<Animator>().SetBool("IsAttacked", true);
    }

    public void EndAnimHurt()
    {
        this.GetComponent<Animator>().SetBool("IsAttacked", false);
    }

    public void EndAnimBool()
    {
        switch (nextActionType)
        {
            case nextActionEnum.Attaque:
                this.GetComponent<Animator>().SetBool("LaunchAttaque", false);
                break;
            case nextActionEnum.Attaque2:
                this.GetComponent<Animator>().SetBool("LaunchAttaque2", false);
                break;
            case nextActionEnum.Buff:
                this.GetComponent<Animator>().SetBool("LaunchBuff", false);
                break;
            case nextActionEnum.Debuff:
                this.GetComponent<Animator>().SetBool("LaunchDebuff", false);
                break;
            default:
                break;
        }
    }

    void LaunchAnimBool()
    {
        switch (nextActionType)
        {
            case nextActionEnum.Attaque:
                this.GetComponent<Animator>().SetBool("LaunchAttaque", true);
                break;
            case nextActionEnum.Attaque2:
                this.GetComponent<Animator>().SetBool("LaunchAttaque2", true);
                break;
            case nextActionEnum.Buff:
                this.GetComponent<Animator>().SetBool("LaunchBuff", true);
                break;
            case nextActionEnum.Debuff:
                this.GetComponent<Animator>().SetBool("LaunchDebuff", true);
                break;
            default:
                break;
        }
    }

    #endregion Animation

}

