using BitcoinQuery.Service.Contracts;
using BitcoinQuery.Service.Dto;
using BitcoinQuery.Service.Models;

namespace BitcoinQuery.Service.Mapper;

public class BitcoinDataMapper : IBitcoinDataMapper
{
    private const short TimestampIndex = 0;
    private const short OpenIndex = 1;
    private const short HighIndex = 2;
    private const short LowIndex = 3;
    private const short CloseIndex = 4;
    private const short VolumeIndex = 5;

    public BitcoinData Map(BitcoinDailyData bitcoinData, long requestTime, BitcoinPriceData priceData)
    {
        var mappedData = new BitcoinData
        {
            // Map Time
            Timestamp = bitcoinData.Time,
            // Map DataPerMinute
            MinuteData = MapToDataPoints(bitcoinData.DataPerMinute, requestTime, priceData),
            // Map DataPerHour
            HourData = MapToDataPoints(bitcoinData.DataPerHour, requestTime, priceData),
            // Map DataPerDay
            DayData = MapToDataPoints(bitcoinData.DataPerDay, requestTime, priceData)
        };

        return mappedData;
    }

    public List<DataPoint> MapToDataPoints(double[][]? data, long requestTime, BitcoinPriceData priceData)
    {
        if (data == null)
        {
            return new List<DataPoint>();
        }

        return data.Select(d => new DataPoint
        {
            RequestTime = requestTime,
            LastPrice = priceData.LastPrice,
            FirstCurrency = priceData.FirstCurrency,
            SecondCurrency = priceData.SecondCurrency,
            Timestamp = (long)d[TimestampIndex],
            Open = d[OpenIndex],
            High = d[HighIndex],
            Low = d[LowIndex],
            Close = d[CloseIndex],
            Volume = d[VolumeIndex]
        }).ToList();
    }
}