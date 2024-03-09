using System.Configuration;
namespace BitcoinQuery.DesktopClient.Configuration
{
    public class AppConfig
    {
        public string BaseServerUri { get; set; }
        public int Timeout { get; set; }
        
        public AppConfig LoadConfiguration()
        {
            return new AppConfig
            {
                BaseServerUri = ConfigurationManager.AppSettings["serverUriString"],
                Timeout = int.TryParse(ConfigurationManager.AppSettings["timeOut"], out var timeout)
                    ? timeout
                    : -1, // or some default value
            };
        }
    }
}