using UnityEngine;

[System.Serializable]
public class AleaManager : MonoBehaviour
{
    [SerializeField] private DialogueManager Dialogue;
    [SerializeField] private Transform spawnPos;

    private EncounterAlea Rencontre;
    private GameObject _pnj;
    private JoueurStat _stat;
    private bool _isAlea = false;
    public bool IsAlea { get => _isAlea; }

    public JoueurStat Stat => _stat;

    

    public void StartAlea(EncounterAlea rencontre)
    {
        Debug.Log("StartAlea");
        _isAlea = true;
        Rencontre = rencontre;
        _stat = GameManager.Instance.playerStat;
        _pnj = Instantiate(Rencontre.Pnj, spawnPos.position, Quaternion.identity, spawnPos);
        Dialogue.SetupDialogue(Rencontre);
    }

    public void EndAlea()
    {
        _isAlea = false;
        GameManager.Instance.playerStat = _stat;
        StartCoroutine(GameManager.Instance.pmm.EndAlea());
    }
}
