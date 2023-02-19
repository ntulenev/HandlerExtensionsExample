# HandlerExtensionsExample

This is a collection of delegate-based helper methods that simplify routine operation with handlers in Background services in .NET.

## Helper methods

### WithScopedService
Runs the specified delegate func with a scoped TService instance from the DI container.

```C#
public static Func<CancellationToken, Task> WithScopedService<TService>(
    this Func<TService, CancellationToken, Task> func,
    IServiceScopeFactory serviceScopeFactory) where TService : notnull
```

### WithLoop
Runs the specified delegate func in a loop.

```C#
public static Func<CancellationToken, Task> WithLoop(this Func<CancellationToken, Task> func)
```

Runs the specified delegate func in a loop with a specific timer.

```C#
public static Func<CancellationToken, Task> WithLoop(
    this Func<CancellationToken, Task> func,
    PeriodicTimer periodicTimer)
```

### Usage
Example usage fluent helpers to simplify work with handler in background service.

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
