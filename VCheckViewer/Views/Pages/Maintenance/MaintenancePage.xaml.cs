using Microsoft.Extensions.Hosting;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VCheck.Interface.API;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;
using VCheckViewer.Services.MessageBox;
using VCheckViewer.ViewModels.Windows;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace VCheckViewer.Views.Pages.Maintenance
{
    /// <summary>
    /// Interaction logic for MaintenancePage.xaml
    /// </summary>
    public partial class MaintenancePage : Page
    {
        public ObservableCollection<ComboBoxItem> cbDateFormat { get; set; }
        public ComboBoxItem SelectedcbDateFormat { get; set; }

        public ConfigurationDBContext configDBContext = new ConfigurationDBContext(ConfigSettings.GetConfigurationSettings());

        public MaintenancePage()
        {
            InitializeComponent();
            DataContext = this;

            BackButton.DataContext = App.MainViewModel;
            App.MainViewModel.BackButtonText = Properties.Resources.Setting_Label_BackButton;
            cbDateFormat = App.MainViewModel.cbDateFormat;

            var sConfigObj = configDBContext.GetConfigurationData("System_DateFormat").FirstOrDefault();
            if(sConfigObj != null)
            {
                SelectedcbDateFormat = cbDateFormat.Where(a => (string)a.Tag == sConfigObj.ConfigurationValue).FirstOrDefault();
            }
            else
            {
                SelectedcbDateFormat = cbDateFormat.FirstOrDefault();
            }

            RefreshAll();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.GoPreviousPageHandler(e, sender);
        }

        public async Task<bool> CheckAPI()
        {
            try
            {
                VCheck.Interface.API.VCheckAPI sAPI = new VCheck.Interface.API.VCheckAPI();

                return await sAPI.TestConnection();
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public bool CheckListener()
        {
            return Process.GetProcessesByName("VCheckListenerWorker").Any();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("VCheckListenerWorker");

            if (!processes.Any())
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "C:\\VCheck\\VCheckListenerWorker\\VCheckListenerWorker.exe", // Replace with your executable or script
                    UseShellExecute = true,   // Required for 'runas'
                    Verb = "runas",           // Triggers UAC prompt for admin rights
                    Arguments = ""            // Optional: pass arguments here
                };

                Process.Start(psi);
            }
            else
            {
                PingPortAsync(processes, GetAssignedIPAddress(), 8585);
            }

            RefreshAll();

            App.RefreshMaintenanceHandler(e, sender);
        }

        private async Task RefreshAll()
        {
            //RefreshButton.IsEnabled = false;

            //var PMS = configDBContext.GetConfigurationData("InterfaceSettingsPMS").FirstOrDefault();
            var SystemVersion = configDBContext.GetConfigurationData("System_Version").FirstOrDefault();

            //APIStatus.Text = Properties.Resources.Maintenance_Label_Checking;
            //APIStatus.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");
            //ListenerStatus.Text = Properties.Resources.Maintenance_Label_Checking;
            //ListenerStatus.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");
            //ListenerIP.Text = GetAssignedIPAddress();
            //ListenerIP.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");
            //ListenerPort.Text = "8585";
            //ListenerPort.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");
            //PMSConnected.Text = PMS != null ? PMS.ConfigurationValue : "None";
            //PMSConnected.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");

            //var APIRunning = await CheckAPI();
            //APIStatus.Text = APIRunning ? Properties.Resources.Maintenance_Label_Connected : Properties.Resources.Maintenance_Label_NotConnected;
            //APIStatus.Foreground = APIRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;

            //var ListenerRunning = CheckListener();
            //ListenerStatus.Text = ListenerRunning ? Properties.Resources.Maintenance_Label_Running : Properties.Resources.Maintenance_Label_NotRunning;
            //ListenerStatus.Foreground = ListenerRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;
            //ListenerIP.Foreground = ListenerRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;
            //ListenerPort.Foreground = ListenerRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;

            //var PMSConnect = PMS != null;
            //PMSConnected.Text = PMSConnect ? PMS.ConfigurationValue : Properties.Resources.Maintenance_Label_None;
            //PMSConnected.Foreground = PMSConnect ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;

            AppVersion.Text = SystemVersion.ConfigurationValue;

            //RefreshButton.IsEnabled = true;
        }

        /// <summary>
        /// Get current IP Address
        /// </summary>
        public static string GetAssignedIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString() != "127.0.0.1")
                {
                    return ip.ToString();
                }
            }
            return "";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)DateFormat.SelectedItem;

            var sConfigObj = configDBContext.GetConfigurationData("System_DateFormat").FirstOrDefault();
            if (sConfigObj != null)
            {
                configDBContext.UpdateConfiguration("System_DateFormat", selectedItem.Tag.ToString());
            }
            else
            {
                configDBContext.AddConfiguration("System_DateFormat", selectedItem.Tag.ToString());
            }

            App.MainViewModel.Origin = "GeneralSettingsUpdated";

            App.PopupHandler(e, sender);
        }

        public static async Task PingPortAsync(Process[] processes, string host, int port, int timeoutMs = 3000)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var connectTask = client.ConnectAsync(host, port);
                    var timeoutTask = Task.Delay(timeoutMs);

                    var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                    if (completedTask == timeoutTask || completedTask.Status == TaskStatus.Faulted)
                    {
                        foreach (Process proc in processes)
                        {
                            try
                            {
                                proc.Kill(); // Forcefully terminate
                                proc.WaitForExit(); // Wait until it exits
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }

                        string exePath = @"C:\VCheck\VCheckListenerWorker\VCheckListenerWorker.exe";

                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = exePath,
                            UseShellExecute = true, // Required for 'runas'
                            Verb = "runas",         // Triggers elevation prompt
                            WorkingDirectory = System.IO.Path.GetDirectoryName(exePath)
                        };

                        Process.Start(psi);
                        //Process.Start(exePath);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            (string SelectedButton, bool IsCheckBoxChecked) result = ("", false);

            VCheckAPI vcheckAPI = new VCheckAPI();
            var SystemVersion = configDBContext.GetConfigurationData("System_Version").FirstOrDefault();

            var IsLatestVersion = false;
            IsLatestVersion = await vcheckAPI.IsLatestVersion(SystemVersion.ConfigurationValue);

            if (!IsLatestVersion)
            {
                //ShowUpdateNotification = true;
                result = CustomMessageBox.Show(1, false);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Current VCheck already the latest version.");
            }


            if (result.SelectedButton == "Yes")
            {
                var downloadSuccess = await vcheckAPI.DownloadLatestInstaller();
                if (downloadSuccess)
                {
                    System.Windows.Forms.MessageBox.Show("Download successful.");

                    string downloadFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
                    string zipPath = downloadFolderPath + @"\Vcheck Patch.zip";
                    string extractPath = downloadFolderPath;

                    ZipFile.ExtractToDirectory(zipPath, extractPath, overwriteFiles: true);

                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = downloadFolderPath + @"\Deploy Patch.bat", // Replace with your batch file path
                        CreateNoWindow = true,           // Prevents the CMD window from showing
                        UseShellExecute = true, // Required for 'runas'
                        Verb = "runas",         // Triggers UAC for admin rights
                        WindowStyle = ProcessWindowStyle.Hidden, // Hide window
                    };

                    using (Process process = new Process { StartInfo = startInfo })
                    {
                        process.Start();
                    }

                    Environment.Exit(0);
                }
                else { System.Windows.Forms.MessageBox.Show("Download unsuccessful. Please try again later."); }

            }
            else
            {

            }
        }
    }
}
