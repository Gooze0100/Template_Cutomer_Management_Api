using AuthApi.Settings;
using Scalar.AspNetCore;
using Shared;

namespace AuthApi.Startup;

public static class Config
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
            .AddEnvironmentVariables();
    }
    
    public static void AddSettings(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions();

        builder.Services.AddOptions<AppSettings>()
            .BindConfiguration(Constants.Config.AppSettingsSectionKey)
            .Validate(config => config.JwtHeader.ContainsKey("kid") && config.JwtHeader["kid"] is string key && !string.IsNullOrWhiteSpace(key), "No key assigned")
            .Validate(config => config.JwtPayload.ContainsKey("iss") && config.JwtPayload["iss"] is string issuer && !string.IsNullOrWhiteSpace(issuer), "No issuer assigned")
            .Validate(config => config.JwtPayload.ContainsKey("aud") && config.JwtPayload["aud"] is string audience && !string.IsNullOrWhiteSpace(audience), "No audience assigned")
            .ValidateOnStart();
    }
    
    public static void UseOpenApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "Auth API";
                options.Theme = ScalarTheme.Mars;
                options.Layout = ScalarLayout.Modern;
                options.HideClientButton = true;
            });
        }
    }
}