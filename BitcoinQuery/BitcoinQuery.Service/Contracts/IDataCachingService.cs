using BitcoinQuery.Service.Models;

namespace BitcoinQuery.Service.Contracts;

public interface IDataCachingService
{
    void SaveDataToCache(List<DataPoint>? yourData);
    List<DataPoint>? GetLatestDataFromCache();
}