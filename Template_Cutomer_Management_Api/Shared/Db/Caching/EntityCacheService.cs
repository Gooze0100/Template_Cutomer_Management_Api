using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using TemplateInfrastructure.Context;

namespace Shared.Db.Caching;

public class EntityCacheService<T> : IEntityCacheService<T>
    where T : class
{
    private readonly HybridCache _hybridCache;
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<EntityCacheService<T>> _logger;
    private readonly ConcurrentDictionary<Type, object> _localCache = new();
    
    private static readonly Meter Meter = new("Shared.Db.Caching", "1.0");
    private static readonly Counter<long> CacheHitsCounter = Meter.CreateCounter<long>("cache_hits");
    private static readonly Counter<long> CacheMissesCounter = Meter.CreateCounter<long>("cache_misses");
    private static readonly Counter<long> CacheInvalidationsCounter = Meter.CreateCounter<long>("cache_invalidations");
    private static readonly Histogram<long> CacheEntrySizeHistogram = Meter.CreateHistogram<long>("cache_entry_size_bytes");
    
    public EntityCacheService(HybridCache hybridCache, DatabaseContext dbContext, ILogger<EntityCacheService<T>> logger)
    {
        _hybridCache = hybridCache ?? throw new ArgumentNullException(nameof(hybridCache));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<List<T>> GetCachedEntitiesAsync(CancellationToken ct = default)
    {
        var type = typeof(T);
        string cacheKey = GenerateCacheKey(type);
        
        if (_localCache.TryGetValue(type, out var cachedObj) && cachedObj is List<T> cachedEntities)
        {
            _logger.LogDebug("Cache hit: {CacheKey}", cacheKey);
            CacheHitsCounter.Add(1);
            return cachedEntities;
        }
        
        _logger.LogDebug("Cache miss: {CacheKey}", cacheKey);
        CacheMissesCounter.Add(1);
        
        var cachedEntry = await _hybridCache.GetOrCreateAsync(
            cacheKey, 
            async token => await _dbContext.Set<T>().AsNoTracking().ToListAsync(token),
            tags: [$"cache_tag_{type.Name}"], 
            cancellationToken: ct);
        
        var approxSize = EstimateSize(cachedEntry);
        CacheEntrySizeHistogram.Record(approxSize);
        
        _localCache[type] = cachedEntry;
        return cachedEntry;
    }

    public async Task InvalidateCacheAsync(Type entityType, CancellationToken ct = default)
    {
        string cacheKey = GenerateCacheKey(entityType);
        
        _logger.LogInformation("Invalidating cache for {CacheKey}", cacheKey);
        CacheInvalidationsCounter.Add(1);
        
        _localCache.TryRemove(entityType, out _);
        await _hybridCache.RemoveAsync(cacheKey, ct);
        await _hybridCache.RemoveByTagAsync($"cache_tag_{entityType.Name}", ct);
    }
    
    private string GenerateCacheKey(Type entityType) => $"cached_entities_{entityType.FullName}";

    private long EstimateSize<TItem>(List<TItem> data)
    {
        const int approxSizePerItem = 1024;
        return data.Count * approxSizePerItem;
    }
}