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

    #region INSPECTABLE PROPERTIES

    public SUPPORTEDLANGUAGES DefaultLanguage = SUPPORTEDLANGUAGES.EN;
    [SerializeField, ReadOnly]
    private SUPPORTEDLANGUAGES _gameLanguage = SUPPORTEDLANGUAGES.None;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField]
    private bool _debugMode;
    [SerializeField]
    private SUPPORTEDLANGUAGES _debugLanguage;

    public enum LanguageErrorSeverity
    {
        Ignore,
        Info,
        Warning,
        Error,
    }

    [Serializable]
    public class SeverityForLanguage
    {
        public SUPPORTEDLANGUAGES Language;
        public LanguageErrorSeverity Severity;
    }

    [Header("Language error severity")]
    public LanguageErrorSeverity DefaultSeverity = LanguageErrorSeverity.Ignore;
    public SeverityForLanguage[] SpecificSeverity = new[]
    {
        new SeverityForLanguage { Language = SUPPORTEDLANGUAGES.FR, Severity = LanguageErrorSeverity.Warning },
        new SeverityForLanguage { Language = SUPPORTEDLANGUAGES.EN, Severity = LanguageErrorSeverity.Warning },
    };
#endif

    #endregion

    public static TradManager instance;

    private const string _playerPrefsLangKey = "Lang";

    private Analyzer _analyzer;

    public static event Action OnRefreshTranslation;

    public SUPPORTEDLANGUAGES Language
    {
#if UNITY_EDITOR
        get => _debugMode ? _debugLanguage : _gameLanguage;
#else
        get => _gameLanguage;
#endif
        set => _gameLanguage = value;
    }

    private Dictionary<string, List<string>> _localizations = new Dictionary<string, List<string>>();
#if UNITY_EDITOR
    private Dictionary<string, List<string>> _idsFoundInFiles = new Dictionary<string, List<string>>();
    private List<string>[] _missingTranslationsByLanguage = null;
#endif

#if UNITY_EDITOR
    public static TradManager CreateEditorInstance()
    {
        var tradGO = new GameObject("EDITOR_TRADMANAGER");
        tradGO.hideFlags = HideFlags.HideAndDontSave;
        return tradGO.AddComponent<TradManager>();
    }

    TradManager()
    {
        _missingTranslationsByLanguage = new List<string>[LanguageCount];
        for (int i = 0; i < LanguageCount; i++)
            _missingTranslationsByLanguage[i] = new List<string>();
    }
#endif

    #region MONOBEHAVIOUR

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logAllErrors">Force error logs as Error severity.</param>
    public bool LoadTrad(bool logAllErrors = false)
    {
#if UNITY_EDITOR
        LoadFromFile(_gamePath, "Game");
        LoadFromFile(_capaPath, "Capa");
        LoadFromFile(_miscPath, "Misc");
        return LogErrors(logAllErrors);
#else
        LoadFromFile(_gamePath);
        LoadFromFile(_capaPath);
        LoadFromFile(_miscPath);
        return true;
#endif
    }

#if UNITY_EDITOR
    private void LoadFromFile(string path, string provenanceStr)
#else
    private void LoadFromFile(string path)
#endif
    {
        var sheet = CSVParser.LoadFromPath(path, Delimiter.Semicolon, Encoding.UTF8);
        foreach (var row in sheet.Skip(1))
        {
            var id = row[0];
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError($"Found an empty Id reading from {path}! (previous was {(_localizations.Count == 0 ? "none" : _localizations.Last().Key)})");
                continue;
            }

#if UNITY_EDITOR
            AddIdProvenance(id, provenanceStr);
#endif

            var trads = row.Skip(1).Take(LanguageCount).ToList();
            _localizations.TryAdd(id, trads);

#if UNITY_EDITOR
            CheckAndAddMissingTranslations(id, trads);
#endif
        }
    }

    #endregion

    #region GETTERS 

    public int IdLanguage => (int)Language;

    public int LanguageCount => Enum.GetValues(typeof(SUPPORTEDLANGUAGES)).Length - 1;

    public string GetTranslation(string key, string defaultTranslation = "missing translation")
    {
        if (!_localizations.TryGetValue(key, out var trads))
        {
            Debug.LogError($"Missing translation for {key} in {Language.ToString()}.");
            return defaultTranslation;
        }

        return _analyzer.Execute(trads[IdLanguage]);
    }

    #endregion

    public void SetLanguage(SUPPORTEDLANGUAGES language)
    {
        PlayerPrefs.SetInt(_playerPrefsLangKey, (int)language);
        RefreshTranslation();
    }

    public void RefreshTranslation()
    {
        OnRefreshTranslation?.Invoke();
    }

