namespace HandlerExtensions.Tests;

/// <summary>
/// Defines a handler interface for processing test data.
/// </summary>
#pragma warning disable CA1515 // Need for Mocking in tests
public interface ITestHandler
#pragma warning restore CA1515 
{
    /// <summary>
    /// Processes the specified test data asynchronously.
    /// </summary>
    /// <param name="data">The test data to be processed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(string data, CancellationToken cancellationToken);
}
