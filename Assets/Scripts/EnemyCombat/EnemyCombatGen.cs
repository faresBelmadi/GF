using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombatGen : MonoBehaviour
{

    public Slider healthSlider;
    public Slider TensionSlider;

    public TextMeshProUGUI HpText;
    public TextMeshProUGUI TensionText;
    public TextMeshProUGUI NameText;
    public GameObject Ciblage;
    public Image Intention;

    public bool TargetingMode = false;

    public Action RaiseEvent;

    public Transform degatSoinParent,buffParents;

    public GameObject soinPrefab;
    public GameObject degatPrefab;
    
    public void updateHp(int newHp,int newMaxHp)
    {
        healthSlider.value = newHp;
        healthSlider.maxValue = newMaxHp;
        HpText.text = newHp+"/"+newMaxHp;
    }

    public void updateTension(int newTension,int nbPalier)
    {
        TensionSlider.value = newTension;
        TensionSlider.maxValue = nbPalier;
        TensionText.text = newTension +"/"+nbPalier;
    }

    public void updateNom(string nom) {
        NameText.text = nom;
    }

    private void OnMouseEnter() {
        //healthSlider.gameObject.SetActive(true);
        TensionSlider.gameObject.SetActive(true);
        NameText.gameObject.SetActive(true);
        if(TargetingMode)
            Ciblage.SetActive(true);
    }

    private void OnMouseExit() {
        //healthSlider.gameObject.SetActive(false);
        TensionSlider.gameObject.SetActive(false);
        NameText.gameObject.SetActive(false);
        if(TargetingMode)
            Ciblage.SetActive(false);
        
    }

    public void SpawnDegatSoin(int value)
    {
        GameObject t;
        if(value > 0)
            t = Instantiate(degatPrefab,degatSoinParent);
        else
            t = Instantiate(soinPrefab,degatSoinParent);
            
        t.GetComponent<TextAnimDegats>().Value = value;
        
    }
    
    public void ChangeIntention(Sprite intent)
    {
        Intention.sprite = intent;
    }

    private void OnMouseDown() 
    {
        if(TargetingMode)
        {
            RaiseEvent();
            Ciblage.SetActive(false);
        }
    }
}
