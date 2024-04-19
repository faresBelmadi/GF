using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Toggle DoTutoCheck;

    public GameObject MainMenuGO;
    public GameObject OptionMenuGO;

    public void Button_StartGame(int classe)
    {
        PlayerPrefs.SetInt("ClassSelected", classe);
        if (TutoManager.Instance != null)
            Destroy(TutoManager.Instance.gameObject);
        if (DoTutoCheck.isOn)
            SceneManager.LoadScene("Tuto");
        else
            SceneManager.LoadSceneAsync(1);
    }

    public void Button_Quit()
    {
        Application.Quit();
    }

    public void SetLanguagePref(int idLanguage)
    {
        PlayerPrefs.SetInt("Lang", idLanguage);
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
