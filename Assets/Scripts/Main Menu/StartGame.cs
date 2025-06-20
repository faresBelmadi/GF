using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Toggle DoTutoCheck;

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
        //TODO: Temporary FIX
        Screen.SetResolution(1920, 1080, true);
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
