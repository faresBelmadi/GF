using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Transform degatSoinParent,buffParents, debuffParents;

    public GameObject soinPrefab;
    public GameObject degatPrefab;
    public List<GameObject> DebuffSpawned = new List<GameObject>();
    public GameObject BuffPrefab;
    public GameObject DebuffPrefab;
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
        if (debuffParents.childCount > 0 || buffParents.childCount > 0)
            GetComponentInChildren<DescriptionHoverTrigger>().SendMessage("ShowDescription");
    }

    private void OnMouseExit() {
        //healthSlider.gameObject.SetActive(false);
        TensionSlider.gameObject.SetActive(false);
        NameText.gameObject.SetActive(false);
        if(TargetingMode)
            Ciblage.SetActive(false);

        if(debuffParents.childCount > 0 || buffParents.childCount > 0)
            GetComponentInChildren<DescriptionHoverTrigger>().SendMessage("HideDescription");
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

    public void AddUIDebuff(BuffDebuff toAdd)
    {
        if (toAdd.IsDebuff)
        {
            var t = DebuffSpawned.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == toAdd.NomDebuff);
            if (t == null)
            {
                t = Instantiate(DebuffPrefab, debuffParents.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = toAdd.NomDebuff;
                t.GetComponent<DescriptionHoverTrigger>().Description.text = toAdd.Description;
                DebuffSpawned.Add(t);
            }
            else
            {
                var s = t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text;
                int nb = int.Parse(s);
                s = nb + 1 + "";
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = s;
            }

        }
        else
        {
            var t = DebuffSpawned.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == toAdd.NomDebuff);
            if (t == null)
            {
                t = Instantiate(BuffPrefab, buffParents.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = toAdd.NomDebuff;
                t.GetComponent<DescriptionHoverTrigger>().Description.text = toAdd.Description;
                DebuffSpawned.Add(t);
            }
            else
            {
                var s = t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text;
                int nb = int.Parse(s);
                s = nb + 1 + "";
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = s;
            }
        }
    }

    public void ClearDebuff(BuffDebuff remove)
    {
        var t = DebuffSpawned.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == remove.NomDebuff);
        if (t != null)
        {
            var s = t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text;
            int nb = int.Parse(s);
            nb -= 1;
            s = nb + "";
            if (nb <= 0)
            {
                DebuffSpawned.Remove(t);
                GameObject.Destroy(t);
            }
            else
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = s;
        }
    }
}
