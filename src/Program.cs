using Prometheus;
using Microsoft.Extensions.Options;
using simple_container.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<AppUiOptions>(builder.Configuration.GetSection("AppUi"));
builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
builder.Services.AddSingleton<ICounterMetrics, PrometheusCounterMetrics>();
builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
{
    var redisOptions = serviceProvider.GetRequiredService<IOptions<RedisOptions>>().Value;
    var configuration = ConfigurationOptions.Parse(redisOptions.Configuration, true);

    configuration.AbortOnConnectFail = false;
    configuration.AllowAdmin = true;
    configuration.ConnectRetry = 3;
    configuration.ClientName = "simple-container";

    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<ICounterStore, RedisCounterStore>();
builder.Services.AddSingleton<ICounterOperations, CounterOperations>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpMetrics();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapGet("/api/counters", async (ICounterOperations counterOperations) =>
{
    var counters = await counterOperations.GetCountersAsync();
    return Results.Ok(counters);
});

app.MapPost("/api/counters/{counterId}/increment", async (string counterId, HttpContext httpContext, ICounterOperations counterOperations, ILogger<Program> logger) =>
{
    try
    {
        var userContext = RequestUserContextReader.Read(httpContext);
        var result = await counterOperations.IncrementAsync(counterId, userContext.EmailAddress);
        return Results.Ok(result);
    }
    catch (ArgumentOutOfRangeException exception)
    {
        return Results.BadRequest(new { error = exception.Message, counterId });
    }
    catch (Exception exception)
    {
        logger.LogError(exception, "Failed to increment counter {CounterId} through the API.", counterId);
        return Results.Problem("Could not increment the counter right now.");
    }
});

app.MapMetrics();
app.MapRazorPages();

app.Run();
