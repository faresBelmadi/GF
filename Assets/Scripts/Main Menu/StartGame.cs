using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Toggle DoTutoCheck;

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
}
