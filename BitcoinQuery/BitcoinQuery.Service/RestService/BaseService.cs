using BitcoinQuery.Service.Exceptions;
using Newtonsoft.Json;
using NLog;
using RestSharp;

namespace BitcoinQuery.Service.RestService;

public class BaseService
{
    private readonly ILogger _logger;
    private readonly int _timeout;
    protected readonly string BaseUrl;
    protected readonly string FirstCurrency;
    protected readonly string SecondCurrency;

    public BaseService(ILogger logger, string baseUrl, int timeout, string firstCurrency, string secondCurrency)
    {
        _logger = logger;
        BaseUrl = baseUrl;
        _timeout = timeout;
        FirstCurrency = firstCurrency;
        SecondCurrency = secondCurrency;
    }

    protected T GetContent<T>(RestResponseBase response, string url)
    {
        if (response.IsSuccessful)
        {
            if (response.Content != null)
            {
                var model = JsonConvert.DeserializeObject<T>(response.Content);
                _logger.Info("Request for cex.io successfully finished {Url}", url);
                if (model != null)
                {
                    return model;
                }

                _logger.Info("Requested data from cex.io is null {Url}", url);
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