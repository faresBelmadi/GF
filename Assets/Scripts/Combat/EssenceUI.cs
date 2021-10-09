using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EssenceUI : MonoBehaviour, IPointerExitHandler
{
    public GameObject buttonActivation;
    public GameObject buttonConsumation;

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonConsumation.SetActive(false);
        buttonActivation.SetActive(true);
    }

    public void Activate() 
    {
        buttonConsumation.SetActive(true);
        buttonActivation.SetActive(false);
    }
}
