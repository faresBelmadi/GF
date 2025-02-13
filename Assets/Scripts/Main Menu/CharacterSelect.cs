using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public enum SelectedCharacter
    {
        None,
        Warrior,
        Witch
    }

    [Header("Description")]
    [SerializeField]
    private TMP_Text _nameText;
    [SerializeField]
    private TMP_Text _loreText;
    [SerializeField]
    private TMP_Text _gameplayText;
    [Header("Warior")]
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
    private GameObject _witchGO;
    [SerializeField]
    private GameObject _witchCristopher;
    [SerializeField]
    private string _witchNameLabel;
    [SerializeField]
    private string _witchLoreLabel;
    [SerializeField]
    private string _witchGameplayLabel;
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
    private void Unselect()
    {
        _selected = SelectedCharacter.None;
        _warriorGO.SetActive(false);
        _witchGO.SetActive(false);
        _warriorCristopher.SetActive(true);
        _witchCristopher.SetActive(true);
        RefreshText();
    }
    private string GetNameLabel() => _selected switch
    {
        SelectedCharacter.Warrior => _warriorNameLabel,
        SelectedCharacter.Witch => _witchNameLabel,
        _ =>"",
    };
    private string GetLoreLabel() => _selected switch
    {
        SelectedCharacter.Warrior => _warriorLoreLabel,
        SelectedCharacter.Witch => _witchLoreLabel,
        _ => "",
    };
    private string GetGameplayLabel() => _selected switch
    {
        SelectedCharacter.Warrior => _warriorGameplayLabel,
        SelectedCharacter.Witch => _witchGameplayLabel,
        _ => "",
    };
    public void ShowWitch()
    {
        _witchGO.SetActive(true);
    }
    public void ShowWarrior()
    {
        _warriorGO.SetActive(true);
    }
    public void HideWitch()
    {
        if (_selected != SelectedCharacter.Witch)
        {
            _witchGO.SetActive(false);
        }
    }
    public void HideWarrior()
    {
        if (_selected != SelectedCharacter.Warrior)
        {
            _warriorGO.SetActive(false);
        }
    }
    public void SelectWarrior()
    {
        Unselect();
        ShowWarrior();
        _selected = SelectedCharacter.Warrior;
        _warriorCristopher.SetActive(false);
        _portal.StartRotate();
        RefreshText();
    }
    public void SelectWitch()
    {
        Unselect();
        ShowWitch();
        _selected = SelectedCharacter.Witch;
        _witchCristopher.SetActive(false);
        _portal.StartRotate();
        RefreshText();
    }
    public void RefreshText()
    {
        if (_selected == SelectedCharacter.None)
        {
            _nameText.text = "";
            _gameplayText.text = "";
            _loreText.text = "";
        }
        else
        {
            _nameText.text = TradManager.instance.GetTranslation(GetNameLabel(), "My name");
            _gameplayText.text = TradManager.instance.GetTranslation(GetGameplayLabel(), "My gameplay");
            _loreText.text = TradManager.instance.GetTranslation(GetLoreLabel(), "My lore");
        }
    }
}
