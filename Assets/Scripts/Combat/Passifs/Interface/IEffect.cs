public interface IEffect
{
}

public interface IEffectOnStat<T> : IEffect where T:StatsHandler
{
    public void Apply(T charStat);
}