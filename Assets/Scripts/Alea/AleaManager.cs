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
        Stat = GameManager.instance.playerStat;
        Pnj = Instantiate(Rencontre.Pnj, spawnPos.position, Quaternion.identity, spawnPos);
        Dialogue.SetupDialogue(Rencontre);
    }

    public void EndAlea()
    {
        GameManager.instance.playerStat = Stat;
        StartCoroutine(GameManager.instance.pmm.EndAlea());
    }
}
