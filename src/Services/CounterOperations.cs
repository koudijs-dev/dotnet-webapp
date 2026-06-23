namespace simple_container.Services;

public class CounterOperations : ICounterOperations
{
    private readonly ICounterMetrics _counterMetrics;
    private readonly ICounterStore _counterStore;

    public CounterOperations(ICounterStore counterStore, ICounterMetrics counterMetrics)
    {
        _counterStore = counterStore;
        _counterMetrics = counterMetrics;
    }

    public async Task<IReadOnlyList<CounterSnapshot>> GetCountersAsync()
    {
        var counters = await _counterStore.GetCountersAsync();
        _counterMetrics.ObserveCounters(counters);
        return counters;
    }

    public async Task<CounterIncrementResult> IncrementAsync(string counterId, string? emailAddress)
    {
        var newValue = await _counterStore.IncrementAsync(counterId);
        _counterMetrics.RecordIncrement(counterId, emailAddress, newValue);
        return new CounterIncrementResult(counterId, newValue);
    }
}
