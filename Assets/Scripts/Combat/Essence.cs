using UnityEngine;

public class Essence : MonoBehaviour
{
    public GameObject ui;
    public bool isEnd;
    public int amount;

    public int Heal { get => (isEnd) ? amount : Mathf.FloorToInt(amount / 2); }

    private EssenceUI _essenceUI;
    private void Start()
    {
        _essenceUI = GetComponentInChildren<EssenceUI>();
    }
    public void AddEssence(int _amount)
    {
        amount = _amount;
    }

    public void ConsumeEssence()
    {
        _essenceUI.StopPreviewOnHP();
        if(isEnd)
            GameManager.Instance.BattleMan.ConsumeEndBattle(amount);
        else
            GameManager.Instance.BattleMan.Consume(Mathf.FloorToInt(amount/2));

        if(GameManager.Instance.BattleMan != null)
        {
            GameManager.Instance.BattleMan.ListEssence.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public int getEssence()
    {
        return amount;
    }
}
