using FluentValidation;
using Shared.Db.Caching;
using TemplateInfrastructure.Db.Entities;
using TemplateManagementApi.Services.Template;
using TemplateManagementApi.Validators.Template;

namespace TemplateManagementApi.Startup;

public static class Dependencies
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITemplateService, TemplateService>();
        builder.Services.AddScoped(typeof(IEntityCacheService<>), typeof(EntityCacheService<>));

        builder.Services
            .AddValidatorsFromAssemblyContaining<TemplateAddValidator>()
            .AddValidatorsFromAssemblyContaining<TemplateUpdateValidator>()
            .AddValidatorsFromAssemblyContaining<TemplateDeleteValidator>();
    }
}