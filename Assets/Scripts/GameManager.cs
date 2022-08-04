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
    public AleaManager AleaMan;
    public MainMenuManager MainMenuMan;

    [Header("Classes & Encounter")]
    public List<ClassPlayer> AllClasses;

    public List<Encounter> AllEncounter;
    public List<EncounterAlea> AllEncounterAlea;

    public ClassPlayer classSO;
    public JoueurStat playerStat;

    [Header("Data")]
    public GameData loadedData;
    public SkillTreePrinter SkillTreeUI;

    private void Awake() {
        if(instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        //LoadSave();
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
                getClassRun();
                playerStat = new JoueurStat()
                {
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
                    Calme = classSO.PlayerStat.Calme
                };

                playerStat.ListSpell = new List<Spell>();

                foreach (var item in loadedData.CurrentRun.player.BoughtSpellID)
                {
                    var temp = classSO.PlayerStat.ListSpell.First(c => c.IDSpell == item);
                    temp.SpellStatue = SpellStatus.bought;
                    foreach (var item2 in temp.IDChildren)
                    {
                        var t = classSO.PlayerStat.ListSpell.First(c => c.IDSpell == item2);
                        if (t.IsAvailable)
                            t.SpellStatue = SpellStatus.unlocked;

                    }
                    playerStat.ListSpell.Add(temp);
                }
            }
        }
        else
        {
            //CreateSave();
            //getClassRun();
        }
    }

    private void CreateSave(int classChosen)
    {
        GameData data = new GameData();
        data.CurrentRun = new RunData() { ClassID = classChosen };
        data.previousRuns = new List<RunData>();
        var spellsToAdd = AllClasses.First(c => c.ID == classChosen).PlayerStat.ListSpell.Where(c => c.SpellStatue == SpellStatus.bought);
        List<int> boughtspells = new List<int>();
        foreach (var item in spellsToAdd)
        {
            boughtspells.Add(item.IDSpell);
        }
        data.CurrentRun.player = new PlayerData()
        {
            Radiance = AllClasses.First(c => c.ID == 0).PlayerStat.Radiance,
            Conscience = AllClasses.First(c => c.ID == 0).PlayerStat.Conscience,
            ForceAme = AllClasses.First(c => c.ID == 0).PlayerStat.ForceAme,
            Vitesse = AllClasses.First(c => c.ID == 0).PlayerStat.Vitesse,
            Volonter = AllClasses.First(c => c.ID == 0).PlayerStat.Volonter,
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
                Radiance = classSO.PlayerStat.Radiance,
                Volonter = classSO.PlayerStat.Volonter,
                Conscience = classSO.PlayerStat.Conscience,
                Essence = classSO.PlayerStat.Essence,
                ForceAme = classSO.PlayerStat.ForceAme,
                Vitesse = classSO.PlayerStat.Vitesse,
                BoughtSpellID = new List<int>() { 0 }
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

    public void LoadCombat()
    {
        BattleMan.LoadEnemy(Instantiate(AllEncounter[UnityEngine.Random.Range(0,AllEncounter.Count)]));
    }

    public void LoadEvent()
    {
        AleaMan.StartAlea(Instantiate(AllEncounterAlea[UnityEngine.Random.Range(0, AllEncounterAlea.Count)]));
    }
    
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
        loadedData.CurrentRun.Ended =true;
        SaveGame();
        LoadSave();
        StartCoroutine(LoadMainMenuFromMonde());
    }

    IEnumerator LoadMainMenuFromMonde()
    {
        yield return SceneManager.UnloadSceneAsync("Monde");
        yield return SceneManager.LoadSceneAsync("MainMenu");

        MainMenuMan = FindObjectOfType<MainMenuManager>();
    }

    public void ChooseCharacterSave(int classChosen)
    {
        CreateSave(classChosen);
        getClassRun();
        StartCoroutine(LoadTutorialScene());
    }

    IEnumerator LoadTutorialScene()
    {
        yield return SceneManager.UnloadSceneAsync("MainMenu");
        yield return SceneManager.LoadSceneAsync("TutorialScene");
    }

    IEnumerator LoadMondeSceneFromTutorialScene()
    {
        yield return SceneManager.UnloadSceneAsync("TutorialScene");
        yield return SceneManager.LoadSceneAsync("Monde");
        pmm = FindObjectOfType<PlayerMapManager>();
        rm = FindObjectOfType<RoomManager>();

    }
}