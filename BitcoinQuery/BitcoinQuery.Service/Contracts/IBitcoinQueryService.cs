using BitcoinQuery.Service.Dto;

namespace BitcoinQuery.Service.Contracts;

public interface IBitcoinQueryService
{
    Task<BitcoinPriceData> GetLastPrice(CancellationToken token);
    Task<BitcoinDailyData> GetDailyData(string date, CancellationToken token);
    Task<List<double[][]>> GetDataFromRange(CancellationToken token);
}