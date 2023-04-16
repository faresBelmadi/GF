using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public GameObject MainMenuGO;
    public GameObject CharacterSelectionGO;



    //Appuie sur le bouton Play
    public void PlayButtonEvent()
    {
        //desactivation du MainMenuGo
        MainMenuGO.SetActive(false);
        //activation du CharacterSelectionGO
        CharacterSelectionGO.SetActive(true);
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
