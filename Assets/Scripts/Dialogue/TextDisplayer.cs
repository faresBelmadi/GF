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
    
    private TMP_Text _tmpText;

    public event Action OnDisplayAnimFinish;
    private void OnEnable()
    {
        _tmpText = GetComponent<TMP_Text>();
        
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }

    private IEnumerator AnimText(TextAnimationType type)
    {
        _tmpText.ForceMeshUpdate();
        while (!DisplayNextElement(type))
        {
            yield return new WaitForSeconds(_delay);
        }
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
                return _tmpText.textInfo.characterCount <= _tmpText.maxVisibleCharacters;
            default:
                return true;
        }
    }
}
