using ExampleApp.Handlers;

using Helpers;

namespace ExampleApp.Services
{
    public class SimpleService : BackgroundService
    {
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
            await handler.HandleAsync(token).ConfigureAwait(false);
        };

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<SimpleService> _logger;
    }
}
