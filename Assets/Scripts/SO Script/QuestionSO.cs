using UnityEngine;

public enum DialogueOption
{
    QuestionEnemy,
    PhraseJoueur,
    ReponseJoueur,
    ReponseEmotionConscience,
    Narrateur,
    ConséquenceInGame,
    ConséquenceInCombat,
}

[System.Serializable]
public struct QuestionSO 
{
    [Tooltip("cet identifiant correspond a l'emplacement de l'ennemi dans la liste de rencontre")]
    public int IDSpeaker;
    public TypeQuestion type;
    public string IdStringQuestion;
}

public enum TypeQuestion
{
    dialogue,
    startCombat,
    endCombat,
    Tutodialogue,
    TutoDialogueAndAction,
    EndTutoDialogue,
    EndAleaDialogue
}