using AuthApi.Service;
using AuthApi.Startup;
using Microsoft.AspNetCore.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddSettings();

builder.Services.AddOpenApi();

builder.AddCors();

builder.Services.AddSingleton<TokenGenerator>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseOpenApi();
app.UseCorsConfig();

app.MapPost("/login", (LoginRequest req, TokenGenerator tokenGenerator) =>
new {
    access_token = tokenGenerator.GenerateToken(req.Email)
});

app.Run();