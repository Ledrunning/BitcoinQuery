using BitcoinQuery.Service.Models;

namespace BitcoinQuery.Service.Contracts;

public interface IDataCachingService
{
    void SaveDataToCache(List<BitcoinData>? yourData);
    List<BitcoinData>? GetLatestDataFromCache();
}