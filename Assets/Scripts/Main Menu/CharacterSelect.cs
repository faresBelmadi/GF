using TMPro;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public enum SelectedCharacter
    {
        None,
        Warrior,
        Witch,
        Unknown
    }

    [Header("Description")]
    [SerializeField]
    private TMP_Text _nameText;
    [SerializeField]
    private TMP_Text _loreText;
    [SerializeField]
    private TMP_Text _gameplayText;
    [SerializeField]
    private GameObject _tutoToggle;
    [SerializeField]
    private string _comingSoonLabel;
    [SerializeField]
    private string _chooseCharLabel;
    [Space]
    [SerializeField]
    private CharacterSpriteHolder _warriorSpriteHolder;
    [Header("Warior")]
    [SerializeField]
    private bool _isWarriorAvailable;
    [SerializeField]
    private GameObject _warriorGO;
    [SerializeField]
    private GameObject _warriorCristopher;
    [SerializeField]
    private string _warriorNameLabel;
    [SerializeField]
    private string _warriorLoreLabel;
    [SerializeField]
    private string _warriorGameplayLabel;

    [Header("Sorciere")]
    [SerializeField]
    private bool _isWitchAvailable;
    [SerializeField]
    private GameObject _witchGO;
    [SerializeField]
    private GameObject _witchCristopher;
    [SerializeField]
    private string _witchNameLabel;
    [SerializeField]
    private string _witchLoreLabel;
    [SerializeField]
    private string _witchGameplayLabel;
    [Header("Inconnu")]
    [SerializeField]
    private bool _isUnkownAvailable;
    [SerializeField]
    private GameObject _unknownGO;
    [SerializeField]
    private GameObject _unknownCristopher;
    [SerializeField]
    private string _unknownNameLabel;
    [SerializeField]
    private string _unknownLoreLabel;
    [SerializeField]
    private string _unknownGameplayLabel;
    [Header("Portal")]
    [SerializeField]
    private RotateObject _portal;

    public int IdClass
    {
        get
        {
            if (_selected == SelectedCharacter.Warrior)
                return 0;
            else
                throw new System.Exception("Class unsoported : " + _selected.ToString());
        }
    }

    private SelectedCharacter _selected;



    private void OnEnable()
    {
        TradManager.OnRefreshTranslation += RefreshText;
        if (TradManager.instance != null)
            RefreshText();
    }
    private void OnDisable()
    {
        TradManager.OnRefreshTranslation -= RefreshText;
    }
  
   

    // Start is called before the first frame update
    void Start()
    {
        Unselect();
    }
    public void Init()
    {
        _portal.Init();
        Unselect();
        _warriorSpriteHolder.InitSprite();
    }
    private void Unselect()
    {
        _selected = SelectedCharacter.None;
        _warriorGO.SetActive(false);
        _witchGO.SetActive(false);
        _unknownGO.SetActive(false);
        _warriorCristopher.SetActive(true);
        _witchCristopher.SetActive(true);
        _unknownCristopher.SetActive(true);
        RefreshText();
    }
    private string GetNameLabel(SelectedCharacter selected) => selected switch
    {
        SelectedCharacter.Warrior => _warriorNameLabel,
        SelectedCharacter.Witch => _witchNameLabel,
        SelectedCharacter.Unknown => _unknownNameLabel,
        _ =>"",
    };
    private string GetLoreLabel(SelectedCharacter selected) => selected switch
    {
        SelectedCharacter.Warrior => _warriorLoreLabel,
        SelectedCharacter.Witch => _witchLoreLabel,
        SelectedCharacter.Unknown => _unknownLoreLabel,
        _ => "",
    };
    private string GetGameplayLabel(SelectedCharacter selected) => selected switch
    {
        SelectedCharacter.Warrior => _warriorGameplayLabel,
        SelectedCharacter.Witch => _witchGameplayLabel,
        SelectedCharacter.Unknown => _unknownGameplayLabel,
        _ => "",
    };
    private bool IsAvailable(SelectedCharacter selected) => selected switch
    {
        SelectedCharacter.Warrior => _isWarriorAvailable,
        SelectedCharacter.Witch => _isWitchAvailable,
        SelectedCharacter.Unknown => _isUnkownAvailable,
        _ => false,
    };
    public void ShowWitch()
    {
        _witchGO.SetActive(true);
        RefreshText(SelectedCharacter.Witch);
    }
    public void ShowWarrior()
    {
        _warriorGO.SetActive(true);
        RefreshText(SelectedCharacter.Warrior);
    }
    public void ShowUnknown()
    {
        _unknownGO.SetActive(true);
        RefreshText(SelectedCharacter.Unknown);
    }
    public void HideWitch()
    {
        if (_selected != SelectedCharacter.Witch)
        {
            _witchGO.SetActive(false);
            RefreshText(_selected);
        }
    }
    public void HideWarrior()
    {
        if (_selected != SelectedCharacter.Warrior)
        {
            _warriorGO.SetActive(false);
            RefreshText(_selected);
        }
    }
    public void HideUnknown()
    {
        if (_selected != SelectedCharacter.Unknown)
        {
            _unknownGO.SetActive(false);
            RefreshText(_selected);
        }
    }
    public void SelectWarrior()
    {
        if (!_isWarriorAvailable)
            return;
        Unselect();
        ShowWarrior();
        _warriorSpriteHolder.LightOn();
        _selected = SelectedCharacter.Warrior;
        _warriorCristopher.SetActive(false);
        _portal.StartRotate();
        RefreshText(SelectedCharacter.Warrior);
    }
    public void SelectWitch()
    {
        if (!_isWitchAvailable)
            return;
        Unselect();
        ShowWitch();
        _selected = SelectedCharacter.Witch;
        _witchCristopher.SetActive(false);
        _portal.StartRotate();
        RefreshText(SelectedCharacter.Witch);
    }
    public void SelectUnknown()
    {
        if (!_isUnkownAvailable)
            return;
        Unselect();
        ShowUnknown();
        _selected = SelectedCharacter.Unknown;
        _unknownCristopher.SetActive(false);
        _portal.StartRotate();
        RefreshText(SelectedCharacter.Unknown);
    }
    public void RefreshText(SelectedCharacter selected)
    {
        if (selected == SelectedCharacter.None)
        {
            _nameText.text = "";
            _gameplayText.text = "";
            _loreText.text = TradManager.instance.GetTranslation(_chooseCharLabel, "Choose"); ;
        }
        else
        {
           
                _nameText.text = TradManager.instance.GetTranslation(GetNameLabel(selected), "My name");
                _gameplayText.text = TradManager.instance.GetTranslation(GetGameplayLabel(selected), "My gameplay");
                _loreText.text = TradManager.instance.GetTranslation(GetLoreLabel(selected), "My lore");
        }
    }
    public void RefreshText() => RefreshText(_selected);
}
