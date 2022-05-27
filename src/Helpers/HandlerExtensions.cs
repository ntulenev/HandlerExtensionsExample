using Microsoft.Extensions.DependencyInjection;

namespace Helpers
{
    public static class HandlerExtensions
    {
        public static Func<CancellationToken, Task> WithScopedService<TService>(
        this Func<TService, CancellationToken, Task> func,
        IServiceScopeFactory serviceScopeFactory) where TService : notnull =>
        async token =>
        {
            using var serviceScope = serviceScopeFactory.CreateScope();

            var service = serviceScope.ServiceProvider.GetRequiredService<TService>();

            await func(service, token).ConfigureAwait(false);
        };

        public static Func<CancellationToken, Task> WithLoop(
        this Func<CancellationToken, Task> func,
        PeriodicTimer periodicTimer) =>
        async token =>
        {
            while (!token.IsCancellationRequested)
            {
                if (await periodicTimer.WaitForNextTickAsync(token).ConfigureAwait(false))
                {
                    await func(token).ConfigureAwait(false);
                }
            }

            token.ThrowIfCancellationRequested();
        };

        public static Func<CancellationToken, Task> WithLoop(this Func<CancellationToken, Task> func) =>
        async token =>
        {
            while (!token.IsCancellationRequested)
            {
                await func(token).ConfigureAwait(false);
            }

            token.ThrowIfCancellationRequested();
        };
    }
}
