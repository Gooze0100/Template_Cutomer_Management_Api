namespace AuthApi.Startup;

public static class CorsConfig
{
    public static void AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                var appSettings = builder.Configuration
                    .GetSection("AccessControlAllowOrigin")
                    .Get<string[]>();
                
                policy.WithOrigins(appSettings)
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }
    
    public static void UseCorsConfig(this WebApplication app)
    {
        app.UseCors();
    }
}