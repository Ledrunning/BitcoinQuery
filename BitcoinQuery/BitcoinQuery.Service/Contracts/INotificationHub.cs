namespace BitcoinQuery.Service.Contracts;

public interface INotificationHub
{
    Task SendNotification(string message);
}