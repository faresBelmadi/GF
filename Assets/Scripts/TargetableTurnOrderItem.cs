using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetableTurnOrderItem : MonoBehaviour
{
    public GameObject Ciblage;
    public Image Intention;

    public Action RaiseEvent;

    private void OnMouseEnter()
    {
        if (Ciblage == null) return;
        Ciblage.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (Ciblage == null) return;
        Ciblage.SetActive(false);
    }

    public void ChangeIntention(Sprite intent)
    {
        Intention.sprite = intent;
    }

    private void OnMouseDown()
    {
        return;//Temp
        if (Ciblage == null) return;
        RaiseEvent();
        Ciblage.SetActive(false);
    }
}
