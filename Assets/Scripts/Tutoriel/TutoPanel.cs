using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutoPanel : MonoBehaviour
{

    [Serializable]
    public class PanelElement
    {
        public string Question;
        public string Answer;
    }

    [SerializeField] private List<PanelElement> ExplicationsListe;

    public List<Image> ExplicationImageListe;
    public int IndexExplication;
    public TextMeshProUGUI TextExplication;
    public TextMeshProUGUI TextReponse;

    public GameObject UIDialogue;
    public GameObject UIJoueur;
    public GameObject UIJoueurTutoExplication;
    public List<GameObject> UiToShowForFigth;
    public GameObject SpawnPos0;
    public GameObject EndBattleButton;
    public Transform StatTransform;

    public void ShowNextExplication()
    {

        Debug.Log("Index explication : " + IndexExplication);
        if (ExplicationImageListe[IndexExplication] != null)
        {
            ExplicationImageListe[IndexExplication].gameObject.SetActive(false);
        }

        IndexExplication++;
        switch (IndexExplication)
        {
            case 0: // montrer les sorts
                GameManager.Instance.BattleMan.player.DesactivateSpells();
                //UiToShowForFigth[0].SetActive(true);
                break;
            case 1: //Explication Volonté et conscience
                //UiToShowForFigth[1].SetActive(true);
                break;
            case 2: //Explication Fin de Tour
                //UiToShowForFigth[2].SetActive(true);
                break;
            case 3: //Explication Tour par Tour
                //UiToShowForFigth[3].SetActive(true);
                //UiToShowForFigth[4].SetActive(true); //les Stats (FA Vitesse etc etc)
                //Montrer la fléche de Vitesse
                break;
            case 4: //Explication blabla
                break;
            case 5: //StartCombat mais pour un seul coup, puis explication suivante??
                break;
            case 6: //Explication tension
                break;
            case 7: //Explication tension2
                break;
            case 9: //Déroulement narmol du combat
                GameManager.Instance.BattleMan.player.ActivateSpells();

                StartCombat();
                //foreach (var ui in UiToShowForFigth)
                //{
                //    ui.SetActive(true);
                //}
                break;
        }

        Debug.Log(IndexExplication);
        if (IndexExplication == 11) //13
            GatherEssence();
        else if (IndexExplication == 12) //14
        {
            this.transform.parent = StatTransform;
            this.transform.Translate(new Vector3(0, 1, 0), Space.Self);
            TutoManager.Instance.ShowSoulConsumation = false;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (IndexExplication == 15)
        {
            var menuStatManager = StatTransform.GetComponentInChildren<MenuStatManager>();
            foreach (var souvenirGo in menuStatManager.Souvenir)
            {
                Destroy(souvenirGo);
            }

            StatTransform.GetComponentInChildren<MenuStatManager>().EquipedSouvenir.Clear();
        }
        else if (IndexExplication == 14) //16
        {

            StatTransform.GetComponentInChildren<MenuStatManager>().End();
            this.gameObject.SetActive(false);
            TutoManager.Instance.NextStep();
            return;
        }

        ShowExplication();
    }

    public void ShowExplication()
    {
        if (IndexExplication == 0)
        {
            //UiToShowForFigth[0].SetActive(true);
        }

        TextExplication.text = TradManager.instance.GetTranslation(ExplicationsListe[IndexExplication].Question);

        if (ExplicationImageListe[IndexExplication] != null)
        {

            ExplicationImageListe[IndexExplication].gameObject.SetActive(true);
        }

        var reponse = ExplicationsListe[IndexExplication].Answer;
        Debug.Log("Panel // Question : " + ExplicationsListe[IndexExplication].Question + " Réponse : " +
                  ExplicationsListe[IndexExplication].Answer);
        if (reponse is not (null or ""))
        {
            TextReponse.text = TradManager.instance.GetTranslation(reponse);
        }
        else
            TextReponse.text = TradManager.instance.GetTranslation("TutoContinue");
    }

    public void GatherEssence()
    {
        //UIJoueur.SetActive(false);
        TutoManager.Instance.ShowSoulConsumation = true;
        TutoManager.Instance.BattleManager.StartCoroutine("GatherEssence");
        this.transform.GetChild(0).gameObject.SetActive(false);
        EndBattleButton.SetActive(false);
    }

    public void StartCombat()
    {
        //UIJoueur.SetActive(true);
        UIDialogue.SetActive(false);
        TutoManager.Instance.StartCombat();
        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void EndCombat()
    {
        EndBattleButton.SetActive(false);
        TutoManager.Instance.Loot();
        ShowNextExplication();
    }
}