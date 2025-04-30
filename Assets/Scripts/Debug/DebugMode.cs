using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugMode : MonoBehaviour
{
    [SerializeField]
    private GameObject _togglePrefab;
    [SerializeField]
    private GameObject _inputFieldPrefab;
    [Header("Souvenir")]
    [SerializeField]
    private TMP_Dropdown _souvenirDropdown;
    [Header("Rencontres")]
    [SerializeField]
    private TMP_Dropdown _aleaDropdown;
    [SerializeField]
    private TMP_Dropdown _combatDropdown;
    [Header("Combat")]
    [SerializeField]
    private GameObject _spellContent;
    [Header("Stats")]
    [SerializeField]
    private GameObject _statContent;
   
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
    private List<Spell> _equipedSpells = new List<Spell>();
    private Dictionary<string, Spell> _dictionnarySpells = new Dictionary<string, Spell>();
    private Dictionary<string, int> _stats = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.IsPaused = true;
        RefreshListSouvenir();
        ListRencontre();
        ListSpell();
        ListStats();
    }

    private void ListStats()
    {
        _stats = new Dictionary<string, int>
        {
            { "Radiance", GameManager.Instance.classSO.PlayerStat.RadianceMaxOriginal },
            { "Force d'ame", GameManager.Instance.classSO.PlayerStat.ForceAmeOriginal },
            { "Vitesse", GameManager.Instance.classSO.PlayerStat.VitesseOriginal },
            { "Resilience", GameManager.Instance.classSO.PlayerStat.ResilienceOriginal },
            { "Clairvoyance", GameManager.Instance.classSO.PlayerStat.ClairvoyanceOriginal },
            { "Conviction", GameManager.Instance.classSO.PlayerStat.ConvictionOriginal },
            { "Calme", GameManager.Instance.classSO.PlayerStat.Calme },
            { "Volonte", GameManager.Instance.classSO.PlayerStat.VolonterMax },
        };

        foreach (var item in _stats)
        {
            var input = Instantiate(_inputFieldPrefab, _statContent.transform);
            input.GetComponentInChildren<TMP_Text>().text = item.Key;
            input.GetComponentInChildren<TMP_InputField>().text = item.Value.ToString();
            input.GetComponentInChildren<TMP_InputField>().onValueChanged.AddListener(x => _stats[item.Key] = int.Parse(x));
        }
    }

    private void ListSpell()
    {
        foreach (var item in GameManager.Instance.classSO.Competences)
        {
            _spells.Add(item.Spell);
            _dictionnarySpells.Add(item.Spell.Nom, item.Spell);
        }

        foreach(var item in GameManager.Instance.playerStat.ListSpell)
        {
            _equipedSpells.Add(item);
        }

        foreach(var item in _spells)
        {
            var toggle = Instantiate(_togglePrefab, _spellContent.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = item.Nom;
            toggle.GetComponent<Toggle>().isOn = _equipedSpells.Find(x=>x.Nom == item.Nom) != null;
            //toggle.GetComponent<Toggle>().onValueChanged.AddListener(x => 
            //{
            //    if (x)
            //    {
            //        _equipedSpells
            //    }
            //})
        }

    }
    private void ListRencontre()
    {
        _aleaString.Add("DEFAULT");
        _combatString.Add("DEFAULT");
        foreach (var item in GameManager.Instance.EncounterSet.EncounterNeutralAleaList)
        {
           
           
            _aleaNeutre.Add(item.name, item);
            _alea.Add(item.name, item);
            _aleaString.Add(item.name);
        }
        foreach (var item in GameManager.Instance.EncounterSet.EncounterClassAleaList)
        {
           
            _aleaClass.Add(item.name, item);
            _alea.Add(item.name, item);
            _aleaString.Add(item.name);
        }

        foreach (var item in GameManager.Instance.EncounterSet.EncounterNeutralList)
        {
           
            _combatNeutre.Add(item.name, item);
            _combat.Add(item.name, item);
            _combatString.Add(item.name);
        }
        foreach (var item in GameManager.Instance.EncounterSet.EncounterClassList)
        {
           
            _combatClass.Add(item.name, item); ;
            _combat.Add(item.name, item);
            _combatString.Add(item.name);
        }

        foreach (var item in GameManager.Instance.EncounterSet.EncounterEliteList)
        {
           
            _eliteNeutre.Add(item.name, item);
            _combat.Add(item.name, item);
            _combatString.Add(item.name);
        }
        foreach (var item in GameManager.Instance.EncounterSet.EncounterClassEliteList)
        {
           
            _eliteClass.Add(item.name, item);
            _combat.Add(item.name, item);
            _combatString.Add(item.name);
        }

        foreach (var item in GameManager.Instance.EncounterSet.EncounterBossList)
        {
           
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

    public void LaunchGame()
    {
        var stat = Instantiate(GameManager.Instance.playerStat);
        stat.Radiance = _stats["Radiance"];
        stat.ForceAmeOriginal = _stats["Force d'ame"];
        stat.ForceAme = _stats["Force d'ame"];
        stat.VitesseOriginal = _stats["Vitesse"];
        stat.Vitesse = _stats["Vitesse"];
        stat.ResilienceOriginal = _stats["Resilience"];
        stat.Resilience = _stats["Resilience"];
        stat.ClairvoyanceOriginal = _stats["Clairvoyance"];
        stat.Clairvoyance = _stats["Clairvoyance"];
        stat.ConvictionOriginal = _stats["Conviction"];
        stat.Conviction = _stats["Conviction"];
        stat.Calme = _stats["Calme"];
        stat.Volonter = _stats["Volonte"];

        stat.ListSpell.Clear();
        for (int i=0; i< _spellContent.transform.childCount; i++)
        {
            var go = _spellContent.transform.GetChild(i).gameObject;
            if (go.GetComponent<Toggle>().isOn)
            {
                Spell spell = _dictionnarySpells[go.GetComponentInChildren<TMP_Text>().text];
                stat.ListSpell.Add(Instantiate(spell));
            }
        }
        
        GameManager.Instance.playerStat = stat;

        if (_souvenirDropdown.value != 0)
        {
            EquipeSouvenir();
        }


        /* Changer la génération de la map*/
        bool isEncounterChanged = false;
        EncounterSetData debugEncounter = Instantiate(GameManager.Instance.EncounterSet);
        if (_aleaDropdown.value != 0)
        {
            isEncounterChanged = true;
            debugEncounter.EncounterClassAleaList.Clear();
            debugEncounter.EncounterNeutralAleaList.Clear();
            string aleaName = _aleaDropdown.options[_aleaDropdown.value].text;
            var alea = _alea[aleaName];
            debugEncounter.EncounterClassAleaList.Add(Instantiate(alea));
            debugEncounter.EncounterNeutralAleaList.Add(Instantiate(alea));
        }
        if (_combatDropdown.value != 0)
        {
            isEncounterChanged = true;
            debugEncounter.EncounterNeutralList.Clear();
            debugEncounter.EncounterEliteList.Clear();
            debugEncounter.EncounterBossList.Clear();
            debugEncounter.EncounterClassList.Clear();
            debugEncounter.EncounterClassEliteList.Clear();

            string combatName = _combatDropdown.options[_combatDropdown.value].text;
            var combat = _combat[combatName];

            debugEncounter.EncounterNeutralList.Add(Instantiate(combat));
            debugEncounter.EncounterEliteList.Add(Instantiate(combat));
            debugEncounter.EncounterBossList.Add(Instantiate(combat));
            debugEncounter.EncounterClassList.Add(Instantiate(combat));
            debugEncounter.EncounterClassEliteList.Add(Instantiate(combat));

        }

        if (isEncounterChanged)
        {
            GameManager.Instance.ChangeEncounterSet(debugEncounter);
            GameManager.Instance.GenerateNewMap();
        }

        gameObject.SetActive(false);
        GameManager.Instance.IsPaused = false;
        Destroy(gameObject);
    }
}
