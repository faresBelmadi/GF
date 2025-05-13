using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
//using static UnityEditor.Progress;

[System.Serializable]
public class BattleManager : MonoBehaviour
{
    [Header("Tuto")][SerializeField] private bool _isTuto = false;

    [Header("BattleLogger")]
    [SerializeField]
    private BattleLog _battleLogger;

    [Header("Prefab CombatNormal")] public JoueurBehavior player;
    public List<GameObject> SpawnedEnemy;
    public List<EnnemyBehavior> EnemyScripts;
    public List<EnnemyBehavior> DeadEnemyScripts;
    public Transform[] spawnPos;
    public Encounter _encounter;

    public GameObject buttonEndCombat;
    [SerializeField] private string _idLabelForEssenceButton;
    const string Target = "Targeting";

    [Header("CrystalSoul Manager")]
    [Tooltip("Put three Essence Prefab, from the smallest, to the greatest")]
    [SerializeField]
    public List<GameObject> _prefabEssenceList;

    [SerializeField] private int _amountForGreaestEssence;
    [SerializeField] private int _amountForMediumEssence;

    [Header("Round/Turn variables")] public List<CombatOrder> IdOrder;
    public int nbPhase = 0;
    public Dictionary<int, int> IdSpeedDictionary;
    public List<GameObject> ListEssence = new List<GameObject>();
    [SerializeField] private TurnOrderUIManager turnOrderUIManager;

    [Header("Stats combats")] public float CalmeMoyen;
    public float CalmeMoyenAdversaire;
    public float CalmeMoyenJoueur;

    [SerializeField] int idIndexer = 0;
    public int idPlayer;
    public int currentIdTurn;
    public int nbTurn;
    public int idTarget = -1;
    public bool endBattle;
    BattleUI battleUI;
    public int MostDamage, MostDamageID;
    public int LastPhaseDamage;

    public int CurrentPhaseDamage;

    // [SerializeField] private DialogueManager DialogueManager;


    public bool IsLoot;
    public bool ConsumedEssence;
    public int EssenceGained;

    public bool IsCombatOn { get; private set; }

    [SerializeField] private Material characterMaterial;
    [SerializeField] private Material ennemiUIMaterial;

    private bool _isDetailledCombat;
    public bool IsDetailledCombat
    {
        get => _isDetailledCombat;
        set
        {
            _isDetailledCombat = value;
        }
    }

    public bool IsTuto
    {
        get => _isTuto;
    }

    public static Action<Transform> OnGatherEssence;

    #region Reference

    /// <summary>
    /// Return Combat behavior linked to the given character stat
    /// </summary>
    /// <param name="stat">CharacterStat to find</param>
    /// <returns>The linked COmbatBehaviour</returns>
    public string GetBehaviorNameFromStat(CharacterStat stat)
    {
        if (stat == null) return null;

        if (player.Stat == stat)
            return player.Name;
        foreach (var ennemy in EnemyScripts)
        {
            if (ennemy.Stat == stat)
                return ennemy.Name;
        }

        return "Undidentified";
    }

    #endregion

    #region Loot

    public void Loot()
    {
        int random = UnityEngine.Random.Range(0, 101);
        Debug.Log("Loot : " + random);
        if (random > _encounter.PourcentageLootSouvenir)
        {
            IsLoot = false;
            return;
        }

        _encounter.LootRarity.Sort((x, y) => x.Pourcentage.CompareTo(y.Pourcentage));
        int PourcentageTotal = 0;
        for (int i = 0; i < _encounter.LootRarity.Count; i++)
        {
            PourcentageTotal += _encounter.LootRarity[i].Pourcentage;
        }

        random = UnityEngine.Random.Range(0, PourcentageTotal + 1);
        Debug.Log("Rarity : " + random);
        for (int i = 0; i < _encounter.LootRarity.Count; i++)
        {
            if (random <= _encounter.LootRarity[i].Pourcentage &&
                GameManager.Instance.CopyAllSouvenir.Any(x => x.Rarete == _encounter.LootRarity[i].rareter))
            {
                var allSouvenirRareter = GameManager.Instance.CopyAllSouvenir
                    .Where(x => x.Rarete == _encounter.LootRarity[i].rareter).ToList();
                int randomSouvenir = UnityEngine.Random.Range(0, allSouvenirRareter.Count());

                string NameLoot = allSouvenirRareter[randomSouvenir].SouvenirName;
                var newSouvenir = GameManager.Instance.CopyAllSouvenir.FirstOrDefault(c => c.SouvenirName == NameLoot);
                player.Stat.ListSouvenir.Add(Instantiate(newSouvenir));
                GameManager.Instance.CopyAllSouvenir.Remove(newSouvenir);
                IsLoot = true;
                return;
            }
            else
            {
                random -= _encounter.LootRarity[i].Pourcentage;
                IsLoot = false;
            }
        }
    }

    #endregion Loot

    #region calcul tension & calme

