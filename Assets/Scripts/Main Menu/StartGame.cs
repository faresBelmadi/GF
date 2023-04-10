using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    public void Button_StartGame(int classe)
    {
        PlayerPrefs.SetInt("ClassSelected", classe);
        SceneManager.LoadSceneAsync(1);


    }
}
