using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    #region UI Reference

    public GameObject UIDialogue;

    [Tooltip("Font size for dialogue options")]
    [SerializeField]
    private float _fontSize = 34f;
    [SerializeField]
    private string _idTradDefaultConscience;
    public GameObject UIJoueur;
    [SerializeField]
    private Color _speakerColor;
    [Space]
    [SerializeField]
    protected DialogPanelComponent _dialogPanelComponent;
    //[Header("Dialogue frame options")]
    //[SerializeField]
    //private GameObject _dialogFrameGO;
    //[SerializeField]
    //private GameObject _dialogBackgroundGO;
    //[SerializeField]
    //private Sprite _twoAnswerDialogFrame;
    //[SerializeField]
    //private Sprite _twoAnswerDialogBG;
    //[SerializeField]
    //private Sprite _threeAnswerDialogFrame;
    //[SerializeField]
    //private Sprite _threeAnswerDialogBG;
    [Header("Dialog references")]
    [SerializeField]
    private List<Button> _answerButtons;
    //public TextMeshProUGUI MainText;
    //public GameObject MainTextGO;
    //public List<TextMeshProUGUI> Reponse;
    //public List<GameObject> ReponseGO;
    //public GameObject EndDialogue;
    //public TextMeshProUGUI EndText;
    public BattleManager ManagerBattle;
    [Header("BuffVisualization")]
    [SerializeField]
    private GameObject _buffPrefab;
    [SerializeField]
    private GameObject _effectPrefab;
    [SerializeField]
    private GameObject _buffContainer;
    [Space]
    public AleaManager ManagerAlea;

    private List<GameObject> _listBuffEffectFromDialog = new List<GameObject>();
    private List<GameObject> _listClairvEffect = new List<GameObject>();

    //public Button skipButton; 
    [SerializeField] private ClairvoyanceIconData _clairvoyanceIconData;

    #endregion UI Reference

    #region SO

    private Encounter _CurrentEncounterBattle;
    private EncounterAlea _CurrentEncounterAlea;
    internal DialogueSO _CurrentDialogue;

    #endregion SO

    #region Dialogue Property

    internal int DialogueIndex = 0;
    protected int NextDialogueIndex = 0;
    private Dictionary<ClairvoyanceIconStatEnum, bool> _displayedClairvoyanceStats;


    #endregion Dialogue Property

    private void OnEnable()
    {
        if (_dialogPanelComponent.Reponse.Count >= 1 && _dialogPanelComponent.Reponse[0] != null)
            _dialogPanelComponent.Reponse[0].GetComponentInChildren<TextDisplayer>().OnDisplayAnimFinish += StopSFX;
        if (_dialogPanelComponent.EndText != null)
            _dialogPanelComponent.EndText.GetComponent<TextDisplayer>().OnDisplayAnimFinish += StopSFX;
    }

    private void OnDisable()
    {
        if (_dialogPanelComponent.Reponse.Count >= 1 && _dialogPanelComponent.Reponse[0] != null)
            _dialogPanelComponent.Reponse[0].GetComponentInChildren<TextDisplayer>().OnDisplayAnimFinish -= StopSFX;
        if (_dialogPanelComponent.EndText != null)
            _dialogPanelComponent.EndText.GetComponent<TextDisplayer>().OnDisplayAnimFinish -= StopSFX;
    }

    private void Start()
    {
        TMP_Text[] dialogArray = UIDialogue.GetComponentsInChildren<TMP_Text>(true);
        for (int i = 0; i < dialogArray.Length; i++)
        {
            dialogArray[i].enableAutoSizing = false;
            dialogArray[i].fontSize = _fontSize;
        }
    }

    public virtual void InitDialogOptionButton()
    {
        for (int i = 0; i < _dialogPanelComponent.Reponse.Count; i++)
        {
            _dialogPanelComponent.Reponse[i].GetComponentInChildren<Button>(true).onClick.RemoveAllListeners();
            int answerNum = i;
            _dialogPanelComponent.Reponse[i].GetComponentInChildren<Button>(true).onClick.AddListener(() => GetRéponse(answerNum));
        }
    }

    public void SetupDialogue(Encounter encounterToSet)
    {
        _CurrentDialogue = encounterToSet.DialogueRencontre;
        _CurrentEncounterBattle = encounterToSet;
        UIJoueur.SetActive(false);
        UIDialogue.SetActive(true);
        startDialogue();
    }

    public void SetupDialogue(EncounterAlea encounterToSet)
    {
        _CurrentDialogue = encounterToSet.DialogueRencontre;
        _CurrentEncounterAlea = encounterToSet;
        UIJoueur.SetActive(false);
        UIDialogue.SetActive(true);
        startDialogue();
    }

    void startDialogue()
    {
        resetRéponse();


        GoNext();
    }

    void GoNext()
    {
        AudioManager.instance.SFX.PlaySFXClip(SFXType.DialogueSFX);
        DialogueIndex = NextDialogueIndex;

        // On affiche le panel de dialogue avec le nombre requis de réponse
        if (_CurrentDialogue.Questions[DialogueIndex].Question.type == TypeQuestion.startCombat
            || _CurrentDialogue.Questions[DialogueIndex].Question.type == TypeQuestion.EndAleaDialogue)
        {
            // Dialogue final, on affiche le layout avec le bouton
            _dialogPanelComponent.SwitchNumberOfAnswer(0);
        }
        else
        {
            _dialogPanelComponent.SwitchNumberOfAnswer(_CurrentDialogue.Questions[DialogueIndex].ReponsePossible.Count);
        }
        _dialogPanelComponent.MainText.text = TextePrincipal();
        _dialogPanelComponent.MainTextGO.SetActive(true);
        InitDialogOptionButton();

        TextDisplayer textDisplayer = _dialogPanelComponent.MainText.GetComponent<TextDisplayer>();


        if (textDisplayer != null)
        {
            _dialogPanelComponent.MainText.GetComponent<TextDisplayer>().OnDisplayAnimFinish += GetAnswerList;
        }
        else
        {
            GetAnswerList();
        }
    }

    private void GetAnswerList()
    {
        //test
        foreach (var panel in _dialogPanelComponent.ClairvContentListGO)
        {
            panel.GetComponent<ClairvoyancePanel>().InitClairvoyancePanel();
            
        }
        //end test
        _displayedClairvoyanceStats = new Dictionary<ClairvoyanceIconStatEnum, bool>();

        TextDisplayer textDisplayer = _dialogPanelComponent.MainText.GetComponent<TextDisplayer>();
        if (textDisplayer != null)
        {
            _dialogPanelComponent.MainText.GetComponent<TextDisplayer>().OnDisplayAnimFinish -= GetAnswerList;
        }

        var currentQuestionType = _CurrentDialogue.Questions[DialogueIndex].Question.type;
        var currentPossibleResponseList = _CurrentDialogue.Questions[DialogueIndex].ReponsePossible;



        if (currentQuestionType == TypeQuestion.startCombat ||
            currentQuestionType == TypeQuestion.EndTutoDialogue)
        {
            string DialogueTrad;
            if (!string.IsNullOrEmpty(currentPossibleResponseList[0].IdStringReponse))
            {
                //DialogueTrad =
                //    TradManager.Instance.DialogueDictionary[currentPossibleResponseList[0].IdStringReponse][
                //        TradManager.Instance.IdLanguage];
                DialogueTrad = TradManager.instance.GetTranslation(currentPossibleResponseList[0].IdStringReponse,
                    "ID_DIALOGUE_NOT_IMPLEMENTED");
            }
            else
            {
                DialogueTrad = "ID_DIALOGUE_NOT_IMPLEMENTED";
            }

            if (_dialogPanelComponent.EndText != null)
            {
                //if(EndText.text == null || EndText.text == "")
                if (currentPossibleResponseList != null &&
                    currentPossibleResponseList.Count > 0 &&
                    !string.IsNullOrEmpty(DialogueTrad))
                    _dialogPanelComponent.EndText.text = DialogueTrad;
                else
                {
                    _dialogPanelComponent.EndText.text = "Continuer";
                }
            }
            else
            {
                var Text = _dialogPanelComponent.EndDialog.GetComponentInChildren<TextMeshProUGUI>();
                if (Text != null)
                {
                    Text.text = DialogueTrad;
                }
            }
            _dialogPanelComponent.HideClairvoyancePanel();
            _dialogPanelComponent.EndDialog.SetActive(true);
        }
        else
        {
            for (int i = 0; i < currentPossibleResponseList.Count; i++)
            {
                _dialogPanelComponent.ClairvoyancePanels[i].SetPanel(currentPossibleResponseList.Count);
                Debug.Log("Conscience requise = " + currentPossibleResponseList[i].SeuilConscience + "\n Conscience joueur : " + GameManager.Instance.playerStat.Conscience);
                string response = "";
                if (GameManager.Instance.playerStat.Conscience >= currentPossibleResponseList[i].SeuilConscience)
                {
                    if (!string.IsNullOrEmpty(currentPossibleResponseList[i].IdStringReponse))
                    {
                        //response =
                        //    TradManager.Instance.DialogueDictionary[currentPossibleResponseList[i].IdStringReponse][
                        //        TradManager.Instance.IdLanguage];
                        response = TradManager.instance.GetTranslation(currentPossibleResponseList[i].IdStringReponse,
                            "ID_DIALOGUE_NOT_IMPLEMENTED");
                    }
                    else
                    {
                        response = "ID_DIALOGUE_NOT_IMPLEMENTED";
                    }
                    _dialogPanelComponent.Reponse[i].GetComponentInChildren<TMP_Text>(true).text = response;
                    _dialogPanelComponent.Reponse[i].SetActive(true);
                    //Réponse[i].GetComponent<TextAnimation>().LaunchAnim();
                    if (ManagerBattle.player.Stat.Clairvoyance >= currentPossibleResponseList[i].SeuilClairvoyanceStat)
                    {
                        if (!GameManager.Instance.IsTuto && currentPossibleResponseList.Count > 1)
                            _dialogPanelComponent.ClairvoyancePanels[i].Show();
                        bool[] displayed = new bool[Enum.GetValues(typeof(ClairvoyanceIconStatEnum)).Length];
                        ShowConsequenceForAnswer(i, ref displayed);
                    }
                    else
                    {
                        if (!GameManager.Instance.IsTuto && currentPossibleResponseList.Count > 1)
                            _dialogPanelComponent.ClairvoyancePanels[i].Hide();
                    }
                }
                else
                {
                    _dialogPanelComponent.Reponse[i].GetComponentInChildren<TMP_Text>(true).text = $"<b><i>{TradManager.instance.GetTranslation(_idTradDefaultConscience, "Conscience required")}</i></b>";
                    _dialogPanelComponent.Reponse[i].SetActive(true);
                }
            }
        }
        foreach (var panel in _dialogPanelComponent.ClairvContentListGO)
        {
            string str = panel.GetComponent<ClairvoyancePanel>().PrintListOfEffect();
            Debug.Log(str);
        }
    }

    private string TextePrincipal()
    {
        string dialogueTrad;
        string colorCode = ColorUtility.ToHtmlStringRGB(_speakerColor);
        if (!string.IsNullOrEmpty(_CurrentDialogue.Questions[DialogueIndex].Question.IdStringQuestion))
        {
            //dialogueTrad =
            //    TradManager.Instance.DialogueDictionary[_CurrentDialogue.Questions[DialogueIndex].Question.IdStringQuestion]
            //        [TradManager.Instance.IdLanguage];
            dialogueTrad = TradManager.instance.GetTranslation(
                _CurrentDialogue.Questions[DialogueIndex].Question.IdStringQuestion, "ID_DIALOGUE_NOT_IMPLEMENTED");
        }
        else
        {
            dialogueTrad = "ID_DIALOGUE_NOT_IMPLEMENTED";
        }

        if (/*ManagerBattle == null && _CurrentEncounterAlea != null*/ ManagerAlea.IsAlea)
        {
            return "<allcaps><u><b><color=#" + colorCode + ">" + _CurrentEncounterAlea.NamePnj +
                   ": </color></b></u></allcaps> " + dialogueTrad;
        }
        else
        {
            string encounteurName = TradManager.instance.GetTranslation(
                _CurrentEncounterBattle.ToFight[_CurrentDialogue.Questions[DialogueIndex].Question.IDSpeaker]
                    .IdTradName,
                _CurrentEncounterBattle.ToFight[_CurrentDialogue.Questions[DialogueIndex].Question.IDSpeaker].Nom);
            return "<allcaps><u><b><color=#" + colorCode + ">" + encounteurName + ": </color></b></u></allcaps>" +
                   dialogueTrad;
        }
    }

    void resetRéponse()
    {
        foreach (var item in _dialogPanelComponent.Reponse)
        {
            item.SetActive(false);
        }

        _dialogPanelComponent.EndDialog.SetActive(false);

        _dialogPanelComponent.MainTextGO.SetActive(false);
        foreach (var item in _dialogPanelComponent.ReponseText)
        {
            item.text = "";
        }
        ClearClairvoyanceIcons();
    }

    public virtual void GetRéponse(int i)
    {
        if (GameManager.Instance.IsPaused)
            return;
        if (GameManager.Instance.playerStat.Conscience < _CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].SeuilConscience)
            return;
        if (_CurrentDialogue.Questions[DialogueIndex].Question.type == TypeQuestion.startCombat)
        {
            StartCombat();
        }
        else if (_CurrentDialogue.Questions[DialogueIndex].Question.type == TypeQuestion.EndAleaDialogue)
        {
            Debug.Log("End Dialog Alea");
            EndDialogueFonction();
        }
        else
        {
            if (_CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].conséquences.Count != 0)
            {
                ApplyConsequence(_CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].conséquences);
            }

            NextDialogueIndex = _CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].IDNextQuestion;
            resetRéponse();
            GoNext();
        }
    }
    
    private string BuildSpriteIcon(Effet effet, int selectedAnswer, ref bool[] displayed)
    {
        StringBuilder strb = new StringBuilder();
        strb.Append("<sprite name=\"");
        Color color = Color.white;
        //Debug.Log($"EffectSprite : {effet.TypeEffet}, cible : {effet.Cible}");

        switch (effet.TypeEffet)
        {
            case TypeEffet.AugmentationBrutFA:
            case TypeEffet.AttaqueFADebuff:
                if ((effet.Cible == Cible.joueur && effet.ValeurBrut < 0)
                    || (effet.Cible != Cible.joueur && effet.ValeurBrut > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ForceDameDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ForceDameDown] = true;
                        strb.Append((_clairvoyanceIconData.StatForceDameDown != null)
                            ? _clairvoyanceIconData.StatForceDameDown.name
                            : "FA");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ForceDameUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ForceDameUp] = true;
                        strb.Append((_clairvoyanceIconData.StatForceDameUp != null)
                            ? _clairvoyanceIconData.StatForceDameUp.name
                            : "FA");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "";
                }

                break;
            case TypeEffet.AugmentationPourcentageFACible:
            case TypeEffet.AugmentationPourcentageFA:
                if ((effet.Cible == Cible.joueur && effet.Pourcentage < 0)
                    || (effet.Cible != Cible.joueur && effet.Pourcentage > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ForceDameDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ForceDameDown] = true;
                        strb.Append((_clairvoyanceIconData.StatForceDameDown != null)
                            ? _clairvoyanceIconData.StatForceDameDown.name
                            : "FA");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ForceDameUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ForceDameUp] = true;
                        strb.Append((_clairvoyanceIconData.StatForceDameUp != null)
                            ? _clairvoyanceIconData.StatForceDameUp.name
                            : "FA");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "";
                }

                break;
            case TypeEffet.RadianceMax:
                if ((effet.Cible == Cible.joueur && effet.ValeurBrut < 0)
                    || (effet.Cible != Cible.joueur && effet.ValeurBrut > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.RadianceDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.RadianceDown] = true;
                        strb.Append((_clairvoyanceIconData.StatRadianceDown != null)
                            ? _clairvoyanceIconData.StatRadianceDown.name
                            : "Rad");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.RadianceUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.RadianceUp] = true;
                        strb.Append((_clairvoyanceIconData.StatRadianceUp != null)
                            ? _clairvoyanceIconData.StatRadianceUp.name
                            : "Rad");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "";
                }

                break;
            case TypeEffet.Resilience:
                if ((effet.Cible == Cible.joueur && effet.ValeurBrut < 0)
                    || (effet.Cible != Cible.joueur && effet.ValeurBrut > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ResilienceDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ResilienceDown] = true;
                        strb.Append((_clairvoyanceIconData.StatResilienceDown != null)
                            ? _clairvoyanceIconData.StatResilienceDown.name
                            : "Res");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ResilienceUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ResilienceUp] = true;
                        strb.Append((_clairvoyanceIconData.StatResilienceUp != null)
                            ? _clairvoyanceIconData.StatResilienceUp.name
                            : "Res");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "";
                }

                break;
            case TypeEffet.Clairvoyance:
                if ((effet.Cible == Cible.joueur && effet.ValeurBrut < 0)
                    || (effet.Cible != Cible.joueur && effet.ValeurBrut > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ClairvoyaneDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ClairvoyaneDown] = true;
                        strb.Append((_clairvoyanceIconData.StatClairvoyanceDown != null)
                            ? _clairvoyanceIconData.StatClairvoyanceDown.name
                            : "Cla");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ClairvoyaneUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ClairvoyaneUp] = true;
                        strb.Append((_clairvoyanceIconData.StatClairvoyanceUp != null)
                            ? _clairvoyanceIconData.StatClairvoyanceUp.name
                            : "Cla");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "";
                }

                break;
            case TypeEffet.Vitesse:
                if ((effet.Cible == Cible.joueur && effet.ValeurBrut < 0)
                    || (effet.Cible != Cible.joueur && effet.ValeurBrut > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.VitesseDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.VitesseDown] = true;
                        strb.Append((_clairvoyanceIconData.StatVitesseDown != null)
                            ? _clairvoyanceIconData.StatVitesseDown.name
                            : "Vit");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.VitesseUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.VitesseUp] = true;
                        strb.Append((_clairvoyanceIconData.StatVitesseUp != null)
                            ? _clairvoyanceIconData.StatVitesseUp.name
                            : "Vit");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "";
                }

                break;
            case TypeEffet.Conviction:
                if ((effet.Cible == Cible.joueur && effet.ValeurBrut < 0)
                    || (effet.Cible != Cible.joueur && effet.ValeurBrut > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ConvictionDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ConvictionDown] = true;
                        strb.Append((_clairvoyanceIconData.StatConvictionDown != null)
                            ? _clairvoyanceIconData.StatConvictionDown.name
                            : "Con");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ConvictionUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ConvictionUp] = true;
                        strb.Append((_clairvoyanceIconData.StatConvictionUp != null)
                            ? _clairvoyanceIconData.StatConvictionUp.name
                            : "Con");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "";
                }

                break;
            case TypeEffet.Conscience:
                if ((effet.Cible == Cible.joueur && effet.ValeurBrut < 0)
                    || (effet.Cible != Cible.joueur && effet.ValeurBrut > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ConscienceDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ConscienceDown] = true;
                        strb.Append((_clairvoyanceIconData.StatConscienceDown != null)
                            ? _clairvoyanceIconData.StatConscienceDown.name
                            : "con");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ConscienceUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ConscienceUp] = true;
                        strb.Append((_clairvoyanceIconData.StatConscienceUp != null)
                            ? _clairvoyanceIconData.StatConscienceUp.name
                            : "con");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "";
                }

                break;
            case TypeEffet.DegatsBrutConsequence:
                if ((effet.Cible == Cible.joueur && effet.ValeurBrut < 0)
                    || (effet.Cible != Cible.joueur && effet.ValeurBrut > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.Degats])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.Degats] = true;
                        strb.Append((_clairvoyanceIconData.Damage != null)
                            ? _clairvoyanceIconData.Damage.name
                            : "DMG");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.Degats])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.Degats] = true;
                        strb.Append((_clairvoyanceIconData.Damage != null)
                            ? _clairvoyanceIconData.Damage.name
                            : "DMG");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "";
                }

                break;
            case TypeEffet.Volonte:
            case TypeEffet.VolonteMax:
                if ((effet.Cible == Cible.joueur && effet.ValeurBrut < 0)
                    || (effet.Cible != Cible.joueur && effet.ValeurBrut > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.VolonteDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.VolonteDown] = true;
                        strb.Append((_clairvoyanceIconData.StatVolonteDown != null)
                            ? _clairvoyanceIconData.StatVolonteDown.name
                            : "Vol");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.VolonteUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.VolonteUp] = true;
                        strb.Append((_clairvoyanceIconData.StatVolonteUp != null)
                            ? _clairvoyanceIconData.StatVolonteUp.name
                            : "Vol");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "";
                }

                break;
            case TypeEffet.TensionStep:
            case TypeEffet.TensionValue:
            case TypeEffet.TensionGainAttaqueValue:
            case TypeEffet.TensionGainDebuffValue:
            case TypeEffet.TensionGainSoinValue:
            case TypeEffet.TensionGainDotValue:
                if ((effet.Cible == Cible.joueur && effet.ValeurBrut < 0)
                   || (effet.Cible != Cible.joueur && effet.ValeurBrut > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.TensionDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.TensionDown] = true;
                        strb.Append((_clairvoyanceIconData.StatTensionDown != null)
                            ? _clairvoyanceIconData.StatTensionDown.name
                            : "FA");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.TensionUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.TensionUp] = true;
                        strb.Append((_clairvoyanceIconData.StatTensionUp != null)
                            ? _clairvoyanceIconData.StatTensionUp.name
                            : "TENS");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "TENS";
                }

                break;
            case TypeEffet.DegatsForceAme:

                if (!displayed[(int)ClairvoyanceIconStatEnum.Degats])
                {
                    displayed[(int)ClairvoyanceIconStatEnum.Degats] = true;
                    strb.Append((_clairvoyanceIconData.Damage != null)
                        ? _clairvoyanceIconData.Damage.name
                        : "DMG");
                    AddClairvoyanceIcone(effet, selectedAnswer, true);
                }

                break;
            case TypeEffet.MultiplDegat:
                if ((effet.Cible == Cible.joueur && effet.Pourcentage < 0)
                   || (effet.Cible != Cible.joueur && effet.Pourcentage > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.MultiAtkDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.MultiAtkDown] = true;
                        strb.Append((_clairvoyanceIconData.DecreaseAtk != null)
                            ? _clairvoyanceIconData.DecreaseAtk.name
                            : "MultATK");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.MultiAtkUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.MultiAtkUp] = true;
                        strb.Append((_clairvoyanceIconData.IncreaseAtk != null)
                            ? _clairvoyanceIconData.IncreaseAtk.name
                            : "MultATK");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "MultATK";
                }

                break;
            case TypeEffet.MultiplDef:
                if ((effet.Cible == Cible.joueur && effet.Pourcentage < 0)
                  || (effet.Cible != Cible.joueur && effet.Pourcentage > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.MultiDefDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.MultiDefDown] = true;
                        strb.Append((_clairvoyanceIconData.DecreaseDef != null)
                            ? _clairvoyanceIconData.DecreaseDef.name
                            : "MultDEF");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.MultiDefUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.MultiDefUp] = true;
                        strb.Append((_clairvoyanceIconData.IncreaseDef != null)
                            ? _clairvoyanceIconData.IncreaseDef.name
                            : "MultDEF");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "MultDEF";
                }

                break;
            case TypeEffet.MultiplSoin:
                if ((effet.Cible == Cible.joueur && effet.Pourcentage < 0)
                  || (effet.Cible != Cible.joueur && effet.Pourcentage > 0))
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.MultiHealDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.MultiHealDown] = true;
                        strb.Append((_clairvoyanceIconData.DecreaseHeal != null)
                            ? _clairvoyanceIconData.DecreaseHeal.name
                            : "MultHeal");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.MultiHealUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.MultiHealUp] = true;
                        strb.Append((_clairvoyanceIconData.IncreaseHeal != null)
                            ? _clairvoyanceIconData.IncreaseHeal.name
                            : "MultHeal");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "MultHeal";
                }

                break;
            case TypeEffet.Colere:
                if (effet.Cible == Cible.joueur)
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ColereDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ColereDown] = true;
                        strb.Append((_clairvoyanceIconData.WrathDown != null)
                            ? _clairvoyanceIconData.WrathDown.name
                            : "WrathDown");
                        AddClairvoyanceIcone(effet, selectedAnswer, false);
                    }
                    else return "WrathDown";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ColereUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ColereUp] = true;
                        strb.Append((_clairvoyanceIconData.WrathUp != null)
                            ? _clairvoyanceIconData.WrathUp.name
                            : "WrathUp");
                        AddClairvoyanceIcone(effet, selectedAnswer, true);
                    }
                    else return "WrathUp";
                }
                break;
            case TypeEffet.DegatPVMax:
            case TypeEffet.DegatsBrut:
            case TypeEffet.AugmentFADernierDegatsSubi:
            case TypeEffet.ConscienceMax:
            case TypeEffet.Soin:
            case TypeEffet.SoinFA:
            case TypeEffet.SoinFANbEnnemi:
            case TypeEffet.SoinRadianceMax:
            case TypeEffet.SoinRadianceActuelle:
            case TypeEffet.RandomAttaque:
            case TypeEffet.AugmentationFaRadianceActuelle:
            case TypeEffet.ConsommeTensionAugmentationFA:
            case TypeEffet.RemoveDebuff:
            case TypeEffet.AttaqueStackAmant:
            case TypeEffet.GainResilienceIncrementale:
            case TypeEffet.DamageLastPhase:
            case TypeEffet.NoEssence:
            case TypeEffet.DoubleBuffDebuff:
            case TypeEffet.AugmentationRadianceMaxPourcentage:
            case TypeEffet.BuffFaCoupRecu:
            case TypeEffet.BuffResilienceCoupRecu:
            case TypeEffet.ConsommeTensionDmgAllExceptCaster:
            case TypeEffet.Provocation:
            case TypeEffet.VolEssence:
            case TypeEffet.RandomChanceCastSpellSelf:
            case TypeEffet.SwapMostLeastBuffDebuff:
            case TypeEffet.RadianceRepartition:
            case TypeEffet.RandomAttaqueDebuff:
            case TypeEffet.DegatsRetourSurAttaque:
            case TypeEffet.RedirectionDegatsOnCasteur:
            case TypeEffet.CancelPourcentageDamage:
            case TypeEffet.RedirectionCancel:
            case TypeEffet.DispellBuffJoueurDamage:
            case TypeEffet.DispellDebuffCasterDamage:
            case TypeEffet.DamageAllEvenly:
            case TypeEffet.DamageUpTargetLowRadiance:
            case TypeEffet.OnKillStunAll:
            case TypeEffet.UntilDeath:
            case TypeEffet.AugmentationFARadianceManquante:
            case TypeEffet.DamageFaBuff:
            case TypeEffet.DamageFaBuffCible:
            case TypeEffet.DamageDebuffCible:
            case TypeEffet.RemoveAllTensionProcDamage:
            case TypeEffet.RemoveAllTensionProcBuffDebuff:
            case TypeEffet.RemoveAllDebuffProcBuffDebuf:
            case TypeEffet.RemoveAllDebuffSelfProcBuffDebuf:
            case TypeEffet.RemoveAllBuffProcBuffDebuf:
            case TypeEffet.RemoveAllDebuffProcDamage:
            case TypeEffet.RemoveAllDebuffSelfProcDamage:
            case TypeEffet.RemoveAllBuffProcDamage:
            case TypeEffet.NoCapaPossible:
            case TypeEffet.ConsommeTensionReduitFa:
            case TypeEffet.AugmentationDegatsHitJoueur:
            case TypeEffet.GainFaBuffCible:
            case TypeEffet.GainFaDebuffCible:
            case TypeEffet.Ponction:
            case TypeEffet.PonctionForceAme:
            case TypeEffet.DegatsFaRadianceManquanteCible:
            case TypeEffet.DegatsFaRadianceManquanteCaster:
            case TypeEffet.PremiereAttaqueJeanne:
            case TypeEffet.DeuxiemeAttaqueJeanne:
            case TypeEffet.SupportJeanne:
            case TypeEffet.UltimeJeanne:
                Debug.Log($"Effet non géré pour la clairvoyance : {effet.TypeEffet})");
                break;

        }
        //strb.Append("\" color=#");
        //strb.Append(ColorUtility.ToHtmlStringRGBA(color));
        strb.Append("\">");
        return strb.ToString();
    }

    private void AddClairvoyanceIcone(Effet effet, int selectedAnswer, bool isBonus)
    {
        GameObject effectGO = Instantiate(_effectPrefab, _dialogPanelComponent.ClairvContentListGO[selectedAnswer].transform);
        ClairvoyancePanel panel = _dialogPanelComponent.ClairvContentListGO[selectedAnswer].GetComponent<ClairvoyancePanel>();
        panel.AddEffect(effet, isBonus);
        effectGO.GetComponent<EffectComponent>().SetSprite(effet.GetSpriteOfEffect());
        effectGO.GetComponent<EnflateSystem>().TriggerInflation();

        _listClairvEffect.Add(effectGO);
    }
    private void ShowConsequenceForAnswer(int selectedAnswer, ref bool[] displayed)
    {
        foreach (var consequence in _CurrentDialogue.Questions[DialogueIndex].ReponsePossible[selectedAnswer]
                     .conséquences)
        {
            foreach (var buffDebuff in consequence.Buffs)
            {
                foreach (Effet effet in buffDebuff.Effet)
                {
                    _dialogPanelComponent.ReponseText[selectedAnswer].text += BuildSpriteIcon(effet, selectedAnswer, ref displayed);
                }
            }

            foreach (var effet in consequence.Effects)
            {
                _dialogPanelComponent.ReponseText[selectedAnswer].text += BuildSpriteIcon(effet, selectedAnswer, ref displayed);
            }
        }
    }

    void ApplyConsequence(List<ConséquenceSO> consequence)
    {
        foreach (var Consequence in consequence)
        {
            // Tout les buff qu'applique le dialogue
            foreach (var buffDebuff in Consequence.Buffs)
            {


                GameObject buff = Instantiate(_buffPrefab, _buffContainer.transform);
                buff.GetComponent<BuffDebuffComponant>().InitBuffDebuff(buffDebuff);
                buff.GetComponent<BuffDebuffComponant>().buffCntLabel.text = "1";
                buff.GetComponent<EnflateSystem>().TriggerInflation();
                
                buff.GetComponent<BuffDebuffComponant>().buffSprite.sprite = buffDebuff.IsDebuff?GameManager.Instance.SpriteData.Debuff:GameManager.Instance.SpriteData.Buff;
                _listBuffEffectFromDialog.Add(buff);

                //Application du buff
                if (/*ManagerBattle == null*/ManagerAlea.IsAlea)
                {
                    ManagerAlea.Stat.ListBuffDebuff.Add(buffDebuff);
                }
                else
                {
                    ChoosePathOfExecution(Consequence, buffDebuff);
                }
            }

            //Tous les effets qu'applique le dialogue
            foreach (var effet in Consequence.Effects)
            {
                //Affichage de l'effet dans le dialogue
                Debug.Log("###Conséquence### - Ajout d'un nouvel Effet de type : " + effet.TypeEffet.ToString());
                GameObject effectGO = Instantiate(_effectPrefab, _buffContainer.transform);
                effectGO.GetComponent<EffectComponent>().SetSprite(effet.GetSpriteOfEffect());
                effectGO.GetComponent<EffectComponent>().SetText(effet.GetTargetStat());
                effectGO.GetComponent<EnflateSystem>().TriggerInflation();
                _listBuffEffectFromDialog.Add(effectGO);
                //Application de l'effet
                if (/*ManagerBattle == null*/ ManagerAlea.IsAlea)
                {
                    ManagerAlea.Stat.ModifStateAll(effet.ResultEffet(ManagerAlea.Stat));
                }
                else
                {
                    ChoosePathOfExecution(Consequence, effet);
                }
            }

        }
    }

    private void ChoosePathOfExecution(ConséquenceSO Consequence, ScriptableObject scriptableObject)
    {
        switch (Consequence.target)
        {
            case CibleDialogue.joueur:
                if (scriptableObject as BuffDebuff)
                {
                    ApplyBuffDebuffOnPlayer((BuffDebuff)scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOnPlayer((Effet)scriptableObject);
                }

                break;
            case CibleDialogue.allEnnemi:
                if (scriptableObject as BuffDebuff)
                {
                    ApplyBuffDebuffOnEnemies((BuffDebuff)scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOnEnemies((Effet)scriptableObject);
                }

                break;
            case CibleDialogue.ennemi:
                if (scriptableObject as BuffDebuff)
                {
                    ApplyBuffDebuffOneEnnemi((BuffDebuff)scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOneEnnemi((Effet)scriptableObject);
                }

                break;
            case CibleDialogue.All:
                if (scriptableObject as BuffDebuff)
                {
                    ApplyBuffDebuffOnPlayer((BuffDebuff)scriptableObject);
                    ApplyBuffDebuffOnEnemies((BuffDebuff)scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOnPlayer((Effet)scriptableObject);
                    ApplyEffectOnEnemies((Effet)scriptableObject);
                }

                break;
            case CibleDialogue.AllExceptSelf:
                break;
            case CibleDialogue.AllAllyExceptSelf:
                break;
            case CibleDialogue.Self:
                break;
        }
    }

    private void ApplyEffectOnPlayer(Effet scriptableObject)
    {
        ManagerBattle.player.Stat.ModifStateAll(scriptableObject.ResultEffet(ManagerBattle.player.Stat));
    }

    private void ApplyEffectOnEnemies(Effet scriptableObject)
    {
        foreach (var enemyScript in ManagerBattle.EnemyScripts)
        {
            enemyScript.Stat.ModifStateAll(scriptableObject.ResultEffet(enemyScript.Stat));
        }
    }

    private void ApplyEffectOneEnnemi(Effet scriptableObject)
    {
        var enemyScript = ManagerBattle.EnemyScripts[Random.Range(0, ManagerBattle.EnemyScripts.Count)];
        enemyScript.Stat.ModifStateAll(scriptableObject.ResultEffet(((JoueurStat)(CharacterStat)enemyScript.Stat),
            enemyScript.LastDamageTaken, enemyScript.Stat));
    }

    private void ApplyBuffDebuffOnPlayer(BuffDebuff scriptableObject)
    {
        ManagerBattle.player.AddDebuff(Instantiate(scriptableObject), scriptableObject.Decompte,
            scriptableObject.timerApplication);
        ManagerBattle.player.AddBuffDebuff(scriptableObject, ManagerBattle.player.Stat);
    }

    private void ApplyBuffDebuffOnEnemies(BuffDebuff scriptableObject)
    {
        foreach (var enemyScript in ManagerBattle.EnemyScripts)
        {
            enemyScript.AddDebuff(Instantiate(scriptableObject), scriptableObject.Decompte,
                scriptableObject.timerApplication);
            enemyScript.AddBuffDebuff(scriptableObject, enemyScript.Stat);
        }
    }

    private void ApplyBuffDebuffOneEnnemi(BuffDebuff scriptableObject)
    {
        var enemyScript = ManagerBattle.EnemyScripts[Random.Range(0, ManagerBattle.EnemyScripts.Count)];
        enemyScript.AddDebuff(Instantiate(scriptableObject), scriptableObject.Decompte,
            scriptableObject.timerApplication);
        enemyScript.AddBuffDebuff(scriptableObject, enemyScript.Stat);
    }

    public void StopSFX()
    {
        AudioManager.instance.SFX.StopPlaying();
    }
    private void ClearClairvoyanceIcons()
    {
        //test
        foreach (var panel in _dialogPanelComponent.ClairvContentListGO)
        {
            panel.GetComponent<ClairvoyancePanel>().ClearClairvoyancePanel();
        }
        //end test
       
        for (int i = _listClairvEffect.Count - 1; i >= 0; i--)
        {
            Destroy(_listClairvEffect[i]);
        }
        _listClairvEffect.Clear();
    }
    private void ResetDialog()
    {
        DialogueIndex = 0;
        NextDialogueIndex = 0;

        for (int i = _listBuffEffectFromDialog.Count - 1; i >= 0; i--)
        {
            Destroy(_listBuffEffectFromDialog[i]);
        }
        _listBuffEffectFromDialog.Clear();

        ClearClairvoyanceIcons();
    }
    public void StartCombat()
    {
        ResetDialog();
        AudioManager.instance.SFX.StopPlaying();
        if (GameManager.Instance.IsTuto/*TutoManager.Instance != null */)
        {
            if (TutoManager.Instance.StepTuto == 1)
            {
                var gO = TutoManager.Instance.TutoPanel;
                var child = gO.transform.GetChild(0);
                child.gameObject.SetActive(true);
                UIDialogue.SetActive(false);
                gO.GetComponent<TutoPanel>().ShowExplication();
                GameManager.Instance.StartCombat();
            }
        }
        else
        {
            UIJoueur.SetActive(true);
            UIDialogue.SetActive(false);
            GameManager.Instance.StartCombat();
        }
    }

    public void EndDialogueFonction()
    {
        ResetDialog();
        AudioManager.instance.SFX.StopPlaying();
        GameManager.Instance.AleaMan.EndAlea();
    }
}