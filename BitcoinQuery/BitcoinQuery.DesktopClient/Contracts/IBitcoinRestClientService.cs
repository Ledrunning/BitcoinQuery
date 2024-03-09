using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BitcoinQuery.DesktopClient.Model;

namespace BitcoinQuery.DesktopClient.Contracts
{
    public interface IBitcoinRestClientService
    {
        Task<IReadOnlyList<DataPoint>> GetDataFromRangeAsync(CancellationToken token);
    }
}