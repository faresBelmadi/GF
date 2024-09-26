using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[System.Serializable]
public class BattleManager : MonoBehaviour
{
    [Header("Prefab CombatNormal")] public JoueurBehavior player;
    public List<GameObject> SpawnedEnemy;
    public List<EnnemyBehavior> EnemyScripts;
    public List<EnnemyBehavior> DeadEnemyScripts;
    public Transform[] spawnPos;
    public Encounter _encounter;
    public GameObject prefabEssence;
    public GameObject buttonEndCombat;
    const string Target = "Targeting";
    public PassifRules passifRules;


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
    [SerializeField] private DialogueManager DialogueManager;
    public PassifManager PassifManager;

    public bool IsLoot;
    public bool ConsumedEssence;
    public int EssenceGained;

    [SerializeField] private Material characterMaterial;
    [SerializeField] private Material ennemiUIMaterial;

    #region Loot

    public void Loot()
    {
        int random = UnityEngine.Random.Range(0, 101);
        Debug.Log("Loot : " + random);
        if (random > _encounter.PourcentageLootSouvenir)
        {
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
                GameManager.instance.CopyAllSouvenir.Any(x => x.Rarete == _encounter.LootRarity[i].rareter))
            {
                var allSouvenirRareter = GameManager.instance.CopyAllSouvenir
                    .Where(x => x.Rarete == _encounter.LootRarity[i].rareter).ToList();
                int randomSouvenir = UnityEngine.Random.Range(0, allSouvenirRareter.Count());

                string NameLoot = allSouvenirRareter[randomSouvenir].Nom;
                var newSouvenir = GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot);
                player.Stat.ListSouvenir.Add(Instantiate(newSouvenir));
                GameManager.instance.CopyAllSouvenir.Remove(newSouvenir);
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
        player.Stat.ValeurPalier = player.Stat.TensionMax / player.Stat.NbPalier;
        if(player.Stat.PalierChangement > 0) { player.Stat.Tension = player.Stat.ValeurPalier * player.Stat.PalierChangement; }
        Debug.Log($"Caculate tension joueur");
        Debug.Log($"TMax = {player.Stat.TensionMax};\nVPal = {player.Stat.ValeurPalier};\nCurrent Tens = {player.Stat.Tension}];\nPalChange = {player.Stat.PalierChangement}]") ;
    }

    private void CalcTensionEnemy()
    {
        foreach (var item in EnemyScripts)
        {
            if (!item.Stat.NoTension)
            {
                item.Stat.TensionMax = (CalmeMoyenJoueur / CalmeMoyen) * item.Stat.Calme;
                item.Stat.ValeurPalier = (item.Stat.TensionMax) / item.Stat.NbPalier;
                if (item.Stat.PalierChangement > 0) { item.Stat.Tension = item.Stat.ValeurPalier * item.Stat.PalierChangement; }
                Debug.Log($"Caculate tension Ennemie");
                Debug.Log($"TMax = {item.Stat.TensionMax};\nVPal = {item.Stat.ValeurPalier};\nCurrent Tens = {item.Stat.Tension}];\nPalChange = {item.Stat.PalierChangement}]");
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
                return true;
            }else
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
        CombatEnableSetup();
    }

    void DialogueEnableSetup()
    {
        player.InitRefBattleMan(this);
        PassifManager = new PassifManager(new List<JoueurBehavior> {player}, EnemyScripts);
        DialogueManager.SetupDialogue(_encounter);
    }

    void CombatEnableSetup()
    {
        idIndexer = 0;
        battleUI = GetComponent<BattleUI>();
        player.Stat = GameManager.instance.playerStat;
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
        _encounter = ToSpawn;
        SpawnEnemy();
        player.UpdateUI();
        player.DesactivateSpells();
        DialogueEnableSetup();


        //StartCombat();
    }

    void SpawnEnemy()
    {
        List<Transform> used = new List<Transform>();
        for (int i = 0; i < _encounter.ToFight.Count; i++)
        {

            var item = _encounter.ToFight[i];

            int index = 0;
            if (_encounter.ToFight.Count == 1)
                index = 2;
            else
                index = UnityEngine.Random.Range(0, spawnPos.Length);

            while (used.Contains(spawnPos[index]))
            {
                index = UnityEngine.Random.Range(0, spawnPos.Length);
            }

            used.Add(spawnPos[index]);

            var temp = Instantiate(item.Spawnable, spawnPos[index].position, Quaternion.identity, spawnPos[index]);
            if (temp != null)
            {
                SpawnedEnemy.Add(temp);
            }

            var tempCombatScript = temp.GetComponent<EnnemyBehavior>();
            //instantiate tout les so modifiable
            if (tempCombatScript != null)
            {
                tempCombatScript.Stat = Instantiate(item);
                tempCombatScript.SetUp();
                tempCombatScript.EndTurnBM = EndTurn;
                tempCombatScript.isMainEnemy = i == _encounter.idMainMob ? true : false;
                EnemyScripts.Add(tempCombatScript);

                IdSpeedDictionary.Add(idIndexer, tempCombatScript.Stat.Vitesse);
                tempCombatScript.combatID = idIndexer;
                tempCombatScript.ChooseNextAction();
                idIndexer++;
            }

            //AddingMaterial
            temp.GetComponent<UIEnnemi>().imageCadreFGs[0].material = new Material(ennemiUIMaterial);
            temp.GetComponent<UIEnnemi>().imageCadreFGs[1].material = new Material(ennemiUIMaterial);

            Material thisCharMaterial = new Material(characterMaterial);
            if (tempCombatScript != null) tempCombatScript.characterMaterial = thisCharMaterial;
            temp.GetComponent<PulseBloom_System>().bloomMaterial = thisCharMaterial;

            
            foreach (SpriteRenderer renderer in temp.GetComponentsInChildren<SpriteRenderer>(true))
            {
                renderer.material = thisCharMaterial;
            }
        }

    }

    public void StartCombat()
    {
        CalcCalmeMoyen();
        CalcTensionEnemy();
        CalcTensionJoueur();
        StartPhase();
    }

    private void EndBattle()
    {
        PassifManager.CurrentEvent = TimerPassif.FinCombat;
        PassifManager.ResolvePassifs();

        Loot();
        player.ResetStat();
        player.Stat.ListBuffDebuff.Clear();
        player.Stat.Volonter = player.Stat.VolonterMax;
        player.Stat.Tension = 0;
        GameManager.instance.playerStat = player.Stat;
        Debug.Log(IsLoot);
        if (TutoManager.Instance != null)
        {
            TutoManager.Instance.Loot();
            var gO = GameObject.Find("TutoPanel");
            var child = gO.transform.GetChild(0);
            child.gameObject.SetActive(true);
            //UIDialogue.SetActive(false);
            var tutoPanelScript = gO.GetComponent<TutoPanel>();
            tutoPanelScript.ShowExplication();
            //HideSoul
            //Hidepos4 child
            GameObject.Find("Soul(Clone)").SetActive(false);
            buttonEndCombat.SetActive(false);
        }
        else
            StartCoroutine(GameManager.instance.pmm.EndBattle(IsLoot));
    }

    #endregion Mise en place combat & fin

    #region Phase

    private void StartPhase()
    {
        PassifManager.CurrentEvent = TimerPassif.DebutPhase;
        PassifManager.ResolvePassifs();

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
            IdOrder.Add(new CombatOrder() {id = item.Key, Played = false});
            if (CheckTension(item.Key))
                IdOrder.Add(new CombatOrder() {id = item.Key, Played = false});
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
            player.StartTurn(nbPhase<2);
            battleUI.textPLayingTurn.text = "Guerrier";
        }
        else
        {
            var playing = EnemyScripts.First(c => c.combatID == key);
            playing.StartTurn(nbPhase<2);
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

            PassifManager.CurrentEvent = TimerPassif.FinPhase;
            PassifManager.ResolvePassifs();

            StartPhase();
        }
        else
            turnOrderUIManager.EvovlveTurnOrder();//StartNextTurn();

    }

