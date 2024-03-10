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
        private static string _connectionId;

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

                // Get server Id
                _connectionId = await _connection.InvokeAsync<string>("GetConnectionId");
                _logger.Info($"Connected to server with connection ID: {_connectionId}", null);
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

        public async Task SendNotificationAsync(string message)
        {
            if (_connectionId != null)
            {
                try
                {
                    await _connection.InvokeAsync("SendNotification", _connectionId, message);
                }
                catch (Exception e)
                {
                    _logger.Error("Failed to send notification!", e);
                }
            }
            else
            {
                _logger.Warn("Connection ID is not available, unable to send notification.", null);
            }
        }
    }

}