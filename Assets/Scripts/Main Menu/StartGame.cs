using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject MainMenuGO;
    public GameObject OptionMenuGO;

    [SerializeField]
    public CharacterSelect _characterSelect;
    [SerializeField]
    private LevelLoader _levelLoader;
    [SerializeField]
    private GameObject _tutoPrompt;

    private void Awake()
    {
        int height = PlayerPrefs.GetInt("ScreenHeight", 1080);
        int width = PlayerPrefs.GetInt("ScreenWidth", 1920);
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        bool vsync = PlayerPrefs.GetInt("Vsync", 0) == 1;

        Screen.SetResolution(width, height, fullscreen);
        QualitySettings.vSyncCount = vsync ? 1 : 0;
        Debug.Log("Starting graphic options :");
        Debug.Log($"Screen : {width}X{height}, fullscreen mode : {fullscreen}, Vsync : {vsync}");

    }
    public void StartWithTuto(int classe) => Button_StartGame(classe, true);
    
    public void StartWithoutTuto(int classe) => Button_StartGame(classe, false);
    

    public void ShowPrompt()
    {
        _tutoPrompt.SetActive(true);
    }
    public void Button_StartGame(int classe, bool tuto)
    {
        PlayerPrefs.SetInt("ClassSelected", classe);
        if (TutoManager.Instance != null)
            Destroy(TutoManager.Instance.gameObject);
        PlayerPrefs.SetInt("DoTutorial", tuto ? 1:0);
        
        _levelLoader.LoadGameScene();
    }

    public void Button_Quit()
    {
        Application.Quit();
    }

    public void SetLanguagePref(int idLanguage)
    {
        PlayerPrefs.SetInt("Lang", idLanguage);
        TradManager.instance.RefreshTranslation();
        OptionButtonEventClose();
    }

    public void OptionButtonEventOpen()
    {
        //desactivation du MainMenuGo
        MainMenuGO.SetActive(false);
        //activation du OptionMenuGO
        OptionMenuGO.SetActive(true);
    }
    public void OptionButtonEventClose()
    {
        //desactivation du MainMenuGo
        MainMenuGO.SetActive(true);
        //activation du OptionMenuGO
        OptionMenuGO.SetActive(false);
       
    }
}