    private void CalcTensionJoueur()
    {
        player.Stat.TensionMax = (CalmeMoyenAdversaire / CalmeMoyen) * player.Stat.Calme;
        player.Stat.ValeurPalier = player.Stat.TensionMax / GameManager.Instance.CommonStatsData.NbPalier;
        if (player.Stat.PalierChangement > 0)
        {
            player.Stat.Tension = player.Stat.ValeurPalier * player.Stat.PalierChangement;
        }

        Debug.Log($"Caculate tension joueur");
        Debug.Log(
            $"TMax = {player.Stat.TensionMax};\nVPal = {player.Stat.ValeurPalier};\nCurrent Tens = {player.Stat.Tension}];\nPalChange = {player.Stat.PalierChangement}]");
    }

    private void CalcTensionEnemy()
    {
        foreach (var item in EnemyScripts)
        {
            if (!item.Stat.NoTension)
            {
                item.Stat.TensionMax = (CalmeMoyenJoueur / CalmeMoyen) * item.Stat.Calme;
                item.Stat.ValeurPalier = (item.Stat.TensionMax) / GameManager.Instance.CommonStatsData.NbPalier;
                if (item.Stat.PalierChangement > 0)
                {
                    item.Stat.Tension = item.Stat.ValeurPalier * item.Stat.PalierChangement;
                }

                Debug.Log($"Caculate tension Ennemie");
                Debug.Log(
                    $"TMax = {item.Stat.TensionMax};\nVPal = {item.Stat.ValeurPalier};\nCurrent Tens = {item.Stat.Tension}];\nPalChange = {item.Stat.PalierChangement}]");
            }
        }
    }

    void CalcCalmeMoyen()
    {
        float tempCalmeEnemy = 0;
        int count = 0;
        for (int i = 0; i < EnemyScripts.Count; i++)
        {
            if (!EnemyScripts[i].Stat.NoTension)
            {
                tempCalmeEnemy += EnemyScripts[i].Stat.Calme;
                count++;
            }
        }

        CalmeMoyenJoueur = player.Stat.Calme;
        CalmeMoyenAdversaire = tempCalmeEnemy / count;
        //remplacer 1 par une variable si le cas de plusieurs personnage jouable arrive
        CalmeMoyen = (tempCalmeEnemy + player.Stat.Calme) / (count + 1);
    }

    private bool CheckTension(int key)
    {

        if (key == idPlayer)
        {
            if (player.CanHaveAnotherTurn())
            {
                player.EndTurnButton.gameObject.GetComponent<UnityEngine.UI.Image>().material.SetInt("_isEnraged", 1);
                //SFX to play when full tension
                AudioManager.instance.SFX.PlaySFXClip(SFXType.PlayerFullTensionSFX);
                return true;
            }
            else
            {
                player.EndTurnButton.GetComponent<UnityEngine.UI.Image>().material.SetInt("_isEnraged", 0);
                return false;
            }
        }
        else
        {
            var t = EnemyScripts.First(c => c.combatID == key);
            if (t.CanHaveAnotherTurn())
            {
                t.GetComponent<UIEnnemi>().imageCadreFGs[0].material.SetInt("_isEnraged", 1);
                t.GetComponent<UIEnnemi>().imageCadreFGs[1].material.SetInt("_isEnraged", 1);
                t.GetComponent<UIEnnemi>().inflateUISystem.TriggerInflation();
                //SFX to play when full tension
                AudioManager.instance.SFX.PlaySFXClip(SFXType.EnnemyFullTensionSFX);
                return true;
            }
            else
            {
                t.GetComponent<UIEnnemi>().imageCadreFGs[0].material.SetInt("_isEnraged", 0);
                t.GetComponent<UIEnnemi>().imageCadreFGs[1].material.SetInt("_isEnraged", 0);
            }
        }

        return false;
    }

    private void UpdateEnrageUI(int turnId)
    {
        if (turnId == idPlayer)
        {
            if (!player.CanHaveAnotherTurn())
            {
                player.EndTurnButton.GetComponent<UnityEngine.UI.Image>().material.SetInt("_isEnraged", 0);
            }
        }
        else
        {
            EnnemyBehavior behaviour = EnemyScripts.First(c => c.combatID == turnId);
            if (!behaviour.CanHaveAnotherTurn())
            {
                behaviour.GetComponent<UIEnnemi>().imageCadreFGs[0].material.SetInt("_isEnraged", 0);
                behaviour.GetComponent<UIEnnemi>().imageCadreFGs[1].material.SetInt("_isEnraged", 0);
            }
        }
    }

    #endregion calcul tension & calme

    #region Mise en place combat & fin

    private void OnEnable()
    {
        //CombatEnableSetup();
        GameManager.OnStartDialog += CombatEnableSetup; // We need to instantiate character for the dialog
    }

    private void OnDisable()
    {
        GameManager.OnStartDialog -= CombatEnableSetup;
    }

    public int getJoueurClairvoyance()
    {
        return player.Stat.Clairvoyance;
    }
    void DialogueEnableSetup()
    {
        player.InitRefBattleMan(this);
       
        GameManager.Instance.DialManager.SetupDialogue(_encounter);
    }

