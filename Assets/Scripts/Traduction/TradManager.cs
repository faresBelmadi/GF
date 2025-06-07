using System;
using System.Collections.Generic;
using System.Linq;
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
    public SUPPORTEDLANGUAGES Language
    {
        get
        {
            switch (IdLanguage)
            {
                case 0:
                    return SUPPORTEDLANGUAGES.FR;
                case 1:
                    return SUPPORTEDLANGUAGES.EN;
                case 2:
                    return SUPPORTEDLANGUAGES.ZH;
                default:
                    return SUPPORTEDLANGUAGES.FR;
            }
        }
    }

    private Dictionary<string, List<string>> _dialogueDictionary = new Dictionary<string, List<string>>();
    private Dictionary<string, List<string>> _capaDictionary = new Dictionary<string, List<string>>();
    private Dictionary<string, List<string>> _miscDictionary = new Dictionary<string, List<string>>();

    private Analyzer _analyzer;

    public static event Action OnRefreshTranslation;

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

    public void SetLanguage(SUPPORTEDLANGUAGES idLanguage)
    {
        PlayerPrefs.SetInt("Lang", (int)idLanguage);
        RefreshTranslation();
    }

    public void RefreshTranslation()
    {
        OnRefreshTranslation?.Invoke();
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

#if UNITY_EDITOR
    private const string _gamePath = "Assets/StreamingAssets/Traduction/GameTraductionFile.csv";
    private const string _capaPath = "Assets/StreamingAssets/Traduction/CapaTraductionFile.csv";
    private const string _miscPath = "Assets/StreamingAssets/Traduction/MiscTraductionFile.csv";
#else
    private readonly string _gamePath = Application.dataPath + "/StreamingAssets/Traduction/GameTraductionFile.csv";
    private readonly string _capaPath = Application.dataPath + "/StreamingAssets/Traduction/CapaTraductionFile.csv";
    private readonly string _miscPath = Application.dataPath + "/StreamingAssets/Traduction/MiscTraductionFile.csv";
#endif

    private static readonly int _languageCount = Enum.GetValues(typeof(SUPPORTEDLANGUAGES)).Length;

    public bool LoadTrad()
    {
        var success = true;
        success &= LoadFromFile(_gamePath, _dialogueDictionary);
        success &= LoadFromFile(_capaPath, _capaDictionary);
        success &= LoadFromFile(_miscPath, _miscDictionary);
        return success;
    }

    private bool LoadFromFile(string path, Dictionary<string, List<string>> tradDico)
    {
        bool success = true;

        var sheet = CSVParser.LoadFromPath(path, Delimiter.Semicolon, Encoding.UTF8);
        foreach (var row in sheet.Skip(1))
        {
            var id = row[0];
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError($"Found an empty Id reading from {path}! (previous was {(tradDico.Count == 0 ? "none" : tradDico.Last().Key)})");
                success = false;
                continue;
            }

            var trads = row.Skip(1).Take(_languageCount).ToList();

            if (trads.Any(t => string.IsNullOrEmpty(t)))
            {
                Debug.LogWarning($"Line {id} is missing translations!");
                success = false;
            }

            if (!tradDico.TryAdd(id, trads))
            {
                Debug.LogError($"Line {id} is duplicated!");
                success = false;
            }
        }

        return success;
    }

    #endregion

    #region GETTERS
    /// <summary>
    /// Get translation of text with the given Key. The text will be in the loaded language.
    /// </summary>
    /// <param name="key">The key of the translated Text</param>
    /// <param name="defaultTranslation">The default translation wanted if the key or language doesn't exist</param>
    /// <returns>Translated text</returns>
    // public string GetTranslation(string key, string defaultTranslation = "missing translation")
    // {
    //     if (_dialogueDictionary.ContainsKey(key) && _dialogueDictionary[key].Count > IdLanguage)
    //     {
    //         return _analyzer.Execute(_dialogueDictionary[key][IdLanguage]);
    //     }
    //     else if (_capaDictionary.ContainsKey(key) && _capaDictionary[key].Count > IdLanguage)
    //     {
    //         return _analyzer.Execute(_capaDictionary[key][IdLanguage]);
    //     }
    //     else if (_miscDictionary.ContainsKey(key) && _miscDictionary[key].Count > IdLanguage)
    //     {
    //         return _analyzer.Execute(_miscDictionary[key][IdLanguage]);
    //     }
    //     LogError(key);
    //     return defaultTranslation;
    // }    
    
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


#if UNITY_EDITOR
    #region EDITOR GETTERS
    public string[] DialogueIds => _dialogueDictionary.Keys.ToArray();
    public string[] CapaIds => _capaDictionary.Keys.ToArray();
    public string[] MiscIds => _miscDictionary.Keys.ToArray();
    #endregion
#endif
}
