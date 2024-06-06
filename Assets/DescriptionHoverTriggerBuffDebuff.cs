using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DescriptionHoverTriggerBuffDebuff : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ToShow;
    public TextMeshProUGUI Description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
        ToShow.SetActive(true);
        if (!IsFullyVisibleFrom(ToShow.GetComponent<RectTransform>()))
        {
            while (!IsFullyVisibleFrom(ToShow.GetComponent<RectTransform>()))
                ToShow.transform.Translate(-1,0,0);
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
        ToShow.SetActive(false);
    }

    public void ShowDescription()
    {
        //var rectTransform = this.GetComponent<RectTransform>();
        //var mousePosition = Input.mousePosition;
        //var normalizedMousePosition = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
        //if (normalizedMousePosition.x > rectTransform.anchorMin.x &&
        //    normalizedMousePosition.x < rectTransform.anchorMax.x &&
        //    normalizedMousePosition.y > rectTransform.anchorMin.y &&
        //    normalizedMousePosition.y < rectTransform.anchorMax.y)
        //{
        //    ToShow.SetActive(true);
        //}

        Ray ray = GameManager.instance.pmm.CurrentRoomCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit[] t = Physics.RaycastAll(ray);
        foreach (var item in t)
        {
            Debug.Log(item.collider.gameObject.name);
        }
        
    
    }

    public void HideDescription()
    {
        ToShow.SetActive(false);
    }
}
