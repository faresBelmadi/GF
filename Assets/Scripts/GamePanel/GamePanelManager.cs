using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvaBattle;
    [SerializeField]
    private GameObject _canvaDialog;
    [SerializeField]
    private GameObject _canvaEnnemy;
    [SerializeField]
    private GameObject _canvaAutel;
    [SerializeField]
    private GameObject _canvaTuto;
    [SerializeField]
    private JoueurBehavior _characterBehavior;

    //[SerializeField]
    //private GameObject _cloudCharacter;
    //TEMP
    [SerializeField]
    private GameObject _canvaMap;

    //Todo : a modifier, image end Game
    [SerializeField]
    private GameObject _endGameImage;
    [SerializeField]
    private Sprite _endFrGameImage;
    [SerializeField]
    private Sprite _endEnGameImage;

    private void OnEnable()
    {
        GameManager.OnStartCombat += StartCombat;
        GameManager.OnStartDialog += StartDialog;
        GameManager.OnStartAutel += StartAutel;
        GameManager.OnLootAfterCombat += StartLoot;
        GameManager.OnShowMap += ShowMap;
        PlayerMapManager.OnEndGame += EndGame;

        TutoManager.OnEndDialog += HideDialog;
        TutoManager.OnStartCombat += StartCombat;
        TutoManager.OnEndCombat += HideBattle;
        TutoManager.OnEndTuto += EndTuto;
    }

    private void OnDisable()
    {
        GameManager.OnStartCombat -= StartCombat;
        GameManager.OnStartDialog -= StartDialog;
        GameManager.OnStartAutel -= StartAutel;
        GameManager.OnLootAfterCombat -= StartLoot;
        GameManager.OnShowMap -= ShowMap;
        PlayerMapManager.OnEndGame -= EndGame;

        TutoManager.OnEndDialog -= HideDialog;
        TutoManager.OnStartCombat -= StartCombat;
        TutoManager.OnEndCombat -= HideBattle;
        TutoManager.OnEndTuto -= EndTuto;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitPanel(bool doTutorial = false)
    {
        _canvaMap.SetActive(true);

        //_canvaMap.SetActive(!doTutorial);
        //_canvaTuto.SetActive(doTutorial);
        _canvaDialog.SetActive(false);
    }

    public void EndTuto()
    {
        InitPanel(false);
    }

    public void StartCombat()
    {
        _characterBehavior.ToggleVisibility(true);
        //_cloudCharacter.SetActive(true);
        _canvaAutel.SetActive(false);
        _canvaDialog.SetActive(false);
        _canvaBattle.SetActive(true);
        _canvaEnnemy.SetActive(true);
    }

    public void StartDialog()
    {
        _characterBehavior.ToggleVisibility(true);
        //_cloudCharacter.SetActive(true);
        _canvaAutel.SetActive(false);
        _canvaDialog.SetActive(true);
        _canvaBattle.SetActive(false);
        _canvaEnnemy.SetActive(true);
    }

    public void StartLoot()
    {
        _characterBehavior.ToggleVisibility(false);
        //_cloudCharacter.SetActive(false);
        _canvaAutel.SetActive(false);
        _canvaDialog.SetActive(false);
        _canvaBattle.SetActive(false);
        _canvaEnnemy.SetActive(false);
    }

    public void StartAutel()
    {
        //TEMP
        _characterBehavior.ToggleVisibility(false);
        //_cloudCharacter.SetActive(false);
        // _canvaMap.SetActive(false);

        _canvaAutel.SetActive(true);
        _canvaDialog.SetActive(false);
        _canvaBattle.SetActive(false);
        _canvaEnnemy.SetActive(false);
    }

    public void ShowMap()
    {
        //TEMP
        _characterBehavior.ToggleVisibility(true);
        //_cloudCharacter.SetActive(false);
        _canvaMap.SetActive(true);

        _canvaAutel.SetActive(false);
        _canvaDialog.SetActive(false);
        _canvaBattle.SetActive(false);
        _canvaEnnemy.SetActive(false);
    }

    public void HideDialog()
    {
        _canvaDialog.SetActive(false);
    }

    public void HideBattle()
    {
        _canvaBattle.SetActive(false);
    }

    private void EndGame()
    {
        _endGameImage.GetComponent<Image>().sprite = TradManager.instance.Language == TradManager.SUPPORTEDLANGUAGES.EN
            ? _endEnGameImage
            : _endFrGameImage;
        _endGameImage.gameObject.SetActive(true);
    }
}