using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInfoTrigger : MonoBehaviour, IPointerEnterHandler
{
    public GameObject Content;
    private IHoverInfoContent _content;

    public void Start()
    {
        _content = Content.GetComponent<IHoverInfoContent>();
        if (_content == null)
            Debug.LogError($"Referenced {nameof(Content)} doesn't have any {nameof(IHoverInfoContent)} component", gameObject);
    }

    #region IPointerEnterHandler
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Content == null)
            return;

        HoverInfo.Instance.ShowContent(_content);
    }
    #endregion
}
