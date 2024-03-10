using System;
using BitcoinQuery.DesktopClient.Contracts;
using NLog;

namespace BitcoinQuery.DesktopClient.Logger
{
    public class NLogLogger : INLogLogger
    {
        private readonly NLog.Logger _logger;

        public NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Info(string message, Exception e)
        {
            _logger.Info(message, e);
        }

        public void Debug(string message, Exception e)
        {
            _logger.Debug(message, e);
        }

        public void Warn(string message, Exception e)
        {
            _logger.Warn(message, e);
        }

        public void Error(string message, Exception e)
        {
            _logger.Error(message, e);
        }

        public void Trace(string message, Exception e)
        {
            _logger.Trace(message, e);
        }
    }
}