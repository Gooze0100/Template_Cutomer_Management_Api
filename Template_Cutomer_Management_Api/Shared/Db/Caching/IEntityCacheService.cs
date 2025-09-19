using Microsoft.EntityFrameworkCore;

namespace Shared.Db.Caching;

public interface IEntityCacheService<T>
    where T : class
{
    Task<List<T>> GetCachedEntitiesAsync(CancellationToken ct = default);
    Task InvalidateCacheAsync(Type entityType, CancellationToken ct = default);
}