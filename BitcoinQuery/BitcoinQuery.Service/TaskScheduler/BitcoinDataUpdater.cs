using BitcoinQuery.Service.Contracts;
using Microsoft.Extensions.Logging;

namespace BitcoinQuery.Service.TaskScheduler;

public class BitcoinDataUpdater
{
    private readonly IBitcoinQueryService _bitcoinRestClientService;
    private readonly ILogger<BitcoinDataUpdater> _logger;
    private readonly INotificationHub _notificationHub;

    public BitcoinDataUpdater(IBitcoinQueryService bitcoinRestClientService, INotificationHub notificationHub,
        ILogger<BitcoinDataUpdater> logger)
    {
        _bitcoinRestClientService = bitcoinRestClientService;
        _notificationHub = notificationHub;
        _logger = logger;
    }

    public async Task UpdateBitcoinData(CancellationToken token)
    {
        try
        {
            var data = await _bitcoinRestClientService.GetDataFromRangeAsync(token);
            if (data is { Count: > 0 })
            {
                
                await _notificationHub.SendNotification("Bitcoin data has been successfully updated!");
            }
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "Bitcoin data has been successfully updated!");
        }
    }
}