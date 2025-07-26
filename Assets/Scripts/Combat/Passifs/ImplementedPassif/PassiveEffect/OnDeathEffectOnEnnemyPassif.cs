using UnityEngine;

[CreateAssetMenu(fileName = "New OnDeathEffectOnEnnemyPassif passiv", menuName = "PassiveEffect/New OnDeathEffectOnEnnemyPassif passiv")]
public class OnDeathEffectOnEnnemyPassif : AbstractPassive, IDeathEffectPassive
{
    [Space]
    [Header("OnDeathEffectOnEnnemyPassif")]
    [SerializeField]
    private EnnemiStat _dependency;
    [SerializeField]
    private StatToModif _statToModif;

    public void Apply(StatsHandler charStat)
    {
        //...
    }

    public void OnDeathAction()
    {
        EnnemyBehavior dependencyBehavior = null;
        foreach (var enemy in GameManager.Instance.BattleMan.EnemyScripts)
        {
            if (enemy.Stat.BaseEnemyStat.IdTradName == _dependency.IdTradName)
            {
                dependencyBehavior = enemy;
            }
        }

        if (dependencyBehavior == null) return;

        switch (_statToModif.Stat)
        {
            case BaseStats.PalierTension:
                dependencyBehavior.Stat.ChangeTension(dependencyBehavior.Stat.ValeurPalier * _statToModif.Ratio);
                break;
        }
    }
}
