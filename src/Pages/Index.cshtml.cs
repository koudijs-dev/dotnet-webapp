using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using simple_container.Services;

namespace simple_container.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ICounterStore _counterStore;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, ICounterStore counterStore, IConfiguration configuration)
    {
        _logger = logger;
        _counterStore = counterStore;
        _configuration = configuration;
    }

    public IReadOnlyList<CounterSnapshot> Counters { get; private set; } = [];
    public string? ExampleVar { get; private set; }
    public string RedisConfiguration { get; private set; } = string.Empty;

    [TempData]
    public string? ErrorMessage { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task OnGetAsync()
    {
        ExampleVar = Environment.GetEnvironmentVariable("EXAMPLE_VAR");
        RedisConfiguration = _configuration["Redis:Configuration"] ?? "not configured";

        try
        {
            Counters = await _counterStore.GetCountersAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to load counters from Valkey.");
            ErrorMessage = "Could not load counters from Valkey yet. Check the cluster and refresh.";
            Counters = _counterStore.CounterDefinitions
                .Select(counter => new CounterSnapshot(counter.Id, counter.DisplayName, 0))
                .ToArray();
        }
    }

    public async Task<IActionResult> OnPostIncrementAsync(string counterId)
    {
        try
        {
            var newValue = await _counterStore.IncrementAsync(counterId);
            StatusMessage = $"{counterId} incremented to {newValue}.";
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to increment counter {CounterId}.", counterId);
            ErrorMessage = $"Could not increment {counterId}. Check the Valkey cluster and try again.";
        }

        return RedirectToPage();
    }
}
