using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private GameObject _objetToHightlight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _objetToHightlight.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _objetToHightlight.SetActive(false);

    }

}
