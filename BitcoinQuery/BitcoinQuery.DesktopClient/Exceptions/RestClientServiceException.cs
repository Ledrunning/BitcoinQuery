using System;

namespace BitcoinQuery.DesktopClient.Exceptions
{
    public class RestClientServiceException : Exception
    {
        public RestClientServiceException(string message) : base(message)
        {
        }
    }
}