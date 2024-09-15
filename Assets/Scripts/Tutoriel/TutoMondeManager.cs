using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoMondeManager : MonoBehaviour
{

    public Text TutoTextDisplay;
    public Dictionary<string, string> TutoMondeText = new Dictionary<string, string>();
    public List<GameObject> AllGameObjectsToDisplay;

    public void Start()
    {
        DisplayInfoTutoMonde();
    }

    public void OnEnable()
    {
        DisplayInfoTutoMonde();
    }

    private void DisplayInfoTutoMonde()
    {
        foreach (var gO in AllGameObjectsToDisplay)
        {
            gO.GetComponent<Image>().color = Color.black;
        }

        for (int i = 0; i < TutoManager.Instance.StepMapTuto; i++)
        {
            AllGameObjectsToDisplay[i].GetComponent<Image>().color = Color.gray;
        }

        AllGameObjectsToDisplay[TutoManager.Instance.StepMapTuto].GetComponent<Image>().color = Color.white;

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
