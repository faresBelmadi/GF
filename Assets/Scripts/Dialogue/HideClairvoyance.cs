using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DialogPosition
{
    One,
    Two,
    Three
}
[RequireComponent(typeof(Image))]
public class HideClairvoyance : MonoBehaviour
{
    [SerializeField]
    private DialogPosition _position;
    [SerializeField]
    private float _duration = 1f;
    [SerializeField]
    private Sprite _panelFor3Anwsers;
    [SerializeField]
    private Sprite _panelFor2Anwsers;
    [SerializeField]
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetPanel(int numberOfReponse)
    {
        _image.enabled = true;
        _image.sprite = numberOfReponse switch
        {
            3 => _panelFor3Anwsers,
            2 => _panelFor2Anwsers,
            _ => null
        };
        if (_panelFor2Anwsers == null & numberOfReponse == 2)
        {
            _image.enabled = false;
        }
        if (numberOfReponse == 0 || numberOfReponse == 1)
        {
            _image.enabled = false;
        }
    }
    public void DisablePanel()
    {
        _image.enabled = false;
    
    }
    public void Hide()
    {
        _image.enabled = true;
    }
    public void Show()
    {
        StartCoroutine(ShowPanel());
    }

    public IEnumerator ShowPanel()
    {
        _image.enabled = true;

        float elapsedTime = 0;
        float startValue = 1f;
        float endValue = 0f;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, startValue);
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / _duration);
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, newAlpha);
            yield return null;
        }

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, endValue);
    }



}
