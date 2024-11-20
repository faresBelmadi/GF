using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor : MonoBehaviour
{
    [SerializeField]
    private float _fadeDuration = 1f;

    private LineRenderer _lineRenderer;

    private void OnEnable()
    {
        PlayerMapManager.OnShowMap += FadeIn;
        GameManager.OnHideMap += FadeOut;
    }
    private void OnDisable()
    {
        PlayerMapManager.OnShowMap -= FadeIn;
        GameManager.OnHideMap -= FadeOut;
    }

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

   
    private void FadeIn()
    {
        StartCoroutine(Fade(false));
    }
    private void FadeOut()
    {
        StartCoroutine(Fade(true));
    }
    private IEnumerator Fade(bool isFadeOut)
    {
        if (!isFadeOut) yield return new WaitForSeconds(1.25f);
        Color startColor = _lineRenderer.startColor;
        Color endColor = _lineRenderer.endColor;
        float timer = 0f;
        float startingAlpha = isFadeOut ? 1f : 0f;
        float targetAlpha = isFadeOut ? 0f : 1f;
        while (timer < _fadeDuration)
        {

            startColor.a = Mathf.Lerp(startingAlpha, targetAlpha, timer);
            endColor.a = Mathf.Lerp(startingAlpha, targetAlpha, timer);
            _lineRenderer.startColor = startColor;
            _lineRenderer.endColor = endColor;
            timer += Time.deltaTime;
            yield return null;
        }

        startColor.a = targetAlpha;
        endColor.a = targetAlpha;
        _lineRenderer.startColor = startColor;
        _lineRenderer.endColor = endColor;
    }
}
