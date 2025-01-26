using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableElement : MonoBehaviour
{ 

    public static event Action<GameObject> OnHoverDragElement;
    public static event Action OnDragElement;

    private bool drag = false;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Drag()
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
           canvas.transform as RectTransform,
           Input.mousePosition,
           canvas.worldCamera,
           out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint;
        }
    }
    public void Drop()
    {
        drag = false;
    }
   
    private void OnMouseEnter()
    {
        Debug.Log("drag enter, " + name);
        if (drag == true) { return; }
        OnHoverDragElement?.Invoke(gameObject);
    }
    private void OnMouseDown()
    {
       //transform.SetParent(gameObject.transform.root);
        drag = true;
        Debug.Log("clic");
        OnDragElement?.Invoke();
    }
    private void OnMouseUp()
    {
        //  Debug.Log("clic");
    }
    private void OnMouseDrag()
    {
        //  Debug.Log("Drag : " + Input.mousePosition);
    }
}
