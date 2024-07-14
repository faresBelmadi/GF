using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffDebuffComponant : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Image buffSprite;
    [SerializeField] public TextMeshProUGUI buffCntLabel;
    [SerializeField] public GameObject popUpPanle;
    [SerializeField] public TextMeshProUGUI buffNameLabel;
    [SerializeField] public TextMeshProUGUI buffDescriptionLabel;
    public string buffName;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
        popUpPanle.SetActive(true);
        if (!IsFullyVisibleFrom(popUpPanle.GetComponent<RectTransform>()))
        {
            while (!IsFullyVisibleFrom(popUpPanle.GetComponent<RectTransform>()))
                popUpPanle.transform.Translate(-1, 0, 0);
        }
    }
    private bool IsFullyVisibleFrom(RectTransform rectTransform)
    {
        return CountCornersVisibleFrom(rectTransform, FindAnyObjectByType<Camera>()) == 4;
    }
    private static int CountCornersVisibleFrom(RectTransform rectTransform, Camera camera)
    {
        Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
        Vector3[] objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);

        int visibleCorners = 0;
        Vector3 tempScreenSpaceCorner; // Cached
        for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
        {
            tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); // Transform world space position of corner to screen space
            if (screenBounds.Contains(tempScreenSpaceCorner)) // If the corner is inside the screen
            {
                visibleCorners++;
            }
        }
        return visibleCorners;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
        popUpPanle.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
