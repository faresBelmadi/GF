using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float _startingSpeed = 0;
    [SerializeField]
    private float _selectedSpeed = 0;
    [SerializeField]
    private float _scaleUpDuration = 2;
    [SerializeField]
    private Color _activeColor;
    [SerializeField]
    private Button _playButton;

    private float _rotationSpeed;
    private bool _rotate;
    [SerializeField]
    private SpriteRenderer _spriteRender;
    [SerializeField]
    private SpriteRenderer _portalRenderer;
    [SerializeField]
    private SpriteRenderer _cloudRenderer;
    [SerializeField]
    private SpriteRenderer _runesRenderer;
    [SerializeField]
    private SpriteRenderer _particleOffRenderer;
    [SerializeField]
    private SpriteRenderer _particleOnRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _runesRenderer.gameObject.SetActive(false);
        _rotate = true;
        _rotationSpeed = _startingSpeed;
        //_runesRenderer.material.color = new Color(_runesRenderer.material.color.r, _runesRenderer.material.color.g, _runesRenderer.material.color.b, 0);
        _particleOnRenderer.material.color = new Color(_particleOnRenderer.material.color.r, _particleOnRenderer.material.color.g, _particleOnRenderer.material.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotate)
            transform.Rotate(0f, 0f, _rotationSpeed);
    }
    public void Init()
    {
        _rotationSpeed = _startingSpeed;

        _playButton.gameObject.SetActive(false);
        _runesRenderer.gameObject.SetActive(false);

        _spriteRender.material.color = new Color(_spriteRender.material.color.r, _spriteRender.material.color.g, _spriteRender.material.color.b, 1);
        _cloudRenderer.material.color = new Color(_cloudRenderer.material.color.r, _cloudRenderer.material.color.g, _cloudRenderer.material.color.b, 1);
        _portalRenderer.material.color = new Color(_portalRenderer.material.color.r, _portalRenderer.material.color.g, _portalRenderer.material.color.b, 1);
        _particleOffRenderer.material.color = new Color(_particleOffRenderer.material.color.r, _particleOffRenderer.material.color.g, _particleOffRenderer.material.color.b, 1);
        _particleOnRenderer.material.color = new Color(_particleOnRenderer.material.color.r, _particleOnRenderer.material.color.g, _particleOnRenderer.material.color.b, 0);
    }
    public void StartRotate()
    {
        StartCoroutine(SpeedUpRotation());
        StartCoroutine(FadeImage());
        _playButton.gameObject.SetActive(true);
        _rotate = true;
    }
    public void StopRotate()
    {
        _rotationSpeed = _startingSpeed;
        _playButton.gameObject.SetActive(false);
    }

    private IEnumerator SpeedUpRotation()
    {
        Debug.Log("Speed");
        float time = 0;
        while (time < _scaleUpDuration)
        {
           _rotationSpeed = Mathf.Lerp(_startingSpeed, _selectedSpeed, time/_scaleUpDuration);
            time += Time.deltaTime;
            yield return null;
        }
        _rotationSpeed = _selectedSpeed;
    }
    private IEnumerator ChangeColor()
    {
        Debug.Log("Color)");
        float time = 0;
        while (time < _scaleUpDuration)
        {
            _spriteRender.material.color = Color.Lerp(Color.white, _activeColor, time / _scaleUpDuration);
            time += Time.deltaTime;
            yield return null;
        }
        _spriteRender.material.color = _activeColor;
    }
    private IEnumerator FadeImage()
    {
        
        float time = 0;
        Color initialCOlor = _spriteRender.material.color;
        //Color initialRuneCOlor = _runesRenderer.material.color;
        Color initialCloudCOlor = _cloudRenderer.material.color;
        Color initialPortalCOlor = _portalRenderer.material.color;
        Color initialParticleCOlor = _particleOffRenderer.material.color;
        while (time < _scaleUpDuration)
        {
            float alpha = Mathf.Lerp(1, 0, time / _scaleUpDuration);
            float alphaInvert = Mathf.Lerp(0,1, time / _scaleUpDuration);
            _spriteRender.material.color = new Color(initialCOlor.r, initialCOlor.g, initialCOlor.b, alpha);
            //_runesRenderer.material.color = new Color(initialRuneCOlor.r, initialRuneCOlor.g, initialRuneCOlor.b, alphaInvert);
            _cloudRenderer.material.color = new Color(initialCloudCOlor.r, initialCloudCOlor.g, initialCloudCOlor.b, alpha);
            _portalRenderer.material.color = new Color(initialPortalCOlor.r, initialPortalCOlor.g, initialPortalCOlor.b, alpha);
            _particleOffRenderer.material.color = new Color(initialParticleCOlor.r, initialParticleCOlor.g, initialParticleCOlor.b, alpha);
            _particleOnRenderer.material.color = new Color(initialParticleCOlor.r, initialParticleCOlor.g, initialParticleCOlor.b, alphaInvert);
            time += Time.deltaTime;
            yield return null;
        }
        _spriteRender.material.color = new Color(initialCOlor.r, initialCOlor.g, initialCOlor.b, 0);
       // _runesRenderer.material.color = new Color(initialRuneCOlor.r, initialRuneCOlor.g, initialRuneCOlor.b, 1);
        _cloudRenderer.material.color = new Color(initialCloudCOlor.r, initialCloudCOlor.g, initialCloudCOlor.b, 0);
        _portalRenderer.material.color = new Color(initialPortalCOlor.r, initialPortalCOlor.g, initialPortalCOlor.b, 0);
        _particleOffRenderer.material.color = new Color(initialParticleCOlor.r, initialParticleCOlor.g, initialParticleCOlor.b, 0);
        _particleOnRenderer.material.color = new Color(initialParticleCOlor.r, initialParticleCOlor.g, initialParticleCOlor.b, 1);
    }
    private IEnumerator ScaleUp()
    {
        Debug.Log("Scale");
        float time = 0;
        while (time < _scaleUpDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / _scaleUpDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        Debug.Log("Button Ex");
        transform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("button en");
        transform.localScale = Vector3.one * 1.1f;
    }
}
