using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BitcoinQuery.DesktopClient.Configuration;
using BitcoinQuery.DesktopClient.Contracts;
using BitcoinQuery.DesktopClient.Model;
using RestSharp;

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

        public async Task<double> GetBitcoinClosingAverage(long startDate, long endDate, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/GetBitcoinClosingAverage?startDate={startDate}&endDate={endDate}");
            var response = await SendRequestAsync(url, Method.Get, token);

            return GetContent<double>(response);
        }
    }
}