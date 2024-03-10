using Hangfire;

namespace BitcoinQuery.Service.TaskScheduler;

public class BitcoinDataScheduler
{
    public void ScheduleBitcoinDataUpdate()
    {
        // Update every 24 hours
        RecurringJob.AddOrUpdate<BitcoinDataUpdater>("BitcoinDataUpdate", updater => updater.UpdateBitcoinData(), Cron.Daily);
    }
}