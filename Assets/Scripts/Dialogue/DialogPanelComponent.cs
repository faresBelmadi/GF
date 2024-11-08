using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanelComponent : MonoBehaviour
{
    [Header("Dialogue frame options")]
    [SerializeField]
    private GameObject _dialogOneOption;
    [SerializeField]
    private GameObject _dialogTwoOrThreeOptions;
    [SerializeField]
    private GameObject _dialogFrameGO;
    [SerializeField]
    private GameObject _dialogBackgroundGO;
    [SerializeField]
    private Sprite _oneAnswerDialogFrame;
    [SerializeField]
    private Sprite _oneAnswerDialogBG;
    [SerializeField]
    private Sprite _twoAnswerDialogFrame;
    [SerializeField]
    private Sprite _twoAnswerDialogBG;
    [SerializeField]
    private Sprite _threeAnswerDialogFrame;
    [SerializeField]
    private Sprite _threeAnswerDialogBG;

    [Header("Dialog references for 2 & 3 options")]
    [SerializeField]
    private GameObject _mainTextGO;
    [SerializeField]
    private List<GameObject> _reponseGO;
    [SerializeField]
    private GameObject _endDialogue;
    [Header("Dialog references for unique option")]
    [SerializeField]
    private GameObject _mainTextOneGO;
    [SerializeField]
    private List<GameObject> _reponseOneGO;
    [SerializeField]
    private GameObject _endOneDialogue;

    private List<TMP_Text> _reponseTextList = new List<TMP_Text>();
    private TMP_Text _mainText;
    private TMP_Text _endText;
    
    public GameObject DialogFrame { get => _dialogFrameGO; }
    public GameObject DialogBG { get => _dialogBackgroundGO; }
    public GameObject MainTextGO { get => (_numberAnswer != 1) ? _mainTextGO : _mainTextOneGO; }
    public List<GameObject> Reponse { get => (_numberAnswer != 1) ? _reponseGO : _reponseOneGO; }
    public List<TMP_Text> ReponseText { get => _reponseTextList; }
    public GameObject EndDialog { get => (_numberAnswer != 1) ? _endDialogue : _endOneDialogue; }
    public TMP_Text MainText { get => _mainText; }
    public TMP_Text EndText { get => _endText; }

    private int _numberAnswer = 0;


    
    void Awake()
    {
        SwitchNumberOfAnswer(2);   
    }

    private void SetReference()
    {
        if (_numberAnswer != 1)
        {
            _reponseTextList.Clear();
            for (int i = 0; i < _reponseGO.Count; i++)
            {
                _reponseTextList.Add(_reponseGO[i].GetComponentInChildren<TMP_Text>(true));
            }
            _mainText = MainTextGO.GetComponent<TMP_Text>();
            _endText = EndDialog.GetComponentInChildren<TMP_Text>(true);
        }
        else // number of answer = 1
        {
            _reponseTextList.Clear();
            _reponseTextList.Add(_reponseOneGO[0].GetComponentInChildren<TMP_Text>());
            _mainText = MainTextGO.GetComponent<TMP_Text>();
            _endText = EndDialog.GetComponent<TMP_Text>();
        }
        
    }

    public void SwitchNumberOfAnswer(int numberOfAnswer)
    {
        _numberAnswer = numberOfAnswer;
        SetReference();
        switch (numberOfAnswer)
        {
            case 1:
                _dialogOneOption.SetActive(true);
                _dialogTwoOrThreeOptions.SetActive(false);
                _dialogBackgroundGO.GetComponent<Image>().sprite = _oneAnswerDialogBG;
                _dialogFrameGO.GetComponent<Image>().sprite = _oneAnswerDialogFrame;
                break;
            case 3:
                _dialogOneOption.SetActive(false);
                _dialogTwoOrThreeOptions.SetActive(true);
                _dialogBackgroundGO.GetComponent<Image>().sprite = _threeAnswerDialogBG;
                _dialogFrameGO.GetComponent<Image>().sprite = _threeAnswerDialogFrame;
                break;
            default:
            case 2:
                _dialogOneOption.SetActive(false);
                _dialogTwoOrThreeOptions.SetActive(true);
                _dialogBackgroundGO.GetComponent<Image>().sprite = _twoAnswerDialogBG;
                _dialogFrameGO.GetComponent<Image>().sprite = _twoAnswerDialogFrame;
                break;
        }
    }

}
