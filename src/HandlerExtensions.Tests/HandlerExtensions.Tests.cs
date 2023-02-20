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

        [Fact(DisplayName = "WithScopedService returns scoped delegate that runs correctly.")]
        public async void ServiceScopeFactoryDelegateRunsCorrectly()
        {
            // Arrange
            ITestHandler resultHandler = null!;
            CancellationToken resultToken = CancellationToken.None;
            var callCount = 0;
            Func<ITestHandler, CancellationToken, Task> func = (handler, token) =>
            {
                resultHandler = handler;
                resultToken = token;
                callCount++;
                return Task.CompletedTask;
            };
            var handlerMock = new Mock<ITestHandler>(MockBehavior.Strict);
            var handlerTest = handlerMock.Object;
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
            serviceProviderMock.Setup(x => x.GetService(typeof(ITestHandler))).Returns(() => handlerTest);
            var scopeMock = new Mock<IServiceScope>(MockBehavior.Strict);
            scopeMock.Setup(x => x.ServiceProvider).Returns(() => serviceProviderMock.Object);
            scopeMock.Setup(x => x.Dispose());
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>(MockBehavior.Strict);
            serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(() => scopeMock.Object);
            var serviceScopeFactory = serviceScopeFactoryMock.Object;
            var resultFunc = Helpers.HandlerExtensions.WithScopedService(func, serviceScopeFactory);
            using var cts = new CancellationTokenSource();

            // Act
            await resultFunc(cts.Token);

            // Assert
            resultHandler.Should().NotBeNull();
            resultHandler.Should().Be(handlerTest);
            resultToken.Should().Be(cts.Token);
            callCount.Should().Be(1);
        }

        [Fact(DisplayName = "WithLoop throws when func is null.")]
        public void WithLoopThrowsArgumentNullExceptionForNullFunc()
        {
            // Act
            var exception = Record.Exception(() => Helpers.HandlerExtensions.WithLoop(null!));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

        [Fact(DisplayName = "WithLoop throws when func is null.")]
        public void WithLoopCanBeCreated()
        {
            //Arrange
            static Task func(CancellationToken _) => Task.CompletedTask;

            // Act
            var exception = Record.Exception(() => Helpers.HandlerExtensions.WithLoop(func));

            // Assert
            exception.Should().BeNull();
        }

        [Fact(DisplayName = "WithLoop not runs on cancelled token.")]
        public async void WithLoopNotRunsOnCancelledToken()
        {
            //Arrange
            CancellationToken resultToken = CancellationToken.None;
            var callCount = 0;
            Task func(CancellationToken token)
            {
                callCount++;
                resultToken = token;
                return Task.CompletedTask;
            }
            var resultFunc = Helpers.HandlerExtensions.WithLoop(func);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            var exception = await Record.ExceptionAsync(async () => await resultFunc(cts.Token));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<OperationCanceledException>();
            callCount.Should().Be(0);
            resultToken.Should().Be(CancellationToken.None);
        }

        [Fact(DisplayName = "WithLoop not runs on valid token.")]
        public async void WithLoopNotRunsOnValidToken()
        {
            //Arrange
            using var cts = new CancellationTokenSource();
            CancellationToken resultToken = CancellationToken.None;
            var callCount = 0;
            Task func(CancellationToken token)
            {
                callCount++;
                resultToken = token;
                cts.Cancel();
                return Task.CompletedTask;
            }
            var resultFunc = Helpers.HandlerExtensions.WithLoop(func);

            // Act
            var exception = await Record.ExceptionAsync(async () => await resultFunc(cts.Token));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<OperationCanceledException>();
            callCount.Should().Be(1);
            resultToken.Should().Be(cts.Token);
        }
    }
}