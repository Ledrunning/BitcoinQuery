using System;
using System.Threading;
using System.Threading.Tasks;
using BitcoinQuery.DesktopClient.Configuration;
using BitcoinQuery.DesktopClient.Exceptions;
using Newtonsoft.Json;
using RestSharp;

namespace BitcoinQuery.DesktopClient.Rest
{
    public class BaseRestClientService
    {
        private readonly int _timeout;
        protected readonly string BaseUrl;

        public BaseRestClientService(AppConfig appConfig)
        {
            BaseUrl = appConfig.BaseServerUri;
            _timeout = appConfig.Timeout;
        }

        /// <summary>
        ///     Method for testing the config parameters
        /// </summary>
        /// <returns></returns>
        public (string baseUrl, int timeout) GetTimeoutAndBaseUrl()
        {
            return (BaseUrl, _timeout);
        }

        public T GetContent<T>(RestResponseBase response)
        {
            CheckResponse(response);

            if (response.Content == null)
            {
                throw new RestClientServiceException(
                    $"Response from BitcoinQuery.WebGateway is failed. Content is null: {response.StatusCode}, {response.ErrorMessage}");
            }

            var model = JsonConvert.DeserializeObject<T>(response.Content);
            if (model != null)
            {
                return model;
            }

            throw new RestClientServiceException(
                $"Response from BitcoinQuery.WebGateway is failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
        }

        protected void CheckResponse(RestResponseBase response)
        {
            if (!response.IsSuccessful)
            {
                throw new RestClientServiceException(
                    $"Response from BitcoinQuery.WebGateway is failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
            }

            if (response.Content == null)
            {
                throw new RestClientServiceException(
                    $"Response from BitcoinQuery.WebGateway is failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
            }
        }

        protected async Task<RestResponse> SendRequestAsync(Uri url, Method method, CancellationToken token)
        {
            var client = new RestClient(SetOptions(url));
            var request = new RestRequest(url, method);

            var response = await client.ExecuteAsync(request, token);
            if (response.IsSuccessful)
            {
                return response;
            }

            throw new RestClientServiceException(
                $"Can not create rest request. Status code: {response.StatusCode}, {response.ErrorMessage}");
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
}