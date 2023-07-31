using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class TutoManager : MonoBehaviour
{
    public Encounter[] _encounter;
    
    [SerializeField]
    private DialogueManager DialogueManager;

    public int StepTuto;

    private static TutoManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            StepTuto = 0;
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

}
