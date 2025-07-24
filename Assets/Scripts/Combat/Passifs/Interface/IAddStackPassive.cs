

public interface IAddStackPassive : IEffect
{
    void AddStack(StatsHolder stat, int numberOfStackToAdd);
    JoueurStat GetStackModifStat(StatsHolder stat, int numberOfStackToAdd);
}
