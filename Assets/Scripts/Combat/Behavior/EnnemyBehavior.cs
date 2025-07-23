using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnnemyBehavior : CombatBehavior<EnnemiStat>
{

    public override EnnemiStat Stat
    {
        get => _stat;
        set
        {
            _stat = value;
           
        }
    }
    public EnemyStatsHolder EnemyStat;
    public UIEnnemi UICombat;
    public int TensionUI;
    
    
    public int combatID;
    public EnnemiSpell nextAction;
    //public GameObject EssencePrefab;
    bool skip;
    public bool IsTurn;
    nextActionEnum nextActionType;
    protected List<EnnemiSpell> Spells;
    public bool isMainEnemy;
    private BattleManager _refBattleMan;
    List<BuffDebuff> tempAddList = new List<BuffDebuff>();

    private int currentHp = 0;
    private int currentTension = 0;
    private Coroutine deathRoutine = null;
    private ClairvoyanceIconData clairvoyanceIconData;
   
    public override string Name
    {
        get { return TradManager.instance.GetTranslation(Stat.IdTradName, Stat.Nom); }
    }

    public bool IsDead { get; private set; } = false;

    public virtual void OnDestroy()
    {
        if (_stat != null)
        {
            foreach (var item in PassiveList)
            {
                if (item is IUpdateStatPassive)
                    ((IUpdateStatPassive)item).Clear();
            }
        }
    }
    #region Divers start & fin

    IEnumerator DeathCoroutine()
    {
        AudioManager.instance.SFX.PlaySFXClip(SFXType.EnnemyDeathSFX, Stat.DeathSFX);
        float time = 0f;
        while (time < deathDisolveTime)
        {
            time += Time.deltaTime;
            characterMaterial.SetFloat("_DisolveHeight", time / deathDisolveTime);
            yield return null;
        }

        Dead();
       
       
        deathRoutine = null;
    }

    public virtual void SetUp()
    {
        if (GameManager.Instance == null)
            _refBattleMan = TutoManager.Instance.BattleManager;
        else
        {
            commonStats = GameManager.Instance.CommonStatsData;
            _refBattleMan = GameManager.Instance.BattleMan;
            clairvoyanceIconData = GameManager.Instance.StatIcons;
        }
        IsDead = false;
        UICombat = this.GetComponent<UIEnnemi>();
        UpdateUI();

        //assignation des container dans le parent
        base.BuffContainer = UICombat.buffParents;
        base.DebuffContainer = UICombat.debuffParents;
        Stat.Radiance = Stat.RadianceMax;
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

        PassiveList = new List<AbstractPassive>();
        for (int i = 0; i < _stat.PassiveList.Count; i++)
        {
            PassiveList.Add(Instantiate(_stat.PassiveList[i]));
        }
        foreach (var item in PassiveList)
        {
            if (item is IUpdateStatPassive passive)
                passive.InitPassif(_stat);
        }

        RefreshPassiveDescription();
    }

    public override void ResetStat()
    {
        Stat.Dissimulation = Stat.DissimulationOriginal;

        base.ResetStat();
    }

    public void StartPhase()
    {
        DecompteDebuffEnnemi(Decompte.phase, TimerApplication.DebutPhase);
    }

    public void StartTurn(bool isFirstTurn = false)
    {
        IsTurn = true;
        foreach (var passif in PassiveList)
        {
            if (passif is IStartTurnPassive)
            {
                IStartTurnPassive startTurnpassif = passif as IStartTurnPassive;
                startTurnpassif.Apply(Stat);
            }
        }

        DecompteDebuffEnnemi(Decompte.tour, TimerApplication.DebutTour);
        if (!isFirstTurn)
        {
            if (!gainedTension)
            {
                ApaisementTension();
            }

            gainedTension = false;
        }

        if (!skip && !Stat.isStun)
        {
            DoAction();
        }

        if (Stat.isStun)
        {
            Debug.Log("is stuned");
            //Stat.isStun = false;
            EndTurn();
        }

    }

    public void EndTurn()
    {
       
        IsTurn = false;
        if (!skip)
            EndAnimBool();
        ChooseNextAction();
        EndTurnBM();

    }

    public void Dead()
    {
        if (IsDead) return;
        IsDead = true;
        

        foreach (var item in PassiveList)
        {
            if (item is IDeathEffectPassive passive)
                passive.OnDeathAction();
        }

        /*
        foreach (var item in Stat.ListBuffDebuff)
        {
            foreach (var effect in item.Effet)
            {
                if (effect.TypeEffet == TypeEffetRemake.DeathTrigger)
                {
                    //to do : change name to official debuff name for debuff intouchable
                    if (item.Nom == "Débuff intouchable")
                    {
                        Stat.Essence = 0;
                    }
                }
            }
        }
        */
        if (Stat.Essence != 0)
        {
            var t = Instantiate(GameManager.Instance.BattleMan.GetPrefabEssence(Stat.Essence), this.transform.parent);
            t.GetComponent<CrystalSoul>().AddAmountOfEssence(Stat.Essence);
            if (GameManager.Instance.IsTuto)
            {
                t.SetActive(false);
            }
            _refBattleMan.ListEssence.Add(t);
        }

        _refBattleMan.DeadEnemy(combatID);
    }

    #endregion Divers start & fin

    #region Tension

    public override bool CanHaveAnotherTurn()
    {
        return base.CanHaveAnotherTurn() && !Stat.NoTension;
    }

    #endregion Tension

    #region Update

    private void Update()
    {
        UpdateUI();
    }

    protected virtual void UpdateUI()
    {
        if (Stat == null)
            return;
        if (currentHp != Stat.Radiance) UICombat.UpdateHp(Stat.Radiance, Stat.RadianceMax);
        currentHp = Stat.Radiance;

        TensionUI = Mathf.FloorToInt((Stat.Tension * GameManager.Instance.CommonStatsData.NbPalier) / Stat.TensionMax);
        if (currentTension != TensionUI) UICombat.UpdateTension(TensionUI, GameManager.Instance.CommonStatsData.NbPalier);
        currentTension = TensionUI;

        string[] t = Stat.Nom.Split('(');
        UICombat.UpdateNom(t[0]);
        UICombat.RaiseEvent = TargetAcquired;
        UICombat.OnPreviewDamage = PreviewDamage;
        UICombat.OnStopPreviewDamage = StopPreviewDamage;

    }

    #endregion Update

    #region IA

    public virtual void ChooseNextAction()
    {
        bool colere = false;
        foreach (var item in Stat.ListBuffDebuff)
        {
            foreach (var effect in item.Effet)
            {
                if (effect.TypeEffet == TypeEffet.Colere)
                {
                    UnityEngine.Random.InitState((int) DateTime.Now.Ticks);
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

    protected void NextActionType()
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

    protected void UpdateIntention()
    {
        if (GameManager.Instance.BattleMan.getJoueurClairvoyance() >= Stat.Dissimulation)
        {
            switch (nextActionType)
            {
                case nextActionEnum.Attaque:
                    UICombat.ChangeIntention(clairvoyanceIconData.IntentionAtk);
                    break;
                case nextActionEnum.Attaque2:
                    UICombat.ChangeIntention(clairvoyanceIconData.IntentionHeavyAtk);
                    break;
                case nextActionEnum.Buff:
                    UICombat.ChangeIntention(clairvoyanceIconData.IntentionBuff);
                    break;
                case nextActionEnum.Debuff:
                    UICombat.ChangeIntention(clairvoyanceIconData.IntentionDebuff);
                    break;
                default:
                    break;
            }
        }
        else
            UICombat.ChangeIntention(clairvoyanceIconData.HiddenIntention);
    }

    #endregion IA

    #region Spell

    public void DoAction()
    {
        AudioManager.instance.SFX.PlaySFXClip(SFXType.EnnemySpellSFX, nextAction.SpellSFX);
        //A Mettre une fois les combats terminer
        LaunchAnimBool();
        //DecompteDebuffEnnemi(Decompte.none, TimerApplication.Attaque);
        //_refBattleMan.LaunchSpellEnnemi(nextAction);
        Debug.Log($"Ennemy {Name} launch Spell {nextAction.Name}");
    }

    public void EndAttackAnimation()
    {
        //Debug.Log("commencement des degats");
        //DecompteDebuffEnnemi(Decompte.none, TimerApplication.Attaque);
        if (!Stat.isStun)
        {
            _refBattleMan.LaunchSpellEnnemi(nextAction);
        }
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

        UnityEngine.Random.InitState((int) DateTime.Now.Ticks);
        foreach (var item in Spells)
        {
            item.Weight += UnityEngine.Random.Range(0, 4);
        }
    }

    #endregion Spell

    #region Passif

    public void RefreshPassiveDescription()
    {
        EnemyPassiveDescription passDesc = GetComponentInChildren<EnemyPassiveDescription>(true);
        if (passDesc != null && PassiveList.Count > 0)
        {
            passDesc.InitTooltip(PassiveList[0].IdTradDesc, PassiveList[0].DefaultDescription);
        }
    }
#endregion

    #region BuffDebuff

    public void AddDebuff(BuffDebuff toAdd, TimerApplication Timer)
    {
        for (int i = 0; i < Stat.MultipleBuffDebuff; i++)
        {
            nbBuffDebuffApplied++;
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

            var modifiedBuff = ApplyConviction(buff, ValueConviction());
            Stat.ListBuffDebuff.Add(modifiedBuff);
            base.AddBuffDebuff(modifiedBuff, Stat);

            ApplicationBuffDebuff(Timer, modifiedBuff);
        }

        UpdateUI();
    }

    private void DecompteDebuffEnnemi(Decompte Decompte, TimerApplication Timer)
    {
        DecompteDebuff(Stat.ListBuffDebuff, Decompte, this.Stat);
        foreach (var item in Stat.ListBuffDebuff)
        {
            if (item.timerApplication == Timer)
                ApplicationBuffDebuff(Timer, item);
        }

        foreach (var item in tempAddList)
        {
            AddDebuff(item, TimerApplication.Persistant);
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
        if (toApply.timerApplication == Timer || toApply.timerApplication == TimerApplication.Persistant ||
            toApply.DirectApplication)
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
                    GameManagerRemake.Instance.BattleMan.PassageEffet(effet, item.IDCombatOrigine, combatID);
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

    public void ApplicationEffet(Effet effet, JoueurStat Caster = null, SourceEffet source = SourceEffet.Spell,
        int idCaster = 0, int NbEnnemies = 1)
    {
        JoueurStat ModifStat;
        if (Caster == null)
        {
            if (idCaster == 0)
            {
                Caster = _refBattleMan.player.Stat;
                ModifStat = effet.ResultEffet(_refBattleMan.player.PlayerStat, LastDamageTaken, EnemyStat, NbEnnemies);
            }
            else
            {
                var caster = _refBattleMan.EnemyScripts.Where(x => x.combatID == idCaster).FirstOrDefault();
                if (caster != null)
                    ModifStat = effet.ResultEffet(caster.EnemyStat, LastDamageTaken, EnemyStat);
                else
                {
                    ModifStat = effet.ResultEffet(EnemyStat, LastDamageTaken, EnemyStat, 1);

                }

            }

        }
        else
        {
            var caster = _refBattleMan.EnemyScripts.Where(x => x.combatID == idCaster).FirstOrDefault();
            if  (caster == null)
            {
                ModifStat = effet.ResultEffet(_refBattleMan.player.PlayerStat, LastDamageTaken, EnemyStat);
            }
            else
            {
                ModifStat = effet.ResultEffet(caster.EnemyStat, LastDamageTaken, EnemyStat);
            }
        }

        if (ModifStat.Radiance < 0)
        {
            var toRemove = Mathf.FloorToInt(ModifStat.Radiance / Stat.MultiplDef);
            toRemove -= Mathf.FloorToInt(((Stat.Resilience * 3) / 100f) * toRemove);
            ModifStat.Radiance = toRemove;
            if (source != SourceEffet.BuffDebuff)
                GetAttacked();
        }

        if (effet.IsFirstApplication && effet.TypeEffet == TypeEffet.RadianceMax)
        {
            effet.IsFirstApplication = false;
            ModifStat.Radiance += ModifStat.RadianceMax;
        }
        GameManager.Instance.BattleMan.LogRadianceChange(this.Name,  GameManager.Instance.BattleMan.GetBehaviorNameFromStat(Caster), ModifStat.Radiance);
        if (ModifStat.Radiance < 0)
        {
            Debug.Log($"{Name} take {ModifStat.Radiance * -1} damage.");
            foreach (var item in PassiveList)
            {
                if (item is IOnDamagePassive passive)
                    passive.Apply(_stat);
            }
        }
        EnemyStat.UpdateStat(ModifStat);
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
            if (!Stat.NoTension)
                ReceiveTension(Source.Soin);
            UICombat.SpawnDegatSoin(ModifStat.Radiance);
        }

        

        UpdateUI();

        if (Stat.Radiance <= 0)
        {
            /*EndTurn();*/
            // provoque une fin de tour du joueur a la mort d'un ennemi, est ce que c'est une feature voulu ?
            deathRoutine = StartCoroutine(DeathCoroutine());
            //Dead();
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

    public void PreviewDamage()
    {
        int[] damageList = new int[_refBattleMan.EnemyScripts.Count + _refBattleMan.DeadEnemyScripts.Count];
        int returnedDmg = 0;
        //int damage = 0;
        foreach (Effet effet in _refBattleMan.player.SelectSpell.ActionEffet)
        {
            effet.VisualizeAttack(_refBattleMan.player.PlayerStat, EnemyStat, out int dmg, out int rDmg,_refBattleMan.EnemyScripts.Count);
            if (effet.Cible == Cible.allEnnemi)
            {
                for (int i = 0; i < damageList.Length; i++)
                {
                        damageList[i] += dmg;
                }
            }
            else if (effet.Cible == Cible.AllEnemyExceptTarget)
            {
                for (int i = 0; i < damageList.Length;i++)
                {
                    if (i + 1 != GameManager.Instance.BattleMan.idPreviewTarget)
                        damageList[i] += dmg;
                }
            }
            else
            {
                damageList[combatID - 1] += dmg;
                returnedDmg += rDmg;
            }
        }

        foreach (EnnemyBehavior ennemy in _refBattleMan.EnemyScripts)
        {
            if (damageList[ennemy.combatID - 1] < 0)
            {
                var toRemove = Mathf.FloorToInt(damageList[ennemy.combatID - 1] / ennemy.Stat.MultiplDef);
                toRemove -= Mathf.FloorToInt(((ennemy.Stat.Resilience * 3) / 100f) * toRemove);
                ennemy.UICombat.PreviewDmg(ennemy.Stat.Radiance + toRemove, ennemy.Stat.RadianceMax);
            }
        }
        if(returnedDmg != 0)
        {
            _refBattleMan.player.PreviewHPBarUpdate(_refBattleMan.player.Stat.Radiance + returnedDmg, _refBattleMan.player.Stat.RadianceMax);
        }
    }

    public void StopPreviewDamage()
    {
        foreach (EnnemyBehavior ennemy in _refBattleMan.EnemyScripts)
        {
            ennemy.UICombat.StopPreview();
            _refBattleMan.player.StopPReviewHPBarUpdate();
        }

    }

    #endregion

    #region Animation

    public void GetAttacked()
    {
        AudioManager.instance.SFX.PlaySFXClip(SFXType.EnnemyDamageTakenSFX, Stat.DamageSFX);
        DecompteDebuffEnnemi(Decompte.none, TimerApplication.Attaque);

        GetComponent<Animator>().SetFloat("SpeedMultiplier", GameManager.Instance.BattleMan.AnimationSpeedMultiplier);
        this.GetComponent<Animator>().SetBool("IsAttacked", true);
        gameObject.GetComponent<PulseBloom_System>().TriggerBloom();
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

        EndAttackAnimation();
    }

    void LaunchAnimBool()
    {
        GetComponent<Animator>().SetFloat("SpeedMultiplier", GameManager.Instance.BattleMan.AnimationSpeedMultiplier);
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