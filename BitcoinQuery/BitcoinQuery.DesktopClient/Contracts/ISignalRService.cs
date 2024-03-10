using System;
using System.Threading.Tasks;

namespace BitcoinQuery.DesktopClient.Contracts
{
    public interface ISignalRService
    {
        event Action<string> OnReceiveNotification;
        Task StartConnectionAsync();
        Task StopConnectionAsync();
    }
}