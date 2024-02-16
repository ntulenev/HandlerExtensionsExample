using ExampleApp.Handlers;
using ExampleApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<SimpleService>();
builder.Services.AddHostedService<SimplePeriodicService>();
builder.Services.AddScoped<IHandler, SimpleHandler>();
builder.Services.AddHealthChecks();
builder.Host.UseDefaultServiceProvider(x =>
{
    x.ValidateScopes = true;
    x.ValidateOnBuild = true;
});
var app = builder.Build();
app.UseHealthChecks("/hc");
await app.RunAsync().ConfigureAwait(false);

