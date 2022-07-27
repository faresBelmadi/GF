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
    public BattleManager ManagerBattle;
    public AleaManager ManagerAlea;
    //public Button skipButton; 
    #endregion UI Reference
    #region SO
    private Encounter _CurrentEncounterBattle;
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
        _CurrentEncounterBattle = encounterToSet;
        UIJoueur.SetActive(false);
        UIDialogue.SetActive(true);
        startDialogue();
    }

    public void SetupDialogue(EncounterAlea encounterToSet)
    {
        _CurrentDialogue = encounterToSet.DialogueRencontre;
        _CurrentEncounterAlea = encounterToSet;
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
        MainText.text = TextePrincipal();
        MainTextGO.SetActive(true);
        if (_CurrentDialogue.Questions[DialogueIndex].ReponsePossible.Count == 0)
        {
            EndDialogue.SetActive(true);
        }
        else
        {
            for (int i = 0; i < _CurrentDialogue.Questions[DialogueIndex].ReponsePossible.Count; i++)
            {
                Réponse[i].text = _CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].TexteRéponse;
                RéponseGO[i].SetActive(true);
                //Réponse[i].GetComponent<TextAnimation>().LaunchAnim();
            }
        }
    }

    private string TextePrincipal()
    {
        if(ManagerBattle == null)
        {
            return _CurrentEncounterAlea.NamePnj + ": " + _CurrentDialogue.Questions[DialogueIndex].Question.Text;
        }
        else
        {
            return _CurrentEncounterBattle.ToFight[_CurrentDialogue.Questions[DialogueIndex].Question.IDSpeaker].Nom + ": " + _CurrentDialogue.Questions[DialogueIndex].Question.Text;
        }
    }

    void resetRéponse()
    {
        foreach (var item in RéponseGO)
        {
            item.SetActive(false);
            EndDialogue.SetActive(false);
        }
        MainTextGO.SetActive(false);
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
            if (ManagerBattle == null)
            {
                foreach (var BuffDebuff in Consequence.Buffs)
                {
                    ManagerAlea.Stat.ListBuffDebuff.Add(BuffDebuff);
                }
                foreach (var effet in Consequence.Effects)
                {
                    ManagerAlea.Stat.ModifStateAll(effet.ResultEffet(ManagerAlea.Stat));
                }
            }
            else
            {
                //A Terminer une fois les effet et buffdebuff finalisé
                foreach (var BuffDebuff in Consequence.Buffs)
                {
                    switch (Consequence.target)
                    {
                        case Cible.joueur:
                            ManagerBattle.player.Stat.ListBuffDebuff.Add(Instantiate(BuffDebuff));
                            break;
                    }
                }
                foreach (var effet in Consequence.Effects)
                {
                    switch (Consequence.target)
                    {
                        case Cible.joueur:
                            ManagerBattle.player.Stat.ModifStateAll(effet.ResultEffet(ManagerBattle.player.Stat));
                            break;
                    }
                }
            }
        }
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