    void CombatEnableSetup()
    {
        idIndexer = 0;
        battleUI = GetComponent<BattleUI>();
        //if (GameManager.Instance == null)
        //    player.Stat = TutoManager.Instance.JoueurStat;
        //else 
        //    player.Stat = GameManager.Instance.playerStat;
        if (GameManager.Instance.IsTuto)
            player.Stat = TutoManager.Instance.JoueurStat;
        else
            player.Stat = GameManager.Instance.playerStat;
        player.EndTurnBM = EndTurn;
        player.StartUp();
        SpawnedEnemy = new List<GameObject>();
        EnemyScripts = new List<EnnemyBehavior>();
        DeadEnemyScripts = new List<EnnemyBehavior>();
        IdOrder = new List<CombatOrder>();
        IdSpeedDictionary = new Dictionary<int, int>
        {
            {idIndexer, player.Stat.Vitesse}
        };
        idPlayer = idIndexer;
        idIndexer++;
    }

    public void LoadEnemy(Encounter ToSpawn)
    {
        _encounter = GameManager.Instance.IsTuto ? Instantiate(TutoManager.Instance._encounter[TutoManager.Instance.IndexEncounter]) : ToSpawn;
        SpawnEnemy();
        player.UpdateUI();
        player.DesactivateSpells();
        GameManager.Instance.DialManager.InitDialogOptionButton();
        DialogueEnableSetup();


        //StartCombat();
    }

    void SpawnEnemy()
    {
        List<int> remainingPos = new List<int> { 0, 1, 2, 3 };
        List<int> ennemyPosIds = new List<int> { -1, -1, -1, -1 };
        List<EncounterOption> encounterOptions = _encounter.forcedOrder.ToList();

        int firstMaxPos = (spawnPos.Length - encounterOptions.Count);
        //Debug.Log($"firstMaxPos = {firstMaxPos}");
        int firstChoosedPos = UnityEngine.Random.Range(0, firstMaxPos + 1);
        //Debug.Log($"firstChoosedPos = {firstChoosedPos}");

        for (int i = 0; i < encounterOptions.Count; i++)
        {
            int ennemiPos = firstChoosedPos + i; //remainingPos[i];//UnityEngine.Random.Range(0, remainingPos.Count)];
            //Debug.Log($"Choosed Forced Pos = {ennemiPos}");

            int ennemyId = encounterOptions[i].possibleId[UnityEngine.Random.Range(0, encounterOptions[i].Count)];
            //Debug.Log($"Choosed Ennemy Id = {ennemyId}");

            for (int j = 0; j < encounterOptions.Count; j++)
            {
                if (encounterOptions[j].possibleId.Contains(ennemyId))
                {
                    encounterOptions[j].possibleId.Remove(ennemyId);
                }
            }

            ennemyPosIds[ennemiPos] = ennemyId;
        }

        //Debug.Log("Forced  Only:");
        //for (int i = 0; i < ennemyPosIds.Count; i++)
        //{
        //Debug.Log($"pos: {i} spawn :{ennemyPosIds[i]}");
        //}
        remainingPos = new List<int>();
        for (int i = 0; i < ennemyPosIds.Count; i++)
        {
            if (ennemyPosIds[i] == -1) remainingPos.Add(i);
        }

        if (_encounter.IsForced)
        {
            InstanciateEnnemy(0, _encounter.ForcedPosition);
        }
        else
        {
            for (int i = 0; i < _encounter.ToFight.Count; i++)
            {
                if (!ennemyPosIds.Contains(i))
                {
                    int choosedPos;
                    choosedPos = remainingPos[UnityEngine.Random.Range(0, remainingPos.Count)];
                    //Debug.Log($"Ennemy {i} not in list");
                    remainingPos.Remove(choosedPos);
                    ennemyPosIds[choosedPos] = i;
                    //Debug.Log($"Adding it to pos {choosedPos}");
                }

            }

            //Debug.Log("Instantiate:");
            for (int i = 0; i < ennemyPosIds.Count; i++)
            {
                //Debug.Log($"pos: {i} spawn :{ennemyPosIds[i]}");
                if (ennemyPosIds[i] > -1)
                {
                    InstanciateEnnemy(ennemyPosIds[i], i);
                }
            }
        }
        Debug.Log("end spawn");
    }

    void InstanciateEnnemy(int ennemyId, int spawnPosId)
    {
        EnnemiStat EnnemyStats = _encounter.ToFight[ennemyId];
        var temp = Instantiate(EnnemyStats.Spawnable, spawnPos[spawnPosId].position, Quaternion.identity,
            spawnPos[spawnPosId]);
        if (temp != null)
        {
            SpawnedEnemy.Add(temp);
        }

        var tempCombatScript = temp.GetComponent<EnnemyBehavior>();
        //instantiate tout les so modifiable
        if (tempCombatScript != null)
        {
            GameManager.Instance.DialManager.AddSpeakers(ennemyId, tempCombatScript);
            tempCombatScript.Stat = Instantiate(EnnemyStats);
            tempCombatScript.SetUp();
            tempCombatScript.EndTurnBM = EndTurn;
            tempCombatScript.isMainEnemy = ennemyId == _encounter.idMainMob ? true : false;
            EnemyScripts.Add(tempCombatScript);

            IdSpeedDictionary.Add(idIndexer, tempCombatScript.Stat.Vitesse);
            tempCombatScript.combatID = idIndexer;
            tempCombatScript.ChooseNextAction();
            idIndexer++;
        }

        //AddingMaterial
        var uiEnnemi = temp.GetComponent<UIEnnemi>();
        if (uiEnnemi != null)
        {
            uiEnnemi.imageCadreFGs[0].material = new Material(ennemiUIMaterial);
            uiEnnemi.imageCadreFGs[1].material = new Material(ennemiUIMaterial);
        }

        Material thisCharMaterial = new Material(characterMaterial);
        if (tempCombatScript != null) tempCombatScript.characterMaterial = thisCharMaterial;

        var pulseBloomSystem = temp.GetComponent<PulseBloom_System>();
        if (pulseBloomSystem != null)
            pulseBloomSystem.bloomMaterial = thisCharMaterial;


        foreach (SpriteRenderer renderer in temp.GetComponentsInChildren<SpriteRenderer>(true))
        {
            renderer.material = thisCharMaterial;
        }
    }

