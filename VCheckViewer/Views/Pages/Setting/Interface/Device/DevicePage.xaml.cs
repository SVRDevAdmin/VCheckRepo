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
using System.Collections.ObjectModel;

namespace VCheckViewer.Views.Pages.Setting.Device
{
    /// <summary>
    /// Interaction logic for DevicePage.xaml
    /// </summary>
    public partial class DevicePage : Page
    {
        public ObservableCollection<ComboBoxItem> deviceComboList = new ObservableCollection<ComboBoxItem>();

        public DevicePage()
        {
            InitializeComponent();
            
            LoadDeviceTypeList();
            LoadDeviceListing();

            DataContext = this;
        }

        private void LoadDeviceTypeList()
        {
            List<DeviceTypeModel> deviceList = DeviceRepository.GetDeviceTypeList(ConfigSettings.GetConfigurationSettings());
            if (deviceList != null)
            {
                foreach(var d in deviceList)
                {
                    deviceComboList.Add(new ComboBoxItem
                    {
                        Content = d.TypeName,
                        Tag = d.id
                    });
                }
            }

            if (deviceComboList.Count > 0)
            {
                cboDeviceType.ItemsSource = deviceComboList;
            }
        }

        private void LoadDeviceListing()
        {          
            dgDevice.ItemsSource = DeviceRepository.GetDeviceList(ConfigSettings.GetConfigurationSettings());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            String sDefaultImagePath = "\\Storage\\Device\\Img_F200.png";
            String sSelectedDeviceType = "";
            int sSelectedDeviceTypeID = 0;

            if (cboDeviceType.SelectedItem != null)
            {
                var sSelectedItem = ((ComboBoxItem)cboDeviceType.SelectedItem);
                int iID = Convert.ToInt32(sSelectedItem.Tag);

                var sDeviceTypeObj = DeviceRepository.GetDeviceTypeByID(ConfigSettings.GetConfigurationSettings(), iID);
                if (sDeviceTypeObj != null)
                {
                    sSelectedDeviceType = sDeviceTypeObj.ImageSource;
                    sSelectedDeviceTypeID = sDeviceTypeObj.id;
                }
            }
            else
            {
                sSelectedDeviceType = sDefaultImagePath;
            }

            DeviceModel sDeviceObj = new DeviceModel()
            {
                DeviceName = txtName.Text,
                DeviceIPAddress = txtIPAddress.Text,
                //DeviceImagePath = "\\Storage\\Device\\Img_F200.png", // Temp Hardcode
                DeviceImagePath = sSelectedDeviceType,
                status = (int)DataDictionary.DeviceListStatus.Active,
                CreatedDate = DateTime.Now,
                CreatedBy = App.MainViewModel.CurrentUsers.FullName,
                DeviceSerialNo = txtSerialNo.Text,
                DeviceTypeID = sSelectedDeviceTypeID
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

                var sDeviceType = DeviceRepository.GetDeviceTypeByID(ConfigSettings.GetConfigurationSettings(), sRowData.DeviceTypeID.Value);

                lbName.Text = sRowData.DeviceName;
                lbIPAddeess.Text = sRowData.DeviceIPAddress;
                lbSerialNo.Text = sRowData.DeviceSerialNo;
                lbDeviceType.Text = (sDeviceType != null) ? sDeviceType.TypeName : "";
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
                txtSerialNo.Text = sDeviceObj.DeviceSerialNo;
                lbSerialNo.Text = sDeviceObj.DeviceSerialNo;
                hidID.Text = sDeviceObj.id.ToString();

                foreach(var itm in cboDeviceType.Items)
                {
                    var cb = itm as ComboBoxItem;

                    if (Convert.ToInt32(cb.Tag) == sDeviceObj.DeviceTypeID)
                    {
                        cb.IsSelected = true;
                    }
                    else
                    {
                        cb.IsSelected = false;
                    }
                }
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
                    sDeviceObj.UpdatedBy = App.MainViewModel.CurrentUsers.FullName;
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
            String sSelectedDeviceType = "";
            String sOldDeviceName = "";
            int iTypeID = 0;

            if (!String.IsNullOrEmpty(hidID.Text))
            {
                int iID = 0;
                int.TryParse(hidID.Text, out iID);

                var sDeviceObj = DeviceRepository.GetDeviceByID(iID, ConfigSettings.GetConfigurationSettings());
                if (sDeviceObj != null)
                {
                    sOldDeviceName = sDeviceObj.DeviceName;

                    if (cboDeviceType.SelectedItem != null)
                    {
                        var sSelectedItem = ((ComboBoxItem)cboDeviceType.SelectedItem);
                        iTypeID = Convert.ToInt32(sSelectedItem.Tag);

                        var sDeviceTypeObj = DeviceRepository.GetDeviceTypeByID(ConfigSettings.GetConfigurationSettings(), iTypeID);
                        if (sDeviceTypeObj != null)
                        {
                            sSelectedDeviceType = sDeviceTypeObj.ImageSource;
                        }
                    }

                    sDeviceObj.DeviceName = txtName.Text;
                    sDeviceObj.DeviceIPAddress = txtIPAddress.Text;
                    sDeviceObj.UpdatedDate = DateTime.Now;
                    sDeviceObj.UpdatedBy = App.MainViewModel.CurrentUsers.FullName;
                    sDeviceObj.DeviceSerialNo = txtSerialNo.Text;
                    sDeviceObj.DeviceTypeID = iTypeID;
                    sDeviceObj.DeviceImagePath = sSelectedDeviceType;
                }

                Popup sConfirmPopup = new Popup();
                sConfirmPopup.IsOpen = true;

                App.MainViewModel.Origin = "DeviceUpdate";
                App.MainViewModel.DeviceModel = sDeviceObj;
                App.MainViewModel.OldDeviceName = sOldDeviceName;

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

                borderSerialNoEdit.Visibility = Visibility.Collapsed;
                borderSerialNoView.Visibility = Visibility.Visible;

                borderDeviceTypeEdit.Visibility = Visibility.Collapsed;
                borderDeviceTypeView.Visibility = Visibility.Visible;

                borderButtonAdd.Visibility = Visibility.Collapsed;
                borderButtonView.Visibility = Visibility.Visible;
                borderButtonUpdate.Visibility = Visibility.Collapsed;

                imgBack.Visibility = Visibility.Visible;
                btnBack.Visibility = Visibility.Visible;

                btnBack.Content = Properties.Resources.Device_Label_Button_BackDevice;
            }
            else if (sType.ToLower() == "update")
            {
                borderNameEdit.Visibility = Visibility.Visible;
                borderNameView.Visibility = Visibility.Collapsed;

                borderIPEdit.Visibility = Visibility.Visible;
                borderIPView.Visibility = Visibility.Collapsed;

                borderSerialNoEdit.Visibility = Visibility.Visible;
                borderSerialNoView.Visibility = Visibility.Collapsed;

                borderDeviceTypeEdit.Visibility = Visibility.Visible;
                borderDeviceTypeView.Visibility = Visibility.Collapsed;

                borderButtonAdd.Visibility = Visibility.Collapsed;
                borderButtonView.Visibility = Visibility.Collapsed;
                borderButtonUpdate.Visibility = Visibility.Visible;

                imgBack.Visibility = Visibility.Visible;
                btnBack.Visibility = Visibility.Visible;
                btnBack.Content = Properties.Resources.Device_Label_Button_BackViewDevice;
            }
            else if (sType.ToLower() == "add")
            {
                borderNameEdit.Visibility = Visibility.Visible;
                borderNameView.Visibility = Visibility.Collapsed;
                txtName.Text = String.Empty;

                borderIPEdit.Visibility = Visibility.Visible;
                borderIPView.Visibility = Visibility.Collapsed;
                txtIPAddress.Text = String.Empty;

                borderSerialNoEdit.Visibility = Visibility.Visible;
                borderSerialNoView.Visibility = Visibility.Collapsed;
                txtSerialNo.Text = String.Empty;

                borderDeviceTypeEdit.Visibility = Visibility.Visible;
                borderDeviceTypeView.Visibility = Visibility.Collapsed;

                borderButtonAdd.Visibility = Visibility.Visible;
                borderButtonView.Visibility = Visibility.Collapsed;
                borderButtonUpdate.Visibility = Visibility.Collapsed;

                imgBack.Visibility = Visibility.Collapsed;
                btnBack.Visibility = Visibility.Collapsed;

                btnBack.Content = Properties.Resources.Device_Label_Button_BackDevice;
            }
        }


        private void cboDeviceType_LostFocus(object sender, RoutedEventArgs e)
        {
            MandatoryFieldValiation(sender);
        }

        private void FieldsVal_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            MandatoryFieldValiation(sender);
        }

