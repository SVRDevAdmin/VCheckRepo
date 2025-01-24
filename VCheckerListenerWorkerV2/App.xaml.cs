using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Configuration;

namespace VCheckerListenerWorkerV2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static readonly IHost _host = Host.CreateDefaultBuilder()
                                                 .ConfigureAppConfiguration(c =>
                                                 {
                                                     c.SetBasePath(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
                                                 }).Build();
        //public static IHost _host;

        //private async void OnStartup(object sender ,StartupEventArgs e)
        //{
        //    try
        //    {
        //        _host = Host.CreateDefaultBuilder()
        //                    .ConfigureAppConfiguration(services =>
        //                    {
        //                        services.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        //                    })
        //                    .Build();
        //    }
        //    catch (Exception ex)
        //    {
        //        String abc = "xxx";
        //    }

        //}

        //private async void OnExit(object sender, ExitEventArgs e)
        //{
        //    try
        //    {
        //        await _host.StopAsync();
        //        _host.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        String abc = "ccc";
        //    }
        //}
    }

}
