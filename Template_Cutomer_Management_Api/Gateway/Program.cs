using Gateway.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddSettings();
builder.AddAuthentication();
builder.AddOcelotSettings();

builder.AddCors();
builder.AddDependencies();
builder.AddOther();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCorsConfig();
app.UseAuthenticationConfig();
app.UseCache();
app.UseRouting();

await app.UseOcelotConfig();
await app.RunAsync();