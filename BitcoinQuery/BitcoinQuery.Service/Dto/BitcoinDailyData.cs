using Newtonsoft.Json;

namespace BitcoinQuery.Service.Dto;

public class BitcoinDailyData
{
    public long Time { get; set; }

    [JsonProperty("data1m")] public List<List<double>>? DataPerMinute { get; set; }

    [JsonProperty("data1h")] public List<List<double>>? DataPerHour { get; set; }

    [JsonProperty("data1d")] public List<List<double>>? DataPerDay { get; set; }
}