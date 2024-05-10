using System;

namespace BitcoinQuery.DesktopClient.Contracts
{
    public interface INLogLogger
    {
        void Info(string message, Exception e);
        void Debug(string message, Exception e);
        void Warn(string message, Exception e);
        void Error(string message, Exception e);
        void Trace(string message, Exception e);
    }
}