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
        protected readonly int MaxPingAttempts;
        protected readonly string PingUrl;
        protected readonly int ServerPingTimeout;
        
        public BaseRestClientService(AppConfig appConfig)
        {
            BaseUrl = appConfig.BaseServerUri;
            _timeout = appConfig.Timeout;
            PingUrl = appConfig.ServerPingUri;
            MaxPingAttempts = appConfig.MaxPingAttempts;
            ServerPingTimeout = appConfig.ServerPingTimeout;
        }
        
        protected T GetContent<T>(RestResponseBase response)
        {
            CheckResponse(response);

            if (response.Content == null)
            {
                throw new RestClientServiceException(
                    $"Response from service is failed. Content is null: {response.StatusCode}, {response.ErrorMessage}");
            }

            var model = JsonConvert.DeserializeObject<T>(response.Content);
            if (model != null)
            {
                return model;
            }

            throw new RestClientServiceException(
                $"Response from service is failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
        }

        protected void CheckResponse(RestResponseBase response)
        {
            if (!response.IsSuccessful)
            {
                throw new RestClientServiceException(
                    $"Response from service is failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
            }

            if (response.Content == null)
            {
                throw new RestClientServiceException(
                    $"Response from service is failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
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