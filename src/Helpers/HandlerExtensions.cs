using Microsoft.Extensions.DependencyInjection;

namespace Helpers
{
    public static class HandlerExtensions
    {
        public static Func<CancellationToken, Task> WithScopedService<TService>(
        this Func<TService, CancellationToken, Task> func,
        IServiceScopeFactory serviceScopeFactory) where TService : notnull
        {
            ArgumentNullException.ThrowIfNull(func);
            ArgumentNullException.ThrowIfNull(serviceScopeFactory);

            return async token =>
            {
                using var serviceScope = serviceScopeFactory.CreateScope();

                var service = serviceScope.ServiceProvider.GetRequiredService<TService>();

                await func(service, token).ConfigureAwait(false);
            };
        }

        public static Func<CancellationToken, Task> WithLoop(
        this Func<CancellationToken, Task> func,
        PeriodicTimer periodicTimer)
        {
            ArgumentNullException.ThrowIfNull(func);
            ArgumentNullException.ThrowIfNull(periodicTimer);

            return async token =>
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
        }

        public static Func<CancellationToken, Task> WithLoop(this Func<CancellationToken, Task> func)
        {
            ArgumentNullException.ThrowIfNull(func);

            return async token =>
            {
                while (!token.IsCancellationRequested)
                {
                    await func(token).ConfigureAwait(false);
                }

                token.ThrowIfCancellationRequested();
            };
        }
    }
}
