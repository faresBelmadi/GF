using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator _cameraAnimator;
    [SerializeField]
    private Animator _canvaAnimator;

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
        _mainMenu.SetActive(false);
        _cameraAnimator.SetTrigger("CharacterSelect");
        _canvaAnimator.SetTrigger("CharacterSelect");
    }
    public void CharacterSelectBack()
    {
        _cameraAnimator.SetTrigger("CharacterBack");
        _canvaAnimator.SetTrigger("CharacterBack");
        ShowMainMenu();
    }
  
}
