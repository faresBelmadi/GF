﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public TextMeshProUGUI EndText;
    public BattleManager ManagerBattle;

    public AleaManager ManagerAlea;
    //public Button skipButton; 

    #endregion UI Reference

    #region SO

    private Encounter _CurrentEncounterBattle;
    private EncounterAlea _CurrentEncounterAlea;
    internal DialogueSO _CurrentDialogue;

    #endregion SO

    #region Dialogue Property

    internal int DialogueIndex = 0;
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
       
        MainText.GetComponent<TextDisplayer>().OnDisplayAnimFinish += GetAnswerList;
        
    }

    private void GetAnswerList()
    {
        MainText.GetComponent<TextDisplayer>().OnDisplayAnimFinish -= GetAnswerList;
        var currentQuestionType = _CurrentDialogue.Questions[DialogueIndex].Question.type;
        var currentPossibleResponseList = _CurrentDialogue.Questions[DialogueIndex].ReponsePossible;
        if ( currentQuestionType == TypeQuestion.startCombat ||
             currentQuestionType == TypeQuestion.EndTutoDialogue)
        {
            string DialogueTrad;
            if (!string.IsNullOrEmpty(currentPossibleResponseList[0].IdStringReponse))
            {
                DialogueTrad =
                    TradManager.instance.DialogueDictionary[currentPossibleResponseList[0].IdStringReponse][
                        TradManager.instance.IdLanguage];
            }
            else
            {
                DialogueTrad = "ID_DIALOGUE_NOT_IMPLEMENTED";
            }
            if (EndText != null)
            {
                //if(EndText.text == null || EndText.text == "")
                if (currentPossibleResponseList != null &&
                    currentPossibleResponseList.Count > 0 &&
                    !string.IsNullOrEmpty(DialogueTrad))
                    EndText.text = DialogueTrad;
                else
                {
                    EndText.text = "Continuer";
                }
            }
            else
            {
                var Text = EndDialogue.GetComponentInChildren<TextMeshProUGUI>();
                if (Text != null)
                {
                    Text.text = DialogueTrad;
                }
            }
            EndDialogue.SetActive(true);
        }
        else
        {
            for (int i = 0; i < currentPossibleResponseList.Count; i++)
            {
                string response;
                if (!string.IsNullOrEmpty(currentPossibleResponseList[i].IdStringReponse))
                {
                    response =
                        TradManager.instance.DialogueDictionary[currentPossibleResponseList[i].IdStringReponse][
                            TradManager.instance.IdLanguage];
                }
                else
                {
                    response = "ID_DIALOGUE_NOT_IMPLEMENTED";
                }
                Réponse[i].text = response;
                RéponseGO[i].SetActive(true);
                //Réponse[i].GetComponent<TextAnimation>().LaunchAnim();
            }
        }
    }

    private string TextePrincipal()
    {
        string dialogueTrad;
        if (!string.IsNullOrEmpty(_CurrentDialogue.Questions[DialogueIndex].Question.IdStringQuestion))
        {
            dialogueTrad =
                TradManager.instance.DialogueDictionary[_CurrentDialogue.Questions[DialogueIndex].Question.IdStringQuestion]
                    [TradManager.instance.IdLanguage];
        }
        else
        {
            dialogueTrad = "ID_DIALOGUE_NOT_IMPLEMENTED";
        }
        if (ManagerBattle == null && _CurrentEncounterAlea != null)
        {
            return "<allcaps><u><b>" + _CurrentEncounterAlea.NamePnj + ": </b></u></allcaps> " + dialogueTrad;
        }
        else
        {
            return "<allcaps><u><b>" + _CurrentEncounterBattle.ToFight[_CurrentDialogue.Questions[DialogueIndex].Question.IDSpeaker].Nom +
                   ": </b></u></allcaps>" + dialogueTrad;
        }
    }
     
    void resetRéponse()
    {
        foreach (var item in RéponseGO)
        {
            item.SetActive(false);
        }
        EndDialogue.SetActive(false);

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
        foreach (var Consequence in consequence)
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
                foreach (var BuffDebuff in Consequence.Buffs)
                {
                    ChoosePathOfExecution(Consequence, BuffDebuff);
                }

                foreach (var effet in Consequence.Effects)
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
                    ApplyBuffDebuffOnPlayer((BuffDebuff) scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOnPlayer((Effet) scriptableObject);
                }

                break;
            case CibleDialogue.allEnnemi:
                if (scriptableObject as BuffDebuff)
                {
                    ApplyBuffDebuffOnEnemies((BuffDebuff) scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOnEnemies((Effet) scriptableObject);
                }
                break;
            case CibleDialogue.ennemi:
                if (scriptableObject as BuffDebuff)
                {
                    ApplyBuffDebuffOneEnnemi((BuffDebuff) scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOneEnnemi((Effet) scriptableObject);
                }
                break;
            case CibleDialogue.All:
                if (scriptableObject as BuffDebuff)
                {
                    ApplyBuffDebuffOnPlayer((BuffDebuff) scriptableObject);
                    ApplyBuffDebuffOnEnemies((BuffDebuff) scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOnPlayer((Effet) scriptableObject);
                    ApplyEffectOnEnemies((Effet) scriptableObject);
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
        var enemyScript = ManagerBattle.EnemyScripts[Random.Range(0,ManagerBattle.EnemyScripts.Count)];
            enemyScript.Stat.ModifStateAll(scriptableObject.ResultEffet(((JoueurStat)(CharacterStat)enemyScript.Stat),enemyScript.LastDamageTaken,enemyScript.Stat));
    }

    private void ApplyBuffDebuffOnPlayer(BuffDebuff scriptableObject)
    {
        ManagerBattle.player.AddDebuff(Instantiate(scriptableObject), scriptableObject.Decompte, scriptableObject.timerApplication);
        ManagerBattle.player.AddBuffDebuff(scriptableObject, ManagerBattle.player.Stat);
    }

    private void ApplyBuffDebuffOnEnemies(BuffDebuff scriptableObject)
    {
        foreach (var enemyScript in ManagerBattle.EnemyScripts)
        {
            enemyScript.AddDebuff(Instantiate(scriptableObject), scriptableObject.Decompte, scriptableObject.timerApplication);
            enemyScript.AddBuffDebuff(scriptableObject, enemyScript.Stat);
        }
    }   
    private void ApplyBuffDebuffOneEnnemi(BuffDebuff scriptableObject)
    {
        var enemyScript = ManagerBattle.EnemyScripts[Random.Range(0,ManagerBattle.EnemyScripts.Count)];
        enemyScript.AddDebuff(Instantiate(scriptableObject), scriptableObject.Decompte, scriptableObject.timerApplication);
        enemyScript.AddBuffDebuff(scriptableObject, enemyScript.Stat);
    }

    public void StartCombat()
    {
        if (TutoManager.Instance != null)
        {
           var gO = GameObject.Find("TutoPanel");
           var child = gO.transform.GetChild(0);
           child.gameObject.SetActive(true);
           UIDialogue.SetActive(false);
           gO.GetComponent<TutoPanel>().ShowExplication();
        }
        else
        {
            UIJoueur.SetActive(true);
            UIDialogue.SetActive(false);
            GameManager.instance.BattleMan.StartCombat();
        }
    }

    public void EndDialogueFonction()
    {
        GameManager.instance.AleaMan.EndAlea();
    }
}