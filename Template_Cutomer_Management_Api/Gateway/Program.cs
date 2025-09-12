using Gateway.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddSettings();

builder.AddCors();
builder.AddDependencies();
builder.AddOther();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCorsConfig();
app.UseCache();

await app.UseOcelotConfig();
await app.RunAsync();