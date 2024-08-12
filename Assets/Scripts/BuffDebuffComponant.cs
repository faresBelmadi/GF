using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffDebuffComponant : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Image buffSprite;
    [SerializeField] public GameObject buffCntHolder;
    [SerializeField] public TextMeshProUGUI buffCntLabel;
    //[SerializeField] public TextMeshProUGUI buffTimeLabel;
    [SerializeField] public GameObject popUpPanel;
    [SerializeField] public TextMeshProUGUI buffNameLabel;
    [SerializeField] public TextMeshProUGUI buffDescriptionLabel;
    public string buffName;

    private BuffDebuff _buffDebuff;

    private void OnEnable()
    {
        CombatBehavior.OnUpdateUI += UpdateUI;
    }
    private void OnDisable()
    {
        CombatBehavior.OnUpdateUI -= UpdateUI;
    }

    public void InitBuffDebuff (BuffDebuff buffDebuff)
    {
        _buffDebuff = buffDebuff;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("enter");
        popUpPanel.SetActive(true);
        if (!IsFullyVisibleFrom(popUpPanel.GetComponent<RectTransform>()))
        {
            while (!IsFullyVisibleFrom(popUpPanel.GetComponent<RectTransform>()))
                popUpPanel.transform.Translate(-1, 0, 0);
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
        //Debug.Log("exit");
        popUpPanel.SetActive(false);
    }
    public void UpdateUI()
    {
        Debug.Log("UpdateBUFFDescription");
        buffDescriptionLabel.text = TradManager.instance.GetTranslation(_buffDebuff.idTradDescription, "Missing description");
    }

}
