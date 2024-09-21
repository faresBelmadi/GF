﻿using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutoManager : MonoBehaviour
{
    [Header("Panels Tuto")]
    public GameObject PanelMap;
    public GameObject PanelBattle;
    public GameObject PanelAutel;

    [Header("Battle")]
    public BattleManager BattleManager;
    public JoueurStat JoueurStat;
    public ClassPlayer TutoClassSo;

    public Encounter[] _encounter;

    [SerializeField] private TutoDialogueManager DialogueManager;

    public int StepTuto;
    public int StepMapTuto;
    public int StepBatlleTuto;

    private static TutoManager instance;

    public bool ShowSoulConsumation;
    private int _indEncounter = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            StepTuto = 0;
            StepMapTuto = 0;
            StepBatlleTuto = 0;
            ShowSoulConsumation = false;
            JoueurStat.ResetStat();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        HideAllPanels();
        ShowPanel(PanelMap);
    }

    public static TutoManager Instance
    {
        get { return instance; }
    }

    public void NextStep()
    {
        StepTuto++;
        Debug.LogWarning("New Step Tuto : " + StepTuto);
        ShowNextStep();
    }

    private void HideAllPanels()
    {
        PanelMap.SetActive(false);
        PanelBattle.SetActive(false);
        /* TO DO
        PanelAutel.SetActive(false);*/
    }

    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    private void ShowNextStep()
    {
        ClearPos();
        Debug.Log("StepTuto = " + StepTuto + " / stepbattle = " + StepBatlleTuto);
        if (StepTuto == 1 || StepTuto == 3 || StepTuto == 4 || StepTuto == 5)
        {
            HideAllPanels();
            ShowPanel(PanelBattle);
            StartBattle();
        }
        else if (StepTuto == 2)
        {
            HideAllPanels();
            ShowPanel(PanelMap);
        }
    }

    private void ClearPos()
    {
        foreach (var spawnPos in BattleManager.spawnPos)
        {
            if (spawnPos.childCount > 0)
                Destroy(spawnPos.GetChild(0).gameObject);
        }
    }

  

    void StartBattle()
    {
        DialogueManager.InitDialogueStep();
        Debug.LogWarning("encounter : " + Instance.StepBatlleTuto);
        BattleManager.LoadEnemy(Instantiate(Instance._encounter[Instance.StepBatlleTuto]));
        
    }

    public void Loot()
    {
        var gO = GameObject.Find("StatPrefab");
        gO.transform.GetChild(0).gameObject.SetActive(true);
        gO.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void SkipTutoDuringTuto()
    {
        SceneManager.LoadSceneAsync(1);
        Destroy(this.gameObject);
    }

    public void EndDialogueTuto()
    {

        if (StepBatlleTuto == 0 || StepBatlleTuto == 1 || StepBatlleTuto == 3)
            NextStep();        
        if (StepBatlleTuto == 2)
            DialogueManager.StartCombat();
        StepBatlleTuto++;
        Debug.Log("New Step Battle Tuto : " + StepBatlleTuto);
        
    }
}