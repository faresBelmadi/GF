using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutoPanel : MonoBehaviour
{
    public List<string> ExplicationListe;
    public List<string> ReponseListe;
    public List<Image> ExplicationImageListe;
    public int IndexExplication;
    public TextMeshProUGUI TextExplication;
    public TextMeshProUGUI TextReponse;
    public Image ImageExplication;

    public GameObject UIDialogue;
    public GameObject UIJoueur;
    public GameObject UIJoueurTutoExplication;
    public List<GameObject> UiToShowForFigth;
    public GameObject SpawnPos0;
    public GameObject EndBattleButton;

    public void ShowNextExplication()
    {
        Debug.Log("Index explication : " + IndexExplication);
        if (ExplicationImageListe[IndexExplication] != null)
        {
            ExplicationImageListe[IndexExplication].gameObject.SetActive(false);
        }
        switch (IndexExplication)
        {
            case 0:     // Explication fin de tour

                break;
            case 1:     //Explication Vitesse
                UiToShowForFigth[2].SetActive(true);
                break;
            case 2:     //Explication Volonté
                UiToShowForFigth[0].SetActive(true);
                break;
            case 3:     //Explication Compétence
                UiToShowForFigth[1].SetActive(true);
                break;
            case 4:     //Explication Tension
                UiToShowForFigth[3].SetActive(true);
                break;
            case 5:     //Explication Buff
                break;
            case 6:     //Explication tension
                break;
        }
        IndexExplication++;
        Debug.Log(IndexExplication);
        if (IndexExplication == 8)
        {
            StartCombat();
            foreach (var ui in UiToShowForFigth)
            {
                ui.SetActive(true);
            }
        }
        else if (IndexExplication == 13)
            GatherEssence();
        else if (IndexExplication == 14)
        {
            TutoManager.Instance.ShowSoulConsumation = false;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (IndexExplication == 16)
        {
            //ICI ne plus affiche le stat et tuto panel ainsi que la soul (la virer ? je crosi ce c'est un mise par le battle manager)
            TutoManager.Instance.ShowSoulConsumation = false;
            TutoManager.Instance.StatPanel.SetActive(false);
            this.gameObject.SetActive(false);
            TutoManager.Instance.NextStep();
        }
        else
            ShowExplication();
    }

    public void ShowExplication()
    {
        if (IndexExplication == 0)
        {
            //UIJoueurTutoExplication.SetActive(true);
        }
            

        //TextExplication.text = TradManager.Instance.DialogueDictionary[ExplicationListe[IndexExplication]][TradManager.Instance.IdLanguage];
        TextExplication.text = TradManager.instance.GetTranslation(ExplicationListe[IndexExplication]);

        if (ExplicationImageListe[IndexExplication] != null)
        {
            ExplicationImageListe[IndexExplication].gameObject.SetActive(true);
        }
        //else
        //    ImageExplication.enabled = false;

        var reponse = ReponseListe[IndexExplication];
        if (reponse is not (null or ""))
        {
            //TextReponse.text = TradManager.Instance.DialogueDictionary[reponse][TradManager.Instance.IdLanguage];
            TextReponse.text = TradManager.instance.GetTranslation(reponse);
        }
        else
            TextReponse.text = "Continue"; // a Rajouter dans le dossier de Trad
    }

    public void GatherEssence()
    {
        UIJoueur.SetActive(false);
        //UIDialogue.SetActive(false);
        TutoManager.Instance.ShowSoulConsumation = true;
        TutoManager.Instance.BattleManager.StartCoroutine("GatherEssence");
        this.transform.GetChild(0).gameObject.SetActive(false);
        //SpawnPos0.transform.GetChild(0).gameObject.SetActive(false);
        EndBattleButton.SetActive(false);
    }

    public void StartCombat()
    {
        //UIJoueurTutoExplication.SetActive(false);
        UIJoueur.SetActive(true);
        UIDialogue.SetActive(false);
        TutoManager.Instance.BattleManager.StartCombat();
        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void EndCombat()
    {
        EndBattleButton.SetActive(false);
        TutoManager.Instance.Loot();
        ShowNextExplication();
    }
}
