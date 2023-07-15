using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
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
        if (ManagerBattle == null)
        {
            return _CurrentEncounterAlea.NamePnj + ": " + _CurrentDialogue.Questions[DialogueIndex].Question.Text;
        }
        else
        {
            return _CurrentEncounterBattle.ToFight[_CurrentDialogue.Questions[DialogueIndex].Question.IDSpeaker].Nom +
                   ": " + _CurrentDialogue.Questions[DialogueIndex].Question.Text;
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
        UIJoueur.SetActive(true);
        UIDialogue.SetActive(false);
        GameManager.instance.BattleMan.StartCombat();
    }

    public void EndDialogueFonction()
    {
        GameManager.instance.AleaMan.EndAlea();
    }
}