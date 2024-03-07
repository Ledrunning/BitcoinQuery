using Newtonsoft.Json;

namespace BitcoinQuery.Service.Dto;

public class BitcoinPriceData
{
    [JsonProperty("lprice")] public string? LastPrice { get; set; }

    [JsonProperty("curr1")] public string? FirstCurrency { get; set; }

    [JsonProperty("curr2")] public string? SecondCurrency { get; set; }
}