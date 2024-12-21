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
    [Space]
    [SerializeField]
    private Sprite _endingDialogFrame;
    [SerializeField]
    private Sprite _endingDialogBG;

    [Header("Dialog references for multi options")]
    [SerializeField]
    private GameObject _mainTextGO;
    [SerializeField]
    private List<GameObject> _reponseGO;
    [SerializeField]
    private GameObject _endDialogue;
    [SerializeField]
    private List<GameObject> _clairvContentListGO;
    [Header("Dialog references for ending option")]
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
    public GameObject MainTextGO { get => (_numberAnswer != 0) ? _mainTextGO : _mainTextOneGO; }
    public List<GameObject> Reponse { get => (_numberAnswer != 0) ? _reponseGO : _reponseOneGO; }
    public List<TMP_Text> ReponseText { get => _reponseTextList; }
    public GameObject EndDialog { get => (_numberAnswer != 0) ? _endDialogue : _endOneDialogue; }
    public TMP_Text MainText { get => _mainText; }
    public TMP_Text EndText { get => _endText; }
    public List<GameObject> ClairvContentListGO { get => _clairvContentListGO; }

    private int _numberAnswer = -1;


    
    void Awake()
    {
        SwitchNumberOfAnswer(2);   
    }

    private void SetReference()
    {
        if (_numberAnswer != 0)
        {
            _reponseTextList.Clear();
            for (int i = 0; i < _reponseGO.Count; i++)
            {
                _reponseTextList.Add(_reponseGO[i].GetComponentInChildren<TMP_Text>(true));
            }
            _mainText = MainTextGO.GetComponent<TMP_Text>();
            _endText = EndDialog.GetComponentInChildren<TMP_Text>(true);
        }
        else // number of answer = 0
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
            case 0:
                _dialogOneOption.SetActive(true);
                _dialogTwoOrThreeOptions.SetActive(false);
                _dialogBackgroundGO.GetComponent<Image>().sprite = _endingDialogBG;
                _dialogFrameGO.GetComponent<Image>().sprite = _endingDialogFrame;
                break;
            case 1:
                _dialogOneOption.SetActive(false);
                _dialogTwoOrThreeOptions.SetActive(true);
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
