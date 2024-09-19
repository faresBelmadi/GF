using UnityEngine;

[System.Serializable]
public class AleaManager : MonoBehaviour
{
    public DialogueManager Dialogue;

    public EncounterAlea Rencontre;
    public Transform spawnPos;
    public GameObject Pnj;

    public JoueurStat Stat;

    public void StartAlea(EncounterAlea rencontre)
    {
        Rencontre = rencontre;
        Stat = GameManager.Instance.playerStat;
        Pnj = Instantiate(Rencontre.Pnj, spawnPos.position, Quaternion.identity, spawnPos);
        Dialogue.SetupDialogue(Rencontre);
    }

    public void EndAlea()
    {
        GameManager.Instance.playerStat = Stat;
        StartCoroutine(GameManager.Instance.pmm.EndAlea());
    }
}
