using StackExchange.Redis;

namespace simple_container.Services;

public class RedisCounterStore : ICounterStore
{
    private static readonly (string Id, string DisplayName)[] Counters =
    [
        ("counter-1", "Counter 1"),
        ("counter-2", "Counter 2"),
        ("counter-3", "Counter 3")
    ];

    private readonly IDatabase _database;
    private readonly RedisOptions _options;

    public RedisCounterStore(IConnectionMultiplexer connectionMultiplexer, IConfiguration configuration)
    {
        _database = connectionMultiplexer.GetDatabase();
        _options = configuration.GetSection("Redis").Get<RedisOptions>() ?? new RedisOptions();
    }

    public IReadOnlyList<(string Id, string DisplayName)> CounterDefinitions => Counters;

    public async Task<IReadOnlyList<CounterSnapshot>> GetCountersAsync()
    {
        var keys = Counters.Select(counter => (RedisKey)BuildKey(counter.Id)).ToArray();
        var values = await _database.StringGetAsync(keys);

        return Counters.Select((counter, index) =>
        {
            var value = values[index].HasValue && long.TryParse(values[index].ToString(), out var parsedValue)
                ? parsedValue
                : 0;

            return new CounterSnapshot(counter.Id, counter.DisplayName, value);
        }).ToArray();
    }

    public Task<long> IncrementAsync(string counterId)
    {
        if (Counters.All(counter => !string.Equals(counter.Id, counterId, StringComparison.Ordinal)))
        {
            throw new ArgumentOutOfRangeException(nameof(counterId), counterId, "Unknown counter.");
        }

        return _database.StringIncrementAsync(BuildKey(counterId));
    }

    private string BuildKey(string counterId) => $"{_options.InstanceName}:counters:{{state-demo}}:{counterId}";
}
