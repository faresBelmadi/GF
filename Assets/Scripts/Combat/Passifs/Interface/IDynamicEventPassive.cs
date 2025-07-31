public interface IDynamicEventPassive<T> : IEffect
{
    void SubscribeEvents(T reference);
    void UnsubscribeEvents();
    void UpdateStat();
}
