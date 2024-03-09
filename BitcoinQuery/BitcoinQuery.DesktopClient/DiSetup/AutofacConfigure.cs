using System;
using Autofac;
using BitcoinQuery.DesktopClient.Contracts;
using BitcoinQuery.DesktopClient.Logger;
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

                builder.RegisterType<NLogLogger>().As<INLogLogger>();

                builder.RegisterType<MainWindow>().InstancePerDependency();
                builder.RegisterType<MainViewModel>().InstancePerDependency();
                builder.RegisterType<BitcoinDataTable>().InstancePerDependency();
                builder.RegisterType<BitcoinDataTableViwModel>().InstancePerDependency();

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