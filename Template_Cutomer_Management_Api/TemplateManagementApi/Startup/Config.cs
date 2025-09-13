using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Shared;
using TemplateInfrastructure.Context;
using TemplateInfrastructure.Context.DataSeed;
using TemplateManagementApi.Settings;
using Infrastructure = TemplateInfrastructure.Constants;

namespace TemplateManagementApi.Startup;

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
    
    public static void AddOpenApiServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<IDatabaseContext, DatabaseContext>((serviceProvider, optionsBuilder) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(Constants.Config.DefaultConnectionString);
            ArgumentNullException.ThrowIfNull(connectionString);
            
            optionsBuilder.UseSqlServer(connectionString,
                contextOptionsBuilder =>
                {
                    contextOptionsBuilder.MigrationsHistoryTable(Infrastructure.Tables.MigrationTable, Infrastructure.Schemas.Dbo);
                });
            
            optionsBuilder.UseAsyncSeeding(GenericSeed.Seed);
        });
    }
    
    public static void AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwtOptions =>
        {
            string[] validIssuers = builder.Configuration
                .GetSection($"{Constants.Config.AuthenticationBearerSectionKey}:ValidIssuers").Get<string[]>();

            string[] validAudiences = builder.Configuration
                .GetSection($"{Constants.Config.AuthenticationBearerSectionKey}:ValidAudiences").Get<string[]>();
            
            string issuerSigningKey = builder.Configuration
                .GetSection($"{Constants.Config.AuthenticationBearerSectionKey}:IssuerSigningKey").Get<string>();
            
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuers = validIssuers,
                ValidAudiences = validAudiences,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey))
            };
        });
        
        builder.Services.AddAuthorization();
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

    public static void AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddSqlServer(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString =
                    configuration.GetConnectionString(Constants.Config.DefaultConnectionString);
                return connectionString!;
            }, name: "Database");
    }
    
    public static void AddOther(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddLogging(logging => { logging.AddConsole(); });

        builder.Services.AddAllHealthChecks();
        
        builder.Services
            .AddMemoryCache()
            .AddOutputCache(options =>
            {
                options.DefaultExpirationTimeSpan = TimeSpan.FromHours(2);
            });
    }
    
    public static void UseOpenApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "Template Management Api";
                options.Theme = ScalarTheme.DeepSpace;
                options.Layout = ScalarLayout.Modern;
                options.HideClientButton = true;
            });
        }
    }

    public static async Task UseDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        using var context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
        await context.Database.MigrateAsync();
    }
    
    public static void UseCorsConfig(this WebApplication app)
    {
        app.UseCors();
    }
    
    public static void UseAuthenticationConfig(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
    
    public static void UseCache(this WebApplication app)
    {
        app.UseOutputCache();
    }
}