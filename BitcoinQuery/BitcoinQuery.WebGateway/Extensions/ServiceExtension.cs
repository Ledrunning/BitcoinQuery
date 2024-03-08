using NLog.Web;
using NLog;
using System.Diagnostics;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace BitcoinQuery.WebGateway.Extensions;

public static class ServiceExtension
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        builder.Host.UseNLog();

        Trace.Listeners.Clear();
        Trace.Listeners.Add(new NLogTraceListener());
    }
}