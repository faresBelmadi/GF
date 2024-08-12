using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using yutokun;

public class TradManager : MonoBehaviour
{
    public enum SUPPORTEDLANGUAGES
    {
        FR,
        EN,
        ZH
    }


    public static TradManager instance;

    //0 = fr, 1 = en, 2 = zh, etc
    public int IdLanguage
    {
        get
        {
            var value = PlayerPrefs.GetInt("Lang", -1000);
            return value;
        }
    }

    public Dictionary<string, List<string>> DialogueDictionary = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> CapaDictionary = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> MiscDictionary = new Dictionary<string, List<string>>();

    private Analyzer _analyzer;

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
        _analyzer = GetComponent<Analyzer>();
    }

    #region LOGGER
    private void LogError(string key)
    {
        StringBuilder strb = new StringBuilder();
        strb.AppendLine($"Error when trying to get translation for this key [{key}].");

        if ((DialogueDictionary.ContainsKey(key) && DialogueDictionary[key].Count <= IdLanguage)
            || (CapaDictionary.ContainsKey(key) && CapaDictionary[key].Count <= IdLanguage)
            || (MiscDictionary.ContainsKey(key) && MiscDictionary[key].Count <= IdLanguage))
        {

            strb.AppendLine($"Missing language : language {IdLanguage.ToString()} with ID ({IdLanguage}) not present in dictionnary.");
        }
        else if (!DialogueDictionary.ContainsKey(key) && !CapaDictionary.ContainsKey(key) && !MiscDictionary.ContainsKey(key))
        {
            strb.AppendLine($"Missing translation with key : {key}.");
        }
        Debug.LogError(strb.ToString());
    }
    #endregion

    #region FILELOADER
    public void LoadTrad()
    {
        LoadTradDialogue();
        LoadTradCapa();
        //LoadTradMisc();
    }

    private void LoadTradDialogue()
    {
#if UNITY_EDITOR
        string path = "Assets/StreamingAssets/Traduction/GameTraductionFile.csv";
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
                DialogueDictionary.Add(row[0], templist);
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
        string path = "Assets/StreamingAssets/Traduction/CapaTraductionFile.csv";
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

        //foreach (var item in CapaDictionary)
        //{
        //    Debug.Log(item.Key + " | " + item.Value.Count);
        //}
    }

    private void LoadTradMisc()
    {
#if UNITY_EDITOR
        string path = "Assets/StreamingAssets/Traduction/GameTraductionFile.csv";
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
    #endregion

    #region GETTERS

    public string GetTranslation(string key, string defaultTranslation = "missing translation")
    {
        if (DialogueDictionary.ContainsKey(key) && DialogueDictionary[key].Count > IdLanguage)
        {
            return _analyzer.Execute(DialogueDictionary[key][IdLanguage]);
        }
        else if (CapaDictionary.ContainsKey(key) && CapaDictionary[key].Count > IdLanguage)
        {
            return _analyzer.Execute(CapaDictionary[key][IdLanguage]);
        }
        else if (MiscDictionary.ContainsKey(key) && MiscDictionary[key].Count > IdLanguage)
        {
            return _analyzer.Execute(MiscDictionary[key][IdLanguage]);
        }
        LogError(key);
        return defaultTranslation;
    }
    [Obsolete]
    private string GetTranslatedDialogue(string key)
    {
        if (DialogueDictionary.ContainsKey(key))
        {

            return _analyzer.Analyze(DialogueDictionary[key][IdLanguage]);
        }
        else
        {
            Debug.LogError($"Missing dialogue with key : {key}");
            return string.Empty;
        }
    }
    [Obsolete]
    public string GetTranslatedCapa(string key)
    {
        if (CapaDictionary.ContainsKey(key))
        {
            return _analyzer.Execute(CapaDictionary[key][IdLanguage]);
        }
        else
        {
            Debug.LogError($"Missing capa with key : {key}");
            return string.Empty;
        }
    }
    [Obsolete]
    private string GetTranslatedMisc(string key)
    {
        if (MiscDictionary.ContainsKey(key))
        {
            return _analyzer.Analyze(MiscDictionary[key][IdLanguage]);
        }
        else
        {
            Debug.LogError($"Missing capa with key : {key}");
            return string.Empty;
        }
    }
    #endregion
}
