using Org.BouncyCastle.Utilities.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;

namespace VCheckViewer.Views.Pages.Setting.Interface
{
    /// <summary>
    /// Interaction logic for ConfigurationPage.xaml
    /// </summary>
    public partial class ConfigurationPage : Page
    {
        public ConfigurationDBContext configDBContext = new ConfigurationDBContext(ConfigSettings.GetConfigurationSettings());
        public String sIPConfigKey = "InterfaceSettingsIP";
        public String sPortNoConfigKey = "InterfaceSettingsPortNo";

        public ConfigurationPage()
        {
            InitializeComponent();

            txtIP.ValidatingType = typeof(System.Net.IPAddress);
            txtIP.ResetOnSpace = false;

            LoadInterfaceConfiguration();
        }

        private void LoadInterfaceConfiguration()
        {          
            var sInterfaceIP = configDBContext.GetConfigurationData(sIPConfigKey).FirstOrDefault();
            if (sInterfaceIP != null)
            {

                System.Net.IPAddress sIP;
                System.Net.IPAddress.TryParse(sInterfaceIP.ConfigurationValue.ToString(), out sIP);

                txtIP.Text = sInterfaceIP.ConfigurationValue.ToString();
            }

            var sInterfacePortNo = configDBContext.GetConfigurationData(sPortNoConfigKey).FirstOrDefault();
            if (sInterfacePortNo != null)
            {
                txtPortNo.Text = sInterfacePortNo.ConfigurationValue.ToString();
            }
        }

        private void btnDevice_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingDevicePageHandler(e, sender);
        }

        private void btnLanguage_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingLanguageCountryPageHandler(e, sender);
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingUserPageHandler(e, sender);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingReportPageHandler(e, sender);
        }

        private void NumberValidationOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FieldsVal_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Boolean isFieldEmpty = false;

            String sIP = txtIP.Text;
            if (sIP.Replace(" ", "").Replace(".", "").Trim().Length == 0)
            {
                borderIP.BorderBrush = System.Windows.Media.Brushes.Red;
                borderIP.BorderThickness = new Thickness(1);
                borderIP.ToolTip = "This is a mandatory field";

                isFieldEmpty = true;
            }
            else
            {
                borderIP.BorderBrush = System.Windows.Media.Brushes.Black;
                borderIP.ToolTip = null;
            }

            System.Net.IPAddress iIP;
            if (!System.Net.IPAddress.TryParse(txtIP.Text.Replace(" ", ""), out iIP))
            {
                borderIP.BorderBrush = System.Windows.Media.Brushes.Red;
                borderIP.BorderThickness = new Thickness(1);
                borderIP.ToolTip = "Invalid IP address entered.";

                isFieldEmpty = true;
            }
            else
            {
                borderIP.BorderBrush = System.Windows.Media.Brushes.Black;
                borderIP.ToolTip = null;
            }

            if (String.IsNullOrEmpty(txtPortNo.Text))
            {
                borderPortNo.BorderBrush = System.Windows.Media.Brushes.Red;
                borderPortNo.BorderThickness = new Thickness(1);
                borderPortNo.ToolTip = "This is a mandatory field.";

                isFieldEmpty = true;
            }
            else
            {
                borderPortNo.BorderBrush = System.Windows.Media.Brushes.Black;
                borderPortNo.ToolTip = null;
            }

            if (isFieldEmpty)
            {
                btnUpdate.IsEnabled = false;
            }
            else
            {
                btnUpdate.IsEnabled = true;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsIPAddressAvailable(txtIP.Text.Replace(" ",""), int.Parse(txtPortNo.Text)))
                {
                    List<ConfigurationModel> sConfigList = new List<ConfigurationModel>();

                    sConfigList.Add(new ConfigurationModel
                    {
                        ConfigurationKey = sIPConfigKey,
                        ConfigurationValue = txtIP.Text
                    });

                    sConfigList.Add(new ConfigurationModel
                    {
                        ConfigurationKey = sPortNoConfigKey,
                        ConfigurationValue = txtPortNo.Text
                    });


                    Popup sConfirmPopup = new Popup();
                    sConfirmPopup.IsOpen = true;

                    App.MainViewModel.Origin = "SettingsUpdate";
                    App.MainViewModel.ConfigurationModel = sConfigList;

                    App.PopupHandler(e, sender);

                    App.RestartListener = true;
                }
                else
                {
                    App.MainViewModel.Origin = "IpPortUnavailable";
                    App.PopupHandler(e, sender);
                }
            }
            catch(Exception ex)
            {
                App.MainViewModel.Origin = "FailedUpdateLIS";
                App.PopupHandler(e, sender);
            }

        }

        private static bool IsIPAddressAvailable(string ipAddress, int port)
        {
            try
            {
                System.Net.IPAddress ip = System.Net.IPAddress.Parse(ipAddress.Trim());
                IPEndPoint endPoint = new IPEndPoint(ip, port);

                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Bind(endPoint);
                    return true;
                }
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }
}
