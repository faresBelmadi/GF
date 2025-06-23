using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class MainMenu : MonoBehaviour
{

    private const string _CHARACTERSELECTTRIGGER = "CharacterSelect";
    private const string _CHARACTERBACKTRIGGER = "CharacterBack";

    [SerializeField]
    private Animator _cameraAnimator;
    [SerializeField]
    private Animator _canvaAnimator;
    [SerializeField]
    private Animator _crystalAnimator;

    [SerializeField]
    private GameObject _welcomeText;
    [SerializeField]
    private GameObject _wipText;
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
        _wipText.SetActive(false);
        _mainMenu.SetActive(false);
       // _optionMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_anyKeyPushed == false && Input.anyKeyDown == true)
        {
            _anyKeyPushed = true;
            ShowWIPText();
        }
    }
    private void ShowWIPText()
    {
        _welcomeText.SetActive(false);
        _wipText.SetActive(true);
    }
    public void ShowMainMenu()
    {
        _wipText.SetActive(false);
        _welcomeText.SetActive(false);
        _mainMenu.SetActive(true);
    }
    public void ShowCharacterSelect()
    {
        _characterSelect.Init();
        _mainMenu.SetActive(false);
        _cameraAnimator.SetTrigger(_CHARACTERSELECTTRIGGER);
        _canvaAnimator.SetTrigger(_CHARACTERSELECTTRIGGER);
        _crystalAnimator.SetTrigger(_CHARACTERSELECTTRIGGER);
    }
    public void CharacterSelectBack()
    {
        _cameraAnimator.SetTrigger(_CHARACTERBACKTRIGGER);
        _canvaAnimator.SetTrigger(_CHARACTERBACKTRIGGER);
        ShowMainMenu();
    }
  
}
