using Hangfire.Dashboard;

namespace BitcoinQuery.WebGateway.Autorization;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // for everyone!
        return true;
    }
}