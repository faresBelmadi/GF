using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugMode : MonoBehaviour
{
    [SerializeField]
    private GameObject _togglePrefab;
    [Header("Souvenir")]
    [SerializeField]
    private TMP_Dropdown _souvenirDropdown;
    [Header("Rencontres")]
    [SerializeField]
    private GameObject _contentAleaNeutre;
    [SerializeField]
    private GameObject _contentAleaClasse;
    [SerializeField]
    private GameObject _contentCombatNeutre;
    [SerializeField]
    private GameObject _contentCombatClasse;
    [SerializeField]
    private GameObject _contentCombatEliteNeutre;
    [SerializeField]
    private GameObject _contentCombatEliteClasse;
    [SerializeField]
    private GameObject _contentBoss;
    





    private Dictionary<string, Souvenir> _souvenir = new Dictionary<string, Souvenir>();
    
    private List<string> _souvenirList = new List<string>();
    private Souvenir _souv;

    private Dictionary<string, EncounterAlea> _aleaNeutre = new Dictionary<string, EncounterAlea>();
    private Dictionary<string, EncounterAlea> _aleaClass = new Dictionary<string, EncounterAlea>();
    private Dictionary<string, Encounter> _combatNeutre = new Dictionary<string, Encounter>();
    private Dictionary<string, Encounter> _combatClass = new Dictionary<string, Encounter>();
    private Dictionary<string, Encounter> _eliteNeutre = new Dictionary<string, Encounter>();
    private Dictionary<string, Encounter> _eliteClass = new Dictionary<string, Encounter>();
    private Dictionary<string, Encounter> _boss = new Dictionary<string, Encounter>();

    // Start is called before the first frame update
    void Start()
    {
        RefreshListSouvenir();
        ListRencontre();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ListRencontre()
    {
        // plutot faireu ne liste des alea, et une liste de tout les combat
        foreach (var item in GameManager.Instance.EncounterSet.EncounterNeutralAleaList)
        {
            var toggle = Instantiate(_togglePrefab, _contentAleaNeutre.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _aleaNeutre.Add(item.name, item);
        }
        foreach (var item in GameManager.Instance.EncounterSet.EncounterClassAleaList)
        {
            var toggle = Instantiate(_togglePrefab, _contentAleaClasse.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _aleaClass.Add(item.name, item);
        }

        foreach (var item in GameManager.Instance.EncounterSet.EncounterNeutralList)
        {
            var toggle = Instantiate(_togglePrefab, _contentCombatNeutre.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _combatNeutre.Add(item.name, item);
        }
        foreach (var item in GameManager.Instance.EncounterSet.EncounterClassList)
        {
            var toggle = Instantiate(_togglePrefab, _contentCombatClasse.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _combatClass.Add(item.name, item); ;
        }

        foreach (var item in GameManager.Instance.EncounterSet.EncounterEliteList)
        {
            var toggle = Instantiate(_togglePrefab, _contentCombatEliteNeutre.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _eliteNeutre.Add(item.name, item);
        }
        foreach (var item in GameManager.Instance.EncounterSet.EncounterClassEliteList)
        {
            var toggle = Instantiate(_togglePrefab, _contentCombatEliteClasse.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _eliteClass.Add(item.name, item);
        }

        foreach (var item in GameManager.Instance.EncounterSet.EncounterBossList)
        {
            var toggle = Instantiate(_togglePrefab, _contentBoss.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _boss.Add(item.name, item);
        }
    }
    private void RefreshListSouvenir()
    {
        _souvenir = new Dictionary<string, Souvenir>();
        _souvenirList = new List<string>
        {
            "NONE"
        };
        _souv = null;
        foreach (var souvenir in GameManager.Instance.CopyAllSouvenir)
        {

            _souvenirList.Add(souvenir.SouvenirName);
            _souvenir.Add(souvenir.SouvenirName, souvenir);
        }
        _souvenirDropdown.ClearOptions();
        _souvenirDropdown.AddOptions(_souvenirList);
    }
    public void SelectSouvenir(int optionID)
    {
        if (optionID == 0)
        {
            _souv = null;
        }
        else
        {
            _souv = _souvenir[_souvenirDropdown.options[optionID].text];
        }
    }

    public void EquipeSouvenir()
    {
        // GameManager.Instance.StatMan.gameObject.SetActive(true);
        
        GameManager.Instance.StatMan.Stat = GameManager.Instance.playerStat;

        GameManager.Instance.StatMan.StatTemp = Instantiate(GameManager.Instance.playerStat);

        _souv.Equiped = true;
        GameManager.Instance.playerStat.ListSouvenir.Add(Instantiate(_souv));
       GameManager.Instance.StatMan.ModifStat(_souv, true);
        if (GameManager.Instance.CopyAllSouvenir.Contains(_souv))
        {
            GameManager.Instance.CopyAllSouvenir.Remove(_souv);
        }
        RefreshListSouvenir();
    }
}
