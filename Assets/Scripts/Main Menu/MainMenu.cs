using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private GameObject _welcomeText;
    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _optionMenu;
    [SerializeField]
    private CharacterSelect _characterSelect;

    private bool _anyKeyPushed = false;


    // Start is called before the first frame update
    void Start()
    {
        //on réinitialise l'animator

        _welcomeText.SetActive(true);
        _mainMenu.SetActive(false);
       // _optionMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_anyKeyPushed == false && Input.anyKeyDown == true)
        {
            _anyKeyPushed = true;
            ShowMainMenu();
        }
    }
    public void ShowMainMenu()
    {
        _welcomeText.SetActive(false);
        _mainMenu.SetActive(true);
    }
    public void ShowCharacterSelect()
    {
        _characterSelect.Init();
        _animator.SetTrigger("CharacterSelect");
    }
    public void CharacterSelectBack()
    {
        _animator.SetTrigger("CharacterBack");
        ShowMainMenu();
    }
    public void OpenSteamPage()
    {
        Application.OpenURL("https://store.steampowered.com/app/1949310/Eternals_Path/");
    }
    public void OpenDiscordPage()
    {
        Application.OpenURL("https://discord.com/invite/naCEPbru");
    }
    public void OpenLinktreePage()
    {
        Application.OpenURL("https://linktr.ee/sleeplessparadise");
    }
}
