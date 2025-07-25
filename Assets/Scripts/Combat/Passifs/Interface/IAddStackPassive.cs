

public interface IAddStackPassive : IEffect
{
    void AddStack(StatsHandler stat, int numberOfStackToAdd);
    JoueurStat GetStackModifStat(StatsHandler stat, int numberOfStackToAdd);
}
