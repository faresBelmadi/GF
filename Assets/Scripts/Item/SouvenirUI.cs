using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SouvenirUI : MonoBehaviour
{
    public Souvenir LeSouvenir;
    public Action<Souvenir> Act;
    public Button button;
    public TextMeshProUGUI TexteDescription;

    public void StartUp()
    {
        button = this.GetComponent<Button>();
        button.GetComponent<Image>().sprite = LeSouvenir.Icon;
        TexteDescription.text = LeSouvenir.Nom + LeSouvenir.Description;
    }
}
