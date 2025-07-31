public interface IDynamicEventPassive<T> : IEffect where T : StatsHandler
{
    void SubscribeEvents(T stat);
    void UnsubscribeEvents();
    void UpdateStat();
}
