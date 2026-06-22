namespace simple_container.Services;

public interface ICounterStore
{
    IReadOnlyList<(string Id, string DisplayName)> CounterDefinitions { get; }
    Task<IReadOnlyList<CounterSnapshot>> GetCountersAsync();
    Task<long> IncrementAsync(string counterId);
}
