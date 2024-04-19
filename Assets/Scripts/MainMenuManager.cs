using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public GameObject MainMenuGO;
    public GameObject OptionMenuGO;
    public GameObject CharacterSelectionGO;

    public void SetLanguagePref(int idLanguage)
    {

    }

    //Appuie sur le bouton Play
    public void PlayButtonEvent()
    {
        //desactivation du MainMenuGo
        MainMenuGO.SetActive(false);
        //activation du CharacterSelectionGO
        CharacterSelectionGO.SetActive(true);
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
    public void CharacterSelectionEvent(int choice)
    {
        switch(choice)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
}
