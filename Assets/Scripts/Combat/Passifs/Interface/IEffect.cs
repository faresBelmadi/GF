public interface IEffect
{
}

public interface IEffectOnStat<T> : IEffect 
{
    public void Apply(T charStat);
}