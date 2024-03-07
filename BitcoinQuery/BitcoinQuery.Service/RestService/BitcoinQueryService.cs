using BitcoinQuery.Service.Conrtacts;
using BitcoinQuery.Service.Dto;
using Newtonsoft.Json;
using NLog;
using RestSharp;

namespace BitcoinQuery.Service.RestService;

public class BitcoinQueryService : BaseService, IBitcoinQueryService
{
    private readonly IBitcoinDataMapper _dataMapper;

    public BitcoinQueryService(ILogger logger, IBitcoinDataMapper dataMapper, string baseUrl, int timeout, string firstCurrency, string secondCurrency)
        : base(logger, baseUrl, timeout, firstCurrency, secondCurrency)
    {
        _dataMapper = dataMapper;
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

    /// <summary>
    ///     Historical 1m OHLCV Chart
    ///     https://cex.io/api/ohlcv/hd/{date}/{symbol1}/{symbol2}
    ///     Put the date in the next format: YYYYMMDD
    ///     1 - A timestamp (in this case represented by a number).
    ///     2 - Open (O): the price of the asset at the opening of the period.
    ///     3 - High(H) : the highest price of the asset during the period.
    ///     4 - Low(L): the lowest price of the asset during the period.
    ///     5 - Close(C): the price of the asset at the closing of the period. NOTE! I need this data in received data
    ///     6 - Volume(V): the amount of the asset traded during the period.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="token"></param>
    /// <returns>BitcoinDailyData with list of daily prices</returns>
    public async Task<BitcoinDailyData> GetDailyData(string date, CancellationToken token)
    {
        var endPoint = $"ohlcv/hd/{date}/{FirstCurrency}/{SecondCurrency}";
        var request = new RestRequest(endPoint);
        var response = await RestClient.ExecuteAsync(request, token);
        var receivedData = GetContent<BitcoinDailyData>(response, new Uri($"{BaseUrl}{endPoint}").AbsoluteUri);

        return receivedData;
    }

    private async Task<List<object>> GetDataFromRange(DateTime startDate, DateTime endDate, string symbol1, string symbol2)
    {
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            
        }

        return new List<object>();
    }
}