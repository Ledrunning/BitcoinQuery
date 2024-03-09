using BitcoinQuery.Service.Dto;
using BitcoinQuery.Service.Models;

namespace BitcoinQuery.Service.Contracts;

public interface IBitcoinDataMapper
{
    BitcoinData Map(BitcoinDailyData bitcoinData, BitcoinPriceData priceData);
}