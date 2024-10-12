using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanelComponent : MonoBehaviour
{
    [Header("Dialogue frame options")]
    [SerializeField]
    private GameObject _dialogFrameGO;
    [SerializeField]
    private GameObject _dialogBackgroundGO;
    [SerializeField]
    private Sprite _twoAnswerDialogFrame;
    [SerializeField]
    private Sprite _twoAnswerDialogBG;
    [SerializeField]
    private Sprite _threeAnswerDialogFrame;
    [SerializeField]
    private Sprite _threeAnswerDialogBG;

    [Header("Dialog references")]
    [SerializeField]
    private GameObject _mainTextGO;
    [SerializeField]
    private List<GameObject> _reponseGO;
    [SerializeField]
    private GameObject _endDialogue;

    private List<TMP_Text> _reponseTextList = new List<TMP_Text>();
    private TMP_Text _mainText;
    private TMP_Text _endText;
    
    public GameObject DialogFrame { get => _dialogFrameGO; }
    public GameObject DialogBG { get => _dialogBackgroundGO; }
    public GameObject MainTextGO { get => _mainTextGO; }
    public List<GameObject> Reponse { get => _reponseGO; }
    public List<TMP_Text> ReponseText { get => _reponseTextList; }
    public GameObject EndDialog { get => _endDialogue; }
    public TMP_Text MainText { get => _mainText; }
    public TMP_Text EndText { get => _endText; }




    
    void Awake()
    {
        //on cache les TMP pour eviter des pertes de perf
        for (int i = 0; i < _reponseGO.Count; i++)
        {
            _reponseTextList.Add(_reponseGO[i].GetComponentInChildren<TMP_Text>());
        }
        _mainText = MainTextGO.GetComponent<TMP_Text>();
        _endText = EndDialog.GetComponent<TMP_Text>();

        SwitchNumberOfAnswer(2);
    }

    public void SwitchNumberOfAnswer(int numberOfAnswer)
    {
        switch (numberOfAnswer)
        {
            case 3:
                _dialogBackgroundGO.GetComponent<Image>().sprite = _threeAnswerDialogBG;
                _dialogFrameGO.GetComponent<Image>().sprite = _threeAnswerDialogFrame;
                break;
            default:
            case 2:
                _dialogBackgroundGO.GetComponent<Image>().sprite = _twoAnswerDialogBG;
                _dialogFrameGO.GetComponent<Image>().sprite = _twoAnswerDialogFrame;
                break;

        }
    }

}