        private void MandatoryFieldValiation(object sender)
        {
            System.Windows.Controls.TextBox sTxtField = sender as System.Windows.Controls.TextBox;
            Boolean IsFieldEmpty = false;

            var sTxtNameBorder = txtName.Parent as Border;
            if (String.IsNullOrEmpty(txtName.Text))
            {
                sTxtNameBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                sTxtNameBorder.BorderThickness = new Thickness(1);
                sTxtNameBorder.ToolTip = Properties.Resources.Setting_ErrorMessage_MandatoryField;
                //sTxtNameBorder.ToolTip = "This is a mandatory field";

                IsFieldEmpty = true;
            }
            else
            {
                sTxtNameBorder.BorderBrush = System.Windows.Media.Brushes.Black;
                sTxtNameBorder.ToolTip = null;
            }

            String sIPAddress = txtIPAddress.Text;
            if (sIPAddress.Replace(" ", "").Replace(".", "").Trim().Length == 0)
            {
                borderIPEdit.BorderBrush = System.Windows.Media.Brushes.Red;
                borderIPEdit.BorderThickness = new Thickness(1);
                borderIPEdit.ToolTip = Properties.Resources.Setting_ErrorMessage_MandatoryField;
                //borderIPEdit.ToolTip = "This is a mandatory field";

                IsFieldEmpty = true;
            }
            else
            {
                borderIPEdit.BorderBrush = System.Windows.Media.Brushes.Black;
                borderIPEdit.ToolTip = null;
            }

            IPAddress iIP;
            if (!IPAddress.TryParse(txtIPAddress.Text.Replace(" ", ""), out iIP))
            {
                borderIPEdit.BorderBrush = System.Windows.Media.Brushes.Red;
                borderIPEdit.BorderThickness = new Thickness(1);
                borderIPEdit.ToolTip = Properties.Resources.Setting_ErrorMessage_InvalidIP;
                //borderIPEdit.ToolTip = "Invalid IP address entered";

                IsFieldEmpty = true;
            }
            else
            {
                borderIPEdit.BorderBrush = System.Windows.Media.Brushes.Black;
                borderIPEdit.ToolTip = null;
            }

            if (String.IsNullOrEmpty(txtSerialNo.Text))
            {
                borderSerialNoEdit.BorderBrush = System.Windows.Media.Brushes.Red;
                borderSerialNoEdit.BorderThickness = new Thickness(1);
                borderSerialNoEdit.ToolTip = Properties.Resources.Setting_ErrorMessage_MandatoryField;
                //borderSerialNoEdit.ToolTip = "This is mandatory fields.";

                IsFieldEmpty = true;
            }
            else
            {
                borderSerialNoEdit.BorderBrush = System.Windows.Media.Brushes.Black;
                borderSerialNoEdit.ToolTip = null;
            }

            if (cboDeviceType.SelectedItem == null)
            {
                borderDeviceTypeEdit.BorderBrush = System.Windows.Media.Brushes.Red;
                borderDeviceTypeEdit.BorderThickness = new Thickness(1);
                borderDeviceTypeEdit.ToolTip = Properties.Resources.Setting_ErrorMessage_MandatoryField;
                //borderDeviceTypeEdit.ToolTip = "This is mandary fields.";

                IsFieldEmpty = true;
            }
            else
            {
                borderDeviceTypeEdit.BorderBrush = System.Windows.Media.Brushes.Black;
                borderDeviceTypeEdit.ToolTip = "";
            }

            String? sColor = System.Windows.Application.Current.Resources["Themes_ButtonColor"].ToString();

            if (IsFieldEmpty)
            {
                if (borderButtonAdd.Visibility != Visibility.Collapsed)
                {
                    btnAdd.IsEnabled = false;
                    btnAdd.Background = new BrushConverter().ConvertFrom(sColor) as SolidColorBrush;
                    //btnAdd.Background = System.Windows.Media.Brushes.Orange;
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
            };
        }
        //private void btnBackDevice_Click(object sender, RoutedEventArgs e)
        //{
        //    if (borderNameEdit.Visibility == Visibility.Visible && borderButtonUpdate.Visibility == Visibility.Visible)
        //    {
        //        ShowHideBorder("View");
        //    }
        //    else if (borderNameEdit.Visibility == Visibility.Collapsed && borderButtonView.Visibility == Visibility.Visible)
        //    {
        //        ShowHideBorder("Add");
        //    }
        //}

        private void LanguageCountry(object sender, RoutedEventArgs e)
        {
            App.GoToSettingLanguageCountryPageHandler(e, sender);
        }

        private void UserPage(object sender, RoutedEventArgs e)
        {
            App.GoToSettingUserPageHandler(e, sender);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingConfigurationPageHandler(e, sender);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingReportPageHandler(e, sender);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigateBackButton();
        }

        private void imgBack_Click(object sender, RoutedEventArgs e)
        {
            NavigateBackButton();
        }

        private void NavigateBackButton()
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
    }
}
