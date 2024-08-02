using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightTriggerEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Action<int, int, int> _actionToTriggerEnter;
    private Action _actionToTriggerExit;
    private int _volCost;
    private int _conscCost;
    private int _radCost;

    public void SetActionsToTrigger(Action<int, int, int> actionToTriggerEnter, Action actionToTriggerExit, int volCost, int radCost, int conscCost)
    {
        _actionToTriggerEnter = actionToTriggerEnter;
        _actionToTriggerExit = actionToTriggerExit;
        _volCost = volCost;
        _conscCost = conscCost;
        _radCost = radCost;
}
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_actionToTriggerEnter != null)
        {
            _actionToTriggerEnter(_volCost, _radCost, _conscCost);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_actionToTriggerExit != null)
        {
            _actionToTriggerExit();
        }
    }

   
}
