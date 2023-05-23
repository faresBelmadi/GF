using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EssenceUI : MonoBehaviour
{
    public GameObject buttonConsumation;

    public void activate()
    {
        buttonConsumation.SetActive(true);
    }

    public void deactivate()
    {
        Invoke("invokedDeactivate", 2f);
    }


    private void invokedDeactivate()
    {
        buttonConsumation.SetActive(false);
    }
}
