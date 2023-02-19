namespace ExampleApp.Handlers;

/// <summary>
/// Implements the IHandler interface for handling data asynchronously.
/// </summary>
public class SimpleHandler : IHandler
{
    /// <summary>
    /// Initializes a new instance of the SimpleHandler class.
    /// </summary>
    /// <param name="logger">The logger used to log events.</param>
    /// <exception cref="ArgumentNullException">Thrown when logger is null.</exception>
    public SimpleHandler(ILogger<SimpleHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the given data asynchronously.
    /// </summary>
    /// <param name="data">The data to be handled.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>

    public async Task HandleAsync(string data, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(data);

        await Task.Delay(1000, cancellationToken);

        _logger.LogInformation("Operation handled for data : {data} ", data);
    }

    private readonly ILogger<SimpleHandler> _logger;
}
