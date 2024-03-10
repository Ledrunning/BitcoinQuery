using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BitcoinQuery.DesktopClient.Contracts;
using BitcoinQuery.DesktopClient.ViewModel.Commands;

namespace BitcoinQuery.DesktopClient.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IBitcoinRestClientService _bitcoinRestClientService;
        private readonly ISignalRService _signalRService;
        private readonly INLogLogger _logger;
        private readonly CancellationToken _token;

        private string _averageCalculatedData;
        private DateTime _endDate;
        private DateTime _startDate;

        public MainViewModel(IBitcoinRestClientService bitcoinRestClientService, ISignalRService signalRService, INLogLogger logger,
            CancellationToken token)
        {
            _bitcoinRestClientService = bitcoinRestClientService;
            _signalRService = signalRService;
            _signalRService.OnReceiveNotification += OnReceiveNotification;
            _logger = logger;
            _token = token;
            CalculateCommand = new RelayCommand(async () => await CalculateBitcoinData());
            _startDate = DateTime.Today;
            _endDate = DateTime.Today;

            InitializeAsync();
            ConnectToServer();
        }

        private async void ConnectToServer()
        {
            await _signalRService.StartConnectionAsync(); 
            _logger.Info("Connected to server.", null);
        }

        private void OnReceiveNotification(string message)
        {
            InitializeAsync();
            _logger.Info($"Received notification: {message}", null);
        }

        public ICommand CalculateCommand { get; }

        public DateTime StartDate
        {
            get => _startDate;
            set => SetField(ref _startDate, value, nameof(StartDate));
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => SetField(ref _endDate, value, nameof(EndDate));
        }

        public string AverageCalculatedData
        {
            get => _averageCalculatedData;
            set => SetField(ref _averageCalculatedData, value, nameof(AverageCalculatedData));
        }

        private async Task CalculateBitcoinData()
        {
            try
            {
                var formattedStartDate = long.Parse(StartDate.ToString("yyyyMMdd"));
                var formattedEndDate = long.Parse(EndDate.ToString("yyyyMMdd"));
                var result =
                    await _bitcoinRestClientService.GetBitcoinClosingAverage(formattedStartDate, formattedEndDate,
                        _token);
                AverageCalculatedData = result.ToString("F3", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                _logger.Error($"{nameof(MainViewModel)}: Error in calculating average data", e);
                MessageBox.Show("Error in calculating average data", "Error!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void InitializeAsync()
        {
            try
            {
                var dateRangeFromServer = await _bitcoinRestClientService.GetDateTimeRange(_token);
                StartDate = dateRangeFromServer.StartDate;
                EndDate = dateRangeFromServer.EndDate;
            }
            catch (Exception e)
            {
                _logger.Error($"{nameof(MainViewModel)}: error obtaining range of date time from server", e);
                MessageBox.Show("Error obtaining range of date time from server!", "Error!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}