    public void StartCombat()
    {
        IsCombatOn = true;
        player.DecompteDebuff(player.Stat.ListBuffDebuff, Decompte.combat, player.Stat);
        CalcCalmeMoyen();
        CalcTensionEnemy();
        CalcTensionJoueur();

        //Launch Passive effect for players
        foreach (var item in player.PassiveList)
        {
            if (item is IStartCombatPassive passive)
                passive.ApplyEffectOnStartCombat();
        }
        //Launch passive effect for ennemies
        for (int i=0;i<EnemyScripts.Count;i++)
        {
            foreach (var item in EnemyScripts[i].PassiveList)
            {
                if (item is IStartCombatPassive passive)
                    passive.ApplyEffectOnStartCombat();
                if (item is IDecoyPassive)
                    EnemyScripts[i].MakeTangible(); //On rend le decoy tangible
                if (item is IUpdateEnnemyBehaviorPassive updatePassive)
                    updatePassive.InitPassif(EnemyScripts[i]);
            }
        }
        StartPhase();
    }

    private void EndBattle()
    {
        IsCombatOn = false;
       
        Loot();
        player.ResetStat();
        //player.Stat.ListBuffDebuff.Clear();
        //player.ClearBuffBar();
        player.ClearBuff();
        player.Stat.Volonter = player.Stat.VolonterMax;
        player.Stat.Tension = 0;
        Debug.Log(IsLoot);
        if (GameManager.Instance.IsTuto)
        {
            TutoManager.Instance.EndCombat();
            TutoManager.Instance.Loot();
            var child = TutoManager.Instance.TutoPanel.transform.GetChild(0);
            child.gameObject.SetActive(true);
            var tutoPanelScript = TutoManager.Instance.TutoPanel.GetComponent<TutoPanel>();
            tutoPanelScript.ShowExplication();
            TutoManager.Instance.TutoPanel.transform.parent = TutoManager.Instance.CanvasMap.transform;
        }
        else
            GameManager.Instance.playerStat = player.Stat;

        buttonEndCombat.SetActive(false);
        StartCoroutine(GameManager.Instance.pmm.EndBattle(IsLoot));
        ClearListEssence();
    }

    #endregion Mise en place combat & fin

    #region Phase

    private void StartPhase()
    {
        //Play start phase sound
        AudioManager.instance.SFX.PlaySFXClip(SFXType.StartPhaseSFX);

        LastPhaseDamage = CurrentPhaseDamage;
        CurrentPhaseDamage = 0;
        DetermTour();
        player.StartPhase();
        foreach (var item in EnemyScripts)
        {
            item.StartPhase();
        }

        currentIdTurn = 0;
        nbPhase++;
        nbTurn = 0;
        turnOrderUIManager.GenerateNextTurnOrder(IdOrder);
        //StartNextTurn();
    }

    private void DetermTour()
    {
        var test = IdSpeedDictionary.OrderByDescending(c => c.Value);
        IdOrder = new List<CombatOrder>();
        foreach (var item in test)
        {
            IdOrder.Add(new CombatOrder() { id = item.Key, Played = false });
            if (CheckTension(item.Key))
                IdOrder.Add(new CombatOrder() { id = item.Key, Played = false });
        }
        //turnOrderUIManager.GenerateTurnItems(IdOrder);
    }

    #endregion Phase

    #region Turn

    public void StartNextTurn()
    {
        Debug.Log($"Start Nex Turn, PhaseNb: {nbPhase}");
        int key = IdOrder.First(c => c.Played == false).id;
        currentIdTurn = key;
        if (key == idPlayer)
        {
            //TEST FOR GENERAL POPUP
            //GeneralPopUp.Instance.InvokePopUp("Start Player Turn","this is a test message describing the pop Up",.5f);

            player.StartTurn(nbPhase < 2);
            battleUI.textPLayingTurn.text = (GameManager.Instance != null)
                ? GameManager.Instance.classSO.NameClass
                : TutoManager.Instance.TutoClassSo.NameClass;
        }
        else
        {
            var playing = EnemyScripts.First(c => c.combatID == key);
            playing.StartTurn(nbPhase < 2);
            battleUI.textPLayingTurn.text = playing.UICombat.NameText.text;
        }

        player.UpdateUI();
        UpdateEnrageUI(key);
    }

