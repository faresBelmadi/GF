using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoDialogueManager : DialogueManager
{
    public TutoBattleManager TutoBattleManager;
    public GameObject CrystauxEssence;
    public GameObject UiHolder;
    public Image Hpfill;
    public GameObject Conscience;
    public Image ConscienceFill;

    //public RectTransform positionDialogueOriginal;
    //public RectTransform positionDialogueUp;
    public void EndDialogueTuto()
    {
        Debug.Log("Dialogue Tuto Fini!");
        TutoManager.Instance.NextStep();
        //SceneManager.LoadScene("TutoMonde");
    }

    public void GetRéponse(int i)
    {
        if (_CurrentDialogue.Questions[DialogueIndex].Question.type == TypeQuestion.TutoDialogueAndAction)
        {
            if (DialogueIndex == 1 && TutoManager.Instance.StepBatlleTuto == 0)
            {
                UiHolder.SetActive(true);
                Hpfill.fillAmount = 0.1f;
                //Dialogue move up
            }
            if (DialogueIndex == 4 && TutoManager.Instance.StepBatlleTuto == 0)
            {
                //UiHolder.SetActive(true);
                Hpfill.fillAmount = 1f;
                //Dialogue move up
            }

            if ((DialogueIndex == 2 || DialogueIndex == 5) && TutoManager.Instance.StepBatlleTuto == 1)
            {
                UiHolder.SetActive(true);
                Hpfill.fillAmount = 1f;
                Conscience.SetActive(true);
                ConscienceFill.fillAmount = 0.2f;
            }
            if ((DialogueIndex == 3 || DialogueIndex == 6) && TutoManager.Instance.StepBatlleTuto == 1)
            {
                ConscienceFill.fillAmount = 0.1f;
                //GoeargeTapeLeMob
                var toDelete = TutoBattleManager.spawnPos[2].GetChild(0);
                Destroy(toDelete.gameObject);
                Instantiate(CrystauxEssence, TutoBattleManager.spawnPos[2].position, Quaternion.identity, TutoBattleManager.spawnPos[2]);
                //Remplacer le mob par de l'essence
            }
        }
        base.GetRéponse(i);
    }
}