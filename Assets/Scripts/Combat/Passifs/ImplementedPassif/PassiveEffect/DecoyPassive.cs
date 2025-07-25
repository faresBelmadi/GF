using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New DecoyPassive passiv", menuName = "PassiveEffect/New DecoyPassive passiv")]
public class DecoyPassive : AbstractPassive, IDecoyPassive, IStartCombatPassive, IDeathEffectPassive
{
    public void Apply(StatsHandler charStat)
    {
        
    }

    public void ApplyEffectOnStartCombat()
    {
        foreach (EnnemyBehavior ennemy in GameManager.Instance.BattleMan.EnemyScripts)
        {
            ennemy.MakeIntangible();
        }
    }

    public void OnDeathAction()
    {
        foreach (EnnemyBehavior ennemy in GameManager.Instance.BattleMan.EnemyScripts)
        {
            ennemy.MakeTangible();
        }
    }
}
