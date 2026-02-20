using TMPro;
using UnityEngine;

public class ShortHoverInfo : MonoBehaviour
{
    public int SecondsBeforeDetailed = 3;
    private float _timeHovering = 0f;

    public DetailedHoverInfo DetailedInfo;
    private IHoverInfoContent _content;

    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI ShortDescriptionText;

    public void UpdateContent(IHoverInfoContent content)
    {
        _content = content;
        _timeHovering = 0f;

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

    #region MonoBehaviour
    public void Update()
    {
        _timeHovering += Time.deltaTime;
        if (_timeHovering > SecondsBeforeDetailed)
        {
            DetailedInfo.gameObject.SetActive(true);
            DetailedInfo.UpdateContent(null);
            gameObject.SetActive(false);
        }
    }
    #endregion
}
