using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI.Extensions;
using yutokun;

public class TradManager : MonoBehaviour
{
    public enum SUPPORTEDLANGUAGES
    {
        FR = 0,
        EN = 1,
        ZH = 2,

        None = 100
    }

    private const string _playerPrefsLangKey = "Lang";

    public SUPPORTEDLANGUAGES DefaultLanguage = SUPPORTEDLANGUAGES.EN;

    [SerializeField, ReadOnly]
    private SUPPORTEDLANGUAGES _gameLanguage = SUPPORTEDLANGUAGES.None;
    public SUPPORTEDLANGUAGES Language
    {
        get => _debugMode ? _debugLanguage : _gameLanguage;
        set => _gameLanguage = value;
    }

    public int IdLanguage => (int)Language;

    [Header("Debug")]
    [SerializeField]
    private bool _debugMode;
    [SerializeField]
    private SUPPORTEDLANGUAGES _debugLanguage;

    public static TradManager instance;

    private Dictionary<string, List<string>> _localizations = new Dictionary<string, List<string>>();

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

        if (!PlayerPrefs.HasKey(_playerPrefsLangKey))
        {
            _gameLanguage = DefaultLanguage;
            PlayerPrefs.SetInt(_playerPrefsLangKey, (int)DefaultLanguage);
            PlayerPrefs.Save();
        }
        else
        {
            var langId = PlayerPrefs.GetInt(_playerPrefsLangKey, -1000);
            _gameLanguage = Enum.IsDefined(typeof(SUPPORTEDLANGUAGES), langId) ? (SUPPORTEDLANGUAGES)langId : DefaultLanguage;
        }

        LoadTrad();
        _analyzer = GetComponent<Analyzer>();
    }

    public void SetLanguage(SUPPORTEDLANGUAGES language)
    {
        PlayerPrefs.SetInt(_playerPrefsLangKey, (int)language);
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

        if (!_localizations.ContainsKey(key))
        {
            strb.AppendLine($"Missing translation with key : {key}.");
        }
        else if (_localizations[key].Count <= IdLanguage)
        {
            strb.AppendLine($"Missing language : language {IdLanguage.ToString()} with ID ({IdLanguage}) not present in dictionnary.");
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
        success &= LoadFromFile(_gamePath, _localizations);
        success &= LoadFromFile(_capaPath, _localizations);
        success &= LoadFromFile(_miscPath, _localizations);
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
    
    public string GetTranslation(string key, string defaultTranslation = "missing translation")
    {
        if (_localizations.TryGetValue(key, out var trads) && IdLanguage <= trads.Count)
        {
            return _analyzer.Execute(trads[IdLanguage]);
        }

        LogError(key);
        return defaultTranslation;
    }

    #endregion
}
