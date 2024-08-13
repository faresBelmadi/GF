using TMPro;
using UnityEngine;
using Text = UnityEngine.UI.Text;

public class TutoAutelManager : MonoBehaviour
{
    public string[] Explications;
    public string[] Reponses;
    public int currentStep = 0;
    public Text description;
    public TextMeshProUGUI ButtonResponse;
    public GameObject[] ObjectToSetVisible;
    public GameObject[] ObjectToSetInvisible;

    public void Start()
    {
        ChangeTextTuto();
    }

    public void ContinueButton()
    {
        currentStep++;
        ChangeTextTuto();
    }

    private void ChangeTextTuto()
    {
        if (currentStep >= Explications.Length)
        {
            TutoManager.Instance.NextStep();
        }
        else
        {
            //description.text = TradManager.instance.DialogueDictionary[Explications[currentStep]][TradManager.instance.IdLanguage];
            //ButtonResponse.text = TradManager.instance.DialogueDictionary[Reponses[currentStep]][TradManager.instance.IdLanguage];
            description.text = TradManager.instance.GetTranslation(Explications[currentStep]);
            ButtonResponse.text = TradManager.instance.GetTranslation(Reponses[currentStep]);

            if (ObjectToSetVisible[currentStep] != null)
                ObjectToSetVisible[currentStep]?.SetActive(true);
            if (ObjectToSetInvisible[currentStep] != null)
                ObjectToSetInvisible[currentStep]?.SetActive(false);
        }
    }
}