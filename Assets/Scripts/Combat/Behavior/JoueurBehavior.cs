using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoueurBehavior : CombatBehavior
{
    [SerializeField] public JoueurStat Stat;
    
    [SerializeField] private List<GameObject> Spells;
    [SerializeField] private Transform DamageSpawn;
    [SerializeField] private GameObject DamagePrefab;
    [SerializeField] private GameObject SoinPrefab;
    [SerializeField] private GameObject SpellPrefab;
    [SerializeField] private GameObject SpellsSpawn;
    [SerializeField] public Button EndTurnButton;
    [SerializeField] private List<BuffDebuff> tempAddList = new List<BuffDebuff>();

    [SerializeField] private ProgressBarManager hPBarManager;
    [SerializeField] private ProgressBarManager tensionBarManager;
    [SerializeField] private ProgressBarManager conscienceBarManager;

    [SerializeField] private Slider VolonteSlider;
    [SerializeField] private Image VolonteBarBack;
    [SerializeField] private HighlightCost _highlightComponant;

    [SerializeField] private Color green = new Color(0.58f, 0.98f, 0.65f);
    [SerializeField] private Color red = new Color(0.996f, 0.47f, 0.40f);
    [SerializeField] private TextMeshProUGUI TensionText;
    [SerializeField] private TextMeshProUGUI HpText;
    [SerializeField] private TextMeshProUGUI HpTextReduced;
    [SerializeField] private TextMeshProUGUI HpToolTipText;
    [SerializeField] private TextMeshProUGUI VolontéText;
    [SerializeField] private TextMeshProUGUI ConscienceText;
    [SerializeField] private TextMeshProUGUI StatForceAmeText;
    [SerializeField] private TextMeshProUGUI StatSpeedText;
    [SerializeField] private TextMeshProUGUI StatResilienceText;
    [SerializeField] private TextMeshProUGUI StatConvictionText;
    [SerializeField] private TextMeshProUGUI StatClairvoyanceText;
    [SerializeField] private Image StatForceAmeBg;
    [SerializeField] private Image StatSpeedBg;
    [SerializeField] private Image StatResilienceBg;
    [SerializeField] private Image StatConvictionBg;
    [SerializeField] private Image StatClairvoyanceBg;

    [SerializeField] private Spell SelectedSpell;

    [SerializeField] private AnimationControllerAttack AnimationController;

    [SerializeField] private BattleManager _refBattleMan;
    [SerializeField] private bool IsTurn;

    public Spell SelectSpell => SelectedSpell;
    #region Divers start & fin

    private int currentHp = -1;
    private float currentTens = -1;
    private int currentCons = -1;
    public void InitRefBattleMan(BattleManager battleManager)
    {
        _refBattleMan = battleManager;
    }

    public void StartUp()
    {

        Stat.RadianceMaxOriginal = Stat.RadianceMax;
        Stat.VitesseOriginal = Stat.Vitesse;
        Stat.ClairvoyanceOriginal = Stat.Clairvoyance;
        Stat.ResilienceOriginal = Stat.Resilience - (int) Stat.ResiliencePassif;
        Stat.ConvictionOriginal = Stat.Conviction;
        Stat.ForceAmeOriginal = Stat.ForceAme;
        foreach (var item in Stat.ListSpell)
        {
            var temp = Instantiate(SpellPrefab, SpellsSpawn.transform);
            Spell SpelleToUse = CheckSouvenirSpell(item);
            temp.GetComponent<SpellCombat>().Action = SpelleToUse;
            temp.GetComponent<SpellCombat>().Act = DoAction;
            temp.GetComponent<SpellCombat>().StartUp();
            int volonteCost = 0, conscCost = 0, radCost = 0;
            foreach(var cost in temp.GetComponent<SpellCombat>().Action.Costs)
            {
                if (cost.typeCost == TypeCostSpell.volonte)
                {
                    volonteCost = cost.Value;
                }
                else if (cost.typeCost == TypeCostSpell.radiance)
                {
                    radCost = cost.Value;
                }
                else if (cost.typeCost == TypeCostSpell.conscience)
                {
                    conscCost = cost.Value;
                }
            }
           
            temp.GetComponent<SpellCombat>().button.onClick.AddListener(delegate { _highlightComponant.SelectCostForHighlighing(volonteCost, radCost, conscCost); });
            temp.GetComponent<HighlightTriggerEvent>().SetActionsToTrigger(_highlightComponant.EnableHighlighting, _highlightComponant.DisableHighlighting, volonteCost, radCost, conscCost);
           
            Spells.Add(temp);
        }

        InitUI();
    }

    private Spell CheckSouvenirSpell(Spell item)
    {
        if (Stat.ListSouvenir == null || Stat.ListSouvenir.Count == 0)
            return item;
        foreach (var souvenir in Stat.ListSouvenir)
        {
            if (souvenir.SouvenirSpell != null && souvenir.Equiped)
            {
                if (souvenir.SouvenirSpell.IDSpell == item.IDSpell)
                {
                    return souvenir.SouvenirSpell;
                }
            }
        }

        return item;
    }
    private void InitUI()
    {
        hPBarManager.InitPBar(Stat.Radiance, Stat.RadianceMax);
        tensionBarManager.InitPBar(0, Stat.NbPalier);
        conscienceBarManager.InitPBar(Stat.Conscience, Stat.ConscienceMax);
    }
    public void UpdateUI()
    {
        //ProgressBar Updates
        if(Stat.Radiance != currentHp) 
        {
            hPBarManager.UpdatePBar(Stat.Radiance, Stat.RadianceMax);
            hPBarManager.ToggleBloomPulses(false);
        }currentHp = Stat.Radiance;

        if (Stat.Tension != currentTens)
        {
            tensionBarManager.UpdatePBar(Mathf.FloorToInt((Stat.Tension * Stat.NbPalier) / Stat.TensionMax), Stat.NbPalier);
            
            tensionBarManager.ToggleBloomPulses(((Stat.Tension * Stat.NbPalier) / Stat.TensionMax) >= Stat.NbPalier);
            
        }
        currentTens = Stat.Tension;
        
        if (Stat.Conscience != currentCons)
        {
            conscienceBarManager.UpdatePBar(Stat.Conscience, Stat.ConscienceMax);
            conscienceBarManager.ToggleBloomPulses(false);
        }
        currentCons = Stat.Conscience;

        VolonteSlider.value = Stat.Volonter;
        VolonteSlider.maxValue = Stat.VolonterMax;


        HpText.text = $"{Stat.Radiance.ToString()}/{Stat.RadianceMax}";
        HpTextReduced.text = Stat.Radiance.ToString();
        HpToolTipText.text = $"Radiance : vos points de vie\nMax: {Stat.RadianceMax.ToString()}"; 
        ConscienceText.text = Stat.Conscience + "/" + Stat.ConscienceMax;

        StatClairvoyanceText.text = Stat.Clairvoyance + "";


        if (Stat.Clairvoyance > Stat.ClairvoyanceOriginal)
            StatClairvoyanceBg.color = green;
        else if (Stat.Clairvoyance < Stat.ClairvoyanceOriginal)
            StatClairvoyanceBg.color = red;
        else
            StatClairvoyanceBg.color = Color.white;


        StatForceAmeText.text = Stat.ForceAme + "";

        if (Stat.ForceAme > Stat.ForceAmeOriginal)
            StatForceAmeBg.color = new Color(147, 250, 165);
        else if (Stat.ForceAme < Stat.ForceAmeOriginal)
            StatForceAmeBg.color = red;
        else
            StatForceAmeBg.color = Color.white;

        StatSpeedText.text = Stat.Vitesse + "";

        if (Stat.Vitesse > Stat.VitesseOriginal)
            StatSpeedBg.color = green;
        else if (Stat.Vitesse < Stat.VitesseOriginal)
            StatSpeedBg.color = red;
        else
            StatSpeedBg.color = Color.white;

        StatConvictionText.text = Stat.Conviction + "";

        if (Stat.Conviction > Stat.ConvictionOriginal)
            StatConvictionBg.color = green;
        else if (Stat.Conviction < Stat.ConvictionOriginal)
            StatConvictionBg.color = red;
        else
            StatConvictionBg.color = Color.white;

        StatResilienceText.text = Stat.Resilience + "";

        if (Stat.Resilience > Stat.ResilienceOriginal)
            StatResilienceBg.color = green;
        else if (Stat.Resilience < Stat.ResilienceOriginal)
            StatResilienceBg.color = red;
        else
            StatResilienceBg.color = Color.white;

        foreach (GameObject spell in Spells)
        {
            spell.GetComponent<SpellCombat>().UpdateDescription();
        }

        _highlightComponant.DisableHighlighting();
        OnUpdate();
    }

    public void StartPhase()
    {
        //ResetStat();
        DecompteDebuffJoueur(Decompte.phase, TimerApplication.DebutPhase);
        UpdateUI();
    }

    public void StartTurn(bool isFirstTurn = false)
    {
        //Play SFX for starting turn
        AudioManager.instance.SFX.PlaySFXClip(SFXType.StartTurnSFX);
        IsTurn = true;
        DecompteDebuffJoueur(Decompte.tour, TimerApplication.DebutTour);
        _refBattleMan.PassifManager.CurrentEvent = TimerPassif.DebutTour;
        _refBattleMan.PassifManager.ResolvePassifs();
        Stat.Volonter = Stat.VolonterMax;
        ActivateSpells();
        
        if (!isFirstTurn)
        {
            if (!gainedTension)
            {
                ApaisementTension();
            }
        }
        else Debug.Log("isFirst Turn");
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
        Stat.Radiance = Mathf.RoundToInt((Stat.Radiance / (Stat.RadianceMax*1f)) * Stat.RadianceMaxOriginal);
        Stat.RadianceMax = Stat.RadianceMaxOriginal;
        Stat.Vitesse = Stat.VitesseOriginal;
        Stat.Clairvoyance = Stat.ClairvoyanceOriginal;
        Stat.Resilience = Stat.ResilienceOriginal;
        Stat.ForceAme = Stat.ForceAmeOriginal;
        Stat.Conviction = Stat.ConvictionOriginal;

    }

    void Dead()
    {
        AudioManager.instance.SFX.PlaySFXClip(SFXType.PlayerDeathSFX, Stat.DeathSFX);
        ResetStat();
        _refBattleMan.DeadPlayer();
    }

    public void FinCombat()
    {
        ResetStat();
    }

    public void PreviewHPBarUpdate(int value, int maxRadiance)
    {
        hPBarManager.PreviewBar(value, maxRadiance);
    }
    public void StopPReviewHPBarUpdate()
    {
        hPBarManager.StopPreview();
    }
    public void PreviewTensionBarUpddate()
    {
        tensionBarManager.PreviewBar(Mathf.FloorToInt(((Stat.Tension + Stat.TensionSoin) * Stat.NbPalier) / Stat.TensionMax), Stat.NbPalier);
    }
    public void StopPreviewTensionBar()
    {
        tensionBarManager.StopPreview();
    }

    #endregion Divers start & fin

    #region Tension

    public void EnervementTension()
    {
        var t = (int) ((Stat.Tension / (Stat.NbPalier * Stat.ValeurPalier)) * Stat.NbPalier);
        if (t >= Stat.NbPalier)
            t = Stat.NbPalier;
        else
            t++;

        Stat.Tension = t * Stat.ValeurPalier;
    }

    public void ApaisementTension()
    {

        var t = (int) ((Stat.Tension / (Stat.NbPalier * Stat.ValeurPalier)) * Stat.NbPalier);
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
        if (Stat.Tension < 0)
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
        foreach (GameObject spell in Spells)
        {
            var temp = spell.GetComponent<SpellCombat>();
            if (temp.Action == toDo)
            {
                temp.selectedSpell.SetActive(true);
            }
            else
            {
                temp.selectedSpell.SetActive(false);
            }
        }

        bool isSelf = true;
        foreach (var effet in SelectedSpell.ActionEffet)
        {
            if (effet.Cible != Cible.Self && effet.Cible != Cible.joueur)
            {
                isSelf = false;
            }
        }

        foreach (var buff in SelectedSpell.ActionBuffDebuff)
        {
            if (buff.CibleApplication != Cible.Self && buff.CibleApplication != Cible.joueur)
            {
                isSelf = false;
            }
        }

        if (!isSelf)
            TakeTarget(SelectedSpell.IDSpell);
        else
        {
            _refBattleMan.idTarget = 0;
            SendSpell(false, SelectedSpell.IDSpell);
        }
    }

    public void DesactivateSpells()
    {
        foreach (var item in Spells)
        {
            item.GetComponent<SpellCombat>().button.interactable = false;
        }

        foreach (GameObject spell in Spells)
        {
            spell.GetComponent<SpellCombat>().selectedSpell.SetActive(false);
        }

        EndTurnButton.interactable = false;
        _highlightComponant.DisableHighlightingBetweenTarget();
    }

    public void ActivateSpells()
    {
        foreach (var item in Spells)
        {
            item.GetComponent<SpellCombat>().button.interactable = item.GetComponent<SpellCombat>().CheckPrice();
        }

        EndTurnButton.interactable = true;
    }

    private void TakeTarget(int IdSpell)
    {
        _refBattleMan.StartTargeting(IdSpell);
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

    public void SendSpell(bool attack, int IdSpell)
    {
        DesactivateSpells();
        DecompteDebuffJoueur(Decompte.none, TimerApplication.Attaque);
        Costs();

        AnimationController.StartAttack(AfterAnim, IdSpell);

        _refBattleMan.LaunchAnimAttacked();
    }

    private void AfterAnim()
    {
        //A Mettre une fois les combats terminer
        _refBattleMan.LaunchSpellJoueur(SelectedSpell);
        _refBattleMan.PassifManager.CurrentEvent = TimerPassif.FinAction;
        _refBattleMan.PassifManager.ResolvePassifs();
        UpdateUI();
      //  ActivateSpells();
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
            if(toAdd.timerApplication != TimerApplication.Attaque)
                ApplicationBuffDebuff(Timer, buff);
        }


        DecompteDebuffJoueur(Decompte, Timer);

        UpdateUI();
    }

    private void DecompteDebuffJoueur(Decompte Decompte, TimerApplication Timer)
    {
        DecompteDebuff(Stat.ListBuffDebuff, Decompte, this.Stat);
        var tempListBuffDebuff = Stat.ListBuffDebuff;
        foreach (var item in tempListBuffDebuff)
        {
            if (item.timerApplication == Timer)
                ApplicationBuffDebuff(Timer, item);
        }

        foreach (var item in tempAddList)
        {
            AddDebuff(item, Decompte.none, TimerApplication.Persistant);
        }

        Stat.ListBuffDebuff = UpdateBuffDebuffGameObject(Stat.ListBuffDebuff, Stat);
        UpdateUI();
    }

    public void ApplicationBuffDebuff(TimerApplication Timer, BuffDebuff toApply)
    {
        //ResetStat();
        //foreach (var item in Stat.ListBuffDebuff)
        //{
        
        if ((toApply.timerApplication == Timer || toApply.timerApplication == TimerApplication.Persistant ||
            toApply.DirectApplication))
        {
            
            foreach (var effet in toApply.Effet)
            { 
                if((toApply.timerApplication != TimerApplication.Attaque) || (toApply.timerApplication == TimerApplication.Attaque && !IsTurn))
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


    }


    #endregion BuffDebuff

    #region Effet

    public void ApplicationEffet(Effet effet, EnnemiStat Caster = null, SourceEffet source = SourceEffet.Spell,
        int idCaster = 0)
    {
        JoueurStat ModifStat;
        if (Caster == null)
        {
            var caster = _refBattleMan.EnemyScripts.Where(x => x.combatID == idCaster).FirstOrDefault();
            if (caster != null)
                ModifStat = effet.ResultEffet(caster.Stat, LastDamageTaken, this.Stat);
            else
                ModifStat = effet.ResultEffet(Stat, LastDamageTaken, Cible: Stat);
        }
        else
        {
            ModifStat = effet.ResultEffet(Caster, LastDamageTaken, Stat);
        }

        if (ModifStat.Radiance < 0)
        {
            var toRemove = ModifStat.Radiance;
            toRemove -= Mathf.FloorToInt(((Stat.Resilience * 3) / 100f) * toRemove);
            ModifStat.Radiance = toRemove;
            if (effet.IsAttaqueEffet)
                getAttacked();
        }

        //if(effet.IsFirstApplication && effet.TypeEffet == TypeEffet.RadianceMax)
        //{
        //    effet.IsFirstApplication = false;
        //    ModifStat.Radiance += ModifStat.RadianceMax;
        //}

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
            temp.GetComponent<TextAnimDegats>().Value = Mathf.FloorToInt(ModifStat.Radiance * Stat.MultiplDef);
        }
        else if (ModifStat.Radiance > 0)
        {
            ReceiveTension(Source.Soin);
            var temp = Instantiate(SoinPrefab, DamageSpawn);
            temp.GetComponent<TextAnimDegats>().Value = Mathf.FloorToInt(ModifStat.Radiance * Stat.MultiplSoin);
        }

        if (effet.IsAttaqueEffet)
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
        if (Stat.Radiance > Stat.RadianceMax)
            Stat.Radiance = Stat.RadianceMax;
        AudioManager.instance.SFX.PlaySFXClip(SFXType.EssenceConsuptionSFX);
        hPBarManager.UpdatePBar(Stat.Radiance, Stat.RadianceMax);
        ReceiveTension(source);
        UpdateUI();
    }


    #endregion Essence

    public void getAttacked()
    {
        AudioManager.instance.SFX.PlaySFXClip(SFXType.PlayerDamageTakenSFX, Stat.DamageSFX);
        DecompteDebuffJoueur(Decompte.none, TimerApplication.Attaque);
        AnimationController.GetAttacked();
    }
    

    public void endHurtAnim()
    {
        AnimationController.EndAnimAttack();
    }
}