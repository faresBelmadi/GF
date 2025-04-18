using UnityEngine;

[CreateAssetMenu(fileName = "New OnDeathEffectOnEnnemyPassif passiv", menuName = "PassiveEffect/New OnDeathEffectOnEnnemyPassif passiv")]
public class OnDeathEffectOnEnnemyPassif : AbstractPassive, IDeathEffectPassive
{
   
    [SerializeField]
    private EnnemiStat _dependency;
    [SerializeField]
    private StatToModif _statToModif;

    public void Apply(CharacterStat charStat)
    {
        //...
    }

    public void OnDeathAction()
    {
        EnnemyBehavior dependencyBehavior = null;
        foreach (var enemy in GameManager.Instance.BattleMan.EnemyScripts)
        {
            if (enemy.Stat.IdTradName == _dependency.IdTradName)
            {
                dependencyBehavior = enemy;
            }
        }

        if (dependencyBehavior == null) return;

        switch (_statToModif.Stat)
        {
            case BaseStats.PalierTension:
                dependencyBehavior.Stat.Tension += dependencyBehavior.Stat.ValeurPalier * _statToModif.Ratio;
                break;
        }
    }
}
