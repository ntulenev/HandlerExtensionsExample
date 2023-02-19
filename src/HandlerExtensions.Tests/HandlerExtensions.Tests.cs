using Microsoft.Extensions.DependencyInjection;

using FluentAssertions;

using Moq;

namespace HandlerExtensions.Tests
{
    public class HandlerExtensionsTests
    {
        [Fact(DisplayName = "WithScopedService throws when func is null.")]
        public void WithScopedServiceThrowsArgumentNullExceptionForNullFunc()
        {
            // Arrange
            var serviceScopeFactory = new Mock<IServiceScopeFactory>(MockBehavior.Strict).Object;

            // Act
            var exception = Record.Exception(() => Helpers.HandlerExtensions.WithScopedService<string>(null!, serviceScopeFactory));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

        [Fact(DisplayName = "WithScopedService throws when serviceScopeFactory is null.")]
        public void WithScopedServiceThrowsArgumentNullExceptionForNullServiceScopeFactory()
        {
            // Arrange
            Func<string, CancellationToken, Task> func = (_, _) => Task.CompletedTask;

            // Act
            var exception = Record.Exception(() => Helpers.HandlerExtensions.WithScopedService(func, null!));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

        [Fact(DisplayName = "WithScopedService returns scoped delegate on valid params.")]
        public void WithScopedServiceReturnServiceScopeFactoryDelegate()
        {
            // Arrange
            Func<string, CancellationToken, Task> func = (_, _) => Task.CompletedTask;
            var serviceScopeFactory = new Mock<IServiceScopeFactory>(MockBehavior.Strict).Object;

            // Act
            var exception = Record.Exception(() => Helpers.HandlerExtensions.WithScopedService(func, serviceScopeFactory));

            // Assert
            exception.Should().BeNull();
        }
    }
}