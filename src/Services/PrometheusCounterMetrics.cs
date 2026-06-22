using Prometheus;

namespace simple_container.Services;

public class PrometheusCounterMetrics : ICounterMetrics
{
    private static readonly Gauge CounterValueGauge = Metrics.CreateGauge(
        "dotnet_webapp_counter_value",
        "Current value of each Redis-backed demo counter.",
        new GaugeConfiguration
        {
            LabelNames = ["counter_id"]
        });

    private static readonly Counter CounterIncrementCounter = Metrics.CreateCounter(
        "dotnet_webapp_counter_increment_total",
        "Number of counter increments performed, labeled by counter and user email address.",
        new CounterConfiguration
        {
            LabelNames = ["counter_id", "email"]
        });

    public void ObserveCounters(IEnumerable<CounterSnapshot> counters)
    {
        foreach (var counter in counters)
        {
            CounterValueGauge.WithLabels(counter.Id).Set(counter.Value);
        }
    }

    public void RecordIncrement(string counterId, string? emailAddress, long newValue)
    {
        CounterIncrementCounter.WithLabels(counterId, NormalizeEmail(emailAddress)).Inc();
        CounterValueGauge.WithLabels(counterId).Set(newValue);
    }

    private static string NormalizeEmail(string? emailAddress) =>
        string.IsNullOrWhiteSpace(emailAddress) ? "unknown" : emailAddress.Trim().ToLowerInvariant();
}