    void EndTurn()
    {
        var turnPlayed = IdOrder.FirstOrDefault(c => c.id == currentIdTurn && !c.Played);
        if (turnPlayed != null)
            turnPlayed.Played = true;
        nbTurn++;
        if (nbTurn >= IdOrder.Count)
        {
            StartPhase();
        }
        else
            turnOrderUIManager.EvovlveTurnOrder(); //StartNextTurn();

    }

    #endregion Turn

    #region Lien Joueur - Ennemi

    public void LaunchSpellJoueur(Spell spell)
    {
        LogLaunchedSpell(player.Name, spell);
        player.DesactivateSpells();
        AudioManager.instance.SFX.PlaySFXClip(SFXType.PlayerSpellSFX, spell.SpellSFX);
        foreach (var effet in spell.ActionEffet)
        {
            PassageEffet(effet, idPlayer, idTarget, SourceEffet.Spell);
            if (effet.AfterEffectToApply != null)
            {
                player.Stat.ListBuffDebuff = player.UpdateBuffDebuffGameObject(player.Stat.ListBuffDebuff, player.Stat);
                var ennemy = EnemyScripts.FirstOrDefault(c => c.combatID == idTarget);
                if (ennemy == null)
                {
                    Debug.Log("l'ennemi est mort");
                    if (effet.AfterEffectToApply.Effet.Any(x => x.TypeEffet == TypeEffet.OnKillStunAll))
                    {
                        effet.nbProcAfterEffect++;
                    }

                    foreach (var enemyScript in EnemyScripts)
                    {
                        enemyScript.Stat.ListBuffDebuff =
                            enemyScript.UpdateBuffDebuffGameObject(enemyScript.Stat.ListBuffDebuff, enemyScript.Stat);
                    }
                    //if (effet.AfterEffectToApply.Effet.Any(x=>x.TypeEffet == TypeEffet.OnKillStunAll))
                    //{
                    //    //appliquer aux autre l'effet de stun

                    //}
                }
                else
                {
                    ennemy.Stat.ListBuffDebuff =
                        ennemy?.UpdateBuffDebuffGameObject(ennemy.Stat.ListBuffDebuff, ennemy?.Stat);
                }

                ApplyAfterEffect(effet);
            }
        }

        //EnemyScripts.First(c => c.combatID == idTarget).ApplicationBuffDebuff(TimerApplication.Attaque);
        GiveBuffDebuff(spell.ActionBuffDebuff, idTarget);
        idTarget = -1;

    }

    public void LaunchSpellEnnemi(EnnemiSpell Spell)
    {
        var playing = EnemyScripts.First(c => c.combatID == currentIdTurn);
        LogLaunchedSpell(playing.Name, Spell);
        foreach (var effet in Spell.Effet)
        {
            PassageEffet(effet, currentIdTurn, -1, SourceEffet.Spell);
            if (effet.AfterEffectToApply != null)
            {
                //var ennemy = EnemyScripts.First(c => c.combatID == Spell.ID);
                //ennemy.UpdateBuffDebuffGameObject(ennemy.Stat.ListBuffDebuffGO);
                ApplyAfterEffect(effet);
            }
        }

        GiveBuffDebuff(Spell.debuffsBuffs);
        //player.ApplicationBuffDebuff(TimerApplication.Attaque);
    }

    public void LogLaunchedSpell(string launcherName, IBattleLogSpell spell)
    {
        if (_battleLogger.gameObject.activeInHierarchy)
            _battleLogger.AddBattleLaunchSpellLogLine(launcherName, spell);
    }

    public void LogRadianceChange(string target, string source, int amount)
    {
        if (_battleLogger.gameObject.activeInHierarchy)
            _battleLogger.AddDamageLogLine(target, source, amount);
    }

    private void ApplyAfterEffect(Effet effet) // ICI DANGER: en cas d'after effect Applique 2 fois les buff debuff!
    {
        List<BuffDebuff> afterEffect = new List<BuffDebuff>();
        for (int i = 0; i < effet.nbProcAfterEffect; i++)
        {
            afterEffect.Add(effet.AfterEffectToApply);
        }

        GiveBuffDebuff(afterEffect, idTarget);
    }
 
