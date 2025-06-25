using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VCheck.Lib.Data.DBContext;
using VCheckViewer.Lib.Function;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace VCheckViewer.Views.Pages.Maintenance
{
    /// <summary>
    /// Interaction logic for MaintenancePage.xaml
    /// </summary>
    public partial class MaintenancePage : Page
    {
        private string apiURL = "http://vcheckstaging.inteleon.xyz/API";
        public ConfigurationDBContext configDBContext = new ConfigurationDBContext(ConfigSettings.GetConfigurationSettings());

        public MaintenancePage()
        {
            InitializeComponent();

            BackButton.DataContext = App.MainViewModel;
            App.MainViewModel.BackButtonText = Properties.Resources.Setting_Label_BackButton;

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

                return await sAPI.TestConnection(apiURL);
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
            RefreshAll();
        }

        private async Task RefreshAll()
        {
            RefreshButton.IsEnabled = false;

            var PMS = configDBContext.GetConfigurationData("InterfaceSettingsPMS").FirstOrDefault();

            APIStatus.Text = Properties.Resources.Maintenance_Label_Checking;
            APIStatus.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");
            ListenerStatus.Text = Properties.Resources.Maintenance_Label_Checking;
            ListenerStatus.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");
            ListenerIP.Text = GetAssignedIPAddress();
            ListenerIP.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");
            ListenerPort.Text = "8585";
            ListenerPort.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");
            PMSConnected.Text = PMS != null ? PMS.ConfigurationValue : "None";
            PMSConnected.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");

            var APIRunning = await CheckAPI();
            APIStatus.Text = APIRunning ? Properties.Resources.Maintenance_Label_Connected : Properties.Resources.Maintenance_Label_NotConnected;
            APIStatus.Foreground = APIRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;

            var ListenerRunning = CheckListener();
            ListenerStatus.Text = ListenerRunning ? Properties.Resources.Maintenance_Label_Running : Properties.Resources.Maintenance_Label_NotRunning;
            ListenerStatus.Foreground = ListenerRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;
            ListenerIP.Foreground = ListenerRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;
            ListenerPort.Foreground = ListenerRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;

            var PMSConnect = PMS != null;
            PMSConnected.Text = PMSConnect ? PMS.ConfigurationValue : "None";
            PMSConnected.Foreground = PMSConnect ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;

            RefreshButton.IsEnabled = true;
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
    }
}
