using ExampleApp.Handlers;

using Helpers;

namespace ExampleApp.Services;

public class SimplePeriodicService : BackgroundService
{
    public SimplePeriodicService(IServiceScopeFactory serviceScopeFactory, ILogger<SimplePeriodicService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var handler = DefaultHandler()
                .WithScopedService(_serviceScopeFactory)
                .WithLoop(_timer);

            await Task.Run(() => handler(stoppingToken), stoppingToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Processing canceled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Procesing stopped");
            throw;
        }
    }

    private static Func<IHandler, CancellationToken, Task> DefaultHandler() =>
    async (handler, token) =>
    {
        await handler.HandleAsync("B", token).ConfigureAwait(false);
    };

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<SimplePeriodicService> _logger;
    private readonly PeriodicTimer _timer;
}
