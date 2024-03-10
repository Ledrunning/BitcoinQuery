using BitcoinQuery.Service.Contracts;
using BitcoinQuery.Service.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BitcoinQuery.Service.Service;

/// <summary>
///     NOTE!
///     This class is created to temporarily store data to avoid repeated
///     requests to the cex.io server. A separate domain layer with CRUD
///     operations and a database, such as Cosmos Db, could also be used here.
/// </summary>
public class DataCachingService : IDataCachingService
{
    private const short Day = 24;
    private readonly ILogger<DataCachingService> _logger;
    private readonly IMemoryCache _memoryCache;

    public DataCachingService(IMemoryCache memoryCache, ILogger<DataCachingService> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public void SaveDataToCache(List<DataPoint>? yourData)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(Day)); // Настройте время жизни кэша по вашему усмотрению

        _logger.LogInformation("The latest data has been stored in cache memory!");
        _memoryCache.Set("LatestData", yourData, cacheEntryOptions);
    }

    public List<DataPoint>? GetLatestDataFromCache()
    {
        if (_memoryCache.TryGetValue("LatestData", out List<DataPoint>? cachedData))
        {
            _logger.LogInformation("Retrieving the latest data from the cache memory");
            return cachedData;
        }

        _logger.LogError("Recent data is missing...");
        return null;
    }
}