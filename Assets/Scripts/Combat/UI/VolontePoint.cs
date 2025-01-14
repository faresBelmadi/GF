using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolontePoint : MonoBehaviour
{
    [SerializeField]
    private Image _volonteSprite;
    [SerializeField]
    private Sprite _volonteFull;
    [SerializeField]
    private Sprite _volonteEmpty;
    [SerializeField]
    private GameObject _highlightPoint;
    [SerializeField]
    private PulseBloom_System _bloomVolonteComponent;



    // Start is called before the first frame update
    void Start()
    {
        _volonteSprite.sprite = _volonteFull;
    }

    public void Full()
    {
        _volonteSprite.sprite = _volonteFull;
    }
    public void Empty()
    {
        _volonteSprite.sprite = _volonteEmpty;
    }
    public void Highlight()
    {
        _highlightPoint.SetActive(true);
        _bloomVolonteComponent.TriggerBloom(true);
    }
    public void StopHighlight()
    {
        _highlightPoint.SetActive(false);
    }
}
