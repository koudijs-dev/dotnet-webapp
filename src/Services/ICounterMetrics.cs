namespace simple_container.Services;

public interface ICounterMetrics
{
    void ObserveCounters(IEnumerable<CounterSnapshot> counters);
    void RecordIncrement(string counterId, string? emailAddress, long newValue);
}
