using BitcoinQuery.Service.Dto;

namespace BitcoinQuery.Service.Conrtacts;

public interface IBitcoinQueryService
{
    Task<BitcoinPriceData> GetLastPrice(CancellationToken token);
}