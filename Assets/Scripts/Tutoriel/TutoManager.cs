using UnityEngine;
using UnityEngine.SceneManagement;

public class TutoManager : MonoBehaviour
{
    [Header("Panels Tuto")]
    public GameObject PanelMap;
    [SerializeField]
    private MapPanel _panelMap;
    public GameObject PanelBattle;
    public GameObject PanelAutel;
    public GameObject StatPanel;
    public GameObject TutoPanel;
    

    [Header("Battle")]
    public BattleManager BattleManager;
    public JoueurStat JoueurStat;
    public ClassPlayer TutoClassSo;

    public Encounter[] _encounter;

    [SerializeField]
    private TutoDialogueManager _dialogueManager;

    public int StepTuto;
    public int StepMapTuto;
    public int IndexEncounter;

    private static TutoManager instance;

    public bool ShowSoulConsumation;
    private int _indEncounter = 0;
    private TutoMondeManager _tutoMondeManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            StepTuto = 0;
            StepMapTuto = 0;
            IndexEncounter = 0;
            ShowSoulConsumation = false;
            JoueurStat.ListBuffDebuff.Clear();          //On clear les buff sinon pour le cas ou le tuto n'es pas complété et qui resterait des objet buff dans le SO
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        //HideAllPanels();
        //ShowPanel(PanelMap);
        _tutoMondeManager = PanelMap.GetComponentInChildren<TutoMondeManager>();
    }

    public static TutoManager Instance
    {
        get { return instance; }
    }

    public void NextStep()
    {
        StepTuto++;
        ShowNextStep();
    }

    private void HideAllPanels()
    {
        PanelMap.SetActive(false);
        //PanelBattle.SetActive(false);
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
        Debug.Log("StepTuto = " + StepTuto + " / stepbattle = " + IndexEncounter);
        if (StepTuto == 4)
            IndexEncounter++;
        if (StepTuto == 1 || StepTuto == 3 || StepTuto == 4)    //Battle moment
        {
            // HideAllPanels();
            //ShowPanel(PanelBattle);
            if (StepTuto != 4)
                _panelMap.Hide();
            _dialogueManager.EnableButtonAnswer();

            StartBattle();
        }
        else if (StepTuto == 2)                                 //Map Moment
        {
            //HideAllPanels();
            //ShowPanel(PanelMap);
            _dialogueManager.DisableButtonAnswer(); //to prevent double clicking
            _panelMap.Show();
            _tutoMondeManager.DisplayInfoTutoMonde();
        }
        else if (StepTuto == 5)                                 //End
        {
            SceneManager.LoadScene("Monde");
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
        _dialogueManager.InitDialogueStep();
        Debug.Log("encounter : " + Instance.IndexEncounter);
        BattleManager.player.Stat.Volonter = 5;
        BattleManager.LoadEnemy(Instantiate(Instance._encounter[Instance.IndexEncounter]));
        
    }

    public void Loot()
    {
        StatPanel.transform.GetChild(0).gameObject.SetActive(true);
        StatPanel.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void SkipTutoDuringTuto()
    {
        SceneManager.LoadSceneAsync(1);
        Destroy(this.gameObject);
    }

    public void EndDialogueTuto()
    {
        if (IndexEncounter == 1)
            _dialogueManager.StartCombat();
        else
        {
            IndexEncounter++;
            NextStep();
        }
    }
}