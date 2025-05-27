using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VCheck.Interface.API;
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
        //private string apiURL = "http://localhost:82/API";
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

        //public async Task<bool> CheckGreywindConnection(string ClinicID)
        //{
        //    try
        //    {
        //        GreywindAPI sAPI = new GreywindAPI();
        //        VCheckAPI VcheckAPI = new VCheckAPI();
        //        var url = await VcheckAPI.GetPMSUrl(2);

        //        return await sAPI.ClinicInfo(ClinicID, url);
        //        //return await sAPI.ClinicInfo("1", url);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return false;
        //}

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
            //APIURL.Text = apiURL;
            //APIURL.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");
            ListenerIP.Text = GetAssignedIPAddress();
            ListenerIP.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");
            PMSConnected.Text = PMS != null ? PMS.ConfigurationValue : "None";
            PMSConnected.Foreground = (Brush)new BrushConverter().ConvertFromString("#fa8219");

            var APIRunning = await CheckAPI();
            APIStatus.Text = APIRunning ? Properties.Resources.Maintenance_Label_Connected : Properties.Resources.Maintenance_Label_NotConnected;
            APIStatus.Foreground = APIRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;
            //APIURL.Foreground = APIRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;

            var ListenerRunning = CheckListener();
            ListenerStatus.Text = ListenerRunning ? Properties.Resources.Maintenance_Label_Running : Properties.Resources.Maintenance_Label_NotRunning;
            ListenerStatus.Foreground = ListenerRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;
            ListenerIP.Foreground = ListenerRunning ? (Brush)new BrushConverter().ConvertFromString("#16c933") : Brushes.Red;

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
            var port = 8585;

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString() != "127.0.0.1")
                {
                    return ip.ToString() + ":" + port;
                }
            }
            //throw new Exception("No network adapters with an IPv4 address in the system!");
            return "";
        }
    }
}
