using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextDisplayer : MonoBehaviour
{
    #region enum
    public enum TextAnimationType
    {
        LetterByLetter,
        LineByLine,
        WordByWord
    }
    #endregion


    [SerializeField]
    private TextAnimationType _type;
    [SerializeField]
    private float _delay = 0.05f;
    [SerializeField]
    private float _skipDelay = 0.01f;
    [SerializeField]
    private Color _color;

    private TMP_Text _tmpText;
    private bool _isAnimated = false;
    private float _baseDelay;

    private string _initialText;
    public event Action OnDisplayAnimFinish;
    private void OnEnable()
    {
        _tmpText = GetComponent<TMP_Text>();
        _baseDelay = _delay;
        _initialText = _tmpText.text;
        switch (_type)
        {
            case TextAnimationType.WordByWord:
                _tmpText.maxVisibleWords = 0;
                break;
            case TextAnimationType.LetterByLetter:
                _tmpText.maxVisibleCharacters = 0;
                break;
            case TextAnimationType.LineByLine:
                _tmpText.maxVisibleLines = 0;
                break;
        }
        StartCoroutine(AnimText(_type));
    }
    private void OnDisable()
    {
        _tmpText.enableAutoSizing = false;
        _delay = _baseDelay;
    }

    private void Update()
    {
        if (_isAnimated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _delay = _skipDelay;
            }
        }
    }
    private IEnumerator AnimText(TextAnimationType type)
    {
        _isAnimated = true;
        _tmpText.ForceMeshUpdate();
        //if (_tmpText.isTextOverflowing)
        //{
        //    _tmpText.enableAutoSizing = true;
        //}
      
        while (!DisplayNextElement(type))
        {
            yield return new WaitForSeconds(_delay);
        }
        // Pour eviter que l'element de text se retrouve malencontreusement tronqué plus tard
        _tmpText.maxVisibleWords = 99999;
        _tmpText.maxVisibleLines = 99999;
        _tmpText.maxVisibleCharacters = 99999;
        _isAnimated = false;
        OnDisplayAnimFinish?.Invoke();
        
    }
    /// <summary>
    /// Affiche l'element suivant de l'animation, renvoie True lorsque c'est finit
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private bool DisplayNextElement (TextAnimationType type)
    {
        switch (type)
        {
            case TextAnimationType.WordByWord:
                _tmpText.maxVisibleWords += 1;
                return _tmpText.textInfo.wordCount <= _tmpText.maxVisibleWords;
            case TextAnimationType.LineByLine:
                _tmpText.maxVisibleLines += 1;
                return _tmpText.textInfo.lineCount <= _tmpText.maxVisibleLines;
            case TextAnimationType.LetterByLetter:
                _tmpText.maxVisibleCharacters += 1;
                if (_tmpText.maxVisibleCharacters >= _initialText.Length)
                {
                    _tmpText.text = _initialText;
                    return true;
                }
                if (_tmpText.maxVisibleCharacters > 1)
                _tmpText.text = $"<line-height=100%>{_initialText.Substring(0, _tmpText.maxVisibleCharacters - 1)}<b><color=#{ColorUtility.ToHtmlStringRGB(_color)}>{_initialText[_tmpText.maxVisibleCharacters]}</color></b>{_initialText.Substring(_tmpText.maxVisibleCharacters)}";
                Debug.Log(_initialText);
                return _tmpText.textInfo.characterCount <= _tmpText.maxVisibleCharacters;
            default:
                return true;
        }
    }

    private void Skip()
    {
        switch (_type)
        {
            case TextAnimationType.WordByWord:
                _tmpText.maxVisibleWords = 99999;
                break;
            case TextAnimationType.LineByLine:
                _tmpText.maxVisibleLines = 99999;
                break;
            case TextAnimationType.LetterByLetter:
                _tmpText.maxVisibleCharacters = 99999;
                break;
        }
    }
}
