using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Shared.HealthChecks;

public class UnhealthHealthCheck: IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Unhealthy("Unhealthy"));
    }
}