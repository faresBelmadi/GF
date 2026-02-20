using UnityEngine;

public class HoverInfo : MonoBehaviour
{
    public static HoverInfo Instance { get; private set; }

    public ShortHoverInfo ShortInfo;
    public DetailedHoverInfo DetailedInfo;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's already an instance of {nameof(HoverInfo)}!", gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowContent(IHoverInfoContent content)
    {
        DetailedInfo.gameObject.SetActive(false);
        ShortInfo.gameObject.SetActive(true);
        ShortInfo.UpdateContent(content);
    }
}