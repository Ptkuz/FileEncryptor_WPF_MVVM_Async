using FileEncryptor.Services;
using FileEncryptor.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Windows;


namespace FileEncryptor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        // Свойство, хранящее окно в фокусе
        public static Window FocusedWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused);

        // Свойство, хранящее активное окно
        public static Window ActiveWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);


        private static IHost __Host;

        public static IHost Host => __Host 
            ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();


        public static IServiceProvider Services => Host.Services;

        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddServices();
            services.AddViewModel();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;

            base.OnStartup(e);

            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            using (Host)
                await Host.StopAsync();
           
        }
    }
}
