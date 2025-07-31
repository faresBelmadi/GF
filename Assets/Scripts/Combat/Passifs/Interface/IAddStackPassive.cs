

public interface IAddStackPassive : IEffect
{
    void AddStack(StatsHandler<CharacterStat> stat, int numberOfStackToAdd);
    JoueurStat GetStackModifStat(AbstractStatsHandler stat, int numberOfStackToAdd);
}
