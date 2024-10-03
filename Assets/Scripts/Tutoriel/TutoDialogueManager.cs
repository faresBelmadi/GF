using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoDialogueManager : DialogueManager
{
    public BattleManager BattleManager;
    public GameObject CrystauxEssence;
    public GameObject UiHolder;
    public Image Hpfill;
    public GameObject Conscience;
    public Image ConscienceFill;
    public GameObject JoueurHolder;

    [SerializeField]
    private ProgressBarManager _hpBarManager;
    //public RectTransform positionDialogueOriginal;
    //public RectTransform positionDialogueUp;
    public void EndDialogueTuto()
    {
        TutoManager.Instance.NextStep();
        //SceneManager.LoadScene("TutoMonde");
    }
    public void InitDialogueStep()
    {
        NextDialogueIndex = 0;
    }
    public void GetRéponse(int i)
    {
        if (_CurrentDialogue.Questions[DialogueIndex].Question.type == TypeQuestion.TutoDialogueAndAction)
        {
            Debug.Log("DialogueIndex = " + DialogueIndex + " / IndexEncounter : " + TutoManager.Instance.IndexEncounter);
            if (DialogueIndex == 0 && TutoManager.Instance.IndexEncounter == 0)
            {
                UiHolder.SetActive(true);
                //Hpfill.fillAmount = 0.1f;
                var joueurBehav = JoueurHolder.GetComponentInChildren<JoueurBehavior>();
                joueurBehav.Stat.Radiance = joueurBehav.Stat.RadianceMax / 10;
                _hpBarManager.InitPBar(joueurBehav.Stat.Radiance, joueurBehav.Stat.RadianceMax);
                joueurBehav.UpdateUI();
            }

            if (DialogueIndex == 4 && TutoManager.Instance.IndexEncounter == 0)
            {
                //Hpfill.fillAmount = 1f;

                var joueurBehav = JoueurHolder.GetComponentInChildren<JoueurBehavior>();
                joueurBehav.Stat.Radiance = joueurBehav.Stat.RadianceMax;
                //_hpBarManager.UpdatePBar(joueurBehav.Stat.Radiance, joueurBehav.Stat.RadianceMax);
                joueurBehav.UpdateUI();
            }

            if ((DialogueIndex == 1 || DialogueIndex == 4) && TutoManager.Instance.IndexEncounter == 1)
            {
                UiHolder.SetActive(true);
                Hpfill.fillAmount = 1f;
                Conscience.SetActive(true);
                ConscienceFill.fillAmount = 0.2f;
            }

            if ((/*DialogueIndex == 2 || */DialogueIndex == 5) && TutoManager.Instance.IndexEncounter == 1)
            {
                ConscienceFill.fillAmount = 0.1f;
                //GoeargeTapeLeMob
                var toDelete = BattleManager.spawnPos.FirstOrDefault(x => x.GetChild(0).gameObject.name == "Choristes_neuf Variant(Clone)");
                Destroy(toDelete.gameObject);
                Instantiate(CrystauxEssence, BattleManager.spawnPos[2].position, Quaternion.identity,
                    BattleManager.spawnPos[2]);
            }

            if (DialogueIndex == 0 && TutoManager.Instance.IndexEncounter == 3)
            {
                //BattleManager.used[0].gameObject.SetActive(true);
            }

            if (DialogueIndex == 8 && TutoManager.Instance.IndexEncounter == 3)
            {
                JoueurHolder.SetActive(false);
            }

            if (DialogueIndex == 8 && TutoManager.Instance.IndexEncounter == 4)
            {
                if (GameManager.Instance != null)
                    Destroy(GameManager.Instance.gameObject);
                SceneManager.LoadSceneAsync(1);
                Destroy(TutoManager.Instance.gameObject);
            }
        }
        
        base.GetRéponse(i);
    }


    public void EnableButtonAnswer()
    {
        ToggleAnswerButton(true);
    }
    public void DisableButtonAnswer()
    {
        ToggleAnswerButton(false);
    }
    private void ToggleAnswerButton(bool value)
    {
        foreach (GameObject repGO in ReponseGO)
        {
            repGO.GetComponent<Button>().interactable = value;
        }
    }

}