    public void GiveBuffDebuff(List<BuffDebuff> BuffDebuff, int target = -1)
    {
        int origine = IsCombatOn ? currentIdTurn : -1;
        Decompte Decompte = Decompte.none;
        TimerApplication Timer = TimerApplication.Attaque;
        foreach (var item in BuffDebuff)
        {
            item.IDCombatOrigine = origine;
            switch (item.CibleApplication)
            {
                case Cible.joueur:
                    player.AddDebuff(item, Decompte, Timer);
                    break;
                case Cible.ennemi:
                    if (target != -1)
                        if (EnemyScripts.FirstOrDefault(c => c.combatID == target) != null)
                            EnemyScripts.First(c => c.combatID == target).AddDebuff(item, Decompte, Timer);
                        else
                        {
                            int index;
                            do
                            {
                                index = UnityEngine.Random.Range(1, EnemyScripts.Count + 1);

                            } while (index == origine && EnemyScripts.Count > 1);

                            var ennemy = EnemyScripts.FirstOrDefault(c => c.combatID == index);
                            if (ennemy != null)
                                ennemy.AddDebuff(item, Decompte, Timer);
                            else
                                EnemyScripts.First().AddDebuff(item, Decompte, Timer);
                        }

                    break;
                case Cible.Ally:
                    int indexAlly;
                    do
                    {
                        indexAlly = UnityEngine.Random.Range(1, EnemyScripts.Count + 1);

                    } while (indexAlly == origine && EnemyScripts.Count > 1);

                    var ennemyAlly = EnemyScripts.FirstOrDefault(c => c.combatID == indexAlly);
                    if (ennemyAlly != null)
                        ennemyAlly.AddDebuff(item, Decompte, Timer);
                    else
                        EnemyScripts.First().AddDebuff(item, Decompte, Timer);
                    break;
                case Cible.Martyr:
                    var martyr = EnemyScripts.FirstOrDefault(c => c.Stat.Nom == "Martyr");
                    if (martyr != null)
                    {
                        martyr.AddDebuff(item, Decompte, Timer);
                    }

                    break;
                case Cible.allEnnemi:
                    foreach (var ennemie in EnemyScripts)
                    {
                        ennemie.AddDebuff(item, Decompte, Timer);
                    }

                    break;
                case Cible.AllAllyExceptSelf:
                    for (int x = EnemyScripts.Count - 1; x >= 0; x--)
                    {
                        var ennemie = EnemyScripts[x];
                        if (ennemie != null && ennemie.combatID != origine)
                            ennemie.AddDebuff(item, Decompte, Timer);
                    }

                    break;
                case Cible.All:
                    player.AddDebuff(item, Decompte, Timer);
                    foreach (var ennemie in EnemyScripts)
                    {
                        ennemie.AddDebuff(item, Decompte, Timer);
                    }

                    break;
                case Cible.Self:
                    var self = EnemyScripts.FirstOrDefault(c => c.combatID == origine);
                    if (self != null)
                        self.AddDebuff(item, Decompte, Timer);
                    break;

            }
        }
    }

