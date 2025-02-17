using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject _fleches;
    private void OnEnable()
    {
        _fleches.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _fleches.SetActive(true);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _fleches.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
    }

    
}
