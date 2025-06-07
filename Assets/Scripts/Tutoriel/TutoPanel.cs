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
    //public GameObject UIJoueur;
    //public GameObject UIJoueurTutoExplication;
    //public List<GameObject> UiToShowForFigth;
    //public GameObject SpawnPos0;
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
                break;
            case 1:
            case 2: //Explication Fin de Tour
            case 3: //Explication Tour par Tour
            case 4: //Explication blabla
            case 5: //StartCombat mais pour un seul coup, puis explication suivante??
            case 6: //Explication tension
            case 7: //Explication tension2
                break;
            case 12: //Déroulement narmol du combat
                GameManager.Instance.BattleMan.player.ActivateSpells();

                StartCombat();
                break;
        }

        Debug.Log(IndexExplication);
        if (IndexExplication == 14) //13
            GatherEssence();
        else if (IndexExplication == 15) //14
        {
            this.transform.parent = StatTransform;
            this.transform.Translate(new Vector3(0, 1, 0), Space.Self);
            TutoManager.Instance.ShowSoulConsumation = false;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (IndexExplication == 18)
        {
            var menuStatManager = StatTransform.GetComponentInChildren<MenuStatManager>();
            foreach (var souvenirGo in menuStatManager.Souvenir)
            {
                Destroy(souvenirGo);
            }

            StatTransform.GetComponentInChildren<MenuStatManager>().EquipedSouvenir.Clear();
        }
        else if (IndexExplication == 17) //16
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
        TutoManager.Instance.ShowSoulConsumation = true;
        TutoManager.Instance.BattleManager.StartCoroutine("GatherEssence");
        this.transform.GetChild(0).gameObject.SetActive(false);
        EndBattleButton.SetActive(false);
    }

    public void StartCombat()
    {
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