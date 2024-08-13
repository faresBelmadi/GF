using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FontManager : MonoBehaviour
{
    [Header("DefaultFont")]
    [Tooltip("Assign here default font who will be assign to all TMP during runtime")]
    [SerializeField]
    private TMP_FontAsset _defaultFontAsset;
    [Tooltip("Assign here default legacy font who will be assign to all legacy Text during runtime")]
    [SerializeField]
    private Font _defaultLegacyFont;
    [SerializeField]
    private TMP_SpriteAsset _TMPSpriteAsset;
    [Header("Debug")]
    [SerializeField]
    private bool _verboseMode = false;
    
    public TMP_FontAsset DefaultFontAsset { get => _defaultFontAsset; }
    public Font DefaultLegacyFont { get => _defaultLegacyFont; }


    public static FontManager instance;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        } 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignFont();
    }
    private void AssignFont()
    {
        TMP_Text[] texts = FindObjectsOfType<TMP_Text>(true);        
        for  (int i = 0; i < texts.Length; i++)
        {
            texts[i].font = _defaultFontAsset;

            //if (texts[i].spriteAsset == null)
            //    texts[i].spriteAsset = _TMPSpriteAsset;
        }
        if (_verboseMode) Debug.Log($"{texts.Length} TMP changed in scene {SceneManager.GetActiveScene().name}");

        Text[] legacyTexts = FindObjectsOfType<Text>(true);
        for (int i = 0; i < legacyTexts.Length; i++)
        {
            legacyTexts[i].font = _defaultLegacyFont;
        }
        if (_verboseMode) Debug.Log($"{legacyTexts.Length} Legacy Text changed in scene {SceneManager.GetActiveScene().name}");
    }


}
