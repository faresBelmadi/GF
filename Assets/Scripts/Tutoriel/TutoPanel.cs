using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutoPanel : MonoBehaviour
{
    public List<string> ExplicationListe;
    public List<string> ReponseListe;
    public List<Sprite> ExplicationImageListe;
    public int IndexExplication;
    public TextMeshProUGUI TextExplication;
    public TextMeshProUGUI TextReponse;
    public Image ImageExplication;

    public GameObject UIDialogue;
    public GameObject UIJoueur;
    public GameObject SpawnPos0;
    public GameObject EndBattleButton;

    public void ShowNextExplication()
    {
        IndexExplication++;
        if (IndexExplication == 8)
            StartCombat();
        else if (IndexExplication == 13)
            GatherEssence();
        else if (IndexExplication == 16)
        {
            TutoManager.Instance.NextStep();
        }
        else 
            ShowExplication();
    }

    public void ShowExplication()
    {
        TextExplication.text = ExplicationListe[IndexExplication];
        var sprite = ExplicationImageListe[IndexExplication];
        if (sprite != null)
        {
            ImageExplication.enabled = true;
            ImageExplication.sprite = sprite;
        }
        else
            ImageExplication.enabled = false;

        var reponse = ReponseListe[IndexExplication];
        if (reponse is not (null or ""))
            TextReponse.text = reponse;
        else
            TextReponse.text = "Continuer";
    }

    public void GatherEssence()
    {
        UIJoueur.SetActive(false);
        //UIDialogue.SetActive(false);
        GameManager.instance.BattleMan.StartCoroutine("GatherEssence");
        this.transform.GetChild(0).gameObject.SetActive(false);
        SpawnPos0.transform.GetChild(0).gameObject.SetActive(false);
        EndBattleButton.SetActive(false);
    }

    public void StartCombat()
    {
        UIJoueur.SetActive(true);
        UIDialogue.SetActive(false);
        GameManager.instance.BattleMan.StartCombat();
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
