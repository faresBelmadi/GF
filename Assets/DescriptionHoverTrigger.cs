using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ToShow;
    public TextMeshProUGUI Description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToShow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToShow.SetActive(false);
    }

    public void ShowDescription()
    {
        var rectTransform = this.GetComponent<RectTransform>();
        var mousePosition = Input.mousePosition;
        var normalizedMousePosition = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
        if (normalizedMousePosition.x > rectTransform.anchorMin.x &&
            normalizedMousePosition.x < rectTransform.anchorMax.x &&
            normalizedMousePosition.y > rectTransform.anchorMin.y &&
            normalizedMousePosition.y < rectTransform.anchorMax.y)
        {
            ToShow.SetActive(true);
        }
        
    }

    public void HideDescription()
    {
        ToShow.SetActive(false);
    }
}
