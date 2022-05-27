# HandlerExtensionsExample

Example of how fluent helpers simplify work with handler in background service.

```C#
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
```
