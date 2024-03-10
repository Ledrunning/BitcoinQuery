using BitcoinQuery.Service.Dto;
using BitcoinQuery.Service.Models;

namespace BitcoinQuery.Service.Contracts;

public interface IBitcoinDataMapper
{
    BitcoinData Map(BitcoinDailyData bitcoinData, long requestTime, BitcoinPriceData priceData);
    List<DataPoint> MapToDataPoints(double[][]? data, long requestTime, BitcoinPriceData priceData);
}