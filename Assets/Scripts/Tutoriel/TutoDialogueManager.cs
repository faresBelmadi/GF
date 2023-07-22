using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoDialogueManager : DialogueManager
{
    public void EndDialogueTuto()
    {
        Debug.Log("Dialogue Tuto Fini!");
        SceneManager.LoadScene("TutoMonde");
    }
}