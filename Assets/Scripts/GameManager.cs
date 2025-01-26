using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    //[Header("Debug")]
    //[SerializeField]
    //private bool _doTuto = true;

    [SerializeField] private GameObject _crystal;
    [SerializeField] private Transform _parent;

    [Header("Managers")]
    //public RoomManager rm;
    public PlayerMapManager pmm;

    public BattleManager BattleMan;
    public TutoManager TutoManager;
    public AleaManager AleaMan;
    public OldAutelManager OldAutelMan;
    public MenuStatManager StatMan;
    public UiMondeManager UiMondeMan;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private GamePanelManager _gamePanelManager;

    [Header("Classes & Encounter")] public List<ClassPlayer> AllClasses;

    public List<Encounter> AllEncounter;
    public int EncounterIndex;
    public List<EncounterAlea> AllEncounterAlea;

    [SerializeField] private List<Encounter> TEMPEncounterNeutral;
    [SerializeField] private List<Encounter> TEMPEncounterClass;
    [SerializeField] private List<Encounter> TEMPEncounterElite;
    [SerializeField] private List<Encounter> TEMPEncounterClassElite;
    [SerializeField] private List<Encounter> TEMPEncounterBoss;

    [field: SerializeField] public EncounterSetData EncounterSet { get; private set; }
    //[SerializeField] private List<Encounter> TutoEncounter;
    //[SerializeField] private int CurrentTutoEncounter = 0;

    public List<Souvenir> AllSouvenir;
    public List<Souvenir> CopyAllSouvenir;

    public ClassPlayer classSO;
    [HideInInspector] public JoueurStat playerStat;

    public int ClassIDSelected;

    public PassifRules passifRules;
    [Header("Data")] [SerializeField] private SpriteData _spriteData;
    public GameData loadedData;
    public SkillTreePrinter SkillTreeUI;
    [SerializeField]
    private ClairvoyanceIconData _clairvoyanceIconData;
    [field: SerializeField]
    public CommonNameData CommonNameData { get; private set; }

    [field: SerializeField]
    public CommonDescData CommonDescData { get; private set; }

    public ClairvoyanceIconData StatIcons
    {
        get => _clairvoyanceIconData;
    }

    public bool IsTuto { get; set; }
    public bool IsPaused { get; set; } = false;

    public GamePanelManager GamePanelMngr
    {
        get => _gamePanelManager;
    }

    public DialogueManager DialManager
    {
        get
        {
            if (!IsTuto)
                return _dialogueManager;
            else
            {
                return TutoManager.Instance.TutoDialogMngr;
            }
        }
    }

    public SpriteData SpriteData
    {
        get { return _spriteData; }
    }

    #region Events

    public static event Action OnStartCombat;
    public static event Action OnLootAfterCombat;
    public static event Action OnStartEvent;
    public static event Action OnStartDialog;
    public static event Action OnHideMap;
    public static event Action OnShowMap;
    public static event Action OnStartAutel;
    public static event Action OnEndGame;

    #endregion



    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        UnityEngine.Random.InitState((int) DateTime.Now.Ticks);
        //LoadSave();
        ClassIDSelected = PlayerPrefs.GetInt("ClassSelected");

        IsTuto = PlayerPrefs.GetInt("DoTutorial", 0) == 0 ? false : true;
        PlayerPrefs.SetInt("DoTutorial", 0); //we set tuto mode to false

        CreateSave();
        GetClassRun();

        _gamePanelManager.InitPanel();
        /*
        if (TutoManager.Instance != null)
            Destroy(TutoManager);
        */
    }

    public void EndTuto()
    {
        IsTuto = false;

        //CreateSave();
        //GetClassRun();
        pmm.ToggleMap(true);
        UiMondeMan.EnableMonde();
        ShowMap();
    }

    private void LoadSave()
    {

#if UNITY_EDITOR
        string path = "Assets/SavedData/GameData/Game.json";
#else
        string path = Application.persistentDataPath + "/SavedData/GameData/Game.json";
#endif
        string dataAsJson;
        if (File.Exists(path))
        {
            // Read the json from the file into a string
            dataAsJson = File.ReadAllText(path);

            // Pass the json to JsonUtility, and tell it to create a SkillTree object from it
            loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
            if (!loadedData.CurrentRun.Ended)
            {
                GetClassRun();

                playerStat = Instantiate(AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat);

                playerStat.Radiance = loadedData.CurrentRun.player.Radiance;
                playerStat.RadianceMax = loadedData.CurrentRun.player.RadianceMax;
                playerStat.Volonter = loadedData.CurrentRun.player.Volonter;
                playerStat.VolonterMax = loadedData.CurrentRun.player.VolonterMax;
                playerStat.Conscience = loadedData.CurrentRun.player.Conscience;
                playerStat.ConscienceMax = loadedData.CurrentRun.player.ConscienceMax;
                playerStat.Conviction = loadedData.CurrentRun.player.Conviction;
                playerStat.Resilience = loadedData.CurrentRun.player.Resilience;
                playerStat.Essence = loadedData.CurrentRun.player.Essence;
                playerStat.ForceAme = loadedData.CurrentRun.player.ForceAme;
                playerStat.Vitesse = loadedData.CurrentRun.player.Vitesse;
                playerStat.Calme = loadedData.CurrentRun.player.Calme;
                playerStat.Clairvoyance = loadedData.CurrentRun.player.Clairvoyance;
                playerStat.ClairvoyanceOriginal = loadedData.CurrentRun.player.Clairvoyance;
                playerStat.SlotsSouvenir = loadedData.CurrentRun.player.SlotsSouvenir;

                for (int i = 0; i < AllSouvenir.Count; i++)
                {
                    CopyAllSouvenir.Add(Instantiate(AllSouvenir[i]));
                }

                playerStat.ListSouvenir = new List<Souvenir>();
                playerStat.ListSpell = new List<Spell>();
                playerStat.ListPassif = new List<Passif>();
                //TODO : a decommenter quand le systeme de save sera mis en ligne
                //       cette boucle load les spells acheté dans les runs d'avant.
                /*foreach (var item in loadedData.CurrentRun.player.BoughtSpellID)
                {
                    var temp = classSO.PlayerStat.ListSpell.First(c => c.IDSpell == item);
                    temp.SpellStatue = SpellStatus.bought;
                    foreach (var item2 in temp.IDChildren)
                    {
                        var t = classSO.PlayerStat.ListSpell.First(c => c.IDSpell == item2);
                        if(t.IsAvailable)
                            t.SpellStatue = SpellStatus.unlocked;

                    }
                    playerStat.ListSpell.Add(temp);
                }*/
                foreach (var item in classSO.PlayerStat.ListSpell)
                {
                    playerStat.ListSpell.Add(item);
                }

                foreach (var item in classSO.PlayerStat.ListPassif)
                {
                    playerStat.ListPassif.Add(item);
                }
            }
        }
        else
        {
            CreateSave();
            GetClassRun();
        }
    }

    private void CreateSave()
    {

        GameData data = new GameData();
        data.CurrentRun = new RunData() {ClassID = ClassIDSelected};
        data.previousRuns = new List<RunData>();
        var spellsToAdd = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.ListSpell
            .Where(c => c.SpellStatue == SpellStatus.bought);
        List<int> boughtspells = new List<int>();
        foreach (var item in spellsToAdd)
        {
            boughtspells.Add(item.IDSpell);
        }

        data.CurrentRun.player = new PlayerData()
        {
            Radiance = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Radiance,
            RadianceMax = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.RadianceMax,
            Conscience = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Conscience,
            ForceAme = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.ForceAme,
            Vitesse = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Vitesse,
            Volonter = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Volonter,
            Clairvoyance = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Clairvoyance,
            Essence = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Essence,
            VolonterMax = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.VolonterMax,
            ConscienceMax = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.ConscienceMax,
            Conviction = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Conviction,
            Resilience = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Resilience,
            Calme = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Calme,
            SlotsSouvenir = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.SlotsSouvenir,
            BoughtSpellID = boughtspells
        };
        data.CurrentRun.map = new MapData()
        {
            usedSeed = pmm.mapUsedSeed,
            visitedRoomIds = pmm.visitedMapIndexs,
            roomSelectedEncounter = pmm.roomSelectedEncounters
        };
        string json = JsonUtility.ToJson(data);

#if UNITY_EDITOR
        string path = "Assets/SavedData/GameData/Game.json";
#else
        string path = Application.persistentDataPath + "/SavedData/GameData/Game.json";
        System.IO.Directory.CreateDirectory(Application.persistentDataPath+"/SavedData");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath+"/SavedData/GameData");
