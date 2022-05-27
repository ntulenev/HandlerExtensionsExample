namespace ExampleApp.Handlers
{
    public class SimpleHandler : IHandler
    {
        public SimpleHandler(ILogger<SimpleHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);

            _logger.LogInformation("Operation handled");
        }

        private readonly ILogger<SimpleHandler> _logger;
    }
}
