using BitcoinQuery.Service.Dto;
using BitcoinQuery.Service.Models;

namespace BitcoinQuery.Service.Conrtacts;

public interface IBitcoinDataMapper
{
    BitcoinData Map(BitcoinDailyData bitcoinData);
}