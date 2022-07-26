using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region UI Reference
    public GameObject UIDialogue;
    public GameObject UIJoueur;
    public TextMeshProUGUI MainText;
    public GameObject MainTextGO;
    public List<TextMeshProUGUI> Réponse;
    public List<GameObject> RéponseGO;
    public GameObject EndDialogue;
    public BattleManager Manager;
    public AleaManager ManagerAlea;
    //public Button skipButton; 
    #endregion UI Reference
    #region SO
    private Encounter _CurrentEncounter;
    private EncounterAlea _CurrentEncounterAlea;
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
        UIJoueur.SetActive(false);
        UIDialogue.SetActive(true);
        startDialogue();
    }

    public void SetupDialogue(EncounterAlea encounterToSet)
    {
        _CurrentDialogue = encounterToSet.DialogueRencontre;
        _CurrentEncounterAlea = encounterToSet;

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
        DialogueIndex = NextDialogueIndex;
        MainText.text = /*_CurrentEncounter.ToFight[_CurrentDialogue.Questions[DialogueIndex].Question.IDSpeaker].name + ": " +*/ _CurrentDialogue.Questions[DialogueIndex].Question.Text;
        //MainTextGO.GetComponent<TextAnimation>().LaunchAnim();
        if (_CurrentDialogue.Questions[DialogueIndex].ReponsePossible.Count == 0)
        {
            EndDialogue.SetActive(true);
        }
        else
        {
            for (int i = 0; i < _CurrentDialogue.Questions[DialogueIndex].ReponsePossible.Count; i++)
            {
                RéponseGO[i].SetActive(true);
                Réponse[i].text = _CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].TexteRéponse;
                //Réponse[i].GetComponent<TextAnimation>().LaunchAnim();
            }
        }
    }

    void resetRéponse()
    {
        foreach (var item in RéponseGO)
        {
            item.SetActive(false);
            EndDialogue.SetActive(false);
        }

        foreach (var item in Réponse)
        {
            item.text = "";
        }
    }

    public void GetRéponse(int i)
    {
        if (_CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].conséquences.Count != 0)
        {
            ApplyConsequence(_CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].conséquences);
        }
        NextDialogueIndex = _CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].IDNextQuestion;
        resetRéponse();
        GoNext();
    }

    void ApplyConsequence(List<ConséquenceSO> consequence)
    {
        foreach(var Consequence in consequence)
        {
            foreach(var BuffDebuff in Consequence.Buffs)
            {
                ManagerAlea.Stat.ListBuffDebuff.Add(BuffDebuff);
            }
            foreach(var effet in Consequence.Effects)
            {
                ManagerAlea.Stat.ModifStateAll(effet.ResultEffet(ManagerAlea.Stat));
            }

        }
        /*int origine;
        List<ActionResult> ResultatEffet = new List<ActionResult>();
        foreach (ConséquenceSO Cons in consequence)
        {
            Manager.ActDebuff(Cons.Buffs, 0, -1);
            foreach(var item in Cons.Effects)
            {
                if(Cons.target== Cible.self)
                {
                    origine = 0;
                }
                else
                {
                    origine = 1;
                }
                ResultatEffet.Add(Manager.GetResult(item));
                Manager.ActResult(ResultatEffet, origine);
                ResultatEffet.Clear();
                //Switchtype(UIJoueur, ennemie)
            }
        }*/
    }

    public void StartCombat()
    {
        UIJoueur.SetActive(true);
        UIDialogue.SetActive(false);
        GameManager.instance.BattleMan.StartCombat();
    }

    public void EndDialogueFonction()
    {
        GameManager.instance.AleaMan.EndAlea();
    }
}
