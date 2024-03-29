﻿using System.Diagnostics;
using BitcoinQuery.WebGateway.Autorization;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using NLog;
using NLog.Web;
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

    /// <summary>
    ///     NOTE! In this case, I am using memory
    ///     as there is no need to implement any databases in this test case.
    ///     https://localhost:7186/hangfire - Dashboard
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureHangFire(this WebApplicationBuilder builder)
    {
        builder.Services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMemoryStorage());

        builder.Services.AddSingleton<IDashboardAuthorizationFilter, HangfireAuthorizationFilter>();
        builder.Services.AddHangfireServer();
    }
}