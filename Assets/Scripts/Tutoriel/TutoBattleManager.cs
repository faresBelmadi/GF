using System.Collections.Generic;
using UnityEngine;

public class TutoBattleManager : MonoBehaviour
{
    //public TutoManager Instance;
    public Transform[] spawnPos;
    
    [SerializeField]
    private DialogueManager DialogueManager;

    public List<Transform> used = new List<Transform>();

    public void Start()
    {
        SpawnEnemy();
        DialogueEnableSetup();
    }


    void DialogueEnableSetup()
    {
        DialogueManager.SetupDialogue(TutoManager.Instance._encounter[TutoManager.Instance.StepBatlleTuto]);
    }

    void SpawnEnemy()
    {
        int posIndex = 0;
        for (int i = 0; i < TutoManager.Instance._encounter[TutoManager.Instance.StepBatlleTuto].ToFight.Count; i++)
        {
            var ennemi = TutoManager.Instance._encounter[TutoManager.Instance.StepBatlleTuto].ToFight[i];
            var temp = Instantiate(ennemi.Spawnable, spawnPos[posIndex].position, Quaternion.identity, spawnPos[posIndex]);
            used.Add(spawnPos[posIndex]);
            var tempCombatScript = temp.GetComponent<EnnemyBehavior>();
            Destroy(tempCombatScript);
            posIndex += 2;
        }

        if (TutoManager.Instance.StepBatlleTuto == 3)
        {
            used[0].gameObject.SetActive(false);
        }
        //foreach (var item in TutoManager.Instance._encounter[TutoManager.Instance.StepBatlleTuto].ToFight)
        //{
        //    //changer pour que le 0 apparaissent tjrs en pos1 et l'autre en pos 3
        //    var index = UnityEngine.Random.Range(0, spawnPos.Length);
        //    while (used.Contains(spawnPos[index]))
        //    {
        //        index = UnityEngine.Random.Range(0, spawnPos.Length);
        //    }

        //    used.Add(spawnPos[index]);

        //    var temp = Instantiate(item.Spawnable, spawnPos[index].position, Quaternion.identity, spawnPos[index]);
            
        //    var tempCombatScript = temp.GetComponent<EnnemyBehavior>();
        //    Destroy(tempCombatScript);
        //}
    }

    public void SwapEnemy()
    {

    }
}
