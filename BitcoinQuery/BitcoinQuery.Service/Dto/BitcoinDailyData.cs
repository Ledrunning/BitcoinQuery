﻿using BitcoinQuery.Service.Attributes;
using Newtonsoft.Json;

namespace BitcoinQuery.Service.Dto;

public class BitcoinDailyData
{
    public long Time { get; set; }

    [JsonProperty("data1m", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(DoubleArrayConverter))]
    public double[][]? DataPerMinute { get; set; }

    [JsonProperty("data1h", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(DoubleArrayConverter))]
    public double[][]? DataPerHour { get; set; }

    [JsonProperty("data1d", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(DoubleArrayConverter))]
    public double[][]? DataPerDay { get; set; }
}