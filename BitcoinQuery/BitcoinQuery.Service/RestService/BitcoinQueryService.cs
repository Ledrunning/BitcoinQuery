using BitcoinQuery.Service.Conrtacts;
using BitcoinQuery.Service.Dto;
using NLog;
using RestSharp;

namespace BitcoinQuery.Service.RestService;

public class BitcoinQueryService : BaseService, IBitcoinQueryService
{
    public BitcoinQueryService(ILogger logger, string baseUrl, int timeout, string firstCurrency, string secondCurrency)
        : base(logger, baseUrl, timeout, firstCurrency, secondCurrency)
    {
    }

    /// <summary>
    ///     Method for getting current price
    ///     https://cex.io/api/last_price/BTC/USD
    /// </summary>
    /// <param name="token"></param>
    /// <returns>BitcoinPriceData current price</returns>
    public async Task<BitcoinPriceData> GetLastPrice(CancellationToken token)
    {
        var endPoint = $"last_price/{FirstCurrency}/{SecondCurrency}";
        var request = new RestRequest(endPoint);
        var response = await RestClient.ExecuteAsync(request, token);

        var receivedData = GetContent<BitcoinPriceData>(response, new Uri($"{BaseUrl}{endPoint}").AbsoluteUri);
        return receivedData;
    }
}