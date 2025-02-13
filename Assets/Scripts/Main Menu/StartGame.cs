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

    private void Awake()
    {
        //TODO: Temporary FIX
        Screen.SetResolution(1920, 1080, true);
    }
    public void Button_StartGame(int classe)
    {
        PlayerPrefs.SetInt("ClassSelected", classe);
        if (TutoManager.Instance != null)
            Destroy(TutoManager.Instance.gameObject);
        PlayerPrefs.SetInt("DoTutorial", DoTutoCheck.isOn ? 1:0);

        /*if (DoTutoCheck.isOn)
            SceneManager.LoadScene("TutoMonde");
        else
            SceneManager.LoadSceneAsync("GameScene");*/
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
