namespace simple_container.Services;

public interface ICounterOperations
{
    Task<IReadOnlyList<CounterSnapshot>> GetCountersAsync();
    Task<CounterIncrementResult> IncrementAsync(string counterId, string? emailAddress);
}