    public void PassageEffet(Effet effet, int Caster, int target = -1, SourceEffet source = SourceEffet.Spell)
    {
        bool isDecoy = false;
        EnnemyBehavior decoy = null;
        foreach (var ennemy in EnemyScripts)
        {
            foreach (var item in ennemy.PassiveList)
            {
                if (item is IDecoyPassive passive)
                {
                    isDecoy = true;
                    decoy = ennemy;
                }
            }
        }
        switch (effet.Cible)
        {
            case Cible.joueur:
                if (Caster == idPlayer)
                {
                    player.ApplicationEffet(effet, null, source);
                }
                else if (isDecoy)
                {
                    if (EnemyScripts.FirstOrDefault(c => c.combatID == Caster) == null)
                    {
                        decoy.ApplicationEffet(effet, null, source,
                            Caster);
                    }
                    else
                    {
                        decoy.ApplicationEffet(effet, null, source,
                            Caster);
                    }
                }
                else
                {
                    if (EnemyScripts.FirstOrDefault(c => c.combatID == Caster) == null)
                    {
                        player.ApplicationEffet(effet, DeadEnemyScripts.First(c => c.combatID == Caster).Stat, source,
                            Caster);
                    }
                    else
                    {
                        player.ApplicationEffet(effet, EnemyScripts.First(c => c.combatID == Caster).Stat, source,
                            Caster);
                    }
                }

                break;
            case Cible.ennemi:

                if (Caster == target)
                {
                    EnemyScripts.First(c => c.combatID == target).ApplicationEffet(effet, null, source, Caster);
                }
                else
                {
                    if (target != -1)
                    {
                        if (EnemyScripts.FirstOrDefault(c => c.combatID == target) != null)
                            EnemyScripts.First(c => c.combatID == target).ApplicationEffet(effet, null, source, Caster);
                    }
                    else
                    {
                        int index = Caster;
                        do
                        {
                            index = UnityEngine.Random.Range(1, EnemyScripts.Count + 1);

                        } while (index == Caster && EnemyScripts.Count > 1);

                        var ennemy = EnemyScripts.FirstOrDefault(c => c.combatID == index);
                        if (ennemy != null)
                            ennemy.ApplicationEffet(effet, null, source, Caster);
                        else
                            EnemyScripts.First().ApplicationEffet(effet, null, source, Caster);
                    }
                }

                break;
            case Cible.Self:
                if (Caster == idPlayer)
                {
                    player.ApplicationEffet(effet, null, source);
                }
                else if (target != -1) // TODO : Fix temporaire, à modifier pour gerer un ciblage plus complet
                {
                    if (target == 0)
                    {
                        player.ApplicationEffet(effet, null, source, Caster);
                    }
                    else
                    {
                        EnemyScripts.FirstOrDefault(c => c.combatID == target)
                            ?.ApplicationEffet(effet, null, source, Caster);
                    }
                }
                else
                {
                    EnemyScripts.FirstOrDefault(c => c.combatID == Caster)
                        ?.ApplicationEffet(effet, null, source, Caster);
                }

                break;
            case Cible.allEnnemi:
                if (isDecoy)
                {
                    decoy.ApplicationEffet(effet, null, source, Caster);
                    break;
                }
                var nbEnemies = EnemyScripts.Count;
                for (int x = EnemyScripts.Count - 1; x >= 0; x--)
                {
                    var ennemie = EnemyScripts[x];
                    if (ennemie != null)
                        ennemie.ApplicationEffet(effet, null, source, Caster, nbEnemies);
                }

                break;
            case Cible.AllExceptSelf:
                for (int x = EnemyScripts.Count - 1; x >= 0; x--)
                {
                    var ennemie = EnemyScripts[x];
                    if (ennemie != null && ennemie.combatID != Caster)
                        ennemie.ApplicationEffet(effet, null, source, Caster);
                }

                if (Caster != idPlayer)
                {
                    player.ApplicationEffet(effet, null, source, Caster);
                }

                break;

            case Cible.All:
                for (int x = EnemyScripts.Count - 1; x >= 0; x--)
                {
                    var ennemie = EnemyScripts[x];
                    if (ennemie != null)
                        ennemie.ApplicationEffet(effet, null, source, Caster);
                }

                if (Caster == idPlayer)
                {
                    player.ApplicationEffet(effet, null, source);
                }
                else
                {
                    if (EnemyScripts.FirstOrDefault(c => c.combatID == Caster) == null)
                    {
                        player.ApplicationEffet(effet, DeadEnemyScripts.First(c => c.combatID == Caster).Stat, source,
                            Caster);
                    }
                    else
                    {
                        player.ApplicationEffet(effet, EnemyScripts.First(c => c.combatID == Caster).Stat, source,
                            Caster);
                    }
                }

                break;
            case Cible.MostDamage:

                if (MostDamageID == idPlayer)
                {
                    if (EnemyScripts.FirstOrDefault(c => c.combatID == Caster) == null)
                    {
                        player.ApplicationEffet(effet, DeadEnemyScripts.First(c => c.combatID == Caster).Stat, source,
                            Caster);
                    }
                    else
                    {
                        player.ApplicationEffet(effet, EnemyScripts.First(c => c.combatID == Caster).Stat, source,
                            Caster);
                    }
                }
                else
                {
                    if (Caster == target)
                    {
                        EnemyScripts.First(c => c.combatID == target).ApplicationEffet(effet, null, source, Caster);
                    }
                    else
                    {
                        EnemyScripts.First(c => c.combatID == target)
                            .ApplicationEffet(effet, null, source, Caster);
                    }
                }

                break;

            case Cible.AllAllyExceptSelf:
                for (int x = EnemyScripts.Count - 1; x >= 0; x--)
                {
                    var ennemie = EnemyScripts[x];
                    if (ennemie != null && ennemie.combatID != Caster)
                        ennemie.ApplicationEffet(effet, null, source, Caster);
                }

                break;
            case Cible.Ally:
                int indexAlly = Caster;
                do
                {
                    indexAlly = UnityEngine.Random.Range(1, EnemyScripts.Count + 1);

                } while (indexAlly == Caster && EnemyScripts.Count > 1);

                var ennemyAlly = EnemyScripts.FirstOrDefault(c => c.combatID == indexAlly);
                if (ennemyAlly != null)
                    ennemyAlly.ApplicationEffet(effet, null, source, Caster);
                else
                    EnemyScripts.First().ApplicationEffet(effet, null, source, Caster);
                break;
            case Cible.Martyr:
                var martyr = EnemyScripts.FirstOrDefault(c => c.Stat.Nom == "Martyr");
                if (martyr != null)
                {
                    martyr.ApplicationEffet(effet, null, source, Caster);
                }

                break;

            case Cible.LastAttacker:
                var LastEnnemy = EnemyScripts.FirstOrDefault(c => c.combatID == currentIdTurn);

                LastEnnemy.ApplicationEffet(effet, null, source, Caster);
                break;
        }
    }

    #endregion Lien Joueur - Ennemi

    #region Essence

    public GameObject GetPrefabEssence(int amount)
    {
        if (amount >= _amountForGreaestEssence)
        {
            return _prefabEssenceList[2];
        }

        if (amount >= _amountForMediumEssence)
        {
            return _prefabEssenceList[1];
        }

        return _prefabEssenceList[0];
    }

    public void Consume(int essence)
    {
        ConsumedEssence = true;
        player.UseEssence(essence, Source.Soin);

    }

    public void ConsumeEndBattle(int essence)
    {
        player.UseEssence(essence, Source.Soin);
        if (endBattle)
            EndBattle();
    }

    private IEnumerator GatherEssence()
    {
        yield return new WaitForEndOfFrame();

        int amount = 0;
        foreach (var item in ListEssence)
        {
            amount += item.GetComponent<CrystalSoul>().Amount;
        }

        OnGatherEssence?.Invoke(spawnPos[3]);
        yield return new WaitForSeconds(0.5f);

        ListEssence.Clear();
       
        var temp = Instantiate(GetPrefabEssence(amount), spawnPos[3]); //we put it in the closest position of the player
        //temp.transform.localScale = new Vector3(1.5f, 1.5f, 0);
        //temp.transform.localScale = new Vector3(1.5f, 1.5f, 0);
        temp.GetComponent<CrystalSoul>().AddAmountOfEssence(amount, true);

        ListEssence.Add(temp);
        foreach (var passif in player.PassiveList)
        {
            if (passif is ILootEssencePassive)
            {
                ILootEssencePassive lootPassif  = passif as ILootEssencePassive;
                lootPassif.Apply(player.Stat);
                if (lootPassif.Value != 0)
                {
                    amount += lootPassif.Value;
                    temp.GetComponent<CrystalSoul>().AddAmountOfEssence(amount, true);
                }
                
            }
        }
        amount = temp.GetComponent<CrystalSoul>().Amount;
        buttonEndCombat.SetActive(true);
        buttonEndCombat.GetComponentInChildren<TMP_Text>().text =
            $"{TradManager.instance.GetTranslation(_idLabelForEssenceButton)}\n({amount})";
        endBattle = true;
    }

