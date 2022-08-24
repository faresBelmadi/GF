using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Access : MonoBehaviour
{
    Scene s;
    GameObject[] rootScene, oldrootScene;
    Transform[] ChildCanvas;
    public GameObject OldCamera;
    public GameObject CurrentRoomCamera;
    public MenuStatManager ManagerStat;
    public AutelManager ManagerAutel;
    public string _PreviousScene;

    public void AccesArbre()
    {
        JoueurStat _Stat = null;
        if (OldCamera.name == "MenuStatCamera")
        {
            _Stat = Instantiate(ManagerStat.StatTemp);
            _PreviousScene = "MenuStat";
            oldrootScene = SceneManager.GetSceneByName("MenuStat").GetRootGameObjects();
            ChildCanvas = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<RectTransform>();
        }
        else if(OldCamera.name == "AutelCamera")
        {
            _Stat = Instantiate(ManagerAutel.Stat);
            _PreviousScene = "Autel";
            oldrootScene = SceneManager.GetSceneByName("Autel").GetRootGameObjects();
            ChildCanvas = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<Transform>();
        }
        StartCoroutine("LoadSceneAsync", _Stat);
    }

    IEnumerator LoadSceneAsync(JoueurStat _Stat)
    {
        yield return SceneManager.LoadSceneAsync("ArbreCompetenceGuerrier", LoadSceneMode.Additive);
        s = SceneManager.GetSceneByName("ArbreCompetenceGuerrier");

        rootScene = s.GetRootGameObjects();
        StartArbre(_Stat);
    }

    public void StartArbre(JoueurStat _Stat)
    {
        CurrentRoomCamera = rootScene.First(c => c.name == "ArbreCamera");
        rootScene.First(c => c.name == "ArbreManager").GetComponent<ArbreManager>().StartArbre(_Stat);
        //rootScene.First(c => c.name == "ArbreManager").GetComponent<ArbreManager>().PreviousScene = _PreviousScene;
        CurrentRoomCamera.SetActive(true);
        OldCamera.SetActive(false);
        /*for (int i = 0; i < oldrootScene.Length; i++)
        {
            if(oldrootScene[i].name == "Canvas")
            {
                for (int y = 0; y < ChildCanvas.Length; y++)
                {
                    if (ChildCanvas[y].gameObject.name == "ArbreAccess")
                    {
                        ChildCanvas[y].gameObject.GetComponent<Button>().interactable = false;
                    }
                    else if (ChildCanvas[y].gameObject.name == "Canvas")
                    {

                    }
                    else
                    {
                        ChildCanvas[y].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                oldrootScene[i].SetActive(false);
            }
        }*/
    }

    public IEnumerator EndArbre(JoueurStat _Stat)
    {
        CurrentRoomCamera.SetActive(false);
        OldCamera.SetActive(true);
        /*for (int i = 0; i < oldrootScene.Length; i++)
        {
            if (oldrootScene[i].name == "Canvas")
            {
                for (int y = 0; y < ChildCanvas.Length; y++)
                {
                    if (ChildCanvas[y].gameObject.name == "ArbreAccess")
                    {
                        ChildCanvas[y].gameObject.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        ChildCanvas[y].gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                oldrootScene[i].SetActive(true);
            }
        }*/
        if (_PreviousScene == "MenuStat")
        {
            ManagerStat.StatTemp = _Stat;
        }
        else if(_PreviousScene == "Autel")
        {
            ManagerAutel.Stat = _Stat;
            ManagerAutel.EndAutel();
        }
        yield return SceneManager.UnloadSceneAsync(s);
    }
}
