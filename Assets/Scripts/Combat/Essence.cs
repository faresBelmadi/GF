using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Essence : MonoBehaviour
{
    public GameObject ui;
    public bool isEnd;
    int amount;

    public void AddEssence(int _amount)
    {
        amount = _amount;
    }

    public void ConsumeEssence()
    {
        if(isEnd)
            GameManagerRemake.instance.BattleMan.ConsumeEndBattle(amount);
        else
            GameManagerRemake.instance.BattleMan.Consume(Mathf.FloorToInt(amount/2));

        GameManagerRemake.instance.BattleMan.ListEssence.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public int getEssence()
    {
        return amount;
    }
}
