using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public bool DoTutorial = true;

    public void Button_StartGame(int classe)
    {
        PlayerPrefs.SetInt("ClassSelected", classe);
        if (DoTutorial)
            SceneManager.LoadScene("Tuto");
        else
            SceneManager.LoadSceneAsync(1);
    }

    public void Button_Quit()
    {
        Application.Quit();
    }
}
