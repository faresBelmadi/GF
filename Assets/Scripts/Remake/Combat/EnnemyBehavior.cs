using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnnemyBehavior : CombatBehavior
{
    public EnnemiStatRemake Stat;
    public EnemyCombatGen UICombat;

    //List<BuffDebuff> Debuffs = new List<BuffDebuff>();
    //List<GameObject> ListBuff = new List<GameObject>();

    public int TensionUI;

    public Action<List<ActionResult>, int, int, bool, bool> actResult;
    public Action<List<BuffDebuff>, int, int> actDebuff;

    public int combatID;
    public EnnemiSpellRemake nextAction;
    public GameObject EssencePrefab;
    bool skip;
    nextActionEnumRemake nextActionType;
    List<EnnemiSpellRemake> Spells;

    private void Update()
    {
        updateUI();
    }

    void updateUI()
    {
        TensionUI = Mathf.FloorToInt((Tension * NbPalier) / TensionMax);
        UICombat.updateHp(Stat.Radiance, Stat.RadianceMax);
        UICombat.updateTension(TensionUI, NbPalier);
        string[] t = Stat.name.Split('(');
        UICombat.updateNom(t[0]);
        UICombat.RaiseEvent = TargetAcquired;
    }

    #region Divers start & fin

    public void SetUp()
    {
        updateUI();

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
    }

    public void StartPhase()
    {
        DecompteDebuffEnnemi(DecompteRemake.phase, TimerApplication.DebutPhase);
    }

    public void StartTurn()
    {
        DecompteDebuffEnnemi(DecompteRemake.tour, TimerApplication.DebutTour);
        if (!skip)
        {
            //DoAction();
        }
    }

    public void EndTurn()
    {
        if (!skip)
            EndAnimBool();
        ChooseNextAction();
        EndTurnBM();

    }

    private void Dead()
    {
        EndTurn();
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
        var t = Instantiate(EssencePrefab, this.transform.parent);
        if (Stat.Essence != 0)
        {
            t.GetComponent<Essence>().AddEssence(Stat.Essence);
            GameManager.instance.BattleMan.ListEssence.Add(t);
        }
        GameManager.instance.BattleMan.DeadEnemy(combatID);
    }

    #endregion Divers start & fin

    #region IA

    public void ChooseNextAction()
    {
        bool colere = false;
        foreach (var item in Stat.ListBuffDebuff)
        {
            foreach (var effect in item.Effet)
            {
                if (effect.TypeEffet == TypeEffetRemake.Colere)
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
                nextAction = item;
        }

        nextAction.Weight += nextAction.AddedWeight;
        foreach (var item in Spells)
        {
            if (item != nextAction)
                item.Weight--;
        }
        NextActionType();
        updateIntention();
    }

    void NextActionType()
    {
        if (Stat.Att1 != null)
            if (Stat.Att1 == nextAction)
                nextActionType = nextActionEnumRemake.Attaque;
        if (Stat.Att2 != null)
            if (Stat.Att2 == nextAction)
                nextActionType = nextActionEnumRemake.Attaque2;
        if (Stat.Buff != null)
            if (Stat.Buff == nextAction)
                nextActionType = nextActionEnumRemake.Buff;
        if (Stat.Debuff != null)
            if (Stat.Debuff == nextAction)
                nextActionType = nextActionEnumRemake.Debuff;
    }

    void updateIntention()
    {
        UICombat.ChangeIntention(nextAction.ImageIntentionSpell);
    }

    #endregion IA

    #region Spell

    public void DoAction()
    {

    }

    public void CreateSpellList()
    {
        Spells = new List<EnnemiSpellRemake>();

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

    public void AddDebuff(BuffDebuffRemake toAdd, DecompteRemake Decompte, TimerApplication Timer)
    {
        if (toAdd.IsDebuff)
        {
            ReceiveTension(Source.Buff);
        }
        Stat.ListBuffDebuff.Add(Instantiate(toAdd));
        base.AddBuffDebuff(toAdd);

        DecompteDebuffEnnemi(Decompte, Timer);

        updateUI();
    }

    private void DecompteDebuffEnnemi(DecompteRemake Decompte, TimerApplication Timer)
    {
        Stat.ListBuffDebuff = DecompteDebuff(Stat.ListBuffDebuff, Decompte);

        ApplicationBuffDebuff(Timer);

        updateUI();
    }

    public void ApplicationBuffDebuff(TimerApplication Timer)
    {
        skip = false;
        ResetStat();
        foreach (var item in Stat.ListBuffDebuff)
        {
            if (item.Activate == true && (item.timerApplication == Timer || item.timerApplication == TimerApplication.Persistant))
            {
                foreach (var effet in item.Effet)
                {
                    if (item.CibleApplication == effet.Cible)
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
        if (skip)
            EndTurn();
    }

    #endregion BuffDebuff

    #region Effet

    public void ApplicationEffet(EffetRemake effet)
    {
        var ModifStat = effet.ResultEffet(Stat);
        Stat.ModifStateAll(ModifStat);
    }

    #endregion Effet

    #region Tension

    public bool CanHaveAnotherTurn()
    {
        if (Tension >= ValeurPalier * NbPalier)
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
        if (Tension >= ValeurPalier * NbPalier)
            Tension = ValeurPalier * NbPalier + 0.2f;
    }

    #endregion Tension

    #region TargetingMode

    public void SetTargetingMode()
    {
        UICombat.TargetingMode = true;
    }

    public void TargetAcquired()
    {
        GameManager.instance.BattleMan.idTarget = combatID;

    }

    public void EndTargetingMode()
    {
        UICombat.TargetingMode = false;
    }

    #endregion

    #region Animation

    public void getAttacked()
    {
        this.GetComponent<Animator>().SetBool("IsAttacked", true);
    }

    public void EndAnimHurt()
    {
        this.GetComponent<Animator>().SetBool("IsAttacked", false);
    }

    void EndAnimBool()
    {
        switch (nextActionType)
        {
            case nextActionEnumRemake.Attaque:
                this.GetComponent<Animator>().SetBool("LaunchAttaque", false);
                break;
            case nextActionEnumRemake.Attaque2:
                this.GetComponent<Animator>().SetBool("LaunchAttaque2", false);
                break;
            case nextActionEnumRemake.Buff:
                this.GetComponent<Animator>().SetBool("LaunchBuff", false);
                break;
            case nextActionEnumRemake.Debuff:
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
            case nextActionEnumRemake.Attaque:
                this.GetComponent<Animator>().SetBool("LaunchAttaque", true);
                break;
            case nextActionEnumRemake.Attaque2:
                this.GetComponent<Animator>().SetBool("LaunchAttaque2", true);
                break;
            case nextActionEnumRemake.Buff:
                this.GetComponent<Animator>().SetBool("LaunchBuff", true);
                break;
            case nextActionEnumRemake.Debuff:
                this.GetComponent<Animator>().SetBool("LaunchDebuff", true);
                break;
            default:
                break;
        }
    }

    #endregion Animation

}

