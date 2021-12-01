using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Source
{
    Attaque,
    Dot,
    Buff,
    Soin
}
[System.Serializable]
public class CombatOrder
{
    public int id;
    public bool Played;
}

public class BattleManager : MonoBehaviour
{
    [Header("Prefab Combat")]
    public PlayerCombat player;
    public List<GameObject> SpawnedEnemy;
    public List<EnemyCombatBehaviour> EnemyScripts;
    public Transform[] spawnPos;
    public Encounter _encounter;
    public GameObject prefabEssence;
    public GameObject buttonEndCombat;

    [Header("Round/Turn variables")]
    public List<CombatOrder> IdOrder;
    public int nbPhase = 0;
    public List<int> AlreadyPlayed;
    public Dictionary<int,int> IdSpeedDictionary;
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


    // Start is called before the first frame update
    private void OnEnable() {
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
        player.stat = GameManager.instance.playerStat;
        player.EndTurnBM = EndTurn;
        player.StartUp();
        SpawnedEnemy = new List<GameObject>();
        EnemyScripts = new List<EnemyCombatBehaviour>();
        IdOrder = new List<CombatOrder>();
        IdSpeedDictionary = new Dictionary<int, int>();
        IdSpeedDictionary.Add(idIndexer,player.stat.Speed);
        idPlayer = idIndexer;
        idIndexer++;
    }
    public void LoadEnemy(Encounter ToSpawn)
    {
        _encounter = ToSpawn;
        SpawnEnemy();
        player.updateUI();
        player.DesactivateSpells();
        DialogueEnableSetup();
    }

