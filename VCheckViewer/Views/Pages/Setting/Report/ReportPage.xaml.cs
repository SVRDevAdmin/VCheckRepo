using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;

namespace VCheckViewer.Views.Pages.Setting.Report
{
    /// <summary>
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        public ConfigurationDBContext configDBContext = new ConfigurationDBContext(ConfigSettings.GetConfigurationSettings());

        public ReportPage()
        {
            InitializeComponent();
            LoadInformation();
        }

        private void LoadInformation()
        {
            var sReportImagePath = configDBContext.GetConfigurationData("ReportImagePath").FirstOrDefault();
            var sClinicName = configDBContext.GetConfigurationData("ClinicName").FirstOrDefault();
            var sReportTitle = configDBContext.GetConfigurationData("ReportTitle").FirstOrDefault();
            var sClinicAddress = configDBContext.GetConfigurationData("ClinicAddress").FirstOrDefault();

            var bitmap = new BitmapImage();

            try
            {
                Uri uri = new Uri(sReportImagePath != null ? sReportImagePath.ConfigurationValue : "pack://application:,,,/Content/Images/Report Logo Default.png");
                bitmap = new BitmapImage(uri);
            }
            catch (Exception ex)
            {
                Uri uri = new Uri("pack://application:,,,/Content/Images/Report Logo Default.png");
                bitmap = new BitmapImage(uri);
            }

            Logo.Source = bitmap;
            ClinicName.Text = sClinicName != null ? sClinicName.ConfigurationValue : "";
            ReportTitle.Text = sReportTitle != null ? sReportTitle.ConfigurationValue : "";
            ClinicAddress.Text = sClinicAddress != null ? sClinicAddress.ConfigurationValue : "";
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

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingConfigurationPageHandler(e, sender);
        }

        private void image_click(object sender, MouseButtonEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                LogoPath.Text = dlg.FileName;
                var uri = new Uri(dlg.FileName);
                var bitmap = new BitmapImage(uri);

                Logo.Source = bitmap;
            }
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(LogoPath.Text) || String.IsNullOrEmpty(ClinicName.Text) || String.IsNullOrEmpty(ReportTitle.Text) || String.IsNullOrEmpty(ClinicAddress.Text))
            {
                UpdateButton.IsEnabled = false;
            }
            else
            {
                List<ConfigurationModel> sConfigList = new List<ConfigurationModel>();

                sConfigList.Add(new ConfigurationModel
                {
                    ConfigurationKey = "ReportImagePath",
                    ConfigurationValue = LogoPath.Text.Replace("\\", "\\\\")
                });

                sConfigList.Add(new ConfigurationModel
                {
                    ConfigurationKey = "ClinicName",
                    ConfigurationValue = ClinicName.Text
                });

                sConfigList.Add(new ConfigurationModel
                {
                    ConfigurationKey = "ReportTitle",
                    ConfigurationValue = ReportTitle.Text
                });

                sConfigList.Add(new ConfigurationModel
                {
                    ConfigurationKey = "ClinicAddress",
                    ConfigurationValue = ClinicAddress.Text
                });

                Popup sConfirmPopup = new Popup();
                sConfirmPopup.IsOpen = true;

                App.MainViewModel.Origin = "SettingsUpdate";
                App.MainViewModel.ConfigurationModel = sConfigList;

                App.PopupHandler(e, sender);
            }
        }
    }
}
