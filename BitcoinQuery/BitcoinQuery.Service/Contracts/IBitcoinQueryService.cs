using BitcoinQuery.Service.Dto;
using BitcoinQuery.Service.Models;

namespace BitcoinQuery.Service.Contracts;

public interface IBitcoinQueryService
{
    Task<BitcoinPriceData> GetLastPriceAsync(CancellationToken token);
    Task<BitcoinDailyData> GetDailyDataAsync(string date, CancellationToken token);
    Task<List<BitcoinData>?> GetDataFromRangeAsync(bool isManualUpdate, CancellationToken token);
}