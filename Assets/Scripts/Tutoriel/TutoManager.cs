using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutoManager : MonoBehaviour
{
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
        LoadNextStep();
    }

    private void LoadNextStep()
    {
        if (StepTuto == 1 || StepTuto == 3 || StepTuto == 5 || StepTuto == 7)
        {
            StepMapTuto++;
            SceneManager.LoadScene("TutoMonde");
        }
        else if (StepTuto == 2 || StepTuto == 8)
        {
            StepBatlleTuto++;
            SceneManager.LoadScene("Tuto");
        }
        else if (StepTuto == 6)
        {
            StepBatlleTuto++;
            StartCoroutine("LoadSceneAsync", "TutoBattle");
            //LoadSceneAsync("TutoBattle");
        }
        else if (StepTuto == 4)
        {
            //SceneManager.LoadScene("TutoAutel");
            SceneManager.LoadScene("TutoAutel OLD");
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
        CurrentRoomCamera = rootScene.First(c => c.name == "BattleCamera");
        var BattleMan = rootScene.First(c => c.name == "BattleManager").GetComponent<BattleManager>();
        //TutoManager.Instance._encounter[TutoManager.Instance.StepBatlleTuto]
        BattleMan.LoadEnemy(Instantiate(TutoManager.Instance._encounter[TutoManager.Instance.StepBatlleTuto]));
        CurrentRoomCamera.SetActive(true);
        SceneManager.UnloadSceneAsync("TutoMonde");
        //MenuCamera.SetActive(false);
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