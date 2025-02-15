using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject _loadingScreen;
    //[SerializeField]
    //private Slider _loadingBar;
    [SerializeField]
    private Image _loadingBar;

    private void Start()
    {
        _loadingScreen.SetActive(false);
    }
    
    public void LoadGameScene()
    {
        _loadingScreen.SetActive(true);

        StartCoroutine(LoadLevel());
    }


    private IEnumerator LoadLevel()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            _loadingBar.fillAmount = progress;
            yield return null;
        }
    }
}
