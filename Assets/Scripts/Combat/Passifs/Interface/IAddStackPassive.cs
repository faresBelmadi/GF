

public interface IAddStackPassive : IEffect
{
    void AddStack(CharacterStat stat, int numberOfStackToAdd);
    JoueurStat GetStackModifStat(CharacterStat stat, int numberOfStackToAdd);
}
