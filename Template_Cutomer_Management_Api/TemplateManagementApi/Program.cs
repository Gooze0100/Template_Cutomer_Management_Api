using TemplateManagementApi.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenApiServices();
builder.AddHealthChecks();
builder.AddConfiguration();
builder.AddSettings();
builder.AddDatabase();

builder.AddAuthentication();
builder.AddCors();
builder.AddDependencies();
builder.AddOther();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseOpenApi();

await app.UseDatabase();

app.UseCorsConfig();
app.UseAuthenticationConfig();
app.UseCache();

app.MapEndpoints();

app.Run();