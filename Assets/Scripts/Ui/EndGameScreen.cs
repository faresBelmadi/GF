using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class EndGameScreen : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private Image _textImage;
    [SerializeField]
    private Sprite _englishTextImage;
    [SerializeField]
    private Sprite _frenchTextImage;
    [Space, Header("Buttons")]
    [SerializeField] private Button _continue;
    [SerializeField] private Button _kickstarter;
    [Space, Header("Buttons Image")]
    [Header("English")]
    [SerializeField] private GameObject _englishButtons;
    [SerializeField] private Sprite _engContinue;
    [SerializeField] private Sprite _engKickstarter;
    [Header("French")]
    [SerializeField] private GameObject _frenchButtons;
    [SerializeField] private Sprite _frenchContinue;
    [SerializeField] private Sprite _frenchKickstarter;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration;

    [Header("GameOver")]
    [SerializeField]
    private Image _backGround;
    [SerializeField]
    private Material _screenMaterial;
    [SerializeField]
    private GameObject _cristopherToExpode;

   

    private float currentTime = 0f;

    private void OnEnable()
    {
        switch (TradManager.instance.Language)
        {
            case TradManager.SUPPORTEDLANGUAGES.FR:
                _englishButtons.SetActive(false);
                _frenchButtons.SetActive(true);
                _textImage.sprite = _frenchTextImage;
                _continue.image.sprite = _frenchContinue;
                _kickstarter.image.sprite = _frenchKickstarter;
                break;
            default:
                _englishButtons.SetActive(true);
                _frenchButtons.SetActive(false);
                _textImage.sprite = _englishTextImage;
                _continue.image.sprite = _engContinue;
                _kickstarter.image.sprite = _engKickstarter;
                break;
        }
        //StartCoroutine(FadeIn());
        DissolveOffScreen();
    }

    private IEnumerator FadeIn()
    {
        currentTime = 0f;
        while (currentTime < _fadeDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(0f,1f, currentTime / _fadeDuration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 1f;
    }
   
    private void DissolveOffScreen()
    {
        _backGround.material = new Material(_screenMaterial);
        StartCoroutine(DissolveOnTIme());
    }
    private IEnumerator DissolveOnTIme()
    {
        float currentTime = 0f;
        while(currentTime < 2f)
        {
            float value = Mathf.Lerp(1f, 0f, currentTime / 2f);
            _backGround.material.SetFloat("_DisolveHeight", value);
            currentTime += Time.deltaTime;
            yield return null;
        }
        _backGround.material.SetFloat("_DisolveHeight", 0);

        _cristopherToExpode.SetActive(true);

        _cristopherToExpode.GetComponent<ExplodeCristopher>().DestroyCristopher();
        yield return new WaitForSeconds(3f);
        currentTime = 0f;
        while (currentTime < _fadeDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, currentTime / _fadeDuration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 1f;
    }
}
