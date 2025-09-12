using CustomerManagementApi.Endpoints;

namespace CustomerManagementApi.Startup;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapCustomerEndpoints();
        app.MapAllHealthChecks();
    }
}