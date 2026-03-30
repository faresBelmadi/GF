using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoueurBehavior : CombatBehavior<PlayerStatsHandler> 
{
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

    [SerializeField] private ConvictionManager _convictionManager;
    [SerializeField] private VolonteManager _volonteManager;
    [SerializeField] private HighlightCost _highlightComponant;

    [SerializeField] private Color green = new Color(0.58f, 0.98f, 0.65f);
    [SerializeField] private Color red = new Color(0.996f, 0.47f, 0.40f);
    [SerializeField] private TextMeshProUGUI ConvictionNbBuffText;
    [SerializeField] private TextMeshProUGUI HpText;
    [SerializeField] private TextMeshProUGUI HpTextReduced;
    [SerializeField] private TextMeshProUGUI HpToolTipText;
    [SerializeField] private string _radianceTextIdTrad;
    [SerializeField] private TextMeshProUGUI VolonteText;
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
    [SerializeField] private GameObject _ciblage;
    [SerializeField] private Animator _deathAnimator;
    
    [HideInInspector] public List<Spell> ListSpell { get; set; }
    [HideInInspector] public int SlotsSouvenir { get; set; }
    [HideInInspector] public List<Souvenir> ListSouvenir { get; set; }

    public static event Action OnConvictionFull;
    public static event Action OnConvictionEmpty;

    private BattleManager _refBattleMan => GameManager.Instance.BattleMan;
    [SerializeField] private bool IsTurn;

    public Spell SelectSpell => SelectedSpell;

    public override string Name { get => GameManager.Instance.classSO.NameClass; }
    public bool IsDead { get; private set; }

    #region Divers start & fin

    private int currentHp = -1;
    private float currentTens = -1;
    private int currentCons = -1;
    private bool _isHurt;
    private int _playedTurn = 0;

    public void InitRefBattleMan(BattleManager battleManager)
    {
       // _refBattleMan = battleManager;
    }
    private void OnEnable()
    {
        
        GetComponent<Animator>().Rebind();
    }

    public void StartUp()
    {
        commonStats = GameManager.Instance.CommonStatsData;
        base.Stat = GameManager.Instance.playerStatHandler;
        
        /* utile ?
        Stat.RadianceMaxOriginal = Stat.RadianceMax;
        Stat.VitesseOriginal = Stat.Vitesse;
        Stat.ClairvoyanceOriginal = Stat.Clairvoyance;
        Stat.ResilienceOriginal = Stat.Resilience - (int) Stat.ResiliencePassif;
        Stat.ConvictionOriginal = Stat.Conviction;
        Stat.ForceAmeOriginal = Stat.ForceAme;
        */
        if (Spells != null && Spells.Count > 0)
            ClearSpells();
        foreach (var item in base.Stat.ListSpell)
        {
            var temp = Instantiate(SpellPrefab, SpellsSpawn.transform);
            Spell SpelleToUse = CheckSouvenirSpell(item);
            temp.GetComponent<SpellCombat>().Action = SpelleToUse;
            temp.GetComponent<SpellCombat>().Act = DoAction;
            temp.GetComponent<SpellCombat>().StartUp();
            int volonteCost = 0, conscCost = 0, radCost = 0;
            foreach (var cost in temp.GetComponent<SpellCombat>().Action.Costs)
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

            temp.GetComponent<SpellCombat>().button.onClick.AddListener(delegate
            {
                _highlightComponant.SelectCostForHighlighing(volonteCost, radCost, conscCost);
            });
            temp.GetComponent<HighlightTriggerEvent>().SetActionsToTrigger(_highlightComponant.EnableHighlighting,
                _highlightComponant.DisableHighlighting, volonteCost, radCost, conscCost);

            Spells.Add(temp);

            base.Stat.OnConvictionChanged += ConvictionChanged;
        }

        //On instancie les passifs
        if (_stat != null)
        {
            PassiveList = new List<AbstractPassive>();
            for (int i = 0; i < _stat.BaseStat.PassiveList.Count; i++)
            {
                PassiveList.Add(Instantiate(_stat.BaseStat.PassiveList[i]));
            }
        }
        foreach (var item in PassiveList)
        {
            if (item is StatPerConsciencePassive passive)
                passive.InitPassif(Stat);
        }
        IsDead = false;
        InitUI();
    }

    private void ClearSpells()
    {
        foreach (var spell in Spells)
        {
            Destroy(spell);
        }
        Spells.Clear();
    }

    private Spell CheckSouvenirSpell(Spell item)
    {
        if (base.Stat.BaseStat.ListSouvenir == null || base.Stat.BaseStat.ListSouvenir.Count == 0)
            return item;
        foreach (var souvenir in base.Stat.BaseStat.ListSouvenir)
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
        hPBarManager.InitPBar(base.Stat.Radiance, base.Stat.RadianceMaxTotal);
        tensionBarManager.InitPBar(0, commonStats.NbPalier);
        conscienceBarManager.InitPBar(base.Stat.Conscience, base.Stat.ConscienceMaxTotal);

        for (int i = 0; i < _passiveTooltips.Count && i < PassiveList.Count; i++)
        {
            _passiveTooltips[i].InitTextComponent(PassiveList[i].IdTradDesc, PassiveList[i].DefaultDescription);
        }
    }

    public void UpdateUI()
    {
        //ProgressBar Updates
        if (base.Stat.Radiance != currentHp)
        {
            hPBarManager.UpdatePBar(base.Stat.Radiance, base.Stat.RadianceMaxTotal);
            hPBarManager.ToggleBloomPulses(false);
            //Debug.Log($"Delta: {Stat.Radiance-currentHp}");
        }
        //Debug.Log($"Radiance Updated: from {currentHp} to {Stat.Radiance}");
        currentHp = base.Stat.Radiance;

        if (base.Stat.Tension != currentTens)
        {
            tensionBarManager.UpdatePBar(Mathf.FloorToInt((base.Stat.Tension * commonStats.NbPalier) / base.Stat.TensionMax),
                commonStats.NbPalier);

            tensionBarManager.ToggleBloomPulses(((base.Stat.Tension * commonStats.NbPalier) / base.Stat.TensionMax) >= commonStats.NbPalier);

        }

        currentTens = base.Stat.Tension;

        if (base.Stat.Conscience != currentCons)
        {
            conscienceBarManager.UpdatePBar(base.Stat.Conscience, base.Stat.ConscienceMaxTotal);
            conscienceBarManager.ToggleBloomPulses(false);
        }

        currentCons = base.Stat.Conscience;


        _volonteManager.UpdateMaxVolonte(base.Stat.VolonteMax);
        _volonteManager.UpdatePoint(base.Stat.Volonte);


        HpText.text = $"{base.Stat.Radiance.ToString()}/{base.Stat.RadianceMaxTotal}";
        HpTextReduced.text = base.Stat.Radiance.ToString();
        HpToolTipText.text = $"{TradManager.instance.GetTranslation(_radianceTextIdTrad)}\nMax: {base.Stat.RadianceMaxTotal.ToString()}";
        ConscienceText.text = base.Stat.Conscience + "/" + base.Stat.ConscienceMaxTotal;

        StatClairvoyanceText.text = base.Stat.ClairvoyanceTotal + "";


        if (base.Stat.ClairvoyanceTotal > base.Stat.BaseClairvoyance)
            StatClairvoyanceBg.color = green;
        else if (base.Stat.ClairvoyanceTotal < base.Stat.BaseClairvoyance)
            StatClairvoyanceBg.color = red;
        else
            StatClairvoyanceBg.color = Color.white;


        StatForceAmeText.text = base.Stat.ForceDameTotal + "";

        if (base.Stat.ForceDameTotal > base.Stat.BaseForceDame)
            StatForceAmeBg.color = new Color(147, 250, 165);
        else if (base.Stat.ForceDameTotal < base.Stat.BaseForceDame)
            StatForceAmeBg.color = red;
        else
            StatForceAmeBg.color = Color.white;

        StatSpeedText.text = base.Stat.VitesseTotal + "";

        if (base.Stat.VitesseTotal > base.Stat.BaseVitesse)
            StatSpeedBg.color = green;
        else if (base.Stat.VitesseTotal < base.Stat.BaseVitesse)
            StatSpeedBg.color = red;
        else
            StatSpeedBg.color = Color.white;

        StatConvictionText.text = base.Stat.ConvictionTotal + "";

        if (base.Stat.ConvictionTotal > base.Stat.BaseConviction)
            StatConvictionBg.color = green;
        else if (base.Stat.ConvictionTotal < base.Stat.BaseConviction)
            StatConvictionBg.color = red;
        else
            StatConvictionBg.color = Color.white;

        StatResilienceText.text = base.Stat.ResilienceTotal + "";

        if (base.Stat.ResilienceTotal > base.Stat.BaseResilience)
            StatResilienceBg.color = green;
        else if (base.Stat.ResilienceTotal < base.Stat.BaseResilience)
            StatResilienceBg.color = red;
        else
            StatResilienceBg.color = Color.white;

        foreach (GameObject spell in Spells)
        {
            spell.GetComponent<SpellCombat>().UpdateDescription();
        }

        //  ConvictionNbBuffText.text = nbBuffDebuffApplied + "/" + commonStats.ConvictionNbBuffTrigger;
        if (base.Stat.ConvictionTotal != 0)
        {
            _convictionManager.UpdateMaxConviction(commonStats.ConvictionNbBuffTrigger);
        }
        else
            _convictionManager.UpdateMaxConviction(0);
        _convictionManager.Positive = base.Stat.ConvictionTotal > 0;
        _convictionManager.UpdatePoint(nbBuffDebuffApplied);


        _highlightComponant.DisableHighlighting();
        OnUpdate();
    }


    protected void ConvictionChanged()
    {

        nbBuffDebuffApplied = 0;
        _convictionManager.UpdatePoint(0);
        _convictionManager.UpdateMaxConviction(0); 
        if (base.Stat.ConvictionTotal != 0)
        {
            _convictionManager.UpdateMaxConviction(commonStats.ConvictionNbBuffTrigger);
        }
        _convictionManager.UpdatePoint(nbBuffDebuffApplied);
    }



    public void StartCombat()
    {
        _playedTurn = 0;
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

        /* Resplenish willpower */
        if (_playedTurn >= 1)
            base.Stat.Volonte = base.Stat.VolonteMax;

        DecompteDebuffJoueur(Decompte.tour, TimerApplication.DebutTour);

        if (!GameManager.Instance.IsTuto|| !isFirstTurn)
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

        if (base.Stat.IsStun)
        {
            Debug.Log("is stuned");
            base.Stat.IsStun = false;
            EndTurn();
        }
    }

    public void EndTurn()
    {

        IsTurn = false;
        DesactivateSpells();
        SelectedSpell = null;
        _refBattleMan.StopTargeting();
        _playedTurn++;
        EndTurnBM();
    }

    public override void ResetStat()
    {
        Stat.ResetStat();
        //Stat.Radiance = Mathf.RoundToInt((Stat.Radiance / (Stat.RadianceMax * 1f)) * Stat.RadianceMaxOriginal);
        nbBuffDebuffApplied = 0;
        _convictionManager.UpdatePoint(0);
        _convictionManager.UpdateMaxConviction(0);
        Stat.OnConvictionChanged -= ConvictionChanged;
        
    }

    void Dead()
    {
        if (IsDead) return;
        IsDead = true;
        AudioManager.instance.SFX.PlaySFXClip(SFXType.PlayerDeathSFX, base.Stat.BaseStat.DeathSFX);
        ResetStat();
        _refBattleMan.DeadPlayer();
    }
    public void DieEffect()
    {
        StartCoroutine(DeathCoroutine());
    }
    IEnumerator DeathCoroutine()
    {
        _deathAnimator.SetTrigger("Die");
        GetComponent<Animator>().enabled = false ;
        AudioManager.instance.SFX.PlaySFXClip(SFXType.EnnemyDeathSFX, base.Stat.BaseStat.DeathSFX);
        float time = 0f;
        while (time < deathDisolveTime)
        {
            //if (time * 2 >= deathDisolveTime)
            //{
            //}
            time += Time.deltaTime;
            characterMaterial.SetFloat("_DisolveHeight", time / deathDisolveTime);
            yield return null;
        }
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
        tensionBarManager.PreviewBar(
            Mathf.FloorToInt(((base.Stat.Tension + commonStats.GainTensionSoin) * commonStats.NbPalier) / base.Stat.TensionMax), commonStats.NbPalier);
    }

    public void StopPreviewTensionBar()
    {
        tensionBarManager.StopPreview();
    }

    #endregion Divers start & fin

    #region Tension

  

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

        bool needCible = false;
        List<Cible> needCiblage = new List<Cible>{ Cible.ennemi, Cible.Ally, Cible.Martyr};

        foreach (Effet effet in SelectedSpell.ActionEffet)
        {
            if (needCiblage.Contains(effet.Cible))
            {
                needCible = true;
            }
        }

        foreach (BuffDebuff buff in SelectedSpell.ActionBuffDebuff)
        {
            if (needCiblage.Contains(buff.CibleApplication))
            {
                needCible = true;
            }
        }
        
        if (needCible)
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
        if (!IsTurn)        //Si ce n'est pas notre tour, on active pas les spell
            return;
        if (_isHurt)        //On prend des dégats, on active pas encore les spells.
            return;
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
                    base.Stat.LoseConscience(price.Value);
                    break;
                case TypeCostSpell.radiance:
                    base.Stat.RemoveAmountRadiance(price.Value);
                    break;
                case TypeCostSpell.volonte:
                    base.Stat.Volonte -= price.Value;
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
       
        UpdateUI();
        //  ActivateSpells();
    }

    #endregion Spell

    #region BuffDebuff

    public void ClearBuff()
    {
        //for (int i = Stat.ListBuffDebuff.Count-1;i>=0;i--)
        //{
        //    if (Stat.ListBuffDebuff[i].Decompte != Decompte.combat)
        //    {
        //        Stat.ListBuffDebuff.RemoveAt(i);
        //    }
        //}
        
        var tempListBuffGO = ListBuffDebuffGO.Where(x => x.GetComponent<BuffDebuffComponant>().BuffDebuffs.FirstOrDefault()?.Decompte != Decompte.combat).ToList();
        //var listBuffNoCombat = Stat.ListBuffDebuff.Where(x => x.Decompte != Decompte.combat).ToList();
        if(tempListBuffGO != null && tempListBuffGO.Count > 0)
        {
            foreach (var buffGO in tempListBuffGO)
            {
                foreach (var buff in buffGO.GetComponent<BuffDebuffComponant>().BuffDebuffs)
                {
                    base.Stat.ListBuffDebuff.Remove(buff);
                }
                ListBuffDebuffGO.Remove(buffGO);
                Destroy(buffGO);
            }
        }
        
        //foreach (var buff in listBuffNoCombat)
        //{
        //    Stat.ListBuffDebuff.Remove(buff);
        //    var tempBuff = ListBuffDebuffGO.First(x => x.GetComponent<BuffDebuffComponant>().buffName == TradManager.instance.GetTranslation(buff.idTradName));
        //    ListBuffDebuffGO.Remove(tempBuff);
        //    Destroy(tempBuff);
        //}
    }

    private void ClearIncreaseConscienceBuff()
    {
        Debug.Log("ClearGainConscienceBuff");
        ClearConditionnalBuff(ConditionalBuff.GainConscience);
        base.Stat.OnConscienceIncrease -= ClearIncreaseConscienceBuff;
    }
    private void ClearDecreaseConscienceBuff()
    {
        Debug.Log("ClearPerteConscienceBuff");
        ClearConditionnalBuff(ConditionalBuff.PerteConscience);
        base.Stat.OnConscienceDecrease -= ClearDecreaseConscienceBuff;
    }
    private void ClearAleaBuff()
    {
        Debug.Log("ClearAleaBuff");
        ClearConditionnalBuff(ConditionalBuff.RoomAlea);
        GameManager.OnStartEvent -= ClearAleaBuff;
    }
    private void ClearAutelBuff()
    {
        Debug.Log("ClearAutelBuff");
        ClearConditionnalBuff(ConditionalBuff.RoomAutel);
        GameManager.OnStartAutel -= ClearAutelBuff;
    }
    private void ClearConditionnalBuff(ConditionalBuff condition)
    {
        var buffList = Stat.ListBuffDebuff.Where(x => x.ConditionnalBuff == condition).ToList();

        foreach(var buff in buffList)
        {
            foreach (var effet in buff.Effet)
            {
                if (effet.TypeEffet != TypeEffet.RadianceMax)
                    base.Stat.RemoveStat(effet.modifstateOutput);
                else
                {
                    effet.modifstateOutput.Radiance =
                        Mathf.FloorToInt((effet.Pourcentage / 100f) * base.Stat.Radiance);
                    base.Stat.RemoveStat(effet.modifstateOutput);
                }


                string buffDebuffName;
                if (buff.idTradName != null)
                {
                    buffDebuffName = TradManager.instance.GetTranslation(buff.idTradName, buff.name);
                }
                else
                {
                    buffDebuffName = buff.name;
                }

                GameObject buffObject = null;
                foreach (GameObject presentBuffObject in ListBuffDebuffGO)
                {
                    if (presentBuffObject.GetComponent<BuffDebuffComponant>().buffName == buffDebuffName)
                    {
                        buffObject = presentBuffObject;
                        break;
                    }
                }

                ListBuffDebuffGO.Remove(buffObject);
                Destroy(buffObject);

            }
            base.Stat.ListBuffDebuff.Remove(buff);
        }
        
    }
    public void AddDebuff(BuffDebuff toAdd, TimerApplication Timer)
    {
        
        if (toAdd.ConditionnalBuff != ConditionalBuff.NONE)
        {
            switch (toAdd.ConditionnalBuff)
            {
                case ConditionalBuff.RoomAutel:
                    Debug.Log("AutelBuff");
                    GameManager.OnStartAutel += ClearAutelBuff;
                    break;
                case ConditionalBuff.RoomAlea:
                    Debug.Log("AleaBuff");
                    GameManager.OnStartEvent += ClearAleaBuff;
                    break;
                case ConditionalBuff.GainConscience:
                    Debug.Log("GainConscienceBuff");
                    base.Stat.OnConscienceIncrease += ClearIncreaseConscienceBuff;
                    break;
                case ConditionalBuff.PerteConscience:
                    Debug.Log("PerteConscienceBuff");
                    base.Stat.OnConscienceDecrease += ClearDecreaseConscienceBuff;
                    break;
                case ConditionalBuff.NouvelEtage:
                    Debug.Log("NouvelEtageBuff");
                    // TODO: Ajouter subscribe a l'evenement de changement d'étage
                    break;
            }
        }
        

        for (int i = 0; i < base.Stat.MultiplBuffDebuff; i++)
        {
            if(((toAdd.IDCombatOrigine == _refBattleMan.idPlayer && base.Stat.ConvictionTotal > 0 && !toAdd.IsDebuff) 
                || (toAdd.IDCombatOrigine != _refBattleMan.idPlayer && base.Stat.ConvictionTotal<0 && toAdd.IsDebuff)) && _refBattleMan.IsCombatOn)
            {

                nbBuffDebuffApplied++;
                if(nbBuffDebuffApplied == commonStats.ConvictionNbBuffTrigger-1) 
                {
                    OnConvictionFull?.Invoke();
                }
                else
                {
                    OnConvictionEmpty?.Invoke();
                }
            }

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
            var modifiedBuff = ApplyConviction(buff, ValueConviction());
            Stat.ListBuffDebuff.Add(modifiedBuff);
            base.AddBuffDebuff(modifiedBuff, Stat);
            if (toAdd.timerApplication != TimerApplication.Attaque)
                ApplicationBuffDebuff(Timer, modifiedBuff);
        }


        UpdateUI();
    }

    private void DecompteDebuffJoueur(Decompte Decompte, TimerApplication Timer)
    {

        DecompteDebuff(base.Stat.ListBuffDebuff, Decompte);
        base.Stat.ListBuffDebuff = UpdateBuffDebuffGameObject(base.Stat.ListBuffDebuff, Stat);

        var tempListBuffDebuff = base.Stat.ListBuffDebuff;


        foreach (var item in tempListBuffDebuff)
        {
            if (item.timerApplication == Timer)
                ApplicationBuffDebuff(Timer, item);
        }

        foreach (var item in tempAddList)
        {
            AddDebuff(item, TimerApplication.Persistant);
        }

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
                if ((toApply.timerApplication != TimerApplication.Attaque) ||
                    (toApply.timerApplication == TimerApplication.Attaque && !IsTurn))
                    _refBattleMan.PassageEffet(effet, toApply.IDCombatOrigine, 0, SourceEffet.BuffDebuff);

                /*if(item.CibleApplication == effet.Cible)
                {
                    ApplicationEffet(effet);
                }
                else
                {
                    //A Mettre une fois les combats terminer
                    GameManagerRemake.Instance.BattleMan.PassageEffet(effet, item.IDCombatOrigine);
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

    public void ApplicationEffet(Effet effet, EnemyStatsHandler Caster = null, SourceEffet source = SourceEffet.Spell,
        int idCaster = 0)
    {
        
        JoueurStat ModifStat;
        var caster = _refBattleMan.EnemyScripts.Where(x => x.combatID == idCaster).FirstOrDefault();
        if (Caster == null)
        {
            if (caster != null)
                ModifStat = effet.ResultEffet(caster.Stat, LastDamageTaken, Stat);
            else
                ModifStat = effet.ResultEffet(Stat, LastDamageTaken, Cible: Stat);
        }
        else
        {
            if (caster != null)
                ModifStat = effet.ResultEffet(caster.Stat, LastDamageTaken, Stat);
            else
                ModifStat = effet.ResultEffet(Stat, LastDamageTaken, Stat);
        }

        if (ModifStat.Radiance < 0)
        {
            var toRemove = ModifStat.Radiance;
            toRemove -= Mathf.FloorToInt(((base.Stat.ResilienceTotal * 3) / 100f) * toRemove);
            ModifStat.Radiance = toRemove;
            if (effet.IsAttaqueEffet)
                GetAttacked();
        }

        //if(effet.IsFirstApplication && effet.TypeEffet == TypeEffet.RadianceMax)
        //{
        //    effet.IsFirstApplication = false;
        //    ModifStat.Radiance += ModifStat.RadianceMax;
        //}
        GameManager.Instance.BattleMan.LogRadianceChange(this.Name, GameManager.Instance.BattleMan.GetBehaviorNameFromStat(Caster==null?null:Caster.BaseStat), ModifStat.Radiance);
        Stat.UpdateStat(ModifStat);
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
            temp.GetComponent<TextAnimDegats>().Value = Mathf.FloorToInt(ModifStat.Radiance * base.Stat.MultiplDef);
        }
        else if (ModifStat.Radiance > 0)
        {
            ReceiveTension(Source.Soin);
            var temp = Instantiate(SoinPrefab, DamageSpawn);
            temp.GetComponent<TextAnimDegats>().Value = Mathf.FloorToInt(ModifStat.Radiance * base.Stat.MultiplSoin);
        }

        if (effet.IsAttaqueEffet)
        {
            foreach (var item in base.Stat.ListBuffDebuff)
            {
                if (item.timerApplication == TimerApplication.Attaque)
                    ApplicationBuffDebuff(TimerApplication.Attaque, item);
            }
        }


        UpdateUI();


        if (base.Stat.Radiance <= 0)
        {
            Dead();
        }
    }

    #endregion Effet


    #region Essence

    public void UseEssence(int Essence, Source source)
    {
        Debug.Log("Player use essence to heal " + Essence + " radiance.");
        base.Stat.ChangeRadiance(Essence);
        
        //if (Stat.Radiance > Stat.RadianceMax)
        //    Stat.Radiance = Stat.RadianceMax;
        AudioManager.instance.SFX.PlaySFXClip(SFXType.EssenceConsuptionSFX);
        hPBarManager.UpdatePBar(base.Stat.Radiance, base.Stat.RadianceMaxTotal);
        ReceiveTension(source);
        UpdateUI();
    }


    #endregion Essence

    public void GetAttacked()
    {
        AudioManager.instance.SFX.PlaySFXClip(SFXType.PlayerDamageTakenSFX, base.Stat.BaseStat.DamageSFX);
        DecompteDebuffJoueur(Decompte.none, TimerApplication.Attaque);
        AnimationController.GetAttacked();
        _isHurt = true;
    }


    public void EndHurtAnim()
    {
        _isHurt = false;
        AnimationController.EndAnimAttack();
    }

    #region Passif
    protected virtual void ResolvePassif()
    {

    }
    protected virtual void UpdateStat()
    {

    }
    #endregion

    #region UI Ciblage
    public void ShowTargeting() => _ciblage.SetActive(true);
    public void HideTargeting() => _ciblage.SetActive(false);
    #endregion

}