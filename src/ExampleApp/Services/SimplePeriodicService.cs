using ExampleApp.Handlers;

using Helpers;

namespace ExampleApp.Services;

/// <summary>
/// Represents a simple background service that executes a
/// default handler with a scoped service and a loop by <see cref="PeriodicTimer"/>.
/// </summary>
public class SimplePeriodicService : BackgroundService
{

    /// <summary>
    /// Initializes a new instance of the SimplePeriodicService class with the
    /// specified parameters.
    /// </summary>
    /// <param name="serviceScopeFactory">The service scope factory to use for
    /// creating new service scopes.</param>
    /// <param name="logger">The logger to use for logging messages.</param>
    public SimplePeriodicService(
        IServiceScopeFactory serviceScopeFactory, 
        ILogger<SimplePeriodicService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory 
                               ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
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

            await Task.Run(() => handler(stoppingToken), stoppingToken) 
                      .ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Processing canceled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Processing stopped");
            throw;
        }
    }

    private static Func<IHandler, CancellationToken, Task> DefaultHandler() =>
    async (handler, token) =>
    {
        await handler.HandleAsync("B", token).ConfigureAwait(false);
    };

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger _logger;
    private readonly PeriodicTimer _timer;
}