#if UNITY_EDITOR
    #region EDITOR

    private void AddIdProvenance(string id, string provenanceStr)
    {
        if (!_idsFoundInFiles.ContainsKey(id))
            _idsFoundInFiles.Add(id, new List<string>());
        _idsFoundInFiles[id].Add(provenanceStr);
    }

    private void CheckAndAddMissingTranslations(string id, List<string> trads)
    {
        for (int lang = 0; lang < trads.Count; lang++)
        {
            string trad = trads[lang];
            if (string.IsNullOrWhiteSpace(trad))
            {
                _missingTranslationsByLanguage[lang].Add(id);
            }
        }
    }

    private bool LogErrors(bool logAllMissingTranslationsAsErrors)
    {
        bool hasErrors = false;
        hasErrors &= FindAndLogDuplications();
        hasErrors &= FindAndLogMissingTranslations(logAllMissingTranslationsAsErrors);
        return hasErrors;
    }

    private bool FindAndLogDuplications()
    {
        var duplicatedIds = _idsFoundInFiles.Where(x => x.Value.Count > 1).ToDictionary(x => x.Key, x => x.Value);
        if (!duplicatedIds.Any())
        {
            return false;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"Found {duplicatedIds.Count} duplicated Ids!");
        sb.AppendLine(string.Join("\n", duplicatedIds.Select(x => $"{x.Key} in {string.Join(" and ", x.Value)}.")));
        Debug.LogError(sb.ToString());
        return true;
    }

    private bool FindAndLogMissingTranslations(bool logAllMissingTranslationsAsErrors)
    {
        bool hasErrors = false;
        for (int langId = 0; langId < _missingTranslationsByLanguage.Length; langId++)
        {
            var lang = (SUPPORTEDLANGUAGES)langId;
            var severity = logAllMissingTranslationsAsErrors ? LanguageErrorSeverity.Error : GetSeverityForLanguage(lang);

            var missingTrads = _missingTranslationsByLanguage[langId];
            if (missingTrads.Any())
            {
                hasErrors = true;
                LogMissingTranslations(lang, missingTrads, severity);
            }
        }
        return hasErrors;
    }

    private void LogMissingTranslations(SUPPORTEDLANGUAGES lang, List<string> missingTrads, LanguageErrorSeverity severity)
    {
        if (severity == LanguageErrorSeverity.Ignore)
            return;

        var sb = new StringBuilder();
        sb.AppendLine($"Language {lang} is missing {missingTrads.Count} translations!");
        sb.AppendLine(string.Join("\n", missingTrads.Select(t => $"[{_idsFoundInFiles[t][0]}] {t}")));

        if (severity == LanguageErrorSeverity.Error)
        {
            Debug.LogError(sb.ToString());
        }
        else if (severity == LanguageErrorSeverity.Warning)
        {
            Debug.LogWarning(sb.ToString());
        }
        else Debug.Log(sb.ToString());
    }

    private LanguageErrorSeverity GetSeverityForLanguage(SUPPORTEDLANGUAGES lang)
    {
        var severityForLang = SpecificSeverity.FirstOrDefault(s => s.Language == lang);
        return severityForLang == null ? DefaultSeverity : severityForLang.Severity;
    }

    #endregion
#endif
}
