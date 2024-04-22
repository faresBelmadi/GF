using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using yutokun;

public class TradManager : MonoBehaviour
{
    public static TradManager instance;

    //0 = fr, 1 = en, 2 = zh, etc
    public int IdLanguage
    {
        get
        {
            return PlayerPrefs.GetInt("Lang");
        }
    }

    public Dictionary<string, List<string>> DialogueDictionary = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> CapaDictionary = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> MiscDictionary = new Dictionary<string, List<string>>();

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        if (!PlayerPrefs.HasKey("Lang"))
        {
            PlayerPrefs.SetInt("Lang", 1);
            PlayerPrefs.Save();
        }

        LoadTrad();
    }

    public void LoadTrad()
    {
        LoadTradDialogue();
        LoadTradCapa();
        //LoadTradMisc();
    }

    private void LoadTradDialogue()
    {
        #if UNITY_EDITOR
                string path = "Assets/Asset_in_game/Traduction/GameTraductionFile.csv";
#else
                        string path = Application.dataPath + "/StreamingAssets/Traduction/GameTraductionFile.csv";
#endif

        var sheet = CSVParser.LoadFromPath(path, Delimiter.Semicolon, Encoding.UTF8);
        foreach (var row in sheet)
        {
            row.RemoveAll(c => c == "");
            if(row.Count > 0 ) 
            {
                List<string> templist = new List<string>();
                templist.AddRange(row);
                templist.RemoveAt(0);
                DialogueDictionary.Add(row[0],templist);
            }
        }

        //foreach (var item in DialogueDictionary)
        //{
        //    Debug.Log(item.Key + " | " + item.Value.Count);
        //}
    }

    private void LoadTradCapa()
    {
#if UNITY_EDITOR
        string path = "Assets/Asset_in_game/Traduction/CapaTraductionFile.csv";
#else
                        string path = Application.dataPath + "/StreamingAssets/Traduction/CapaTraductionFile.csv";
#endif

        var sheet = CSVParser.LoadFromPath(path, Delimiter.Semicolon, Encoding.UTF8);
        foreach (var row in sheet)
        {
            row.RemoveAll(c => c == "");
            if (row.Count > 0)
            {
                List<string> templist = new List<string>();
                templist.AddRange(row);
                templist.RemoveAt(0);
                CapaDictionary.Add(row[0], templist);
            }

        }

        foreach (var item in CapaDictionary)
        {
            Debug.Log(item.Key + " | " + item.Value.Count);
        }
    }

    private void LoadTradMisc()
    {
#if UNITY_EDITOR
        string path = "Assets/Asset_in_game/Traduction/GameTraductionFile.csv";
#else
                        string path = Application.dataPath + "/StreamingAssets/Traduction/GameTraductionFile.csv";
#endif

        var sheet = CSVParser.LoadFromPath(path, Delimiter.Semicolon, Encoding.UTF8);
        foreach (var row in sheet)
        {
            row.RemoveAll(c => c == "");
            if (row.Count > 0)
            {
                List<string> templist = new List<string>();
                templist.AddRange(row);
                templist.RemoveAt(0);
                MiscDictionary.Add(row[0], templist);
            }
        }

        foreach (var item in MiscDictionary)
        {
            Debug.Log(item.Key + " | " + item.Value.Count);
        }
    }
}
