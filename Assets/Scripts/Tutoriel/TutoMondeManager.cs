using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoMondeManager : MonoBehaviour
{

    public Text TutoTextDisplay;
    public Dictionary<string, string> TutoMondeText;
    public int indexTextTuto = 0;
    public List<GameObject> AllGameObjectsToDisplay;
    private bool alreadyClicked;

    public void Start()
    {
        alreadyClicked = false;
        indexTextTuto = TutoManager.Instance.StepTuto;
        CreateDictionary();
        DisplayInfoTutoMonde();
    }

    private void DisplayInfoTutoMonde()
    {
        TutoTextDisplay.text = TutoMondeText[TutoManager.Instance.StepMapTuto.ToString()];
        if (TutoManager.Instance.StepMapTuto == 1)
        {
            AllGameObjectsToDisplay[0].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            AllGameObjectsToDisplay[2].gameObject.SetActive(false);
            AllGameObjectsToDisplay[3].gameObject.SetActive(false);
            AllGameObjectsToDisplay[4].gameObject.SetActive(false);
        }
        else if (TutoManager.Instance.StepMapTuto == 2)
        {
            AllGameObjectsToDisplay[0].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            AllGameObjectsToDisplay[1].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            AllGameObjectsToDisplay[3].gameObject.SetActive(false);
            AllGameObjectsToDisplay[4].gameObject.SetActive(false);
        }
        else if (TutoManager.Instance.StepMapTuto == 3)
        {
            AllGameObjectsToDisplay[0].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            AllGameObjectsToDisplay[1].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            AllGameObjectsToDisplay[2].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            AllGameObjectsToDisplay[4].gameObject.SetActive(false);
        }
        else if (TutoManager.Instance.StepMapTuto == 4)
        {
            AllGameObjectsToDisplay[0].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            AllGameObjectsToDisplay[1].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            AllGameObjectsToDisplay[2].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            AllGameObjectsToDisplay[3].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        }

    }

    private void CreateDictionary()
    {
        TutoMondeText = new Dictionary<string, string>();
        TutoMondeText.Add("1", TradManager.instance.DialogueDictionary["TutoQ9"][TradManager.instance.IdLanguage]);
        TutoMondeText.Add("2", TradManager.instance.DialogueDictionary["TutoQ20"][TradManager.instance.IdLanguage]);
        TutoMondeText.Add("3", TradManager.instance.DialogueDictionary["TutoQ25"][TradManager.instance.IdLanguage]);
        TutoMondeText.Add("4", TradManager.instance.DialogueDictionary["TutoQ45"][TradManager.instance.IdLanguage]);
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
