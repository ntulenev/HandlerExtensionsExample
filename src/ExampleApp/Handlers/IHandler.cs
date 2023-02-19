namespace ExampleApp.Handlers;

/// <summary>
/// Defines the contract for implementing a handler.
/// </summary>
public interface IHandler
{
    /// <summary>
    /// Handles the incoming data asynchronously.
    /// </summary>
    /// <param name="data">The data to be handled.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task HandleAsync(string data, CancellationToken cancellationToken);
}
