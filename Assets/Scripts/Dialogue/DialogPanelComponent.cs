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
    private GameObject _dialogTwoOptions;
    [SerializeField]
    private GameObject _dialogThreeOptions;
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
    private List<GameObject> _reponseThreeGO;
    [SerializeField]
    private List<GameObject> _reponseTwoGO;
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
    [Header("Dialog references for ClairvoyancePanel")]
    [SerializeField]
    private List<HideClairvoyance> _clairvoyancePanels;
    
    [Header("Panel for clairvoyance hints")]
    [SerializeField]
    private GameObject _clairvoyanceHintsPanel;
    [SerializeField]
    private TMP_Text _clairvoyanceHintsText;

    private List<TMP_Text> _reponseTextList = new List<TMP_Text>();
    private TMP_Text _mainText;
    private TMP_Text _endText;
    
    public GameObject DialogFrame { get => _dialogFrameGO; }
    public GameObject DialogBG { get => _dialogBackgroundGO; }
    public GameObject MainTextGO { get => (_numberAnswer != 0) ? _mainTextGO : _mainTextOneGO; }
    public List<GameObject> Reponse 
    { 
        get
        {
            if (_numberAnswer > 1)
            {
                if (_numberAnswer == 3)
                    return _reponseThreeGO;
                else
                    return _reponseTwoGO;
            }
            else
                return _reponseOneGO;
        }
    }
    public List<TMP_Text> ReponseText { get => _reponseTextList; }
    public GameObject EndDialog { get => (_numberAnswer != 0) ? _endDialogue : _endOneDialogue; }
    public TMP_Text MainText { get => _mainText; }
    public TMP_Text EndText { get => _endText; }
    public List<GameObject> ClairvContentListGO { get => _clairvContentListGO; }
    public List<HideClairvoyance> ClairvoyancePanels{ get => _clairvoyancePanels; }


    private int _numberAnswer = -1;


    
    void Awake()
    {
        SwitchNumberOfAnswer(2);   
    }
    private void OnAnimatorIK(int layerIndex)
    {
        _clairvoyanceHintsPanel.SetActive(false);
    }

    private void SetReference()
    {
        if (_numberAnswer != 0)
        {
            _reponseTextList.Clear();
            if (_numberAnswer == 3)
            {
                for (int i = 0; i < _reponseThreeGO.Count; i++)
                {
                    _reponseTextList.Add(_reponseThreeGO[i].GetComponentInChildren<TMP_Text>(true));
                }
                _mainText = MainTextGO.GetComponent<TMP_Text>();
                _endText = EndDialog.GetComponentInChildren<TMP_Text>(true);
            }
            else if (_numberAnswer == 2)
            {
                for (int i = 0; i < _reponseTwoGO.Count; i++)
                {
                    _reponseTextList.Add(_reponseTwoGO[i].GetComponentInChildren<TMP_Text>(true));
                }
                _mainText = MainTextGO.GetComponent<TMP_Text>();
                _endText = EndDialog.GetComponentInChildren<TMP_Text>(true);
            }
            else if (_numberAnswer == 1)
            {
                _reponseTextList.Add(_reponseOneGO[0].GetComponentInChildren<TMP_Text>(true));
                _mainText = MainTextGO.GetComponent<TMP_Text>();
                _endText = EndDialog.GetComponentInChildren<TMP_Text>(true);
            }
        }
        else // number of answer = 0
        {
            _reponseTextList.Clear();
            _reponseTextList.Add(_reponseOneGO[0].GetComponentInChildren<TMP_Text>());
            _mainText = MainTextGO.GetComponent<TMP_Text>();
            _endText = EndDialog.GetComponentInChildren<TMP_Text>();
        }
        
    }
    public void HideClairvoyancePanel()
    {
        foreach (var panel in _clairvoyancePanels)
        {
            panel.DisablePanel();
        }
    }

    public void SwitchNumberOfAnswer(int numberOfAnswer)
    {
        _numberAnswer = numberOfAnswer;
        SetReference();
        switch (numberOfAnswer)
        {
            case 0:
            case 1:
                _dialogOneOption.SetActive(true);
                _dialogTwoOptions.SetActive(false);
                _dialogThreeOptions.SetActive(false);
                _dialogBackgroundGO.GetComponent<Image>().sprite = _oneAnswerDialogBG;
                _dialogFrameGO.GetComponent<Image>().sprite = _oneAnswerDialogFrame;
                break;
            case 3:
                _dialogOneOption.SetActive(false);
                _dialogTwoOptions.SetActive(false);
                _dialogThreeOptions.SetActive(true);
                _dialogBackgroundGO.GetComponent<Image>().sprite = _threeAnswerDialogBG;
                _dialogFrameGO.GetComponent<Image>().sprite = _threeAnswerDialogFrame;
                break;
            default:
            case 2:
                _dialogOneOption.SetActive(false);
                _dialogTwoOptions.SetActive(true);
                _dialogThreeOptions.SetActive(false);
                _dialogBackgroundGO.GetComponent<Image>().sprite = _twoAnswerDialogBG;
                _dialogFrameGO.GetComponent<Image>().sprite = _twoAnswerDialogFrame;
                break;
        }

        for (int i=0; i< _clairvoyancePanels.Count;i++)
        {
            _clairvoyancePanels[i].SetPanel(numberOfAnswer);
        }
    }
    public void ShowPanel(int selectedPanel)
    {
        _clairvoyanceHintsPanel.SetActive(true);
        _clairvoyanceHintsText.text = ClairvContentListGO[selectedPanel].GetComponent<ClairvoyancePanel>().PrintListOfEffect();
    }
    public void HidePanel()
    {
        _clairvoyanceHintsPanel.SetActive(false);
    }

}
