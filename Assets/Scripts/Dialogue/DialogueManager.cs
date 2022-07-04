using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region UI Reference
    public TextMeshProUGUI MainText;
    public GameObject MainTextGO;
    public List<TextMeshProUGUI> Réponse;
    public List<GameObject> RéponseGO;
    public Button skipButton; 
    #endregion UI Reference
    #region SO
    private Encounter _CurrentEncounter;
    private DialogueSO _CurrentDialogue;
    #endregion SO
    #region Dialogue Property
    private int DialogueIndex = 0;
    private int NextDialogueIndex = 0;
    #endregion Dialogue Property

    public void SetupDialogue(Encounter encounterToSet)
    {
        _CurrentDialogue = encounterToSet.DialogueRencontre;
        _CurrentEncounter = encounterToSet;
        //startDialogue();
        StartCombat();
    }

    void startDialogue()
    {
        resetRéponse();
        GoNext();
    }

    void GoNext()
    {
        DialogueIndex = NextDialogueIndex;
        MainText.text = _CurrentEncounter.ToFight[_CurrentDialogue.Questions[DialogueIndex].Question.IDSpeaker].name + ": " + _CurrentDialogue.Questions[DialogueIndex].Question.Text;
        MainTextGO.GetComponent<TextAnimation>().LaunchAnim();
        for (int i = 0; i < _CurrentDialogue.Questions[DialogueIndex].ReponsePossible.Count; i++)
        {   
            RéponseGO[i].SetActive(true);
            Réponse[i].text = _CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].TexteRéponse;
            //Réponse[i].GetComponent<TextAnimation>().LaunchAnim();
        }
    }

    void resetRéponse()
    {
        foreach (var item in RéponseGO)
        {
            item.SetActive(false);
        }

        foreach (var item in Réponse)
        {
            item.text = "";
        }
    }

    public void GetRéponse(int i)
    {
        NextDialogueIndex = _CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].IDNextQuestion;
        resetRéponse();
        GoNext();
    }
    void StartCombat()
    {
        GameManager.instance.BattleMan.StartCombat();
    }
}
