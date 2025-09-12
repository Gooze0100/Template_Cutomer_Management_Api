using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Shared.HealthChecks;

public class DegradedHealthCheck: IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Degraded("Degraded"));
    }
}