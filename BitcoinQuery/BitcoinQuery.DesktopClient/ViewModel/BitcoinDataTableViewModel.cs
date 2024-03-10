using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BitcoinQuery.DesktopClient.Contracts;
using BitcoinQuery.DesktopClient.Extensions;
using BitcoinQuery.DesktopClient.Model;
using BitcoinQuery.DesktopClient.ViewModel.Commands;

namespace BitcoinQuery.DesktopClient.ViewModel
{
    public class BitcoinDataTableViewModel : BaseViewModel
    {
        private const string GermanyTimeStandart = "dd.MM.yyyy HH:mm:ss";
        private readonly IBitcoinRestClientService _bitcoinRestClientService;
        private readonly INLogLogger _logger;
        private readonly ISignalRService _signalRService;
        private readonly CancellationToken _token;
        private ObservableCollection<DataPoint> _allBitcoinData = new ObservableCollection<DataPoint>();
        private string _lastTimeUpdateText;

        public BitcoinDataTableViewModel(IBitcoinRestClientService bitcoinRestClientService,
            ISignalRService signalRService, INLogLogger logger,
            CancellationToken token)
        {
            _bitcoinRestClientService = bitcoinRestClientService;
            _signalRService = signalRService;
            _signalRService.OnReceiveNotification += OnReceiveNotification;
            _logger = logger;
            _token = token;
            UpdateCommand = new RelayCommand(async () => await UpdateBitcoinData());
            ConnectToServer();
            _ = GetAndUpdateBitcoinData();
        }

        public ICommand UpdateCommand { get; }

        public string LastTimeUpdateText
        {
            get => _lastTimeUpdateText;
            set => SetField(ref _lastTimeUpdateText, value, nameof(LastTimeUpdateText));
        }

        public ObservableCollection<DataPoint> AllBitcoinData
        {
            get => _allBitcoinData;
            set => SetField(ref _allBitcoinData, value);
        }

        /// <summary>
        ///     Update data by server push message and Cron
        /// </summary>
        /// <param name="message"></param>
        private async void OnReceiveNotification(string message)
        {
            // German standart 
            LastTimeUpdateText = DateTime.Now.ToString(GermanyTimeStandart);
            await _signalRService.SendNotificationAsync("Update!");
            await GetAndUpdateBitcoinData();
            _logger.Info($"Received notification: {message}", null);
        }

        private async void ConnectToServer()
        {
            await _signalRService.StartConnectionAsync();
            _logger.Info("Connected to server, waiting for push messages!.", null);
        }

        public async Task UpdateBitcoinData()
        {
            await GetAndUpdateBitcoinData();
        }

        private async Task GetAndUpdateBitcoinData()
        {
            try
            {
                var resultDataPointList = await _bitcoinRestClientService.GetDataFromRangeAsync(_token);
                foreach (var dataPoint in resultDataPointList)
                {
                    var dtFromTimeStamp = dataPoint.Timestamp.UnixTimeStampToDateTime();
                    dataPoint.FormattedTimestamp = dtFromTimeStamp.ToString(GermanyTimeStandart);
                    AllBitcoinData.Add(dataPoint);
                }

                // German standart 
                LastTimeUpdateText = DateTime.Now.ToString(GermanyTimeStandart);
            }
            catch (Exception e)
            {
                _logger.Error($"{nameof(BitcoinDataTableViewModel)}: error obtaining data from server!", e);
                MessageBox.Show("Error obtaining data from server!", "Error!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}