using Newtonsoft.Json;

namespace BitcoinQuery.Service.Models;

public class DataPoint
{
    [JsonIgnore]
    public long RequestTime { get; set; }
    public string? LastPrice { get; set; }
    public string? FirstCurrency { get; set; }
    public string? SecondCurrency { get; set; }
    public long Timestamp { get; set; }
    public double Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Close { get; set; }
    public double Volume { get; set; }
}