    #endregion Turn

    #region Lien Joueur - Ennemi

    public void LaunchSpellJoueur(Spell Spell)
    {
        foreach (var effet in Spell.ActionEffet)
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
        GiveBuffDebuff(Spell.ActionBuffDebuff, idTarget);
        idTarget = -1;

    }

    public void LaunchSpellEnnemi(EnnemiSpell Spell)
    {
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
        int origine = currentIdTurn;
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
        switch (effet.Cible)
        {
            case Cible.joueur:
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
            case Cible.ennemi:

                if (Caster == target)
                {
                    EnemyScripts.First(c => c.combatID == target).ApplicationEffet(effet, null, source, Caster);
                }
                else
                {
                    if (target != -1)
                    {
                        if(EnemyScripts.FirstOrDefault(c => c.combatID == target) != null)
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
                else
                {
                    EnemyScripts.FirstOrDefault(c => c.combatID == Caster)
                        ?.ApplicationEffet(effet, null, source, Caster);
                }

                break;
            case Cible.allEnnemi:
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
        yield return new WaitForSeconds(1f);

        int amount = 0;
        foreach (var item in ListEssence)
        {
            amount += item.GetComponent<Essence>().getEssence();
        }

        for (int i = 0; i < ListEssence.Count; i++)
        {
            Destroy(ListEssence[i]);
        }

        ListEssence.Clear();
        var temp = Instantiate(prefabEssence, spawnPos[0]);
        temp.transform.localScale = new Vector3(1.5f, 1.5f, 0);
        temp.GetComponent<Essence>().AddEssence(amount);
        temp.transform.localScale = new Vector3(1.5f, 1.5f, 0);
        temp.GetComponent<Essence>().isEnd = true;
        ListEssence.Add(temp);
        buttonEndCombat.SetActive(true);
        buttonEndCombat.GetComponentInChildren<TMP_Text>().text += $" ({amount})";
        endBattle = true;
    }

    public void KeepEssence()
    {

        int amount = 0;
        foreach (var item in ListEssence)
        {
            amount += item.GetComponent<Essence>().getEssence();
        }

        player.Stat.Essence += amount;
        EndBattle();
    }

    #endregion Essence

    #region Death

    public void DeadEnemy(int id)
    {
        var killed = EnemyScripts.FirstOrDefault(c => c.combatID == id);
        if (killed != null)
        {
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
                if (TutoManager.Instance != null)
                {
                    //TutoManager.Instance.NextStep();
                    var gO = GameObject.Find("TutoPanel");
                    var child = gO.transform.GetChild(0);
                    child.gameObject.SetActive(true);
                    //UIDialogue.SetActive(false);
                    var tutoPanelScript = gO.GetComponent<TutoPanel>();
                    tutoPanelScript.ShowExplication();
                    tutoPanelScript.UIJoueur.SetActive(false);
                    //Ici le TutoPanel
                }
                else
                {
                    StartCoroutine("GatherEssence");
                }

            }

            if (currentIdTurn == id && IdOrder.Count > 2)
            {
                var enemi = IdOrder.FirstOrDefault(c => c.id != currentIdTurn && !c.Played);
                if (enemi != null)
                    currentIdTurn = enemi.id;
            }

            if (currentIdTurn != idPlayer)
                EndTurn();
        }
    }

    public void DeadPlayer()
    {
        GameManager.instance.DeadPlayer();
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

        player.endHurtAnim();
    }

    #endregion Animation

}