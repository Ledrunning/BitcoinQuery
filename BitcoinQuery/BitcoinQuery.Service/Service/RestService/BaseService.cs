using BitcoinQuery.Service.Dto;
using BitcoinQuery.Service.Exceptions;
using Newtonsoft.Json;
using NLog;
using RestSharp;

namespace BitcoinQuery.Service.Service.RestService;

public class BaseService
{
    private readonly int _timeout;
    protected readonly string BaseUrl;
    protected readonly string FirstCurrency;
    protected readonly ILogger Logger;
    protected readonly RestClient RestClient;
    protected readonly string SecondCurrency;

    public BaseService(ILogger logger, string baseUrl, int timeout, string firstCurrency, string secondCurrency)
    {
        Logger = logger;
        BaseUrl = baseUrl;
        _timeout = timeout;
        FirstCurrency = firstCurrency;
        SecondCurrency = secondCurrency;

        RestClient = new RestClient(SetOptions(new Uri(BaseUrl)));
    }

    protected T GetContent<T>(RestResponseBase response, string url)
    {
        if (response.IsSuccessful)
        {
            if (response.Content != null)
            {
                var model = JsonConvert.DeserializeObject<T>(response.Content);
                Logger.Info("Request for cex.io successfully finished {Url}", url);
                if (model != null)
                {
                    return model;
                }

                Logger.Info("Requested data from cex.io is null {Url}", url);
            }
        }

        throw new BitcoinQueryServiceException(
            $"Response from cex.io failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
    }

    protected BitcoinDailyData GetDailyDataContent(RestResponseBase response, string url)
    {
        if (response.IsSuccessful)
        {
            if (!string.IsNullOrWhiteSpace(response.Content) && response.Content != "[]")
            {
                var model = JsonConvert.DeserializeObject<BitcoinDailyData>(response.Content);
                Logger.Info("Request for cex.io successfully finished {Url}", url);
                if (model != null)
                {
                    return model;
                }

                Logger.Info("Requested data from cex.io is null {Url}", url);
            }
            else
            {
                return new BitcoinDailyData();
            }
        }

        throw new BitcoinQueryServiceException(
            $"Response from cex.io failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
    }

    protected RestClientOptions SetOptions(Uri url)
    {
        return new RestClientOptions(url)
        {
            ThrowOnAnyError = true,
            MaxTimeout = _timeout
        };
    }
}