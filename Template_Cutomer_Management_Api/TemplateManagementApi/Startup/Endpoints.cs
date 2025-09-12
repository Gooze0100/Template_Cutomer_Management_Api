using TemplateManagementApi.Endpoints;

namespace TemplateManagementApi.Startup;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapTemplateEndpoints();
        app.MapAllHealthChecks();
    }
}