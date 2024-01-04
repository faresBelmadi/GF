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

    public void Start()
    {
        indexTextTuto = TutoManager.Instance.StepTuto;
        CreateDictionary();
        DisplayInfoTutoMonde();
    }

    private void DisplayInfoTutoMonde()
    {
        Debug.Log("Map Tuto" + TutoManager.Instance.StepMapTuto);

        TutoTextDisplay.text = TutoMondeText[TutoManager.Instance.StepMapTuto.ToString()];
    }

    private void CreateDictionary()
    {
        TutoMondeText = new Dictionary<string, string>();
        TutoMondeText.Add("1",
            "Ce “vieux parchemin” te permettra de ne pas te perdre sur le chemin de ta prochaine vie. Avance jusqu’à la prochaine étape, je dois encore te montrer certaines choses.");
        TutoMondeText.Add("2",
            "Cependant, tu ne vas pas garder ces cristaux éternellement. Avançons, que je puisse te montrer comment rendre un dernier hommage à ces âme fragmentées.");
        TutoMondeText.Add("3",
            "C’est là que nos routes se séparent. Je parie que tu en avais assez de m’avoir dans les parages, pas vrai ?");
    }

    void LateUpdate()
    {
        if (Input.anyKeyDown)
        {
            ChangeTextTuto();
        }
    }

    private void ChangeTextTuto()
    {
        TutoManager.Instance.NextStep();
    }

}
