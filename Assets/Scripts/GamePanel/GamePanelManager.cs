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
    private void OnEnable()
    {
        GameManager.OnStartCombat += StartCombat;
        GameManager.OnStartDialog += StartDialog;
    }
    private void OnDisable()
    {
        GameManager.OnStartCombat -= StartCombat;
        GameManager.OnStartDialog -= StartDialog;
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
        _canvaDialog.SetActive(false);
        _canvaBattle.SetActive(true);
        _canvaEnnemy.SetActive(true);
    }
    public void StartDialog()
    {
        _canvaDialog.SetActive(true);
        _canvaBattle.SetActive(false);
        _canvaEnnemy.SetActive(true);
    }

}
