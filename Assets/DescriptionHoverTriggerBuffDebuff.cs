﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionHoverTriggerBuffDebuff : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ToShow;
    public TextMeshProUGUI Description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
        ToShow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
        ToShow.SetActive(false);
    }

    public void ShowDescription()
    {
        //var rectTransform = this.GetComponent<RectTransform>();
        //var mousePosition = Input.mousePosition;
        //var normalizedMousePosition = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
        //if (normalizedMousePosition.x > rectTransform.anchorMin.x &&
        //    normalizedMousePosition.x < rectTransform.anchorMax.x &&
        //    normalizedMousePosition.y > rectTransform.anchorMin.y &&
        //    normalizedMousePosition.y < rectTransform.anchorMax.y)
        //{
        //    ToShow.SetActive(true);
        //}

        Ray ray = GameManager.instance.pmm.CurrentRoomCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit[] t = Physics.RaycastAll(ray);
        foreach (var item in t)
        {
            Debug.Log(item.collider.gameObject.name);
        }
        
    
    }

    public void HideDescription()
    {
        ToShow.SetActive(false);
    }
}