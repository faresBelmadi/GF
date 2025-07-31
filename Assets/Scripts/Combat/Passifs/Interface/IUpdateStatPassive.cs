public interface IUpdateStatPassive : IEffect
{
    void InitPassif(StatsHandler stat);
    void Clear();
    void UpdateStat ();
}
public interface IUpdateEnemyStatPassive : IEffect
{
    void InitPassif(EnemyStatsHandler stat);
    void Clear();
    void UpdateStat();
}

