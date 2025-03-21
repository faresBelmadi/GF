using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugMode : MonoBehaviour
{
    [SerializeField]
    private GameObject _togglePrefab;
    [Header("Souvenir")]
    [SerializeField]
    private TMP_Dropdown _souvenirDropdown;
    [Header("Rencontres")]
    [SerializeField]
    private TMP_Dropdown _aleaDropdown;
    [SerializeField]
    private TMP_Dropdown _combatDropdown;
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
    [Header("Combat")]
    [SerializeField]
    private GameObject _spellContent;






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

    private Dictionary<string, EncounterAlea> _alea = new Dictionary<string, EncounterAlea>();
    private Dictionary<string, Encounter> _combat = new Dictionary<string, Encounter>();

    private List<string> _aleaString = new List<string>();
    private List<string> _combatString = new List<string>();

    private List<Spell> _spells = new List<Spell>();
    private List<string> _equipedSpells = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        RefreshListSouvenir();
        ListRencontre();
        ListSpell();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ListSpell()
    {
        foreach (var item in GameManager.Instance.classSO.Competences)
        {
            _spells.Add(item.Spell);
        }

        foreach(var item in GameManager.Instance.playerStat.ListSpell)
        {
            _equipedSpells.Add(item.Nom);
        }

        foreach(var item in _spells)
        {
            var toggle = Instantiate(_togglePrefab, _spellContent.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.Nom;
            toggle.GetComponent<Toggle>().isOn = _equipedSpells.Contains(item.Nom);
        }

    }
    private void ListRencontre()
    {
        // plutot faireu ne liste des alea, et une liste de tout les combat
        foreach (var item in GameManager.Instance.EncounterSet.EncounterNeutralAleaList)
        {
            var toggle = Instantiate(_togglePrefab, _contentAleaNeutre.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _aleaNeutre.Add(item.name, item);
            _alea.Add(item.name, item);
            _aleaString.Add(item.name);
        }
        foreach (var item in GameManager.Instance.EncounterSet.EncounterClassAleaList)
        {
            var toggle = Instantiate(_togglePrefab, _contentAleaClasse.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _aleaClass.Add(item.name, item);
            _alea.Add(item.name, item);
            _aleaString.Add(item.name);
        }

        foreach (var item in GameManager.Instance.EncounterSet.EncounterNeutralList)
        {
            var toggle = Instantiate(_togglePrefab, _contentCombatNeutre.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _combatNeutre.Add(item.name, item);
            _combat.Add(item.name, item);
            _combatString.Add(item.name);
        }
        foreach (var item in GameManager.Instance.EncounterSet.EncounterClassList)
        {
            var toggle = Instantiate(_togglePrefab, _contentCombatClasse.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _combatClass.Add(item.name, item); ;
            _combat.Add(item.name, item);
            _combatString.Add(item.name);
        }

        foreach (var item in GameManager.Instance.EncounterSet.EncounterEliteList)
        {
            var toggle = Instantiate(_togglePrefab, _contentCombatEliteNeutre.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _eliteNeutre.Add(item.name, item);
            _combat.Add(item.name, item);
            _combatString.Add(item.name);
        }
        foreach (var item in GameManager.Instance.EncounterSet.EncounterClassEliteList)
        {
            var toggle = Instantiate(_togglePrefab, _contentCombatEliteClasse.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _eliteClass.Add(item.name, item);
            _combat.Add(item.name, item);
            _combatString.Add(item.name);
        }

        foreach (var item in GameManager.Instance.EncounterSet.EncounterBossList)
        {
            var toggle = Instantiate(_togglePrefab, _contentBoss.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.name;
            _boss.Add(item.name, item);
            _combat.Add(item.name, item);
            _combatString.Add(item.name);
        }

        _aleaDropdown.ClearOptions();
        _combatDropdown.ClearOptions();

        _aleaDropdown.AddOptions(_aleaString);
        _combatDropdown.AddOptions(_combatString);
            
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
