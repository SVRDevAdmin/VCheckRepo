using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
using VCheck.Lib.Data;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;
using VCheckViewer.Lib;
using System.Net;

namespace VCheckViewer.Views.Pages.Setting.Device
{
    /// <summary>
    /// Interaction logic for DevicePage.xaml
    /// </summary>
    public partial class DevicePage : Page
    {
        public DevicePage()
        {
            InitializeComponent();
            LoadDeviceListing();
        }

        private void LoadDeviceListing()
        {
            dgDevice.ItemsSource = DeviceRepository.GetDeviceList(ConfigSettings.GetConfigurationSettings());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DeviceModel sDeviceObj = new DeviceModel()
            {
                DeviceName = txtName.Text,
                DeviceIPAddress = txtIPAddress.Text,
                DeviceImagePath = "C:\\Dev\\VCheck\\VCheckViewer\\Storage\\Device\\Img_F200.png", // Temp Hardcode
                status = (int)DataDictionary.DeviceListStatus.Active,
                CreatedDate = DateTime.Now,
                CreatedBy = "Test"
            };

            Popup sConfirmPopup = new Popup();
            sConfirmPopup.IsOpen = true;

            App.MainViewModel.Origin = "DeviceAdd";
            App.MainViewModel.DeviceModel = sDeviceObj;

            App.PopupHandler(e, sender);
        }

        private void dgDevice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sRowData = ((DataGrid)sender).SelectedValue as DeviceModel;
            if (sRowData != null)
            {
                ShowHideBorder("View");

                lbName.Text = sRowData.DeviceName;
                lbIPAddeess.Text = sRowData.DeviceIPAddress;
                hidID.Text = sRowData.id.ToString();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ShowHideBorder("Update");

            int iID = Convert.ToInt32(hidID.Text);

            var sDeviceObj = DeviceRepository.GetDeviceByID(iID, ConfigSettings.GetConfigurationSettings());
            if (sDeviceObj != null)
            {
                txtName.Text = sDeviceObj.DeviceName;
                lbName.Text = sDeviceObj.DeviceName;
                txtIPAddress.Text = sDeviceObj.DeviceIPAddress;
                lbIPAddeess.Text = sDeviceObj.DeviceIPAddress;
                hidID.Text = sDeviceObj.id.ToString();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(hidID.Text))
            {
                int iID = 0;
                int.TryParse(hidID.Text, out iID);

                var sDeviceObj = DeviceRepository.GetDeviceByID(iID, ConfigSettings.GetConfigurationSettings());
                if (sDeviceObj != null)
                {
                    sDeviceObj.status = 2;
                    sDeviceObj.UpdatedDate = DateTime.Now;
                    sDeviceObj.UpdatedBy = "YY";
                }
                
                Popup sConfirmPopup = new Popup();
                sConfirmPopup.IsOpen = true;

                App.MainViewModel.Origin = "DeviceDelete";
                App.MainViewModel.DeviceModel = sDeviceObj;

                App.PopupHandler(e, sender);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ShowHideBorder("View");
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(hidID.Text))
            {
                int iID = 0;
                int.TryParse(hidID.Text, out iID);

                var sDeviceObj = DeviceRepository.GetDeviceByID(iID, ConfigSettings.GetConfigurationSettings());
                if (sDeviceObj != null)
                {
                    sDeviceObj.DeviceName = txtName.Text;
                    sDeviceObj.DeviceIPAddress = txtIPAddress.Text;
                    sDeviceObj.DeviceIPAddress = txtIPAddress.Text;
                    sDeviceObj.UpdatedDate = DateTime.Now;
                    sDeviceObj.UpdatedBy = "YY";
                }

                Popup sConfirmPopup = new Popup();
                sConfirmPopup.IsOpen = true;

                App.MainViewModel.Origin = "DeviceUpdate";
                App.MainViewModel.DeviceModel = sDeviceObj;

                App.PopupHandler(e, sender);
            }
        }

