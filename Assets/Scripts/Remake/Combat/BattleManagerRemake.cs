using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BattleManagerRemake : MonoBehaviour
{
    [Header("Prefab Combat")]
    public JoueurBehavior player;
    public List<GameObject> SpawnedEnemy;
    public List<EnnemyBehavior> EnemyScripts;
    public Transform[] spawnPos;
    public EncounterRemake _encounter;
    public GameObject prefabEssence;
    public GameObject buttonEndCombat;

    [Header("Round/Turn variables")]
    public List<CombatOrderRemake> IdOrder;
    public int nbPhase = 0;
    public Dictionary<int, int> IdSpeedDictionary;
    public List<GameObject> ListEssence = new List<GameObject>();

    [Header("Stats combats")]
    public float CalmeMoyen;
    public float CalmeMoyenAdversaire;
    public float CalmeMoyenJoueur;

    [SerializeField]
    int idIndexer = 0;
    int idPlayer;
    public int currentIdTurn;
    public int nbTurn;
    public int idTarget = -1;
    bool endBattle;
    BattleUI battleUI;

    [SerializeField]
    private DialogueManager DialogueManager;

    #region calcul tension & calme

    private void CalcTensionJoueur()
    {
        player.TensionMax = (CalmeMoyenAdversaire / CalmeMoyen) * player.Stat.Calme;
        player.ValeurPalier = player.TensionMax / player.NbPalier;
    }

    private void CalcTensionEnemy()
    {
        foreach (var item in EnemyScripts)
        {
            item.TensionMax = (CalmeMoyenJoueur / CalmeMoyen) * item.Stat.Calme;
            item.ValeurPalier = (item.TensionMax) / item.NbPalier;
        }
    }

    void CalcCalmeMoyen()
    {
        float tempCalmeEnemy = 0;
        for (int i = 0; i < EnemyScripts.Count; i++)
        {
            tempCalmeEnemy += EnemyScripts[i].Stat.Calme;
        }
        CalmeMoyenJoueur = player.Stat.Calme;
        CalmeMoyenAdversaire = tempCalmeEnemy / EnemyScripts.Count;
        //remplacer 1 par une variable si le cas de plusieurs personnage jouable arrive
        CalmeMoyen = (tempCalmeEnemy + player.Stat.Calme) / (EnemyScripts.Count + 1);
    }

    private bool CheckTension(int key)
    {

        if (key == idPlayer)
        {
            if (player.CanHaveAnotherTurn())
            {
                return true;
            }
        }
        else
        {
            var t = EnemyScripts.First(c => c.combatID == key);
            if (t.CanHaveAnotherTurn())
                return true;
        }
        return false;
    }

    #endregion calcul tension & calme

    #region Mise en place combat & fin

    private void OnEnable()
    {
        CombatEnableSetup();
    }

    void DialogueEnableSetup()
    {
        //DialogueManager.SetupDialogue(_encounter);
    }

    void CombatEnableSetup()
    {
        idIndexer = 0;
        battleUI = GetComponent<BattleUI>();
        //player.Stat = GameManager.instance.playerStat;
        player.EndTurnBM = EndTurn;
        player.StartUp();
        SpawnedEnemy = new List<GameObject>();
        EnemyScripts = new List<EnnemyBehavior>();
        IdOrder = new List<CombatOrderRemake>();
        IdSpeedDictionary = new Dictionary<int, int>();
        IdSpeedDictionary.Add(idIndexer, player.Stat.Vitesse);
        idPlayer = idIndexer;
        idIndexer++;
    }

    public void LoadEnemy(EncounterRemake ToSpawn)
    {
        _encounter = ToSpawn;
        SpawnEnemy();
        player.UpdateUI();
        player.DesactivateSpells();
        DialogueEnableSetup();
    }

    void SpawnEnemy()
    {
        List<Transform> used = new List<Transform>();
        foreach (var item in _encounter.ToFight)
        {
            var index = UnityEngine.Random.Range(0, spawnPos.Length);
            while (used.Contains(spawnPos[index]))
            {
                index = UnityEngine.Random.Range(0, spawnPos.Length);
            }
            used.Add(spawnPos[index]);

            var temp = Instantiate(item.Spawnable, spawnPos[index].position, Quaternion.identity, spawnPos[index]);
            var tempCombatScript = temp.GetComponent<EnnemyBehavior>();
            //instantiate tout les so modifiable
            tempCombatScript.Stat = Instantiate(item);
            tempCombatScript.SetUp();
            tempCombatScript.EndTurnBM = EndTurn;
            //tempCombatScript.actResult = ActResult;
            //tempCombatScript.actDebuff = ActDebuff;
            SpawnedEnemy.Add(temp);
            EnemyScripts.Add(tempCombatScript);

            IdSpeedDictionary.Add(idIndexer, tempCombatScript.Stat.Vitesse);
            tempCombatScript.combatID = idIndexer;
            tempCombatScript.ChooseNextAction();
            idIndexer++;
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
        player.ResetStat();
        player.Stat.Volonter = player.Stat.VolonterMax;
        player.Tension = 0;
        //GameManager.instance.playerStat = player.Stat;

        StartCoroutine(GameManager.instance.pmm.EndBattle());
    }

    #endregion Mise en place combat & fin

    #region Phase

    private void StartPhase()
    {
        DetermTour();
        player.StartPhase();
        foreach (var item in EnemyScripts)
        {
            item.StartPhase();
        }
        currentIdTurn = 0;
        nbPhase++;
        nbTurn = 0;
        StartNextTurn();
    }

    private void DetermTour()
    {
        var test = IdSpeedDictionary.OrderByDescending(c => c.Value);
        IdOrder = new List<CombatOrderRemake>();
        foreach (var item in test)
        {
            IdOrder.Add(new CombatOrderRemake() { id = item.Key, Played = false });
            if (CheckTension(item.Key))
                IdOrder.Add(new CombatOrderRemake() { id = item.Key, Played = false });
        }
    }

    #endregion Phase

    #region Turn

    private void StartNextTurn()
    {
        int key = IdOrder.First(c => c.Played == false).id;
        currentIdTurn = key;
        if (key == idPlayer)
        {
            player.StartTurn();
            battleUI.textPLayingTurn.text = "Guerrier";
        }
        else
        {
            var playing = EnemyScripts.First(c => c.combatID == key);
            playing.StartTurn();
            battleUI.textPLayingTurn.text = playing.UICombat.NameText.text;
        }
    }

    void EndTurn()
    {
        var turnPlayed = IdOrder.FirstOrDefault(c => c.id == currentIdTurn && !c.Played);
        if (turnPlayed != null)
            turnPlayed.Played = true;
        nbTurn++;
        if (nbTurn >= IdOrder.Count)
            StartPhase();
        else
            StartNextTurn();
    }

    #endregion Turn

    #region Targeting

    public void StartTargeting()
    {
        StopCoroutine("Targeting");
        StartCoroutine("Targeting");
    }

    private IEnumerator Targeting()
    {
        foreach (var item in EnemyScripts)
        {
            item.SetTargetingMode();
        }

        do
        {
            yield return new WaitForSeconds(0.1f);
        }
        while (idTarget == -1);

        foreach (var item in EnemyScripts)
        {
            item.EndTargetingMode();
        }

        player.SendSpell();
    }

    #endregion Targeting

    #region Essence

    public void Consume(int essence)
    {
        //player.TakeDamage(-essence, Source.Soin);
    }
    public void ConsumeEndBattle(int essence)
    {
        //player.TakeDamage(-essence, Source.Soin);
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
        nbTurn -= IdOrder.Count(c => c.id == id && c.Played == true);
        IdOrder.RemoveAll(c => c.id == id);
        IdSpeedDictionary.Remove(id);
        var todestroy = EnemyScripts.First(c => c.combatID == id).gameObject;
        EnemyScripts.RemoveAll(c => c.combatID == id);
        Destroy(todestroy);
        if (EnemyScripts.Count <= 0)
            StartCoroutine("GatherEssence");
    }

    public void DeadPlayer()
    {
        GameManager.instance.DeadPlayer();
    }

    #endregion Death

    #region Animation

    public void LaunchAnimAttacked()
    {
        EnemyScripts.First(c => c.combatID == idTarget).getAttacked();
    }

    public void EndCurrentAttaque()
    {
        EnemyScripts.FirstOrDefault(c => c.combatID == currentIdTurn).EndAnim();
    }

    public void EndHurtAnim()
    {
        foreach (var item in EnemyScripts)
        {
            item.EndAnimHurt();
        }
    }

    #endregion Animation



}
