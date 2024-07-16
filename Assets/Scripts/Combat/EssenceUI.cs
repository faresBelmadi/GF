using TMPro;
using UnityEngine;

public class EssenceUI : MonoBehaviour
{
    public GameObject buttonConsumation;

    [SerializeField]
    private Essence _essenceRootObject;

    private void Start()
    {
        buttonConsumation.GetComponentInChildren<TMP_Text>().text += $" ({_essenceRootObject.Heal})";
    }

    public void activate()
    {
        if (TutoManager.Instance != null && !TutoManager.Instance.ShowSoulConsumation)
            return;
        buttonConsumation.SetActive(true);
    }

    public void deactivate()
    {
        Invoke("invokedDeactivate", 2f);
    }


    private void invokedDeactivate()
    {
        buttonConsumation.SetActive(false);
    }
}
