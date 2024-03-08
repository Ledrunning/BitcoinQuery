using BitcoinQuery.Service.Contracts;
using BitcoinQuery.Service.Dto;
using BitcoinQuery.Service.Exceptions;
using NLog;
using RestSharp;

namespace BitcoinQuery.Service.Service.RestService;

public class BitcoinQueryService : BaseService, IBitcoinQueryService
{
    private const short FirstDayOfMonth = 1;
    private const short OneMonth = 1;
    private const short DecreaseForLastDayInCurrentMonth = -1;
    private readonly IDataCachingService _cachingService;
    private readonly IBitcoinDataMapper _dataMapper;

    public BitcoinQueryService(ILogger logger, IBitcoinDataMapper dataMapper, IDataCachingService cachingService,
        string baseUrl, int timeout, string firstCurrency, string secondCurrency)
        : base(logger, baseUrl, timeout, firstCurrency, secondCurrency)
    {
        _dataMapper = dataMapper;
        _cachingService = cachingService;
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

    /// <summary>
    ///     Method of obtaining data for the last month
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="BitcoinQueryServiceException"></exception>
    public async Task<List<double[][]>> GetDataFromRange(CancellationToken token)
    {
        GetDateRange(out var startDate, out var endDate);

        var dataPerDay = new List<double[][]>();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var dailyData = await GetDailyData(date.ToString("yyyyMMdd"), token);
            if (dailyData.DataPerDay != null)
            {
                dataPerDay.Add(dailyData.DataPerDay);
            }
        }

        _cachingService.SaveDataToCacheAsync(dataPerDay);
        var resultData = _cachingService.GetLatestDataFromCacheAsync();
        if (resultData != null)
        {
            return resultData;
        }

        Logger.Error($"{nameof(BitcoinQueryService)}: Bitcoin data per day is null");
        throw new BitcoinQueryServiceException($"{nameof(BitcoinQueryService)}: Bitcoin data per day is null");
    }

    private static void GetDateRange(out DateTime startDate, out DateTime endDate)
    {
        startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, FirstDayOfMonth);
        endDate = startDate.AddMonths(OneMonth).AddDays(DecreaseForLastDayInCurrentMonth);
    }
}