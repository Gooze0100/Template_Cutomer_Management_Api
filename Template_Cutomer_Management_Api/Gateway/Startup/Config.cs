using Gateway.Settings;
using Shared;

namespace Gateway.Startup;

public static class Config
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true);
    }

    public static void AddSettings(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions();
        
        builder.Services.AddOptions<AppSettings>()
            .BindConfiguration(Constants.Config.AppSettingsSectionKey)
            .Validate(config => config.AccessControlAllowOrigin.Count > 0, "Empty access control allow origin configuration.")
            .ValidateOnStart();
    }
    public static void AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                var appSettings = builder.Configuration.GetSection(Constants.Config.AppSettingsSectionKey).Get<AppSettings>();
                policy.WithOrigins(appSettings.AccessControlAllowOrigin.ToArray())
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }
    public static void AddOther(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddLogging(logging => { logging.AddConsole(); });
        
        builder.Services
            .AddMemoryCache()
            .AddOutputCache(options =>
            {
                options.DefaultExpirationTimeSpan = TimeSpan.FromHours(2);
            });
    }
    
    public static void UseCorsConfig(this WebApplication app)
    {
        app.UseCors();
    }
    
    public static void UseCache(this WebApplication app)
    {
        app.UseOutputCache();
    }
}