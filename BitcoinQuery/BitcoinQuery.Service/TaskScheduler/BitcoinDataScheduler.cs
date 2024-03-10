using Hangfire;

namespace BitcoinQuery.Service.TaskScheduler;

public class BitcoinDataScheduler
{
    //"0 */2 * * * *" - 2 min. cron expression for debugging 
    public void ScheduleBitcoinDataUpdate(CancellationToken token)
    {
        // Update every 24 hours
        RecurringJob.AddOrUpdate<BitcoinDataUpdater>("BitcoinDataUpdate", updater => updater.UpdateBitcoinData(token), Cron.Daily);
    }
}