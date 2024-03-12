using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipTutoButtonManager : MonoBehaviour
{
    void Start()
    {
        if (TutoManager.Instance != null)
            this.GetComponent<Button>().onClick.AddListener(() => TutoManager.Instance.SkipTutoDuringTuto());
    }
}
