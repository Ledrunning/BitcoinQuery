using BitcoinQuery.Service.Dto;
using BitcoinQuery.Service.Models;

namespace BitcoinQuery.Service.Contracts;

public interface IBitcoinQueryService
{
    Task<BitcoinPriceData> GetLastPriceAsync(CancellationToken token);
    Task<BitcoinDailyData> GetDailyDataAsync(string date, CancellationToken token);
    Task<List<DataPoint>?> GetDataFromRangeAsync(CancellationToken token);
    double GetBitcoinClosingAverageFromRange(long startDate, long endDate);
    DateRange GetDateRange();
}