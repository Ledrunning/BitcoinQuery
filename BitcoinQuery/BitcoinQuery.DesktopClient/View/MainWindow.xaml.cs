using System.Windows;
using Autofac;
using BitcoinQuery.DesktopClient.DiSetup;
using BitcoinQuery.DesktopClient.ViewModel;

namespace BitcoinQuery.DesktopClient.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = AutofacConfigure.Container.Resolve<MainViewModel>();
        }
    }
}