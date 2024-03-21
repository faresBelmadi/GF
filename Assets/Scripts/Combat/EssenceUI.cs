using UnityEngine;

public class EssenceUI : MonoBehaviour
{
    public GameObject buttonConsumation;

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
