using BitcoinQuery.Service.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace BitcoinQuery.Service.Push;

public class NotificationHub : Hub, INotificationHub
{
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync("SendNotification", message);
    }
}