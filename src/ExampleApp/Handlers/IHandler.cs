namespace ExampleApp.Handlers
{
    public interface IHandler
    {
        public Task HandleAsync(CancellationToken cancellationToken);
    }
}
