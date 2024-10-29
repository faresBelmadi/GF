using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject _characterGameObject;
    //TEMP
    [SerializeField]
    private GameObject _canvaMap;
    private void OnEnable()
    {
        GameManager.OnStartCombat += StartCombat;
        GameManager.OnStartDialog += StartDialog;
        GameManager.OnStartAutel += StartAutel;
        GameManager.OnLootAfterCombat += StartLoot;
        GameManager.OnShowMap += ShowMap;

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
       
            _canvaMap.SetActive(!doTutorial);
            _canvaTuto.SetActive(doTutorial);
       
    }
    public void EndTuto()
    {
        InitPanel(false);
    }
    public void StartCombat()
    {
        _characterGameObject.SetActive(true);
        _canvaAutel.SetActive(false);
        _canvaDialog.SetActive(false);
        _canvaBattle.SetActive(true);
        _canvaEnnemy.SetActive(true);
    }
    public void StartDialog()
    {
        _characterGameObject.SetActive(true);
        _canvaAutel.SetActive(false);
        _canvaDialog.SetActive(true);
        _canvaBattle.SetActive(false);
        _canvaEnnemy.SetActive(true);
    }
    public void StartLoot()
    {
        _characterGameObject.SetActive(false);
        _canvaAutel.SetActive(false);
        _canvaDialog.SetActive(false);
        _canvaBattle.SetActive(false);
        _canvaEnnemy.SetActive(false);
    }
    public void StartAutel()
    {
        //TEMP
        _characterGameObject.SetActive(false);
       // _canvaMap.SetActive(false);

        _canvaAutel.SetActive(true);
        _canvaDialog.SetActive(false);
        _canvaBattle.SetActive(false);
        _canvaEnnemy.SetActive(false);
    }
    public void ShowMap()
    {
        //TEMP
        _characterGameObject.SetActive(true);
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
}
