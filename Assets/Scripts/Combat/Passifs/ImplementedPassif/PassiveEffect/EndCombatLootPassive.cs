using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public enum EndCombatCondition
{
    NoCossumption
}
public enum EndCombatReward
{
    IncreaseEssence,
    GrantConscience
}

[System.Serializable]
public struct EndReward
{
    public EndCombatCondition Condition;
    public EndCombatReward Reward;
    public int value;
}
[CreateAssetMenu(fileName = "New Loot Passiv", menuName = "PassiveEffect/New Loot Passive")]
public class EndCombatLootPassive : AbstractPassive, ILootEssencePassive
{
    [Space]
    [Header("EndCombatLootPassive")]
    [SerializeField]
    private List<EndReward> _endRewardList;
    public int Value { get; private set; }

    private void ApplyReward(CharacterStat charStat, EndReward reward)
    {
        BattleManager battleM = GameManager.Instance.BattleMan;
        switch (reward.Reward)
        {
            case EndCombatReward.GrantConscience:
                if (!battleM.ConsumedEssence)
                {
                    JoueurStat modifStat = CreateInstance<JoueurStat>();
                    modifStat.Conscience = reward.value;
                    ((JoueurStat)charStat).ModifStateAll(modifStat);
                    GameManager.Instance.BattleMan.player.UpdateUI();
                    
                }
                break;
            case EndCombatReward.IncreaseEssence:
                if (!battleM.ConsumedEssence)
                {
                    var essenceAmount = battleM.ListEssence.First().GetComponent<CrystalSoul>().Amount;
                    Value = (int)Math.Round(essenceAmount * ((reward.value / 100f)));
                    battleM.ListEssence.First().GetComponent<CrystalSoul>().Amount = essenceAmount + Value;

                }
                else
                {
                    Value = 0;
                }
                break;
        }
    }
    public void Apply(CharacterStat charStat)
    {
        Debug.Log("Apply Passif END COMBAT LOOT");
        foreach (var item in _endRewardList)
        {
            switch (item.Condition)
            {
                case EndCombatCondition.NoCossumption:
                    ApplyReward(charStat, item);
                    break;
            }
        }
    }
}
