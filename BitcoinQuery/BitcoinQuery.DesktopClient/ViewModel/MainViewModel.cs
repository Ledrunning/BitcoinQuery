using System;
using System.Windows.Controls;
using System.Windows.Input;
using BitcoinQuery.DesktopClient.Contracts;
using BitcoinQuery.DesktopClient.ViewModel.Commands;

namespace BitcoinQuery.DesktopClient.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INLogLogger _logger;

        private string _averageCalculatingText;
        private DateTime _startDate;
        private DateTime _endDate;

        public MainViewModel(INLogLogger logger)
        {
            _logger = logger;
            CalculateCommand = new RelayCommand(CalculateBitcoinData);
            _startDate = DateTime.Today;
            _endDate = DateTime.Today;
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

        public string AverageCalculatingText
        {
            get => _averageCalculatingText;
            set => SetField(ref _averageCalculatingText, value, nameof(AverageCalculatingText));
        }

        private void CalculateBitcoinData()
        {
            var s = StartDate;
            var e = EndDate;
        }
    }
}