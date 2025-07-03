using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    [Header("Componanats")]
    [SerializeField] public GameObject backGroundObject;
    [SerializeField] public GameObject titleObject;
    [SerializeField] public GameObject coreTextObject;

    public void SetTitleText(string text)
    {
        titleObject.GetComponent<TextMeshProUGUI>().text = text;
    }
    public void SetCoreText(string text)
    {
        coreTextObject.GetComponent<TextMeshProUGUI>().text = text;
    }
}
