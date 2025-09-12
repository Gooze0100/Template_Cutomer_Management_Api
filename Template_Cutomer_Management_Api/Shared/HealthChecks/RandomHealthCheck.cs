using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Shared.HealthChecks;

public class RandomHealthCheck: IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        int randomResult = Random.Shared.Next(1, 4);
        
        return randomResult switch
        {
            1 => Task.FromResult(HealthCheckResult.Healthy("Healthy random")),
            2 => Task.FromResult(HealthCheckResult.Degraded("Degraded random")),
            3 => Task.FromResult(HealthCheckResult.Unhealthy("Unhealthy random")),
            _ => Task.FromResult(HealthCheckResult.Healthy("Anything else random"))
        };
    }
}