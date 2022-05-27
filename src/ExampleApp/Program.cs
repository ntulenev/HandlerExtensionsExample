using ExampleApp.Handlers;
using ExampleApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<SimpleService>();
builder.Services.AddScoped<IHandler, SimpleHandler>();
builder.Services.AddHealthChecks();
var app = builder.Build();
app.UseHealthChecks("/hñ");
await app.RunAsync().ConfigureAwait(false);

