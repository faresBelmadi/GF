using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoMondeManager : MonoBehaviour
{

    public GameObject PanelTutoTextDisplay;
    //public Dictionary<string, string> TutoMondeText = new Dictionary<string, string>();
    public List<GameObject> AllGameObjectsToDisplay;

    public void Start()
    {
        DisplayInfoTutoMonde();
    }

    public void OnEnable()
    {
    //     DisplayInfoTutoMonde();
    }

    public void DisplayInfoTutoMonde()
    {
        foreach (var gO in AllGameObjectsToDisplay)
        {
            gO.GetComponent<Image>().color = Color.black;
            gO.GetComponentInChildren<TMP_Text>().enabled = false;
        }

        for (int i = 0; i < TutoManager.Instance.StepMapTuto; i++)
        {
            AllGameObjectsToDisplay[i].GetComponent<Image>().color = Color.gray;
        }

        AllGameObjectsToDisplay[TutoManager.Instance.StepMapTuto].GetComponent<Image>().color = Color.white;
        AllGameObjectsToDisplay[TutoManager.Instance.StepMapTuto].GetComponentInChildren<TMP_Text>().enabled = true;

        if (TutoManager.Instance.StepMapTuto == 1)
        {
            PanelTutoTextDisplay.SetActive(true);
        }

    }
    
    public void LoadNextScene()
    {
        TutoManager.Instance.StepMapTuto++;
        TutoManager.Instance.NextStep();
        //SceneManager.LoadScene(sceneName);
    }

    //void LateUpdate()
    //{
    //    if (Input.anyKeyDown && !alreadyClicked)
    //    {
    //        alreadyClicked = true;
    //        GoNextStepTuto();
    //    }
    //}

    public void GoNextStepTuto()
    {
        TutoManager.Instance.NextStep();
    }

}
