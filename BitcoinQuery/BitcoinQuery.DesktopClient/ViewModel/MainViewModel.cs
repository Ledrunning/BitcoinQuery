using BitcoinQuery.DesktopClient.Contracts;

namespace BitcoinQuery.DesktopClient.ViewModel
{
    public class MainViewModel
    {
        private readonly INLogLogger _logger;

        public MainViewModel(INLogLogger logger)
        {
            _logger = logger;
        }
    }
}