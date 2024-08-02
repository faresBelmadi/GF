using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightTriggerEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Action<int> _actionToTriggerEnter;
    private Action _actionToTriggerExit;
    private int _cost;

    public void SetActionsToTrigger(Action<int> actionToTriggerEnter, Action actionToTriggerExit, int cost)
    {
        _actionToTriggerEnter = actionToTriggerEnter;
        _actionToTriggerExit = actionToTriggerExit;
        _cost = cost;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _actionToTriggerEnter(_cost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _actionToTriggerExit();
    }

   
}
