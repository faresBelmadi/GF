using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoDialogueManager : DialogueManager
{
    public TutoBattleManager TutoBattleManager;
    public GameObject CrystauxEssence;
    public GameObject UiHolder;
    public Image Hpfill;
    public GameObject Conscience;
    public Image ConscienceFill;
    public GameObject JoueurHolder;

    //public RectTransform positionDialogueOriginal;
    //public RectTransform positionDialogueUp;
    public void EndDialogueTuto()
    {
        Debug.Log("Dialogue Tuto Fini!");
        TutoManager.Instance.NextStep();
        //SceneManager.LoadScene("TutoMonde");
    }

    public void GetRéponse(int i)
    {
        if (_CurrentDialogue.Questions[DialogueIndex].Question.type == TypeQuestion.TutoDialogueAndAction)
        {
            if (DialogueIndex == 1 && TutoManager.Instance.StepBatlleTuto == 0)
            {
                UiHolder.SetActive(true);
                Hpfill.fillAmount = 0.1f;
            }

            if (DialogueIndex == 4 && TutoManager.Instance.StepBatlleTuto == 0)
            {
                Hpfill.fillAmount = 1f;
            }

            if ((DialogueIndex == 2 || DialogueIndex == 5) && TutoManager.Instance.StepBatlleTuto == 1)
            {
                UiHolder.SetActive(true);
                Hpfill.fillAmount = 1f;
                Conscience.SetActive(true);
                ConscienceFill.fillAmount = 0.2f;
            }

            if ((DialogueIndex == 3 || DialogueIndex == 6) && TutoManager.Instance.StepBatlleTuto == 1)
            {
                ConscienceFill.fillAmount = 0.1f;
                //GoeargeTapeLeMob
                var toDelete = TutoBattleManager.spawnPos[2].GetChild(0);
                Destroy(toDelete.gameObject);
                Instantiate(CrystauxEssence, TutoBattleManager.spawnPos[2].position, Quaternion.identity,
                    TutoBattleManager.spawnPos[2]);
            }

            if (DialogueIndex == 0 && TutoManager.Instance.StepBatlleTuto == 3)
            {
                TutoBattleManager.used[0].gameObject.SetActive(true);
            }

            if (DialogueIndex == 5 && TutoManager.Instance.StepBatlleTuto == 3)
            {
                JoueurHolder.SetActive(false);
            }

            if (DialogueIndex == 6 && TutoManager.Instance.StepBatlleTuto == 3)
            {
                Destroy(GameManager.instance.gameObject);
                SceneManager.LoadSceneAsync(1);
                Destroy(TutoManager.Instance.gameObject);
            }
        }
        
        base.GetRéponse(i);
    }
}