#endif

        System.IO.File.WriteAllText(path, json);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        LoadSave();
    }

    public void SaveGame()
    {
        SavePlayer();
        if (loadedData.CurrentRun.Ended)
        {
            loadedData.previousRuns.Add(loadedData.CurrentRun);
            loadedData.CurrentRun.player = new PlayerData()
            {
                Radiance = playerStat.Radiance,
                RadianceMax = playerStat.RadianceMax,
                Volonter = playerStat.Volonter,
                Conscience = playerStat.Conscience,
                Essence = playerStat.Essence,
                ForceAme = playerStat.ForceAme,
                Vitesse = playerStat.Vitesse,
                Clairvoyance = playerStat.Clairvoyance,
                VolonterMax = playerStat.VolonterMax,
                ConscienceMax = playerStat.ConscienceMax,
                Conviction = playerStat.Conviction,
                Resilience = playerStat.Resilience,
                Calme = playerStat.Calme,
                SlotsSouvenir = playerStat.SlotsSouvenir,
                BoughtSpellID = new List<int>() {0}
            };
            loadedData.CurrentRun.Ended = false;
        }

        string json = JsonUtility.ToJson(loadedData);
#if UNITY_EDITOR
        string path = "Assets/SavedData/GameData/Game.json";
#else
        string path = Application.persistentDataPath + "/SavedData/GameData/Game.json";
#endif

        System.IO.File.WriteAllText(path, json);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private void SavePlayer()
    {
        loadedData.CurrentRun.player = new PlayerData()
        {
            Radiance = playerStat.Radiance,
            Volonter = playerStat.Volonter,
            Conscience = playerStat.Conscience,
            Essence = playerStat.Essence,
            ForceAme = playerStat.ForceAme,
            Vitesse = playerStat.Vitesse,
            Clairvoyance = playerStat.Clairvoyance,
            VolonterMax = playerStat.VolonterMax,
            ConscienceMax = playerStat.ConscienceMax,
            Conviction = playerStat.Conviction,
            Resilience = playerStat.Resilience,
            Calme = playerStat.Calme,
            SlotsSouvenir = playerStat.SlotsSouvenir
        };
        loadedData.CurrentRun.player.BoughtSpellID = new List<int>();
        foreach (var item in playerStat.ListSpell)
        {
            loadedData.CurrentRun.player.BoughtSpellID.Add(item.IDSpell);
        }

    }

    public void SetRoom(Room set)
    {
        pmm.CurrentRoom = set;
    }

    public int SelectEncounterId(TypeRoom type)
    {
        switch (type)
        {
            case TypeRoom.ENCOUNTER:
                return UnityEngine.Random.Range(0, EncounterSet.EncounterNeutralList.Count);
                break;
            case TypeRoom.CLASS_ENCOUNTER:
                return UnityEngine.Random.Range(0, EncounterSet.EncounterClassList.Count);
                break;
            case TypeRoom.ELITE:
                return UnityEngine.Random.Range(0, EncounterSet.EncounterEliteList.Count);
                break;
            case TypeRoom.CLASS_ELITE:
                return UnityEngine.Random.Range(0, EncounterSet.EncounterClassEliteList.Count);
                break;
            case TypeRoom.BOSS:
                return UnityEngine.Random.Range(0, EncounterSet.EncounterBossList.Count);
                break;
            default: return 0;
        }
    }

    public void StartCombat()
    {
        Debug.Log("Raise event : OnStartCombat");
        OnStartCombat?.Invoke();
        BattleMan.StartCombat();
    }

    public void LoadCombat()
    {
        Debug.Log("Raise event : OnStartDialog");
        OnStartDialog?.Invoke();
        if (IsTuto)
        {
            TutoManager.Instance._encounter[1].ToFight = AllEncounter[EncounterIndex].ToFight;
            TutoManager.Instance._encounter[1].forcedOrder = AllEncounter[EncounterIndex].forcedOrder;
            BattleMan.LoadEnemy(Instantiate(TutoManager.Instance.CurrentEncounter));
            if (TutoManager.Instance.CurrentEncounter.ToFight.All(x =>
                    !x.Spawnable.gameObject.name.Contains("Fred")))
                TutoManager.Instance.SpawnFredForFight();
        }
        else
        {
            BattleMan.LoadEnemy(Instantiate(AllEncounter[EncounterIndex]));
            EncounterIndex++;
        }
    }

    public void Loot()
    {
        Debug.Log("Loot", gameObject);
        OnLootAfterCombat.Invoke();
    }

    public void UnloadCombat()
    {
        Debug.Log("Unload Combat");
    }

    public void LoadChoosenCombat(TypeRoom roomType, int encounterId)
    {
        OnStartDialog?.Invoke();
        //if (IsTuto)
        //{
        //    BattleMan.LoadEnemy(Instantiate(TutoEncounter[CurrentTutoEncounter]));
        //    CurrentTutoEncounter++;
        //}
        switch (roomType)
        {
            case TypeRoom.ENCOUNTER:
                BattleMan.LoadEnemy(Instantiate(EncounterSet.EncounterNeutralList[encounterId]));
                break;
            case TypeRoom.CLASS_ENCOUNTER:
                BattleMan.LoadEnemy(Instantiate(EncounterSet.EncounterClassList[encounterId]));
                break;
            case TypeRoom.ELITE:
                BattleMan.LoadEnemy(Instantiate(EncounterSet.EncounterEliteList[encounterId]));
                break;
            case TypeRoom.CLASS_ELITE:
                BattleMan.LoadEnemy(Instantiate(EncounterSet.EncounterClassEliteList[encounterId]));
                break;
            case TypeRoom.BOSS:
                BattleMan.LoadEnemy(Instantiate(EncounterSet.EncounterBossList[encounterId]));
                break;
        }
    }

    public void LoadTuto()
    {
        OnStartDialog?.Invoke();
    }

    public void LoadCombatNormal()
    {
        //BattleMan.LoadEnemy(Instantiate(AllEncounter[0]));
        OnStartDialog?.Invoke();
        BattleMan.LoadEnemy(
            Instantiate(EncounterSet.EncounterNeutralList[UnityEngine.Random.Range(0, EncounterSet.EncounterNeutralList.Count())]));
    }

    public void LoadCombatClass()
    {
        OnStartDialog?.Invoke();
        BattleMan.LoadEnemy(Instantiate(EncounterSet.EncounterClassList[UnityEngine.Random.Range(0, EncounterSet.EncounterClassList.Count())]));
    }

    public void LoadCombatElite()
    {
        OnStartDialog?.Invoke();
        //BattleMan.LoadEnemy(Instantiate(AllEncounter[1]));
        BattleMan.LoadEnemy(Instantiate(EncounterSet.EncounterEliteList[UnityEngine.Random.Range(0, EncounterSet.EncounterEliteList.Count())]));
    }

    public void LoadCombatClassElite()
    {
        OnStartDialog?.Invoke();
        //BattleMan.LoadEnemy(Instantiate(AllEncounter[1]));
        BattleMan.LoadEnemy(
            Instantiate(EncounterSet.EncounterClassEliteList[UnityEngine.Random.Range(0, EncounterSet.EncounterClassEliteList.Count())]));
    }

    public void LoadCombatBoss()
    {
        OnStartDialog?.Invoke();
        //BattleMan.LoadEnemy(Instantiate(AllEncounter[2]));
        BattleMan.LoadEnemy(Instantiate(EncounterSet.EncounterBossList[UnityEngine.Random.Range(0, EncounterSet.EncounterBossList.Count())]));
    }

    public void LoadEvent()
    {
        OnStartEvent?.Invoke();
        OnStartDialog?.Invoke();
        AleaMan.StartAlea(Instantiate(EncounterSet.EncounterAleaList[UnityEngine.Random.Range(0, EncounterSet.EncounterAleaList.Count)]));
    }

    public void UnloadEvent()
    {
        Debug.Log("UnloadEvent;");
    }

    public void LoadAutel()
    {

        Debug.Log("Load Autel");
        OnStartAutel?.Invoke();
    }

    public void UnloadAutel()
    {
        Debug.Log("Unload Autel");
    }

    public void StartStatJoueur()
    {
        pmm.ShowMenuStat();
    }

    //public void ShowMenuStat()
    //{
    //    StatMan.StartMenuStat();
    //}

    void GetClassRun()
    {
        if (classSO != null && IsTuto /*TutoManager.Instance != null*/)
        {
            Debug.Log("Coucouuuuuuu");
            return;
        }

        classSO = Instantiate(AllClasses.First(c => c.ID == loadedData.CurrentRun.ClassID));
        classSO.PlayerStat = Instantiate(AllClasses.First(c => c.ID == loadedData.CurrentRun.ClassID).PlayerStat);
        classSO.PlayerStat.ListSpell.Clear();
        foreach (var item in AllClasses.First(c => c.ID == loadedData.CurrentRun.ClassID).PlayerStat.ListSpell)
        {
            classSO.PlayerStat.ListSpell.Add(Instantiate(item));
        }
    }

    public void DeadPlayer()
    {
        //loadedData.CurrentRun.Ended =true;
        //SaveGame();
        //LoadSave();
        //StartCoroutine(Reload());

        //ResetJoueurStat ?0
        GameManager.Instance.playerStat.ResetStat();

        //SceneManager.LoadScene("MainMenu");
        //Destroy(GameManager.Instance.gameObject);
        EndGame();
    }

    public void HideMap()
    {
        OnHideMap?.Invoke();
    }

    public void ShowMap()
    {
        Debug.Log("ShowMap");
        OnShowMap?.Invoke();
    }

    public void EndGame()
    {
        OnEndGame?.Invoke();
    }

    public IEnumerator Reload()
    {
        yield return SceneManager.UnloadSceneAsync(1);
        yield return SceneManager.LoadSceneAsync(0);
        pmm = FindObjectOfType<PlayerMapManager>();
        //rm = FindObjectOfType<RoomManager>();
    }
}