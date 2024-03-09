using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        TutoMondeText.Add("1",
            "Ce “vieux parchemin” te permettra de ne pas te perdre sur le chemin de ta prochaine vie. Avance jusqu’à la prochaine étape, je dois encore te montrer certaines choses.");
        TutoMondeText.Add("2",
            "Cependant, tu ne vas pas garder ces cristaux éternellement. Avançons, que je puisse te montrer comment rendre un dernier hommage à ces âme fragmentées.");
        TutoMondeText.Add("3",
            "Tu vas avoir l’occasion de retrouver tes sensations : j’ai l’impression que quelqu’un t’attend avec impatience et a hâte de régler ses comptes avec toi.");
        TutoMondeText.Add("4",
            "C’est là que nos routes se séparent. Je parie que tu en avais assez de m’avoir dans les parages, pas vrai ?");
    }

    void LateUpdate()
    {
        if (Input.anyKeyDown && !alreadyClicked)
        {
            alreadyClicked = true;
            ChangeTextTuto();
        }
    }

    private void ChangeTextTuto()
    {
        TutoManager.Instance.NextStep();
    }

}
