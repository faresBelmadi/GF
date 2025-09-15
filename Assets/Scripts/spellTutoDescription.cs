using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class spellTutoDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ToShow;
    public string Description;
    public TextMeshProUGUI DescriptionText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
        ToShow.SetActive(true);
        DescriptionText.text = Description;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
        ToShow.SetActive(false);
    }
}
