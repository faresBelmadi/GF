using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoMondeManager : MonoBehaviour
{

    public Text TutoTextDisplay;
    public string[] TutoMondeText;
    public int indexTextTuto = 0;
    public List<GameObject> AllGameObjectsToDisplay;

    void LateUpdate()
    {
        if (Input.anyKeyDown)
        {
            ChangeTextTuto();
        }
    }

    private void ChangeTextTuto()
    {
        indexTextTuto++;
        if (indexTextTuto >= TutoMondeText.Length)
        {
            indexTextTuto = 0;
            //TutoManager.Instance.StepTuto++;
            SceneManager.LoadScene("TutoAutel");
            //SceneManager.LoadScene("Tuto");
        }
        else
        {
            DisplayTutoText();
        }
    }

    private void DisplayTutoText()
    {
        TutoTextDisplay.text = TutoMondeText[indexTextTuto];
        foreach (var gameObject in AllGameObjectsToDisplay)
        {
            if (AllGameObjectsToDisplay[indexTextTuto] == gameObject)
                gameObject.SetActive(true);
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
