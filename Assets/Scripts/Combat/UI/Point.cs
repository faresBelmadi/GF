using UnityEngine;

public abstract class Point : MonoBehaviour
{
    [SerializeField]
    protected GameObject _highlightPoint;
    [SerializeField]
    protected PulseBloom_System _bloomVolonteComponent;

    public void Highlight()
    {
        _highlightPoint.SetActive(true);
        _bloomVolonteComponent.TriggerBloom(true);
    }
    public void StopHighlight()
    {
        _highlightPoint.SetActive(false);
    }

    public abstract void Empty();

    public abstract void Full();

}