namespace ExampleApp.Handlers;

public interface IHandler
{
    public Task HandleAsync(string data, CancellationToken cancellationToken);
}
