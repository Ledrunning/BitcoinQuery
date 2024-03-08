namespace BitcoinQuery.Service.Contracts;

public interface IDataCachingService
{
    void SaveDataToCacheAsync(List<double[][]>? yourData);
    List<double[][]>? GetLatestDataFromCacheAsync();
}