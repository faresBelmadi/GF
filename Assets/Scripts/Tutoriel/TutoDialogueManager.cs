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
    public GameObject HpHolder;

    public void EndDialogueTuto()
    {
        Debug.Log("Dialogue Tuto Fini!");
        SceneManager.LoadScene("TutoMonde");
    }

    public void GetRéponse(int i)
    {
        if (_CurrentDialogue.Questions[DialogueIndex].Question.type == TypeQuestion.TutoDialogueAndAction)
        {
            Debug.Log(DialogueIndex);
            if (DialogueIndex == 2)
            {
                HpHolder.SetActive(true);
            }
        }
        base.GetRéponse(i);
    }
}