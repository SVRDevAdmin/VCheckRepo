using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows;
using VCheck.Lib.Data.DBContext;
using VCheckListener.Lib.Logics;
using VCheckListener.Services;
using VCheckListener.Views;

namespace VCheckListener
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TrayIcon trayIcon;
        private Worker server;
        private LogWindow logWindow;
        private int connectionCount = 0;
        public static string IPAddress { get; set; }
        public static int Port { get; set; } = 8585;

        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static LocalizationService _localizationService = new LocalizationService();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _localizationService.SetCulture(TestResultRepository.GetConfigurationByKey("SystemSettings_Language").ConfigurationValue);

            trayIcon = new TrayIcon();
            server = new Worker();
            var MachineIPAddress = "";


            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString() != "127.0.0.1")
                {
                    MachineIPAddress = ip.ToString();
                }
            }

            logWindow = new LogWindow() { Title = _localizationService.GetString("General_Label_Logs") + " (" + MachineIPAddress + ":8585)"};

            server.OnClientConnect = msg =>
            {
                var ipPort = msg.Split(":");
                connectionCount++;
                trayIcon.ShowBalloon(_localizationService.GetString("General_Message_ClientConnected"), $"IP Address: {ipPort[0]} \n Port : {ipPort[1]}", BalloonIcon.Info);
                logWindow.AddLog($"{_localizationService.GetString("General_Message_ClientConnected")} ({msg})");
                trayIcon.UpdateTooltip($"{_localizationService.GetString("General_Message_VCheckListener")} ({MachineIPAddress}) — {connectionCount} connection(s)");
            };

            server.OnDataReceived = msg =>
            {
                logWindow.AddLog($"[Data] \n\n{msg}\n");
            };

            server.OnError = msg =>
            {
                logWindow.AddLog($"[Error] {msg}");
            };

            server.OnClientDisconnect = msg =>
            {
                var ipPort = msg.Split(":");
                connectionCount--;
                trayIcon.ShowBalloon(_localizationService.GetString("General_Message_ClientDisconnected"), $"IP Address: {ipPort[0]} \n Port : {ipPort[1]}", BalloonIcon.Info);
                logWindow.AddLog($"{_localizationService.GetString("General_Message_ClientDisconnected")} ({msg})");
                trayIcon.UpdateTooltip($"{_localizationService.GetString("General_Message_VCheckListener")} ({MachineIPAddress}) — {connectionCount} connection(s)");
            };

            Task.Run(() => server.Start(MachineIPAddress, Port));
            trayIcon.ShowBalloon(_localizationService.GetString("General_Message_VCheckListener"), _localizationService.GetString("General_Message_ServerNowListen") + Port + ".", BalloonIcon.Info);
            trayIcon.UpdateTooltip($"{_localizationService.GetString("General_Message_VCheckListener")} ({MachineIPAddress}) — 0 connection(s)");

            // Don’t show any window
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            server?.Stop();
            trayIcon?.Dispose();
        }

        public void ShowLogWindow()
        {
            if (logWindow.IsVisible)
                logWindow.Activate();
            else
                logWindow.Show();
        }
    }

}