    public void KeepEssence()
    {
        int amount = 0;
        foreach (var item in ListEssence)
        {
            amount += item.GetComponent<CrystalSoul>().Amount;
        }

        player.Stat.Essence += amount;
        if (GameManager.Instance.IsTuto)
        {
            buttonEndCombat.SetActive(false);
            TutoManager.Instance.TutoPanel.GetComponent<TutoPanel>().EndCombat();
            for (int i = 0; i < ListEssence.Count; i++)
            {
                Destroy(ListEssence[i]);
            }

            ListEssence.Clear();
        }
        EndBattle();

    }

    private void ClearListEssence()
    {
        for (int i = ListEssence.Count - 1; i >= 0; i--)
        {
            Destroy(ListEssence[i]);
            ListEssence.RemoveAt(i);
        }

        ListEssence.Clear();
    }

    #endregion Essence

    #region Death

    public void DeadEnemy(int id)
    {
        var killed = EnemyScripts.FirstOrDefault(c => c.combatID == id);
        if (killed != null)
        {
            //var i = IdOrder.FindIndex(c => c.id == id);
            //if (i + 1 < IdOrder.Count && IdOrder[i + 1].id == idPlayer)
            //    player.ActivateSpells();

            nbTurn -= IdOrder.Count(c => c.id == id && c.Played == true);
            IdOrder.RemoveAll(c => c.id == id);
            IdSpeedDictionary.Remove(id);
            turnOrderUIManager.RemoveDeadsTurn(id);
            if (killed.isMainEnemy)
            {
                List<EnnemyBehavior> tempList = new List<EnnemyBehavior>(EnemyScripts);
                foreach (var item in tempList)
                {
                    if (item.combatID != killed.combatID)
                        item.Dead();
                }
            }

            var todestroy = killed.gameObject;
            DeadEnemyScripts.Add(killed);
            EnemyScripts.RemoveAll(c => c.combatID == id);
            SpawnedEnemy.Remove(todestroy);
            Destroy(todestroy);

            if (EnemyScripts.Count <= 0)
            {
                if (GameManager.Instance.IsTuto /*TutoManager.Instance != null*/)
                {
                    var child = TutoManager.Instance.TutoPanel.transform.GetChild(0);
                    child.gameObject.SetActive(true);
                    var tutoPanelScript = TutoManager.Instance.TutoPanel.GetComponent<TutoPanel>();
                    tutoPanelScript.ShowExplication();
                    //tutoPanelScript.UIJoueur.SetActive(false);
                    //Ici le TutoPanel
                }
                else
                {
                    StartCoroutine("GatherEssence");
                }

            }

            if (currentIdTurn != idPlayer)
                EndTurn();

            if (currentIdTurn == id && IdOrder.Count > 2)
            {
                var nextPlayer = IdOrder.FirstOrDefault(c => c.id != currentIdTurn && !c.Played);
                if (nextPlayer != null)
                    currentIdTurn = nextPlayer.id;
            }




        }
    }

    public void DeadPlayer()
    {
        GameManager.Instance.DeadPlayer();
    }

    #endregion Death

    #region Targeting

    public void StartTargeting(int IdSpell)
    {
        StopCoroutine(Target);
        StartCoroutine(Target, IdSpell);
    }

    private IEnumerator Targeting(int IdSpell)
    {
        foreach (var item in EnemyScripts)
        {
            item.SetTargetingMode();
        }

        do
        {
            yield return new WaitForSeconds(0.1f);
        } while (idTarget == -1);

        foreach (var item in EnemyScripts)
        {
             item.EndTargetingMode();
        }

        player.SendSpell(true, IdSpell);
    }

    #endregion Targeting

    #region Animation

    public void LaunchAnimAttacked()
    {
        var tempEnemy = EnemyScripts.FirstOrDefault(c => c.combatID == idTarget);
        if (tempEnemy != null)
        {
            tempEnemy.GetAttacked();
        }
    }

    public void EndCurrentAttaque()
    {

        var temp = EnemyScripts.FirstOrDefault(c => c.combatID == currentIdTurn);
        if (temp != null)
            temp.EndTurn();
        else
            foreach (var item in EnemyScripts)
            {
                item.EndAnimBool();
            }
    }

    public void EndHurtAnim()
    {
        foreach (var item in EnemyScripts)
        {
            item.EndAnimHurt();
        }
    }

    public void EndHurtAnimPlayer()
    {

        player.EndHurtAnim();
    }

    #endregion Animation

}