using BitcoinQuery.Service.Contracts;
using BitcoinQuery.Service.Dto;
using BitcoinQuery.Service.Exceptions;
using BitcoinQuery.Service.Models;
using NLog;
using RestSharp;

namespace BitcoinQuery.Service.Service.RestService;

public class BitcoinQueryService : BaseService, IBitcoinQueryService
{
    private const short DecreaseOneMonth = -1;
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
    public async Task<BitcoinPriceData> GetLastPriceAsync(CancellationToken token)
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
    public async Task<BitcoinDailyData> GetDailyDataAsync(string date, CancellationToken token)
    {
        var endPoint = $"ohlcv/hd/{date}/{FirstCurrency}/{SecondCurrency}";
        var request = new RestRequest(endPoint);
        var response = await RestClient.ExecuteAsync(request, token);
        var receivedData = GetDailyDataContent(response, new Uri($"{BaseUrl}{endPoint}").AbsoluteUri);

        return receivedData;
    }

    /// <summary>
    ///     Method of obtaining data for the last month
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="BitcoinQueryServiceException"></exception>
    public async Task<List<DataPoint>?> GetDataFromRangeAsync(CancellationToken token)
    {
        try
        {
            var resultDateRange = GetDateRange();

            var dataPerDay = new List<DataPoint>();
            for (var date = resultDateRange.StartDate; date <= resultDateRange.EndDate; date = date.AddDays(1))
            {
                var dailyData = await GetDailyDataAsync(date.ToString("yyyyMMdd"), token);
                var lastPrice = await GetLastPriceAsync(token);
                var mappedData = _dataMapper.MapToDataPoints(dailyData.DataPerDay, lastPrice);

                dataPerDay.AddRange(mappedData);
            }

            // In order not to send unnecessary requests to the server, we will work with memory cache
            _cachingService.SaveDataToCache(dataPerDay);

            return dataPerDay;
        }
        catch (Exception e)
        {
            Logger.Error(e, $"{nameof(BitcoinQueryService)}: error when obtaining data from month range");
            throw new BitcoinQueryServiceException(
                $"{nameof(BitcoinQueryService)}: error when obtaining data from month range");
        }
    }

    public static DateRange GetDateRange()
    {
        var today = DateTime.Today;
        return new DateRange
        {
            StartDate = today.AddMonths(DecreaseOneMonth),
            EndDate = today
        };
    }
}