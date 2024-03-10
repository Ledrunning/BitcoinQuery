using BitcoinQuery.DesktopClient.Configuration;
using BitcoinQuery.DesktopClient.Contracts;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using BitcoinQuery.DesktopClient.Model;

namespace BitcoinQuery.DesktopClient.Rest
{
    public class BitcoinRestClientService : BaseRestClientService, IBitcoinRestClientService
    {
        public BitcoinRestClientService(AppConfig appConfig) : base(appConfig)
        {
        }

        public async Task<IReadOnlyList<DataPoint>> GetDataFromRangeAsync(CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/GetDataFromRange");
            var response = await SendRequestAsync(url, Method.Get, token);

            return GetContent<IReadOnlyList<DataPoint>>(response);
        }

        public async Task<DateRange> GetDateTimeRange(CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/GetDateTimeRange");
            var response = await SendRequestAsync(url, Method.Get, token);

            return GetContent<DateRange>(response);
        }
    }
}