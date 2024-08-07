using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEnnemi : MonoBehaviour
{

    [SerializeField] private ProgressBarManager HPBarManager;
    [SerializeField] private ProgressBarManager TensionBarManager;
    [SerializeField] public EnflateSystem inflateUISystem;
    [SerializeField] public Image[] imageCadreFGs;

    public TextMeshProUGUI HpText;
    public TextMeshProUGUI NameText;
    public GameObject Ciblage;
    public Image Intention;

    public bool TargetingMode = false;

    public Action RaiseEvent;

    public Transform degatSoinParent, buffParents, debuffParents;

    public GameObject soinPrefab;
    public GameObject degatPrefab;
    public List<GameObject> DebuffSpawned = new List<GameObject>();

    public void UpdateHp(int newHp, int newMaxHp)
    {
        if (!HPBarManager) return;
        HPBarManager.UpdatePBar(newHp, newMaxHp);
        HPBarManager.ToggleBloomPulses(false);
        HpText.text = newHp.ToString();// + "/" + newMaxHp;
    }

    public void UpdateTension(int newTension, int nbPalier)
    {
        if (!TensionBarManager) return;
        TensionBarManager.UpdatePBar(newTension, nbPalier);
    }

    public void UpdateNom(string nom)
    {
        NameText.text = nom;
    }

    private void OnMouseEnter()
    {
        if (TargetingMode)
            Ciblage.SetActive(true);
        //if (debuffParents.childCount > 0 || buffParents.childCount > 0)
        //    GetComponentInChildren<DescriptionHoverTrigger>().SendMessage("ShowDescription");
    }

    private void OnMouseExit()
    {
        if (TargetingMode)
            Ciblage.SetActive(false);

        //if (debuffParents.childCount > 0 || buffParents.childCount > 0)
        //    GetComponentInChildren<DescriptionHoverTrigger>().SendMessage("HideDescription");
    }

    public void SpawnDegatSoin(int value)
    {
        GameObject t;
        if (value < 0)
            t = Instantiate(degatPrefab, degatSoinParent);
        else
            t = Instantiate(soinPrefab, degatSoinParent);

        t.GetComponent<TextAnimDegats>().Value = value;

    }

    public void ChangeIntention(Sprite intent)
    {
        Intention.sprite = intent;
    }

    private void OnMouseDown()
    {
        if (TargetingMode)
        {
            RaiseEvent();
            Ciblage.SetActive(false);
        }
    }
    
}
