using System;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour
{
    private static TutoManager instance;

    public GameObject StatPanel;
    public GameObject TutoPanel;

    [Header("Battle")] public BattleManager BattleManager;
    public JoueurStat JoueurStat;
    public ClassPlayer TutoClassSo;
    [SerializeField] private JoueurBehavior _playerHolder;

    public List<Encounter> _encounter;
    public GameObject FredPos;
    public GameObject Fred;
    public GameObject CanvasMap;
    public Encounter SavedEncounter;
    [SerializeField] private TutoDialogueManager _dialogueManager;

    public int StepTuto;
    public int IndexEncounter;


    public bool ShowSoulConsumation;
    [Header("Datas")] [SerializeField] private ClairvoyanceIconData _clairvoyanceIconData;
    [SerializeField] private Souvenir _souvenirToLoot;
    [field:SerializeField] public Transform SouvenirPosTuto { get; private set; }

    public ClairvoyanceIconData StatIcons
    {
        get => _clairvoyanceIconData;
    }

    public Encounter CurrentEncounter
    {
        get => _encounter[IndexEncounter];
    }

    public DialogueManager TutoDialogMngr
    {
        get => _dialogueManager;
    }

    public JoueurBehavior Player
    {
        get => _playerHolder;
    }

    public static event Action OnEndDialog;
    public static event Action OnStartCombat;
    public static event Action OnEndCombat;

    public static event Action OnEndTuto;
    private GameObject FredForFight;

    private void Awake()
    {
        if (instance == null && GameManager.Instance.IsTuto)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            StepTuto = 0;
            IndexEncounter = 0;
            ShowSoulConsumation = false;
            //JoueurStat.ListBuffDebuff.Clear();          //On clear les buff sinon pour le cas ou le tuto n'es pas complété et qui resterait des objet buff dans le SO
            JoueurStat = GameManager.Instance.playerStat;
            JoueurStat.ListSouvenir.Add(_souvenirToLoot);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (!GameManager.Instance.IsTuto)
        {
            Destroy(gameObject);
        }
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

    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    private void ShowNextStep()
    {
        ClearPos();
        Debug.Log("StepTuto = " + StepTuto + " / stepbattle = " + IndexEncounter);
        if (StepTuto == 2)
            IndexEncounter++;
        if (StepTuto == 1 || StepTuto == 2) //Battle moment
        {
            _dialogueManager.EnableButtonAnswer();

            StartBattle();
        }
        else if (StepTuto == 3) //End
        {
            Player.ToggleVisibility(true);
            EndTuto();
        }
    }

    private void ClearPos()
    {
        foreach (var spawnPos in BattleManager.spawnPos)
        {
            if (spawnPos.childCount > 0)
                Destroy(spawnPos.GetChild(0).gameObject);
        }
        if (FredPos.transform.childCount > 0)
            Destroy(FredPos.transform.GetChild(0).gameObject);
    }



    void StartBattle()
    {
        _dialogueManager.InitDialogueStep();
        Debug.Log("encounter : " + Instance.IndexEncounter);
        GameManager.Instance.LoadCombat();
        BattleManager.player.Stat.Volonter = 5;
    }

    public void Loot()
    {
        StatPanel.SetActive(true);
        TutoPanel.transform.localPosition = new Vector3(0f,-150f,0f); 
    }

    public void SkipTutoDuringTuto()
    {
        EndTuto();
    }

    // Raise the event to trigger UI panel
    public void StartCombat()
    {
        OnStartCombat?.Invoke();
        BattleManager.StartCombat();
    }

    public void EndCombat()
    {
        OnEndCombat?.Invoke();
    }

    public void EndDialogueTuto()
    {
        OnEndDialog?.Invoke();
        if (IndexEncounter == 1)
        {
            _dialogueManager.StartCombat();
        }
        else
        {
            IndexEncounter++;
            NextStep();
        }
    }

    public void EndTuto()
    {
        ClearPos();
        GameManager.Instance.EndTuto();

        OnEndTuto?.Invoke();

        Destroy(gameObject);
    }

    public void SpawnFredForFight()
    {
        FredForFight = Instantiate(Fred, FredPos.transform.position, Quaternion.identity, FredPos.transform);
    }
}