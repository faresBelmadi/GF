using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuCristopher : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _hoverEnter;
    [SerializeField]
    private UnityEvent _hoverExit;
    [SerializeField]
    private UnityEvent _select;


    private void OnMouseEnter()
    {
        _hoverEnter.Invoke();
    }
    private void OnMouseExit()
    {
        _hoverExit.Invoke();
    }
    private void OnMouseDown()
    {
        _select.Invoke();
    }
}
