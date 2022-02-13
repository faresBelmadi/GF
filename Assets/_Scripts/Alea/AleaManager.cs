using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AleaManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueSO dialogue;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager.SetupDialogue(dialogue);
    }
}
