using FluentValidation;
using TemplateManagementApi.Services.Template;
using TemplateManagementApi.Validators.Template;

namespace TemplateManagementApi.Startup;

public static class Dependencies
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITemplateService, TemplateService>();

        builder.Services
            .AddValidatorsFromAssemblyContaining<TemplateAddValidator>()
            .AddValidatorsFromAssemblyContaining<TemplateUpdateValidator>()
            .AddValidatorsFromAssemblyContaining<TemplateDeleteValidator>();
    }
}