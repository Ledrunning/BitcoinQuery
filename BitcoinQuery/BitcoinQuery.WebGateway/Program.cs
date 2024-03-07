using System.Diagnostics;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

const string loggerConfig = "NLog.config";
var logger = LogManager.GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);


    builder.Logging.ClearProviders().SetMinimumLevel(LogLevel.Debug).AddNLog(loggerConfig);

    // Add services to the container.
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