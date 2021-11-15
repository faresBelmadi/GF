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

    [Header("Classes & Encounter")]
    public List<ClassPlayer> AllClasses;

    public List<Encounter> AllEncounter;

    public ClassPlayer classSO;
    public PlayerStat playerStat;

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
        LoadSave();
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
                playerStat = new PlayerStat(){
                    HP = loadedData.CurrentRun.player.HP,
                    MaxHP = classSO.PlayerStat.HP,
                    Volonté = loadedData.CurrentRun.player.Volonté,
                    MaxVolonté = classSO.PlayerStat.Volonté,
                    Conscience = loadedData.CurrentRun.player.Conscience,
                    MaximumConscience = classSO.PlayerStat.ConscienceMax,
                    Essence = loadedData.CurrentRun.player.Essence,
                    Dmg = loadedData.CurrentRun.player.dmg,
                    armor = loadedData.CurrentRun.player.armor,
                    Speed = loadedData.CurrentRun.player.Speed,
                    Calme = classSO.PlayerStat.Calme
                };

                playerStat.AvailableSpell = new List<Spell>();

                foreach (var item in loadedData.CurrentRun.player.BoughtSpellID)
                {
                    var temp = classSO.spellClass.First(c => c.ID == item);
                    temp.Status = SpellStatus.bought;
                    foreach (var item2 in temp.idChildren)
                    {
                        var t = classSO.spellClass.First(c => c.ID == item2);
                        if(t.isAvailable)
                            t.Status = SpellStatus.unlocked;
                        
                    }
                    playerStat.AvailableSpell.Add(temp);
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
        data.CurrentRun = new RunData(){ClassID = 0};
        data.previousRuns = new List<RunData>();
        var spellsToAdd = AllClasses.First(c => c.ID == 0).spellClass.Where(c => c.Status == SpellStatus.bought);
        List<int> boughtspells = new List<int>();
        foreach (var item in spellsToAdd)
        {
            boughtspells.Add(item.ID);   
        }
        data.CurrentRun.player = new PlayerData()
        {   
            HP = AllClasses.First(c => c.ID == 0).PlayerStat.HP,
            Conscience = AllClasses.First(c => c.ID == 0).PlayerStat.Conscience,
            dmg = AllClasses.First(c => c.ID == 0).PlayerStat.dmg,
            Speed = AllClasses.First(c => c.ID == 0).PlayerStat.Speed,
            Volonté = AllClasses.First(c => c.ID == 0).PlayerStat.Volonté,
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
                HP = classSO.PlayerStat.HP,
                Volonté = classSO.PlayerStat.Volonté,
                Conscience = classSO.PlayerStat.Conscience,
                Essence = classSO.PlayerStat.Essence,
                dmg = classSO.PlayerStat.dmg,
                Speed = classSO.PlayerStat.Speed,
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
            HP = playerStat.HP,
            Volonté = playerStat.Volonté,
            Conscience = playerStat.Conscience,
            Essence = playerStat.Essence,
            dmg = playerStat.Dmg,
            armor = playerStat.armor,
            Speed = playerStat.Speed
        };
        loadedData.CurrentRun.player.BoughtSpellID = new List<int>();
        foreach (var item in playerStat.AvailableSpell)
        {
            loadedData.CurrentRun.player.BoughtSpellID.Add(item.ID);
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

    }
    
    void getClassRun()
    {
        classSO = Instantiate(AllClasses.First(c => c.ID == loadedData.CurrentRun.ClassID));
        classSO.PlayerStat = Instantiate(AllClasses.First(c => c.ID == loadedData.CurrentRun.ClassID).PlayerStat);
        classSO.spellClass.Clear();
        foreach (var item in AllClasses.First(c => c.ID == loadedData.CurrentRun.ClassID).spellClass)
        {
            classSO.spellClass.Add(Instantiate(item));
        }
    }

    public void DeadPlayer()
    {
        loadedData.CurrentRun.Ended =true;
        SaveGame();
        LoadSave();
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return SceneManager.UnloadSceneAsync(0);
        yield return SceneManager.LoadSceneAsync(0);
        pmm = FindObjectOfType<PlayerMapManager>();
        rm = FindObjectOfType<RoomManager>();
    }
}