using Autofac;
using BitcoinQuery.DesktopClient.DiSetup;
using BitcoinQuery.DesktopClient.ViewModel;
using System.Windows.Controls;

namespace BitcoinQuery.DesktopClient.View.UserControls
{
    /// <summary>
    ///     Interaction logic for BitcoinDataTable.xaml
    /// </summary>
    public partial class BitcoinDataTable : UserControl
    {
        public BitcoinDataTable()
        {
            InitializeComponent();
            DataContext = AutofacConfigure.Container.Resolve<BitcoinDataTableViewModel>();
        }
    }
}