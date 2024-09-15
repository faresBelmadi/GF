using System.Collections;
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

    public Encounter[] _encounter;

    [SerializeField] private DialogueManager DialogueManager;

    public int StepTuto;
    public int StepMapTuto;
    public int StepBatlleTuto;

    private static TutoManager instance;

    public bool ShowSoulConsumation;

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
        }
        else
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

    private void HideAllPanels()
    {
        PanelMap.SetActive(false);
        PanelBattle.SetActive(false);
    }

    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    private void ShowNextStep()
    {
        if (StepTuto == 1)
        {
            HideAllPanels();
            ShowPanel(PanelBattle);
            StartBattle();
        }
    }

    //public GameObject MenuCamera;
    public GameObject CurrentRoomCamera;
    GameObject[] rootScene;
    Scene s;

    IEnumerator LoadSceneAsync(string name)
    {
        yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        s = SceneManager.GetSceneByName(name);

        rootScene = s.GetRootGameObjects();
        StartBattle();
    }

    //public void LoadSceneAsync(string name)
    //{
    //    SceneManager.LoadScene(name);
    //    s = SceneManager.GetSceneByName(name);

    //    rootScene = s.GetRootGameObjects(); 
    //    StartBattle();
    //}

    void StartBattle()
    {
        Debug.Log("coucou");
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
}