using MySqlX.XDevAPI;
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
using VCheck.Interface.API;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;
using RadioButton = System.Windows.Controls.RadioButton;

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
        public String sUsernameConfigKey = "InterfaceSettingsUsername";
        public String sPasswordConfigKey = "InterfaceSettingsPassword";
        public String sPMSConfigKey = "InterfaceSettingsPMS";
        public String sClinicIDConfigKey = "ClinicID";
        public String sClinicPhoneNumConfigKey = "ClinicPhoneNum";

        public string PMSURL = "";
        public string ClinicID = "";

        public ConfigurationPage()
        {
            InitializeComponent();

            //txtIP.ValidatingType = typeof(System.Net.IPAddress);
            //txtIP.ResetOnSpace = false;

            //LoadInterfaceConfiguration();
            GetPMSURLAsync(2);
        }

        private void CheckPMSSelected()
        {
            var sInterfacePMS = configDBContext.GetConfigurationData(sPMSConfigKey).FirstOrDefault();
            if (sInterfacePMS != null)
            {
                if(sInterfacePMS.ConfigurationValue.ToString() == "Greywind")
                {
                    Greywind.IsChecked = true;
                    CheckGreywindConnection();
                }
                else
                {
                    Other.IsChecked = true;
                }
            }
            else
            {
                Other.IsChecked = true;
            }
        }

        private void LoadInterfaceConfiguration()
        {
            var selected = "";

            var sInterfacePMS = configDBContext.GetConfigurationData(sPMSConfigKey).FirstOrDefault();
            if (sInterfacePMS != null)
            {
                selected = sInterfacePMS.ConfigurationValue.ToString();
            }
            else { txtIP.Text = ""; }

            if (selected == "Greywind")
            {
                txtIP.Text = "https://bionote.api.test.hl7i.com";
                txtPortNo.Text = "80";
                txtIP.IsEnabled = false;
                txtPortNo.IsEnabled = false;
            }
            else
            {
                var sInterfaceIP = configDBContext.GetConfigurationData(sIPConfigKey).FirstOrDefault();
                if (sInterfaceIP != null)
                {
                    txtIP.Text = sInterfaceIP.ConfigurationValue.ToString();
                }
                else { txtIP.Text = ""; }

                var sInterfacePortNo = configDBContext.GetConfigurationData(sPortNoConfigKey).FirstOrDefault();
                if (sInterfacePortNo != null)
                {
                    txtPortNo.Text = sInterfacePortNo.ConfigurationValue.ToString();
                }
                else { txtPortNo.Text = ""; }

                var sInterfaceUsername = configDBContext.GetConfigurationData(sUsernameConfigKey).FirstOrDefault();
                if (sInterfaceUsername != null)
                {
                    txtUsername.Text = sInterfaceUsername.ConfigurationValue.ToString();
                }
                else { txtUsername.Text = ""; }

                var sInterfacePassword = configDBContext.GetConfigurationData(sPasswordConfigKey).FirstOrDefault();
                if (sInterfacePassword != null)
                {
                    txtPassword.Password = sInterfacePassword.ConfigurationValue.ToString();
                }
                else { txtPassword.Password = ""; }

                txtIP.IsEnabled = true;
                txtPortNo.IsEnabled = true;
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

        private void ClinicInfoPage(object sender, RoutedEventArgs e)
        {
            App.GoToClinicInfoPageHandler(e, sender);
        }

        private void NumberValidationOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FieldsVal_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Boolean isFieldEmpty = false;

            if (!Greywind.IsChecked.GetValueOrDefault())
            {
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

                //System.Net.IPAddress iIP;
                //if (!System.Net.IPAddress.TryParse(txtIP.Text.Replace(" ", ""), out iIP) || txtIP.Text.Split(".").Count() != 4)
                //{
                //    borderIP.BorderBrush = System.Windows.Media.Brushes.Red;
                //    borderIP.BorderThickness = new Thickness(1);
                //    borderIP.ToolTip = "Invalid IP address entered.";

                //    isFieldEmpty = true;
                //}
                //else
                //{
                //    borderIP.BorderBrush = System.Windows.Media.Brushes.Black;
                //    borderIP.ToolTip = null;
                //}

                if (String.IsNullOrEmpty(txtPortNo.Text) || int.Parse(txtPortNo.Text) == 0)
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

                if (String.IsNullOrEmpty(txtUsername.Text))
                {
                    borderUsername.BorderBrush = System.Windows.Media.Brushes.Red;
                    borderUsername.BorderThickness = new Thickness(1);
                    borderUsername.ToolTip = "This is a mandatory field.";

                    isFieldEmpty = true;
                }
                else
                {
                    borderUsername.BorderBrush = System.Windows.Media.Brushes.Black;
                    borderUsername.ToolTip = null;
                }

                if (String.IsNullOrEmpty(txtPassword.Password))
                {
                    borderPassword.BorderBrush = System.Windows.Media.Brushes.Red;
                    borderPassword.BorderThickness = new Thickness(1);
                    borderPassword.ToolTip = "This is a mandatory field.";

                    isFieldEmpty = true;
                }
                else
                {
                    borderPassword.BorderBrush = System.Windows.Media.Brushes.Black;
                    borderPassword.ToolTip = null;
                }
            }
            else
            {
                if (txtIP.Text.Replace(" ", "").Replace(".", "").Trim().Length == 0)
                {
                    isFieldEmpty = true;
                }

                if (string.IsNullOrEmpty(txtClinicPhoneNum.Text))
                {
                    borderClinicPhoneNum.BorderBrush = System.Windows.Media.Brushes.Red;
                    borderClinicPhoneNum.BorderThickness = new Thickness(1);
                    borderClinicPhoneNum.ToolTip = "Please create clinic information first.";
                    isFieldEmpty = true;
                }
                else
                {
                    borderClinicPhoneNum.BorderBrush = System.Windows.Media.Brushes.Black;
                    borderClinicPhoneNum.ToolTip = null;
                }

                if (btnConnect.IsEnabled) { isFieldEmpty = true; }

                borderIP.BorderBrush = System.Windows.Media.Brushes.Black;
                borderIP.ToolTip = null;
                borderPortNo.BorderBrush = System.Windows.Media.Brushes.Black;
                borderPortNo.ToolTip = null;
                borderUsername.BorderBrush = System.Windows.Media.Brushes.Black;
                borderUsername.ToolTip = null;
                borderPassword.BorderBrush = System.Windows.Media.Brushes.Black;
                borderPassword.ToolTip = null;
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
                //if (IsIPAddressAvailable(txtIP.Text.Replace(" ",""), int.Parse(txtPortNo.Text)))
                //{
                //    List<ConfigurationModel> sConfigList = new List<ConfigurationModel>();

                //    sConfigList.Add(new ConfigurationModel
                //    {
                //        ConfigurationKey = sIPConfigKey,
                //        ConfigurationValue = txtIP.Text
                //    });

                //    sConfigList.Add(new ConfigurationModel
                //    {
                //        ConfigurationKey = sPortNoConfigKey,
                //        ConfigurationValue = txtPortNo.Text
                //    });


                //    Popup sConfirmPopup = new Popup();
                //    sConfirmPopup.IsOpen = true;

                //    App.MainViewModel.Origin = "SettingsUpdate";
                //    App.MainViewModel.ConfigurationModel = sConfigList;

                //    App.PopupHandler(e, sender);

                //    App.RestartListener = true;
                //}
                //else
                //{
                //    App.MainViewModel.Origin = "IpPortUnavailable";
                //    App.PopupHandler(e, sender);
                //}

                List<ConfigurationModel> sConfigList = new List<ConfigurationModel>();

                if (!Greywind.IsChecked.GetValueOrDefault())
                {
                    sConfigList.Add(new ConfigurationModel
                    {
                        ConfigurationKey = sIPConfigKey,
                        ConfigurationValue = txtIP.Text
                    });

                    sConfigList.Add(new ConfigurationModel
                    {
                        ConfigurationKey = sPortNoConfigKey,
                        ConfigurationValue = int.Parse(txtPortNo.Text).ToString()
                    });

                    sConfigList.Add(new ConfigurationModel
                    {
                        ConfigurationKey = sUsernameConfigKey,
                        ConfigurationValue = txtUsername.Text
                    });

                    sConfigList.Add(new ConfigurationModel
                    {
                        ConfigurationKey = sPasswordConfigKey,
                        ConfigurationValue = txtPassword.Password
                    });
                }

                var selectedPMS = gridPMSSelection.Children.OfType<RadioButton>().FirstOrDefault(x => x.IsChecked.GetValueOrDefault()).Name;
                sConfigList.Add(new ConfigurationModel
                {
                    ConfigurationKey = sPMSConfigKey,
                    ConfigurationValue = selectedPMS
                });

                Popup sConfirmPopup = new Popup();
                sConfirmPopup.IsOpen = true;

                App.MainViewModel.Origin = "SettingsUpdate";
                App.MainViewModel.ConfigurationModel = sConfigList;

                App.PopupHandler(e, sender);
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if(selected.Name == "Greywind")
            {
                CheckGreywindConnection();
                var sClinicPhoneNum = configDBContext.GetConfigurationData(sClinicPhoneNumConfigKey).FirstOrDefault();
                if (sClinicPhoneNum != null)
                {
                    txtClinicPhoneNum.Text = "+" + sClinicPhoneNum.ConfigurationValue.ToString();
                }
                else { txtClinicPhoneNum.Text = ""; }

                //GetPMSURLAsync(2);

                txtIP.Text = PMSURL;
                txtPortNo.Text = "80";
                txtUsername.Text = "svrtech";
                txtPassword.Password = "V7FZ+fu9sQ9*";
                txtIP.IsEnabled = false;
                txtPortNo.IsEnabled = false;

                ClinicPhoneNumLabel.Visibility = Visibility.Visible;
                UsernameLabel.Visibility = Visibility.Collapsed;
                PasswordLabel.Visibility = Visibility.Collapsed;
                borderClinicPhoneNum.Visibility = Visibility.Visible;
                borderUsername.Visibility = Visibility.Collapsed;
                borderPassword.Visibility = Visibility.Collapsed;
                btnConnect.Visibility = Visibility.Visible;
            }
            else
            {
                var sInterfaceIP = configDBContext.GetConfigurationData(sIPConfigKey).FirstOrDefault();
                if (sInterfaceIP != null)
                {
                    txtIP.Text = sInterfaceIP.ConfigurationValue.ToString();
                }
                else { txtIP.Text = ""; }

                var sInterfacePortNo = configDBContext.GetConfigurationData(sPortNoConfigKey).FirstOrDefault();
                if (sInterfacePortNo != null)
                {
                    txtPortNo.Text = sInterfacePortNo.ConfigurationValue.ToString();
                }
                else { txtPortNo.Text = ""; }

                var sInterfaceUsername = configDBContext.GetConfigurationData(sUsernameConfigKey).FirstOrDefault();
                if (sInterfaceUsername != null)
                {
                    txtUsername.Text = sInterfaceUsername.ConfigurationValue.ToString();
                }
                else { txtUsername.Text = ""; }

                var sInterfacePassword = configDBContext.GetConfigurationData(sPasswordConfigKey).FirstOrDefault();
                if (sInterfacePassword != null)
                {
                    txtPassword.Password = sInterfacePassword.ConfigurationValue.ToString();
                }
                else { txtPassword.Password = ""; }

                txtIP.IsEnabled = true;
                txtPortNo.IsEnabled = true;
                ClinicPhoneNumLabel.Visibility = Visibility.Collapsed;
                UsernameLabel.Visibility = Visibility.Visible;
                PasswordLabel.Visibility = Visibility.Visible;
                borderClinicPhoneNum.Visibility = Visibility.Collapsed;
                borderUsername.Visibility = Visibility.Visible;
                borderPassword.Visibility = Visibility.Visible;
                btnConnect.Visibility = Visibility.Collapsed;
            }            

            FieldsVal_KeyUp(null, null);
        }

        public async Task GetPMSURLAsync(int clientID)
        {
            VCheckAPI VcheckAPI = new VCheckAPI();
            PMSURL =  await VcheckAPI.GetPMSUrl(clientID);
            

            if (string.IsNullOrEmpty(PMSURL)) { Greywind.IsEnabled = false; }
            else { Greywind.IsEnabled = true; txtIP.Text = PMSURL; }

            CheckPMSSelected();
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            btnConnect.IsEnabled = false;
            var success = await CreateClinicInfoPMS();

            if (success)
            {
                btnConnect.Content = Properties.Resources.Maintenance_Label_Connected; ;
                FieldsVal_KeyUp(new RadioButton() { Name = "Greywind"}, null);
                App.MainViewModel.Origin = "GreywindConnected";
                App.PopupHandler(e, sender);
            }
            else
            {
                btnConnect.IsEnabled = true;
                App.MainViewModel.Origin = "FailedGreywindConnect";
                App.PopupHandler(e, sender);
            }
        }

        private async Task CheckGreywindConnection()
        {
            var sClinicID = configDBContext.GetConfigurationData(sClinicIDConfigKey).FirstOrDefault();
            var sClinicPhoneNum = configDBContext.GetConfigurationData(sClinicPhoneNumConfigKey).FirstOrDefault();
            var hasClinicID = sClinicID != null && !string.IsNullOrEmpty(sClinicID.ConfigurationValue);
            var noPhoneNumber = sClinicPhoneNum == null;
            bool disableConnect = false;
            bool cannotConnect = false;

            if (hasClinicID)
            {
                ClinicID = sClinicID.ConfigurationValue.ToString();

                disableConnect = await CheckGreywindClinic();
            }
            else if(noPhoneNumber)
            {
                cannotConnect = true;
            }

            if (cannotConnect)
            {
                btnConnect.IsEnabled = false;
            }
            else if (disableConnect)
            {
                btnConnect.IsEnabled = false;
                btnConnect.Content = Properties.Resources.Maintenance_Label_Connected;
            }
            else
            {
                btnConnect.IsEnabled = true;
            }
        }

        private async Task<bool> CheckGreywindClinic()
        {
            GreywindAPI sAPI = new GreywindAPI();

            return await sAPI.GetClinicInfo(ClinicID, PMSURL);
        }

        private async Task<bool> CreateClinicInfoPMS()
        {
            var sSettingsObj = configDBContext.GetConfigurationData("");
            var clinic = sSettingsObj.FirstOrDefault(x => x.ConfigurationKey == "ClinicID");

            if (clinic != null)
            {
                ClinicID = clinic.ConfigurationKey;
            }
            else
            {
                VCheckAPI vcheckAPI = new VCheckAPI();
                GreywindAPI greywindAPI = new GreywindAPI();

                var name = sSettingsObj.FirstOrDefault(x => x.ConfigurationKey == "ClinicName").ConfigurationValue;
                var address = sSettingsObj.FirstOrDefault(x => x.ConfigurationKey == "ClinicAddress").ConfigurationValue;
                var city = sSettingsObj.FirstOrDefault(x => x.ConfigurationKey == "ClinicCity").ConfigurationValue;
                var state = sSettingsObj.FirstOrDefault(x => x.ConfigurationKey == "ClinicState").ConfigurationValue;
                var phoneNum = sSettingsObj.FirstOrDefault(x => x.ConfigurationKey == "ClinicPhoneNum").ConfigurationValue;

                VCheck.Interface.API.Lib.General.LocationModel location = new VCheck.Interface.API.Lib.General.LocationModel()
                {
                    ID = "",
                    Name = name,
                    Address = address,
                    ContactName = sSettingsObj.FirstOrDefault(x => x.ConfigurationKey == "ClinicContactName").ConfigurationValue,
                    PhoneNum = phoneNum,
                    Email = sSettingsObj.FirstOrDefault(x => x.ConfigurationKey == "ClinicEmail").ConfigurationValue,
                    Description = "Clinic",
                    Status = 1,
                    CreatedBy = "VCheck Viewer"
                };

                ClinicID = await vcheckAPI.UpdateLocation(location);

                VCheck.Interface.API.Greywind.RequestMessage.InsertLocationRequest insertLocation = new VCheck.Interface.API.Greywind.RequestMessage.InsertLocationRequest()
                {
                    clinic_id = ClinicID,
                    name = name,
                    address_1 = address,
                    city = city,
                    state = state,
                    phone = phoneNum,
                    api_access = "1"
                };

                await greywindAPI.InsertClinicInfo(insertLocation, PMSURL);
            }

            if (string.IsNullOrEmpty(ClinicID))
            {
                return false;
            }
            else
            {
                configDBContext.AddConfiguration("ClinicID", ClinicID);
                return await CheckGreywindClinic();
            }
        }
    }
}
