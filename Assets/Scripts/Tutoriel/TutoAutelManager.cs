﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.Jobs;
using UnityEditor.Experimental.GraphView;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;
using Image = UnityEngine.UI.Image;
using Text = UnityEngine.UI.Text;

public class TutoAutelManager : MonoBehaviour
{
    //public GameObject MenuUiPanel;
    //public GameObject LevelUpUiPanel;

    //public TextMeshProUGUI EssenceText;
    //public TextMeshProUGUI DescriptionSpellText;
    //public TextMeshProUGUI CostCapaText;

    //public List<GameObject> AllSpellsIcon;

    //public Button BuyButton;

    //public Button RetourButton;

    //public TextMeshProUGUI ValeurRadiance, ValeurFA, ValeurVitesse, ValeurConviction, ValeurResilience, ValeurCalme, ValeurVolonter, ValeurConscience, ValeurClairvoyance;
    //public TextMeshProUGUI ModifRadiance, ModifFA, ModifVitesse, ModifConviction, ModifResilience, ModifCalme, ModifVolonter, ModifConscience, ModifClairvoyance;


    public string[] Explications;
    public int currentStep = 0;
    public Text description;
    public GameObject[] ObjectToSetVisible;

    public void Start()
    {
        ChangeTextTuto();
    }

    public void Update()
    {
        if (Input.anyKeyDown)
        {
            ChangeTextTuto();
            currentStep++;
        }
    }

    private void ChangeTextTuto()
    {
        if (currentStep >= Explications.Length)
        {
            TutoManager.Instance.StepTuto++;
            SceneManager.LoadScene("Tuto");
        }
        else
        {
            description.text = Explications[currentStep];
            ObjectToSetVisible[currentStep].SetActive(true);
        }
    }
    //void FixedUpdate()
    //{
    //    EssenceText.text = "Essence : " + GameManager.instance.playerStat.Essence;
    //    SetUpStatsDescription();
    //}

    //public void ShowMenuUiPanel()
    //{
    //    LevelUpUiPanel.SetActive(false);
    //    MenuUiPanel.SetActive(true);
    //}

    //public void SetLvlUpActive()
    //{
    //    LevelUpUiPanel.SetActive(true);
    //    MenuUiPanel.SetActive(false);
    //    SetUpAllSpells();
    //}

    //private void SetUpAllSpells()
    //{
    //    var listOfCompetences = GameManager.instance.classSO.Competences;

    //    foreach (var capa in listOfCompetences)
    //    {
    //        if (capa.Spell?.Sprite)
    //            AllSpellsIcon[capa.Spell.IDSpell].GetComponent<Image>().sprite = capa.Spell.Sprite;
    //        //spellIcon.gameObject.GetComponent<Image>().sprite = GameManager.instance.playerStat.ListSpell[0].Sprite;
    //    }

    //    SetUpStatsDescription();
    //}
}