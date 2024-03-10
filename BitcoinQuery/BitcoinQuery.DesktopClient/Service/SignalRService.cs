using System;
using System.Threading.Tasks;
using BitcoinQuery.DesktopClient.Contracts;
using Microsoft.AspNetCore.SignalR.Client;

namespace BitcoinQuery.DesktopClient.Service
{
    public class SignalRService : ISignalRService
    {
        private readonly INLogLogger _logger;
        private readonly HubConnection _connection;

        public SignalRService(string serverPushUrl, INLogLogger logger)
        {
            _logger = logger;
            _connection = new HubConnectionBuilder()
                .WithUrl(serverPushUrl) 
                .Build();

            _connection.On<string>("ReceiveNotification", message =>
            {
                OnReceiveNotification?.Invoke(message);
            });
        }

        public event Action<string> OnReceiveNotification;

        public async Task StartConnectionAsync()
        {
            try
            {
                await _connection.StartAsync(); 
            }
            catch (Exception e)
            {
                _logger.Error("Connection failed!", e);
                throw new ApplicationException($"Connection failed: {e}");
            }
        }

        public async Task StopConnectionAsync()
        {
            await _connection.StopAsync(); 
        }
    }
}