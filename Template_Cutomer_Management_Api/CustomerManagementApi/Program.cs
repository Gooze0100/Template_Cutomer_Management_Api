using CustomerManagementApi.Startup;

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

app.MapEndpoints();

app.Run();

// TODO: pagalvoti apie unicode ar tipo bus var char ar ne tas pats Body ir pan??? 