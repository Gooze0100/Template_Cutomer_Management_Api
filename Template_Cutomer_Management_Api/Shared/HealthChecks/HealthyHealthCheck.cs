using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Shared.HealthChecks;

public class HealthyHealthCheck: IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("Healthy"));
    }
}