    void SpawnEnemy()
    {
        List<Transform> used = new List<Transform>();
        foreach (var item in _encounter.ToFight)
        {
            var index = UnityEngine.Random.Range(0,spawnPos.Length);
            while (used.Contains(spawnPos[index]))
            {
                index = UnityEngine.Random.Range(0,spawnPos.Length);
            }
            used.Add(spawnPos[index]);

            var temp = Instantiate(item.Spawnable,spawnPos[index].position,Quaternion.identity,spawnPos[index]);
            var tempCombatScript = temp.GetComponent<EnemyCombatBehaviour>();
            //instantiate tout les so modifiable
            tempCombatScript.current = Instantiate(item);
            tempCombatScript.SetUp();
            tempCombatScript.EndTurnBM = EndTurn;
            tempCombatScript.actResult = ActResult;
            tempCombatScript.actDebuff = ActDebuff;
            SpawnedEnemy.Add(temp);
            EnemyScripts.Add(tempCombatScript);

            IdSpeedDictionary.Add(idIndexer,tempCombatScript.current.Speed);
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
        StartFirstPhase();
    }

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
        private void StartFirstPhase()
    {
        DetermFirstTurn();
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

#region Turn
    void EndTurn()
    {
        IdOrder.FirstOrDefault(c => c.id == currentIdTurn && !c.Played).Played = true;
        nbTurn++;
        if(nbTurn >= IdOrder.Count)
            StartPhase();
        else
            StartNextTurn();
    }

    private void StartNextTurn()
    {
        int key = IdOrder.First(c => c.Played == false).id;
        currentIdTurn = key;
        if(key == idPlayer)
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

    private void DetermTour()
    {
        var test = IdSpeedDictionary.OrderByDescending(c => c.Value);
        IdOrder = new List<CombatOrder>();
        foreach (var item in test)
        {
            IdOrder.Add(new CombatOrder(){id = item.Key,Played = false});
            if(CheckTension(item.Key))
                IdOrder.Add(new CombatOrder(){id = item.Key,Played = false});
        }
    }    
    private void DetermFirstTurn()
    {
        var test = IdSpeedDictionary.OrderByDescending(c => c.Value);
        IdOrder = new List<CombatOrder>();
        IdOrder.Add(new CombatOrder(){id = idPlayer, Played = false});
        foreach (var item in test)
        {
            if(item.Key != idPlayer)
            {
                IdOrder.Add(new CombatOrder(){id = item.Key,Played = false});
                if(CheckTension(item.Key))
                    IdOrder.Add(new CombatOrder(){id = item.Key,Played = false});

            }
        }


    }
#endregion Turn
#region calcul tension & calme
    private void CalcTensionJoueur()
    {
        player.stat.TensionMax = (CalmeMoyenAdversaire/CalmeMoyen)*player.stat.Calme;
        player.stat.ValeurPalier = player.stat.TensionMax / player.stat.NbPalier;
    }

    private void CalcTensionEnemy()
    {
        foreach (var item in EnemyScripts)
        {
            item.TensionMax = (CalmeMoyenJoueur/CalmeMoyen) * item.current.Calme;
            item.TensionPalier = (item.TensionMax) / item.current.NbPalier;
        }
    }

    void CalcCalmeMoyen()
    {
        float tempCalmeEnemy = 0;
        for (int i = 0; i < EnemyScripts.Count; i++)
        {
            tempCalmeEnemy += EnemyScripts[i].current.Calme;
        }
        CalmeMoyenJoueur = player.stat.Calme;
        CalmeMoyenAdversaire = tempCalmeEnemy/EnemyScripts.Count;
        //remplacer 1 par une variable si le cas de plusieurs personnage jouable arrive
        CalmeMoyen = (tempCalmeEnemy + player.stat.Calme)/(EnemyScripts.Count + 1);
    }

    private bool CheckTension(int key)
    {
        
        if(key == idPlayer)
        {
            if(player.CanHaveAnotherTurn())
            {
                return true;
            }
        }
        else
        {
            var t = EnemyScripts.First(c => c.combatID == key);
            if(t.CanHaveAnotherTurn())
                return true;
        }
        return false;
    }

#endregion calcul tension & calme
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
        while(idTarget == -1);
        
        foreach (var item in EnemyScripts)
        {
            item.EndTargetingMode();    
        }

        player.SendSpell();
    }


#endregion Targeting
#region Result
    public void GetListEffectPlayer(Spell ToGet)
    {
        ActResult(GetResult(ToGet),0,idTarget);
        ActDebuff(ToGet.debuffsBuffs,0,idTarget);
        idTarget = -1;
    }

    public void LaunchAnimAttacked()
    {
        EnemyScripts.First(c => c.combatID == idTarget).getAttacked();
    }
    public List<ActionResult> GetResult(Spell ToGet)
    {
        List<ActionResult> temp = new List<ActionResult>();
        var Fa = player.stat.Dmg;

        foreach (var item in player.debuffs)
        {
            foreach (var effect in item.effects)
            {
            
                if(effect.type == BuffType.AttBrut)
                {
                    Fa += effect.pourcentageEffet;
                }
                if(effect.type == BuffType.Att)
                {
                    var tempResi = 1 - (effect.pourcentageEffet/100f);

                    Fa += Fa - Mathf.RoundToInt(tempResi * Fa); 
                }
                if(effect.type == BuffType.AttUpPVMiss)
                {
                    var pvMiss = (100 - ((EnemyScripts.First(c => c.combatID == idTarget).current.HP * 100) / EnemyScripts.First(c => c.combatID == idTarget).current.MaxHP));
                    Fa += Fa - Mathf.RoundToInt((pvMiss * ((2*Fa)/100) + Fa));
                }
                if(effect.type == BuffType.AttUpLastDmgTaken)
                {
                    Fa += player.LastDamageTaken;
                }
                if(effect.type == BuffType.AttUpPVMissSelf)
                {
                    var pvMiss = (100 - ((player.stat.HP * 100) / player.stat.MaxHP));
                    Fa += Fa - Mathf.RoundToInt((pvMiss * ((2*Fa)/100) + Fa));
                }
            }
        }
        foreach (var item in ToGet.Effet)
        {
            int tempHp = 0;
            int nbAttaque = 0;
            item.DoAction(Fa,out tempHp, out nbAttaque);
            temp.Add(new ActionResult(){HpModif = tempHp,target=item.target,nbAttaque = nbAttaque});
        }

        return temp;

    } 


    public void ActDebuff(List<BuffDebuff> ToApply,int idOrigin, int Target = -1)
    {
        if(idOrigin != idPlayer)
        {
            foreach (var item in ToApply)
            {
                switch(item.target)
                {
                    case Cible.All:
                    foreach (var enemy in EnemyScripts)
                    {

                        if(item != null) enemy.AddDebuff(item);
                    }
                    break;

                    case Cible.allAllies:

                    foreach (var enemy in EnemyScripts)
                    {

                        if(item != null) enemy.AddDebuff(item);
                    }
                    break;

                    case Cible.allEnnemi :

                    if(item != null) player.AddDebuff(item);
                    break;

                    case Cible.Ally:

                    var Enemy = EnemyScripts.First(c => c.combatID == UnityEngine.Random.Range(1,idIndexer));
                    
                    if(item != null) Enemy.AddDebuff(item);
                    break;

                    case Cible.ennemi:

                    
                    if(item != null) player.AddDebuff(item);
                    break;

                    case Cible.self:
                    
                    var self = EnemyScripts.First(c => c.combatID == idOrigin);
                   
                    if(item != null) self.AddDebuff(item);
                    break;
                }
            }
        }
        else
        {
             foreach (var item in ToApply)
            {
                switch(item.target)
                {
                    case Cible.All:

                    if(item != null) player.AddDebuff(item);
                    foreach (var enemy in EnemyScripts)
                    {
                        
                        if(item != null) enemy.AddDebuff(item);
                    }
                    break;

                    case Cible.allEnnemi:

                    foreach (var enemy in EnemyScripts)
                    {

                        if(item != null) enemy.AddDebuff(item);
                    }
                    break;

                    case Cible.ennemi:
                    
                    var Enemy = EnemyScripts.First(c => c.combatID == Target);
                    if(Enemy == null)
                        Enemy = EnemyScripts.First(c => c.combatID == UnityEngine.Random.Range(1,idIndexer));
                    if(item != null) Enemy.AddDebuff(item);
                    break;

                    case Cible.allAllies :
                    case Cible.Ally:
                    case Cible.self:
                    if(item != null) player.AddDebuff(item);
                    break;
                }
            }
        }
    }
    public void ActResult(List<ActionResult> actions,int idOrigin,int target = -1,bool Enervement = false, bool Apaisement = false)
    {
        if(idOrigin != idPlayer)
        {
            foreach (var item in actions)
            {
                switch(item.target)
                {
                    case Cible.All:
                    if(item.HpModif != 0)
                    {
                        for(int i = 0; i < item.nbAttaque; i++)
                        {
                            player.TakeDamage(item.HpModif,Source.Attaque);
                        }
                    }
                    if(Enervement)
                        player.EnervementTension();
                    if(Apaisement)
                        player.ApaisementTension();
                    foreach (var enemy in EnemyScripts)
                    {
                        if(item.HpModif != 0)
                        {
                            for(int i = 0; i < item.nbAttaque; i++)
                            {
                                enemy.TakeDamage(item.HpModif,Source.Attaque);
                            }
                        }
                    }
                    break;

                    case Cible.allAllies:

                    foreach (var enemy in EnemyScripts)
                    {
                        if(item.HpModif != 0)
                        {
                            for(int i = 0; i < item.nbAttaque; i++)
                            {
                                enemy.TakeDamage(item.HpModif,Source.Attaque);
                            }
                        }
                    }
                    break;

                    case Cible.allEnnemi :

                    if(item.HpModif != 0)
                    {
                        for(int i = 0; i < item.nbAttaque; i++)
                        {
                            player.TakeDamage(item.HpModif,Source.Attaque);
                        }
                    }
                        
                        if(Enervement)
                            player.EnervementTension();
                        if(Apaisement)
                            player.ApaisementTension();
                    break;

                    case Cible.Ally:

                    var Enemy = EnemyScripts.First(c => c.combatID == UnityEngine.Random.Range(1,idIndexer));
                    if(item.HpModif != 0)
                    {
                        for(int i = 0; i < item.nbAttaque; i++)
                        {
                            Enemy.TakeDamage(item.HpModif,Source.Attaque);
                        }
                    }
                    break;

                    case Cible.ennemi:

                    if(item.HpModif != 0)
                    {
                        for(int i = 0; i < item.nbAttaque; i++)
                        {
                            player.TakeDamage(item.HpModif,Source.Attaque);
                        }
                    }
                        
                        if(Enervement)
                            player.EnervementTension();
                        if(Apaisement)
                            player.ApaisementTension();
                    break;

                    case Cible.self:
                    
                    var self = EnemyScripts.First(c => c.combatID == idOrigin);
                    if(item.HpModif != 0)
                    {
                        for(int i = 0; i < item.nbAttaque; i++)
                        {
                            self.TakeDamage(item.HpModif,Source.Attaque);
                        }
                    }
                    break;
                }
            }
        }
        else
        {
             foreach (var item in actions)
            {
                switch(item.target)
                {
                    case Cible.All:

                    if(item.HpModif != 0)
                    {
                        for(int i = 0; i < item.nbAttaque; i++)
                        {
                            player.TakeDamage(item.HpModif,Source.Attaque);
                        }
                    }
                    foreach (var enemy in EnemyScripts)
                    {
                        if(item.HpModif != 0)
                        {
                            for(int i = 0; i < item.nbAttaque; i++)
                            {
                                enemy.TakeDamage(item.HpModif,Source.Attaque);
                            }
                        }
                    }
                    break;

                    case Cible.allEnnemi:

                    foreach (var enemy in EnemyScripts)
                    {
                        if(item.HpModif != 0)
                        {
                            for(int i = 0; i < item.nbAttaque; i++)
                            {
                                enemy.TakeDamage(item.HpModif,Source.Attaque);
                            }
                        }
                    }
                    break;

                    case Cible.ennemi:
                    
                    var Enemy = EnemyScripts.First(c => c.combatID == target);
                    if(Enemy == null)
                        Enemy = EnemyScripts.First(c => c.combatID == UnityEngine.Random.Range(1,idIndexer));
                    if(item.HpModif != 0)
                    {
                        for(int i = 0; i < item.nbAttaque; i++)
                        { 
                            Enemy.TakeDamage(item.HpModif,Source.Attaque);
                        }
                    }
                    break;

                    case Cible.allAllies :
                    case Cible.Ally:
                    case Cible.self:

                    if(item.HpModif != 0)
                    {
                        for(int i = 0; i < item.nbAttaque; i++)
                        {
                            player.TakeDamage(item.HpModif,Source.Attaque);
                        }

                    }
                    break;
                }
            }
        }
        
    }
    #endregion Result
#region Death
    public void DeadEnemy(int id)
    {
        nbTurn -= IdOrder.Count(c => c.id == id && c.Played == true); 
        IdOrder.RemoveAll(c => c.id == id);
        IdSpeedDictionary.Remove(id);
        var todestroy = EnemyScripts.First(c => c.combatID == id).gameObject;
        EnemyScripts.RemoveAll(c => c.combatID == id);
        Destroy(todestroy);
        if(EnemyScripts.Count <= 0)
            StartCoroutine("GatherEssence");
    }    
    
    public void DeadPlayer()
    {
        GameManager.instance.DeadPlayer();
    }
#endregion Death
#region Essence
    public void Consume(int essence)
    {
        player.TakeDamage(-essence,Source.Soin);
        if(endBattle)
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
        var temp = Instantiate(prefabEssence,spawnPos[0]);
        temp.transform.localScale = new Vector3(1.5f,1.5f,0);
        temp.GetComponent<Essence>().AddEssence(amount);
        temp.transform.localScale = new Vector3(1.5f,1.5f,0);
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

        player.stat.Essence += amount;
        EndBattle();
    }
#endregion Essence
    
    public void EndCurrentAttaque()
    {
        foreach (var item in EnemyScripts)
        {
            if(item.combatID == currentIdTurn)
                item.EndAnim();
        }
        //EnemyScripts.FirstOrDefault(c => c.combatID == currentIdTurn).EndAnim();
    }

    public void EndHurtAnim()
    {
        foreach (var item in EnemyScripts)
        {
            item.EndAnimHurt();
        }
    }

    private void EndBattle()
    {
        player.stat.Volonté = player.stat.MaxVolonté;
        player.stat.Tension = 0;
        GameManager.instance.playerStat = player.stat;

        StartCoroutine(GameManager.instance.pmm.EndBattle());
    }


}

