using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour
{
    public Encounter _encounter;
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
        DialogueManager.SetupDialogue(_encounter);
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
            Destroy(tempCombatScript);
        }
    }
}
