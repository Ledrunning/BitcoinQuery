using BitcoinQuery.DesktopClient.DiSetup;
using System.Windows;

namespace BitcoinQuery.DesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AutofacConfigure.Configure();
        }
    }
}