        private void ShowHideBorder(string sType)
        {
            if (sType.ToLower() == "view")
            {
                borderNameEdit.Visibility = Visibility.Collapsed;
                borderNameView.Visibility = Visibility.Visible;

                borderIPEdit.Visibility = Visibility.Collapsed;
                borderIPView.Visibility = Visibility.Visible;

                borderButtonAdd.Visibility = Visibility.Collapsed;
                borderButtonView.Visibility = Visibility.Visible;
                borderButtonUpdate.Visibility = Visibility.Collapsed;

                btnBackDevice.Visibility = Visibility.Visible;
                btnBackDevice.Content = "Back to Device.";
            }
            else if (sType.ToLower() == "update")
            {
                borderNameEdit.Visibility = Visibility.Visible;
                borderNameView.Visibility = Visibility.Collapsed;

                borderIPEdit.Visibility = Visibility.Visible;
                borderIPView.Visibility = Visibility.Collapsed;

                borderButtonAdd.Visibility = Visibility.Collapsed;
                borderButtonView.Visibility = Visibility.Collapsed;
                borderButtonUpdate.Visibility = Visibility.Visible;

                btnBackDevice.Visibility = Visibility.Visible;
                btnBackDevice.Content = "Back to View Device.";
            }
            else if (sType.ToLower() == "add")
            {
                borderNameEdit.Visibility = Visibility.Visible;
                borderNameView.Visibility = Visibility.Collapsed;
                txtName.Text = String.Empty;

                borderIPEdit.Visibility = Visibility.Visible;
                borderIPView.Visibility = Visibility.Collapsed;
                txtIPAddress.Text = String.Empty;

                borderButtonAdd.Visibility = Visibility.Visible;
                borderButtonView.Visibility = Visibility.Collapsed;
                borderButtonUpdate.Visibility = Visibility.Collapsed;

                btnBackDevice.Visibility = Visibility.Collapsed;
            }
        }

        private void FieldsVal_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            System.Windows.Controls.TextBox sTxtField = sender as System.Windows.Controls.TextBox;
            Boolean IsFieldEmpty = false;

            var sTxtNameBorder = txtName.Parent as Border;
            if (String.IsNullOrEmpty(txtName.Text))
            {
                sTxtNameBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                sTxtNameBorder.BorderThickness = new Thickness(1);
                sTxtNameBorder.ToolTip = "This is a mandatory field";

                IsFieldEmpty = true;
            }
            else
            {
                sTxtNameBorder.BorderBrush = System.Windows.Media.Brushes.Black;
                sTxtNameBorder.ToolTip = "";
            }

            String sIPAddress = txtIPAddress.Text;
            if (sIPAddress.Replace(" ", "").Replace(".", "").Trim().Length == 0)
            {
                borderIPEdit.BorderBrush = System.Windows.Media.Brushes.Red;
                borderIPEdit.BorderThickness = new Thickness(1);
                borderIPEdit.ToolTip = "This is a mandatory field";

                IsFieldEmpty = true;
            }
            else
            {
                borderIPEdit.BorderBrush = System.Windows.Media.Brushes.Black;
                borderIPEdit.ToolTip = "";
            }

            IPAddress iIP;
            if (!IPAddress.TryParse(txtIPAddress.Text.Replace(" ", ""), out iIP))
            {
                borderIPEdit.BorderBrush = System.Windows.Media.Brushes.Red;
                borderIPEdit.BorderThickness = new Thickness(1);
                borderIPEdit.ToolTip = "Invalid IP address entered";

                IsFieldEmpty = true;
            }
            else
            {
                borderIPEdit.BorderBrush = System.Windows.Media.Brushes.Black;
                borderIPEdit.ToolTip = "";
            }


            if (IsFieldEmpty)
            {
                if (borderButtonAdd.Visibility != Visibility.Collapsed)
                {
                    btnAdd.IsEnabled = false;
                    btnAdd.Background = System.Windows.Media.Brushes.Orange;
                }
                if (borderButtonUpdate.Visibility != Visibility.Collapsed)
                {
                    btnUpdate.IsEnabled = false;
                }
            }
            else
            {
                if (borderButtonAdd.Visibility != Visibility.Collapsed)
                {
                    btnAdd.IsEnabled = true;
                }
                if (borderButtonUpdate.Visibility != Visibility.Collapsed)
                {
                    btnUpdate.IsEnabled = true;
                }
            }
        }

        private void btnBackDevice_Click(object sender, RoutedEventArgs e)
        {
            if (borderNameEdit.Visibility == Visibility.Visible && borderButtonUpdate.Visibility == Visibility.Visible)
            {
                ShowHideBorder("View");
            }
            else if (borderNameEdit.Visibility == Visibility.Collapsed && borderButtonView.Visibility == Visibility.Visible)
            {
                ShowHideBorder("Add");
            }
        }

        private void LanguageCountry(object sender, RoutedEventArgs e)
        {
            App.GoToSettingLanguageCountryPageHandler(e, sender);
        }

        private void UserPage(object sender, RoutedEventArgs e)
        {
            App.GoToSettingUserPageHandler(e, sender);
        }
    }
}
