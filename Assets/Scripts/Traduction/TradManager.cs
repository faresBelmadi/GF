using System;
using System.Collections.Generic;
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

    [SerializeField]
    private bool _debugMode;
    [SerializeField]
    private SUPPORTEDLANGUAGES _debugLanguage;

    public static TradManager instance;

    //0 = fr, 1 = en, 2 = zh, etc
    public int IdLanguage
    {
        get
        {
            var value = (_debugMode)? (int)_debugLanguage:PlayerPrefs.GetInt("Lang", -1000);
            return value;
        }
    }

    private Dictionary<string, List<string>> _dialogueDictionary = new Dictionary<string, List<string>>();
    private Dictionary<string, List<string>> _capaDictionary = new Dictionary<string, List<string>>();
    private Dictionary<string, List<string>> _miscDictionary = new Dictionary<string, List<string>>();

    // Pour la vérification de l'unicité des ID de traductions
    private HashSet<string> _idList = new HashSet<string>();

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

        if ((_dialogueDictionary.ContainsKey(key) && _dialogueDictionary[key].Count <= IdLanguage)
            || (_capaDictionary.ContainsKey(key) && _capaDictionary[key].Count <= IdLanguage)
            || (_miscDictionary.ContainsKey(key) && _miscDictionary[key].Count <= IdLanguage))
        {

            strb.AppendLine($"Missing language : language {IdLanguage.ToString()} with ID ({IdLanguage}) not present in dictionnary.");
        }
        else if (!_dialogueDictionary.ContainsKey(key) && !_capaDictionary.ContainsKey(key) && !_miscDictionary.ContainsKey(key))
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
        LoadTradMisc();
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
                if (!_dialogueDictionary.TryAdd(row[0], templist))
                {
                    Debug.LogError("Error when adding key " + row[0] + " to dialogue dictionnary, key already added");
                }
                if (!row[0].ToLower().Equals("id") && !_idList.Add(row[0]))
                {
                    Debug.LogError("Duplicate Key : " + (row[0]));
                }
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
                _capaDictionary.Add(row[0], templist);

                if (!row[0].ToLower().Equals("id") && !_idList.Add(row[0]))
                {
                    Debug.LogError("Duplicate Key : " + (row[0]));
                }
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
        string path = "Assets/StreamingAssets/Traduction/MiscTraductionFile.csv";
#else
                        string path = Application.dataPath + "/StreamingAssets/Traduction/MiscTraductionFile.csv";
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
                _miscDictionary.Add(row[0], templist);

                if (!row[0].ToLower().Equals("id") && !_idList.Add(row[0]))
                {
                    Debug.LogError("Duplicate Key : " + (row[0]));
                }
            }
        }

        //foreach (var item in _miscDictionary)
        //{
        //    Debug.Log(item.Key + " | " + item.Value.Count);
        //}
    }
    #endregion

    #region GETTERS
    /// <summary>
    /// Get translation of text with the given Key. The text will be in the loaded language.
    /// </summary>
    /// <param name="key">The key of the translated Text</param>
    /// <param name="defaultTranslation">The default translation wanted if the key or language doesn't exist</param>
    /// <returns>Translated text</returns>
    public string GetTranslation(string key, string defaultTranslation = "missing translation")
    {
        if (_dialogueDictionary.ContainsKey(key) && _dialogueDictionary[key].Count > IdLanguage)
        {
            return _analyzer.Execute(_dialogueDictionary[key][IdLanguage]);
        }
        else if (_capaDictionary.ContainsKey(key) && _capaDictionary[key].Count > IdLanguage)
        {
            return _analyzer.Execute(_capaDictionary[key][IdLanguage]);
        }
        else if (_miscDictionary.ContainsKey(key) && _miscDictionary[key].Count > IdLanguage)
        {
            return _analyzer.Execute(_miscDictionary[key][IdLanguage]);
        }
        LogError(key);
        return defaultTranslation;
    }
    [Obsolete]
    private string GetTranslatedDialogue(string key)
    {
        if (_dialogueDictionary.ContainsKey(key))
        {

            return _analyzer.Analyze(_dialogueDictionary[key][IdLanguage]);
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
        if (_capaDictionary.ContainsKey(key))
        {
            return _analyzer.Execute(_capaDictionary[key][IdLanguage]);
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
        if (_miscDictionary.ContainsKey(key))
        {
            return _analyzer.Analyze(_miscDictionary[key][IdLanguage]);
        }
        else
        {
            Debug.LogError($"Missing capa with key : {key}");
            return string.Empty;
        }
    }
    #endregion
}
