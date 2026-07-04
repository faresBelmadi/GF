using System;
using System.Collections;
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

    [SerializeField] private ProgressBarManager _hpBarManager;
    [SerializeField]
    public GameObject TutoDamageCalque;

    public void EndDialogueTuto()
    {
        TutoManager.Instance.NextStep();
    }

    public void InitDialogueStep()
    {
        NextDialogueIndex = 0;
    }

    public override void GetRéponse(int i)
    {
        if (GameManager.Instance.IsPaused)
            return;
        if (_CurrentDialogue.Questions[DialogueIndex].Question.type == TypeQuestion.EndTutoDialogue)
        {
            TutoManager.Instance.EndDialogueTuto();
            return;
        }
        else if (_CurrentDialogue.Questions[DialogueIndex].Question.type == TypeQuestion.TutoDialogueAndAction)
        {
            Debug.Log("DialogueIndex = " + DialogueIndex + " / IndexEncounter : " +
                      TutoManager.Instance.IndexEncounter);
            if (DialogueIndex == 0 && TutoManager.Instance.IndexEncounter == 0)
            {
                UiHolder.SetActive(true);
                //Hpfill.fillAmount = 0.1f;
                var joueurBehav = TutoManager.Instance.Player;
                joueurBehav.Stat.SetRadiance(joueurBehav.Stat.RadianceMaxTotal / 10);
                _hpBarManager.InitPBar(joueurBehav.Stat.Radiance, joueurBehav.Stat.RadianceMaxTotal);
                joueurBehav.UpdateUI();
            }

            if (DialogueIndex == 4 && TutoManager.Instance.IndexEncounter == 0)
            {
                //Hpfill.fillAmount = 1f;
                StartCoroutine(FadeOut(1));
                var joueurBehav = TutoManager.Instance.Player;
                joueurBehav.Stat.SetRadiance(joueurBehav.Stat.RadianceMaxTotal);
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

            if ((DialogueIndex == 5) && TutoManager.Instance.IndexEncounter == 1)
            {
                ConscienceFill.fillAmount = 0.1f;
                //GoeargeTapeLeMob
                var toDelete = BattleManager.spawnPos.FirstOrDefault(x =>
                    x.GetChild(0).gameObject.name == "Choristes_neuf Variant(Clone)");
                Destroy(toDelete.gameObject);
                Instantiate(CrystauxEssence, BattleManager.spawnPos[2].position, Quaternion.identity,
                    BattleManager.spawnPos[2]);
            }

            if (DialogueIndex == 7 && TutoManager.Instance.IndexEncounter == 2)
            {
                TutoManager.Instance.Player.ToggleVisibility(false);
            }

            if (DialogueIndex == 8 && TutoManager.Instance.IndexEncounter == 4)
            {
                TutoManager.Instance.Player.ToggleVisibility(true);
                TutoManager.Instance.EndTuto();
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
        foreach (GameObject repGO in _dialogPanelComponent.Reponse)
        {
            repGO.GetComponentInChildren<Button>(true).interactable = value;
        }
        
    }

    public void FadeTuto()
    {
        StartCoroutine(FadeIn(1));
    }
    private IEnumerator FadeIn(float duration)
    {
        TutoDamageCalque.SetActive(true);
        float currentTime = 0;
        while (currentTime < duration)
        {
            TutoDamageCalque.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        TutoDamageCalque.GetComponent<CanvasGroup>().alpha = 1;
    }
    private IEnumerator FadeOut(float duration)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            TutoDamageCalque.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1,0, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        TutoDamageCalque.GetComponent<CanvasGroup>().alpha = 0;
        TutoDamageCalque.SetActive(false);
    }
    public override void AddSpeakers(int id, EnnemyBehavior speaker)
    {
        
    }
}