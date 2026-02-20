using UnityEngine;

public class DetailedHoverInfo : MonoBehaviour
{
    public void UpdateContent(IHoverInfoContent content)
    {
        // TODO place on trigger item
        PlaceAtMouse();
    }

    private void PlaceAtMouse()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            Input.mousePosition,
            FindAnyObjectByType<Camera>(),
            out localPoint
        );
        transform.localPosition = localPoint;
    }
}
