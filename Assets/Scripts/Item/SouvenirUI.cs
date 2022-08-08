using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SouvenirUI : MonoBehaviour
{
    public Souvenir LeSouvenir;
    public GameObject SouvenirObject;
    public TextMeshProUGUI TexteDescription;

    public void StartUp()
    {
        SouvenirObject = this.gameObject;
        SouvenirObject.GetComponent<Image>().sprite = LeSouvenir.Icon;
        TexteDescription.text = LeSouvenir.Nom + "\n" + LeSouvenir.Description;
    }

    public void OnMouseEnter()
    {
        
    }

    public void OnMouseExit()
    {
        
    }
}
