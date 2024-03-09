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
        private readonly CancellationToken _token;
        private ObservableCollection<DataPoint> _allBitcoinData = new ObservableCollection<DataPoint>();
        private string _lastTimeUpdateText;

        public BitcoinDataTableViewModel(IBitcoinRestClientService bitcoinRestClientService, INLogLogger logger,
            CancellationToken token)
        {
            _bitcoinRestClientService = bitcoinRestClientService;
            _logger = logger;
            _token = token;
            UpdateCommand = new RelayCommand(async () => await UpdateBitcoinData());
        }

        public ICommand UpdateCommand { get; }

        public string LastTimeUpdateText
        {
            get => _lastTimeUpdateText;
            set => SetField(ref _lastTimeUpdateText, value, nameof(LastTimeUpdateText));
        }

        //TODO add needed model
        public ObservableCollection<DataPoint> AllBitcoinData
        {
            get => _allBitcoinData;
            set => SetField(ref _allBitcoinData, value);
        }

        private async Task UpdateBitcoinData()
        {
            try
            {
                var resultDataPointList = await _bitcoinRestClientService.GetDataFromRangeAsync(_token);
                foreach (var dataPoint in resultDataPointList)
                {
                    var dtFromTimeStamp = dataPoint.Timestamp.UnixTimeStampToDateTime();
                    dataPoint.FormattedTimestamp = dataPoint.Timestamp.ToString();//dtFromTimeStamp.ToString(GermanyTimeStandart);
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