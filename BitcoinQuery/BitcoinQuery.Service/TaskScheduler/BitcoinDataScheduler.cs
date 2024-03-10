using Hangfire;

namespace BitcoinQuery.Service.TaskScheduler;

public class BitcoinDataScheduler
{
    public void ScheduleBitcoinDataUpdate(CancellationToken token)
    {
        // Update every 24 hours
        RecurringJob.AddOrUpdate<BitcoinDataUpdater>("BitcoinDataUpdate", updater => updater.UpdateBitcoinData(token), "0 */2 * * * *");
    }
}