using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMondeManager : MonoBehaviour
{
    public GameObject UiMondeHolder;
    public GameObject UiStatHolder;

    public void EnableSkillTree()
    {
        UiMondeHolder.SetActive(false);
    }

    public void EnableStat()
    {
        UiMondeHolder.SetActive(false);
        UiStatHolder.SetActive(true);
    }

    public void EnableMonde()
    {
        UiMondeHolder.SetActive(true);
        UiStatHolder.SetActive(false);
    }

    public void DisableStat()
    {
        UiStatHolder.SetActive(false);
    }

    public void RetourMenuPrincipale()
    {
        SceneManager.LoadScene("MainMenu");
        Destroy(GameManager.Instance.gameObject);
    }
}
