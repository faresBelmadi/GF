using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour
{
    public Encounter _encounter;

    [SerializeField]
    private DialogueManager DialogueManager;

    public void LoadTutorial(Encounter ToSpawn) // appelé dans le gameManager
    {
        _encounter = ToSpawn;
        //SpawnEnemy();
        //player.UpdateUI();
        //player.DesactivateSpells();
        DialogueEnableSetup();
    }


    void DialogueEnableSetup()
    {
        DialogueManager.SetupDialogue(_encounter);
    }



    void StartTutorial() //Proabblement a deplacer dans le game manager/mapmanager
    {
        //CurrentRoomCamera = rootScene.First(c => c.name == "BattleCamera");
        GameManager.instance
            .TutoManager = /*rootScene.First(c => c.name == "BattleManager").GetComponent<BattleManager>();*/this;

        //if (enemieType.Equals("normal"))
        GameManager.instance.LoadTuto();
        //else if (enemieType.Equals("elite"))
        //{
        //    GameManager.instance.LoadCombatElite();
        //}
        //else if (enemieType.Equals("boss"))
        //{
        //    GameManager.instance.LoadCombatBoss();
        //}
        //CurrentRoomCamera.SetActive(true);
        //MenuCamera.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
