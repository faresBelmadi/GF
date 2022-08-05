using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BattleManager : MonoBehaviour
{
    [Header("Prefab Combat")]
    public JoueurBehavior player;
    public List<GameObject> SpawnedEnemy;
    public List<EnnemyBehavior> EnemyScripts;
    public List<EnnemyBehavior> DeadEnemyScripts;
    public Transform[] spawnPos;
    public Encounter _encounter;
    public GameObject prefabEssence;
    public GameObject buttonEndCombat;
    const string Target = "Targeting";

    [Header("Round/Turn variables")]
    public List<CombatOrder> IdOrder;
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
    public int MostDamage, MostDamageID;

    [SerializeField]
    private DialogueManager DialogueManager;

    #region calcul tension & calme

    private void CalcTensionJoueur()
    {
        player.Stat.TensionMax = (CalmeMoyenAdversaire / CalmeMoyen) * player.Stat.Calme;
        player.Stat.ValeurPalier = player.Stat.TensionMax / player.Stat.NbPalier;
    }

    private void CalcTensionEnemy()
    {
        foreach (var item in EnemyScripts)
        {
            item.Stat.TensionMax = (CalmeMoyenJoueur / CalmeMoyen) * item.Stat.Calme;
            item.Stat.ValeurPalier = (item.Stat.TensionMax) / item.Stat.NbPalier;
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
            { idIndexer, player.Stat.Vitesse }
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
        player.Stat.Tension = 0;
        GameManager.instance.playerStat = player.Stat;

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
        IdOrder = new List<CombatOrder>();
        foreach (var item in test)
        {
            IdOrder.Add(new CombatOrder() { id = item.Key, Played = false });
            if (CheckTension(item.Key))
                IdOrder.Add(new CombatOrder() { id = item.Key, Played = false });
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

    #region Lien Joueur - Ennemi

    public void LaunchSpellJoueur(Spell Spell)
    {
        GiveBuffDebuff(Spell.ActionBuffDebuff, idTarget);
        foreach(var effet in Spell.ActionEffet)
        {
            PassageEffet(effet, idPlayer, idTarget, SourceEffet.Spell);
        }
        //EnemyScripts.First(c => c.combatID == idTarget).ApplicationBuffDebuff(TimerApplication.Attaque);
        idTarget = -1;
    }

    public void LaunchSpellEnnemi(EnnemiSpell Spell)
    {
        GiveBuffDebuff(Spell.debuffsBuffs);
        foreach(var effet in Spell.Effet)
        {
            PassageEffet(effet, currentIdTurn, -1,SourceEffet.Spell);
        }
        //player.ApplicationBuffDebuff(TimerApplication.Attaque);
    }

    public void GiveBuffDebuff(List<BuffDebuff> BuffDebuff, int target = -1)
    {
        int origine = currentIdTurn;
        Decompte Decompte = Decompte.none;
        TimerApplication Timer = TimerApplication.Persistant;
        foreach(var item in BuffDebuff)
        {
            item.IDCombatOrigine = origine;
            switch (item.CibleApplication)
            {
                case Cible.joueur:
                    player.AddDebuff(item, Decompte, Timer);
                    break;
                case Cible.ennemi:
                    EnemyScripts.First(c => c.combatID == target).AddDebuff(item, Decompte, Timer);
                    break;
                case Cible.Ally:

                    break;
                case Cible.allEnnemi:
                    foreach(var ennemie in EnemyScripts)
                    {
                        ennemie.AddDebuff(item, Decompte, Timer);
                    }
                    break;
                case Cible.allAllies:

                    break;
                case Cible.All:
                    player.AddDebuff(item, Decompte, Timer);
                    foreach (var ennemie in EnemyScripts)
                    {
                        ennemie.AddDebuff(item, Decompte, Timer);
                    }
                    break;
            }
        }
    }

    public void PassageEffet(Effet effet, int Caster, int target = -1, SourceEffet source = SourceEffet.Spell)
    {
        switch (effet.Cible)
        {
            case Cible.joueur:
                if(Caster == idPlayer)
                {
                    player.ApplicationEffet(effet, null, source);
                }
                else
                {
                    if (EnemyScripts.FirstOrDefault(c => c.combatID == Caster) == null)
                    {
                        player.ApplicationEffet(effet, DeadEnemyScripts.First(c => c.combatID == Caster).Stat, source, Caster);
                    }
                    else
                    {
                        player.ApplicationEffet(effet, EnemyScripts.First(c => c.combatID == Caster).Stat, source, Caster);
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
                    EnemyScripts.First(c => c.combatID == target).ApplicationEffet(effet, player.Stat, source, Caster);
                }
                break;
            case Cible.Ally:

                break;
            case Cible.allEnnemi:
                foreach (var ennemie in EnemyScripts)
                {
                    ennemie.ApplicationEffet(effet, null, source, Caster);
                }
                break;
            case Cible.allAllies:

                break;
            case Cible.All:
                foreach (var ennemie in EnemyScripts)
                {
                    ennemie.ApplicationEffet(effet, null, source, Caster);
                }
                break;
            case Cible.MostDamage:

                if (MostDamageID == idPlayer)
                {
                    if (EnemyScripts.FirstOrDefault(c => c.combatID == Caster) == null)
                    {
                        player.ApplicationEffet(effet, DeadEnemyScripts.First(c => c.combatID == Caster).Stat, source, Caster);
                    }
                    else
                    {
                        player.ApplicationEffet(effet, EnemyScripts.First(c => c.combatID == Caster).Stat, source, Caster);
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
                        EnemyScripts.First(c => c.combatID == target).ApplicationEffet(effet, player.Stat, source, Caster);
                    }
                }

                break;
        }
    }

    #endregion Lien Joueur - Ennemi

    #region Essence

    public void Consume(int essence)
    {
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
        DeadEnemyScripts.Add(EnemyScripts.FirstOrDefault(c => c.combatID == id));
        EnemyScripts.RemoveAll(c => c.combatID == id);
        SpawnedEnemy.Remove(todestroy);
        Destroy(todestroy);
        if (EnemyScripts.Count <= 0)
            StartCoroutine("GatherEssence");
    }

    public void DeadPlayer()
    {
        GameManager.instance.DeadPlayer();
    }

    #endregion Death

    #region Targeting

    public void StartTargeting()
    {
        StopCoroutine(Target);
        StartCoroutine(Target);
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

    #region Animation

    public void LaunchAnimAttacked()
    {
        EnemyScripts.First(c => c.combatID == idTarget).GetAttacked();
    }

    public void EndCurrentAttaque()
    {
        EnemyScripts.FirstOrDefault(c => c.combatID == currentIdTurn).EndTurn();
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
