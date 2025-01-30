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

    private bool _anyKeyPushed = false;

    // Start is called before the first frame update
    void Start()
    {
        _welcomeText.SetActive(true);
        _mainMenu.SetActive(false);
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

        _animator.SetTrigger("CharacterSelect");
    }
}
