using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;

public class TutoManager : MonoBehaviour
{
    public Encounter[] _encounter;
    
    [SerializeField]
    private DialogueManager DialogueManager;

    public int StepTuto;
    public int StepMapTuto;
    public int StepBatlleTuto;

    private static TutoManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            StepTuto = 0;
            StepMapTuto = 0;
            StepBatlleTuto = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static TutoManager Instance
    {
        get { return instance; }
    }

    public void NextStep()
    {
        StepTuto++;
        LoadNextStep();
    }

    private void LoadNextStep()
    {
        if (StepTuto == 1 || StepTuto == 3 || StepTuto == 6)
        {
            StepMapTuto++;
            SceneManager.LoadScene("TutoMonde");
        }
        else if (StepTuto == 2 || StepTuto == 5 || StepTuto == 7)
        {
            StepBatlleTuto++;
            SceneManager.LoadScene("Tuto");
        }
        else if(StepTuto == 4)
        {
            SceneManager.LoadScene("TutoAutel");
        }
    }
}
