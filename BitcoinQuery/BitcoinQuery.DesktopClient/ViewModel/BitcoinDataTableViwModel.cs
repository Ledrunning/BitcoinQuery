using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BitcoinQuery.DesktopClient.ViewModel.Commands;

namespace BitcoinQuery.DesktopClient.ViewModel
{
    public class BitcoinDataTableViwModel : BaseViewModel
    {
        private ObservableCollection<object> _allBitcoinData;

        private string _lastTimeUpdateText;

        public BitcoinDataTableViwModel()
        {
            UpdateCommand = new RelayCommand(UpdateBitcoinData);
        }

        public ICommand UpdateCommand { get; }

        public string LastTimeUpdateText
        {
            get => _lastTimeUpdateText;
            set => SetField(ref _lastTimeUpdateText, value, nameof(LastTimeUpdateText));
        }

        //TODO add needed model
        public ObservableCollection<object> AllBitcoinData
        {
            get => _allBitcoinData;
            set => SetField(ref _allBitcoinData, value);
        }

        private void UpdateBitcoinData()
        {
            // German standart 
            LastTimeUpdateText = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }
}