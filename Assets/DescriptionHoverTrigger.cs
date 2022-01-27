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


    // Update is called once per frame
    void Update()
    {
        
    }
}
