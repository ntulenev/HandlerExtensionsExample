using Microsoft.Extensions.DependencyInjection;

namespace Helpers
{
    /// <summary>
    /// Delegate-based helpers methods that simplify routine operation
    /// with handlers in Background services.
    /// </summary>
    public static class HandlerExtensions
    {
        /// <summary>
        /// Helpers runs <paramref name="func"/> with <typeparamref name="TService"/> from scope.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <param name="func">Target delegate.</param>
        /// <param name="serviceScopeFactory">DI service factory.</param>
        /// <returns>Decorated delegate with service from scope.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="func"/>
        /// or <paramref name="serviceScopeFactory"/> is null.</exception>
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

        /// <summary>
        /// Runs target delegate <paramref name="func"/> in loop with specific timer.
        /// </summary>
        /// <param name="func">Target delegate.</param>
        /// <param name="periodicTimer">Timer for the loop.</param>
        /// <returns>Decorated delegate with timer loop.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="func"/>
        /// or <paramref name="periodicTimer"/> is null.</exception>
        public static Func<CancellationToken, Task> WithLoop(
        this Func<CancellationToken, Task> func,
        PeriodicTimer periodicTimer)
        {
            ArgumentNullException.ThrowIfNull(func);
            ArgumentNullException.ThrowIfNull(periodicTimer);

            return async token =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    
                    if (await periodicTimer.WaitForNextTickAsync(token)
                                           .ConfigureAwait(false))
                    {
                        await func(token).ConfigureAwait(false);
                    }
                }
            };
        }

        /// <summary>
        /// Runs target delegate <paramref name="func"/> in loop.
        /// </summary>
        /// <param name="func">Target delegate.</param>
        /// <returns>Decorated delegate with loop.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="func"/> is null</exception>
        public static Func<CancellationToken, Task> WithLoop(this Func<CancellationToken, Task> func)
        {
            ArgumentNullException.ThrowIfNull(func);

            return async token =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    await func(token).ConfigureAwait(false);
                }
            };
        }
    }
}
