using System;
using System.Threading;
using Autofac;
using BitcoinQuery.DesktopClient.Configuration;
using BitcoinQuery.DesktopClient.Contracts;
using BitcoinQuery.DesktopClient.Logger;
using BitcoinQuery.DesktopClient.Rest;
using BitcoinQuery.DesktopClient.View;
using BitcoinQuery.DesktopClient.View.UserControls;
using BitcoinQuery.DesktopClient.ViewModel;
using NLog;

namespace BitcoinQuery.DesktopClient.DiSetup
{
    public static class AutofacConfigure
    {
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        public static IContainer Container { get; private set; }

        public static void Configure()
        {
            try
            {
                var builder = new ContainerBuilder();

                var appConfig = new AppConfig();
                var loadedConfiguration = appConfig.LoadConfiguration();

                builder.RegisterInstance(new CancellationTokenSource()).AsSelf();
                // Register a factory to get the CancellationToken from the source.
                builder.Register(c => c.Resolve<CancellationTokenSource>().Token).As<CancellationToken>();

                builder.RegisterType<NLogLogger>().As<INLogLogger>();

                builder.Register(ctx => new BitcoinRestClientService(loadedConfiguration))
                    .As<IBitcoinRestClientService>().InstancePerDependency();

                builder.RegisterType<MainWindow>().InstancePerDependency();
                builder.RegisterType<MainViewModel>().InstancePerDependency();
                builder.RegisterType<BitcoinDataTable>().InstancePerDependency();
                builder.RegisterType<BitcoinDataTableViewModel>().InstancePerDependency();

                Container = builder.Build();
            }
            catch (Exception e)
            {
                Logger.Error("Application startup error! {e}", e);
                throw new ApplicationException($"{e.Message}. Application startup error!");
            }
        }
    }
}