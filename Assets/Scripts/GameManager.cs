using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public static GameManager instance;

    [Header("Managers")]
    public RoomManager rm;
    public PlayerMapManager pmm;
    public BattleManager BattleMan;
    public TutoManager TutoManager;
    public AleaManager AleaMan;
    public OldAutelManager OldAutelMan;
    public MenuStatManager StatMan;

    [Header("Classes & Encounter")]
    public List<ClassPlayer> AllClasses;

    public List<Encounter> AllEncounter;
    public List<EncounterAlea> AllEncounterAlea;

    public List<Souvenir> AllSouvenir;
    public List<Souvenir> CopyAllSouvenir;

    public ClassPlayer classSO;
    public JoueurStat playerStat;

    public int ClassIDSelected;

    public PassifRules passifRules;
    [Header("Data")]
    public GameData loadedData;
    public SkillTreePrinter SkillTreeUI;

    private void Awake() {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        //LoadSave();
        ClassIDSelected = PlayerPrefs.GetInt("ClassSelected");
        CreateSave();
        getClassRun();
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
            if(!loadedData.CurrentRun.Ended)
            {
                getClassRun();
                playerStat = new JoueurStat() {
                    Radiance = loadedData.CurrentRun.player.Radiance,
                    RadianceMax = classSO.PlayerStat.RadianceMax,
                    Volonter = loadedData.CurrentRun.player.Volonter,
                    VolonterMax = classSO.PlayerStat.Volonter,
                    Conscience = loadedData.CurrentRun.player.Conscience,
                    ConscienceMax = classSO.PlayerStat.ConscienceMax,
                    Conviction = classSO.PlayerStat.Conviction,
                    Resilience = classSO.PlayerStat.Resilience,
                    Essence = loadedData.CurrentRun.player.Essence,
                    ForceAme = loadedData.CurrentRun.player.ForceAme,
                    Vitesse = loadedData.CurrentRun.player.Vitesse,
                    Calme = classSO.PlayerStat.Calme,
                    SlotsSouvenir = classSO.PlayerStat.SlotsSouvenir
                };
                for(int i = 0; i < AllSouvenir.Count; i++)
                {
                    CopyAllSouvenir.Add(Instantiate(AllSouvenir[i]));
                }
                playerStat.ListSouvenir = new List<Souvenir>();
                playerStat.ListSpell = new List<Spell>();
                playerStat.ListPassif = new List<Passif>();
                //TODO : Angela a mis ça en commentaire pour que val puisse faire des test, a voir si c'est a remetre 
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
            getClassRun();
        }
    }

    private void CreateSave()
    {

        GameData data = new GameData();
        data.CurrentRun = new RunData(){ClassID = ClassIDSelected};
        data.previousRuns = new List<RunData>();
        var spellsToAdd = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.ListSpell.Where(c => c.SpellStatue == SpellStatus.bought);
        List<int> boughtspells = new List<int>();
        foreach (var item in spellsToAdd)
        {
            boughtspells.Add(item.IDSpell);   
        }
        data.CurrentRun.player = new PlayerData()
        {   
            Radiance = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Radiance,
            Conscience = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Conscience,
            ForceAme = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.ForceAme,
            Vitesse = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Vitesse,
            Volonter = AllClasses.First(c => c.ID == ClassIDSelected).PlayerStat.Volonter,
            BoughtSpellID = boughtspells
        };
        string json = JsonUtility.ToJson(data);
        
        #if UNITY_EDITOR
        string path = "Assets/SavedData/GameData/Game.json";
        #else
        string path = Application.persistentDataPath + "/SavedData/GameData/Game.json";
        System.IO.Directory.CreateDirectory(Application.persistentDataPath+"/SavedData");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath+"/SavedData/GameData");
        #endif

        System.IO.File.WriteAllText(path,json);

        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
        LoadSave();
    }

    public void SaveGame()
    {
        SavePlayer();
        if(loadedData.CurrentRun.Ended)
        {
            loadedData.previousRuns.Add(loadedData.CurrentRun);
            loadedData.CurrentRun.player = new PlayerData()
            {
                Radiance = classSO.PlayerStat.Radiance,
                Volonter = classSO.PlayerStat.Volonter,
                Conscience = classSO.PlayerStat.Conscience,
                Essence = classSO.PlayerStat.Essence,
                ForceAme = classSO.PlayerStat.ForceAme,
                Vitesse = classSO.PlayerStat.Vitesse,
                BoughtSpellID = new List<int>(){0}
            };
            loadedData.CurrentRun.Ended = false;
        }
        string json = JsonUtility.ToJson(loadedData);
        #if UNITY_EDITOR
        string path = "Assets/SavedData/GameData/Game.json";
        #else
        string path = Application.persistentDataPath + "/SavedData/GameData/Game.json";
        #endif

        System.IO.File.WriteAllText(path,json);

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
            Vitesse = playerStat.Vitesse
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

    public void LoadCombatNormal()
    {
        BattleMan.LoadEnemy(Instantiate(AllEncounter[0]));
    }

    public void LoadCombatElite()
    {
        BattleMan.LoadEnemy(Instantiate(AllEncounter[1]));
    }

    public void LoadCombatBoss()
    {
        BattleMan.LoadEnemy(Instantiate(AllEncounter[2]));
    }

    public void LoadEvent()
    {
        AleaMan.StartAlea(Instantiate(AllEncounterAlea[UnityEngine.Random.Range(0, AllEncounterAlea.Count)]));
    }

    public void LoadAutel()
    {
        OldAutelMan.StartAutel();
    }
        
    public void StartStatJoueur()
    {
        pmm.ShowMenuStat();
    }

    //public void ShowMenuStat()
    //{
    //    StatMan.StartMenuStat();
    //}
    
    void getClassRun()
    {
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
        SceneManager.LoadScene("MainMenu");
        Destroy(GameManager.instance.gameObject);
    }

    public IEnumerator Reload()
    {
        yield return SceneManager.UnloadSceneAsync(1);
        yield return SceneManager.LoadSceneAsync(0);
        pmm = FindObjectOfType<PlayerMapManager>();
        rm = FindObjectOfType<RoomManager>();
    }
}