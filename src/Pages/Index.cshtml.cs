using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using simple_container.Services;

namespace simple_container.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly AppUiOptions _appUiOptions;
    private readonly ICounterOperations _counterOperations;
    private readonly ICounterStore _counterStore;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        ILogger<IndexModel> logger,
        ICounterOperations counterOperations,
        ICounterStore counterStore,
        IConfiguration configuration,
        Microsoft.Extensions.Options.IOptions<AppUiOptions> appUiOptions)
    {
        _logger = logger;
        _counterOperations = counterOperations;
        _counterStore = counterStore;
        _configuration = configuration;
        _appUiOptions = appUiOptions.Value;
    }

    public IReadOnlyList<CounterSnapshot> Counters { get; private set; } = [];
    public string? ExampleVar { get; private set; }
    public string BackingServiceName { get; private set; } = "Valkey";
    public string RedisInstanceName { get; private set; } = string.Empty;
    public string EnvironmentLabel { get; private set; } = string.Empty;
    public RequestUserContext UserContext { get; private set; } = new(null, null, null);

    [TempData]
    public string? ErrorMessage { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task OnGetAsync()
    {
        ExampleVar = Environment.GetEnvironmentVariable("EXAMPLE_VAR");
        RedisInstanceName = _configuration["Redis:InstanceName"] ?? "not configured";
        EnvironmentLabel = _appUiOptions.EnvironmentLabel;
        UserContext = RequestUserContextReader.Read(HttpContext);

        try
        {
            Counters = await _counterOperations.GetCountersAsync();
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
            var userContext = RequestUserContextReader.Read(HttpContext);
            var result = await _counterOperations.IncrementAsync(counterId, userContext.EmailAddress);
            StatusMessage = $"{counterId} incremented to {result.Value}.";
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to increment counter {CounterId}.", counterId);
            ErrorMessage = $"Could not increment {counterId}. Check the Valkey cluster and try again.";
        }

        return RedirectToPage();
    }
}
