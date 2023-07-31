using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class TutoBattleManager : MonoBehaviour
{
    //public TutoManager Instance;
    public Transform[] spawnPos;
    
    [SerializeField]
    private DialogueManager DialogueManager;

    public void Start()
    {
        SpawnEnemy();
        DialogueEnableSetup();
    }


    void DialogueEnableSetup()
    {
        DialogueManager.SetupDialogue(TutoManager.Instance._encounter[TutoManager.Instance.StepTuto]);
    }

    void SpawnEnemy()
    {
        List<Transform> used = new List<Transform>();
        foreach (var item in TutoManager.Instance._encounter[TutoManager.Instance.StepTuto].ToFight)
        {
            var index = UnityEngine.Random.Range(0, spawnPos.Length);
            while (used.Contains(spawnPos[index]))
            {
                index = UnityEngine.Random.Range(0, spawnPos.Length);
            }

            used.Add(spawnPos[index]);

            var temp = Instantiate(item.Spawnable, spawnPos[index].position, Quaternion.identity, spawnPos[index]);
            
            var tempCombatScript = temp.GetComponent<EnnemyBehavior>();
            Destroy(tempCombatScript);
        }
    }
}
