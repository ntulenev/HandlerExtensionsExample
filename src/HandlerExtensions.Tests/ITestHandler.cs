namespace HandlerExtensions.Tests;

/// <summary>
/// Defines a handler interface for processing test data.
/// </summary>
public interface ITestHandler
{
    /// <summary>
    /// Processes the specified test data asynchronously.
    /// </summary>
    /// <param name="data">The test data to be processed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task HandleAsync(string data, CancellationToken cancellationToken);
}
