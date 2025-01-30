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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        Debug.Log("enter");
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
