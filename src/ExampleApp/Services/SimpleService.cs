using ExampleApp.Handlers;

using Helpers;

namespace ExampleApp.Services;

/// <summary>
/// Represents a simple background service that executes a default handler with a scoped service and a loop.
/// </summary>
public class SimpleService : BackgroundService
{

    /// <summary>
    /// Initializes a new instance of the SimpleService class with the specified service scope factory and logger.
    /// </summary>
    /// <param name="serviceScopeFactory">The service scope factory.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Thrown when serviceScopeFactory or logger is null.</exception>

    public SimpleService(IServiceScopeFactory serviceScopeFactory, ILogger<SimpleService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var handler = DefaultHandler()
                .WithScopedService(_serviceScopeFactory)
                .WithLoop();

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
        await handler.HandleAsync("A", token).ConfigureAwait(false);
    };

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<SimpleService> _logger;
}
