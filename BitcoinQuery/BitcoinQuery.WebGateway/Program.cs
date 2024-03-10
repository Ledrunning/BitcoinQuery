using System.Diagnostics;
using BitcoinQuery.Service.Contracts;
using BitcoinQuery.Service.Exceptions;
using BitcoinQuery.Service.Mapper;
using BitcoinQuery.Service.Push;
using BitcoinQuery.Service.Service;
using BitcoinQuery.Service.Service.RestService;
using BitcoinQuery.Service.TaskScheduler;
using BitcoinQuery.WebGateway.Configuration;
using BitcoinQuery.WebGateway.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

const string loggerConfig = "NLog.config";
var logger = LogManager.GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders().SetMinimumLevel(LogLevel.Debug).AddNLog(loggerConfig);

    //Setup cex.io config section
    var cexConfigSection = builder.Configuration.GetSection(CexConfigSection.SectionName).Get<CexConfigSection>();

    // Add services to the container.
    builder.ConfigureLogging();
    builder.Services.AddTransient<IBitcoinDataMapper, BitcoinDataMapper>();
    builder.Services.AddTransient<IDataCachingService, DataCachingService>();
    builder.Services.AddTransient<IBitcoinQueryService>(serviceProvider =>
    {
        var bitcoinDataMapper = serviceProvider.GetRequiredService<IBitcoinDataMapper>();
        var ñachingService = serviceProvider.GetRequiredService<IDataCachingService>();
        // If something went wrong with the config section - throw it up!
        if (cexConfigSection is { BaseUrl: { }, FirstCurrency: { }, SecondCurrency: { } })
        {
            return new BitcoinQueryService(logger, bitcoinDataMapper, ñachingService, cexConfigSection.BaseUrl,
                cexConfigSection.Timeout,
                cexConfigSection.FirstCurrency, cexConfigSection.SecondCurrency);
        }

        throw new BitcoinQueryServiceException("Configuration error or invalid file! Check the appsettings.json file.");
    });

    //Configure task scheduler
    builder.ConfigureHangFire();
    builder.Services.AddSingleton(serviceProvider =>
    {
        var service = serviceProvider.GetRequiredService<BitcoinDataScheduler>();
        service.ScheduleBitcoinDataUpdate();
        return service;
    });

    builder.Services.AddSignalR();
    builder.Services.AddMemoryCache();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        _ = endpoints.MapHub<NotificationHub>("/notificationHub");
    });

    logger.Info("Server has been started...");
    app.Run();
}
catch (Exception e)
{
    var name = typeof(Program).Assembly.GetName().Name;
    Trace.Write(
        $"[{DateTime.Now:HH:mm:ss.fff}] Application startup error [{name}]! Details {e.Message}");
    logger.Fatal(e, $"Application startup error [{name}]");
}
finally
{
    LogManager.Shutdown();
}