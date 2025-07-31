using UnityEngine;

[System.Serializable]
public class AleaManager : MonoBehaviour
{
    [SerializeField] private DialogueManager Dialogue;
    [SerializeField] private Transform spawnPos;

    private EncounterAlea Rencontre;
    private GameObject _pnj;
    private PlayerStatsHandler _stat;
    private bool _isAlea = false;
    public bool IsAlea { get => _isAlea; }

    public PlayerStatsHandler Stat => _stat;

    

    public void StartAlea(EncounterAlea rencontre)
    {
        Debug.Log("StartAlea");
        _isAlea = true;
        Rencontre = rencontre;
        _stat = GameManager.Instance.playerStatHandler;
        _pnj = Instantiate(Rencontre.Pnj, spawnPos.position, Quaternion.identity, spawnPos);
        GameManager.Instance.DialManager.AddSpeakers(0, _pnj.GetComponentInChildren<SpeakComponent>());
        Dialogue.SetupDialogue(Rencontre);
    }

    public void EndAlea()
    {
        _isAlea = false;
        GameManager.Instance.playerStatHandler = _stat;
        Destroy(_pnj);
        StartCoroutine(GameManager.Instance.pmm.EndAlea());
    }
}
