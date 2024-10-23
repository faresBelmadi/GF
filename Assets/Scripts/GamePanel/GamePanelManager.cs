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
    //TEMP
    [SerializeField]
    private GameObject _canvaMap;
    private void OnEnable()
    {
        GameManager.OnStartCombat += StartCombat;
        GameManager.OnStartDialog += StartDialog;
        GameManager.OnStartAutel += StartAutel;
    }
    private void OnDisable()
    {
        GameManager.OnStartCombat -= StartCombat;
        GameManager.OnStartDialog -= StartDialog;
        GameManager.OnStartAutel -= StartAutel;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCombat()
    {
        _canvaAutel.SetActive(false);
        _canvaDialog.SetActive(false);
        _canvaBattle.SetActive(true);
        _canvaEnnemy.SetActive(true);
    }
    public void StartDialog()
    {
        _canvaAutel.SetActive(false);
        _canvaDialog.SetActive(true);
        _canvaBattle.SetActive(false);
        _canvaEnnemy.SetActive(true);
    }
    public void StartAutel()
    {
        //TEMP
        _canvaMap.SetActive(false);

        _canvaAutel.SetActive(true);
        _canvaDialog.SetActive(false);
        _canvaBattle.SetActive(false);
        _canvaEnnemy.SetActive(false);
    }
}
