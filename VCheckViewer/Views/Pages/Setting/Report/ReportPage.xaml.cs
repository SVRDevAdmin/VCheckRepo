using System;
using System.Collections.Generic;
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
using VCheck.Lib.Data.Models;

namespace VCheckViewer.Views.Pages.Setting.Report
{
    /// <summary>
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
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

        private void txtFileName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                txtFileName.Text = filename;

                btnUpdate.IsEnabled = true;
            }
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtFileName.Text) || String.IsNullOrEmpty(txtContactNo.Text))
            {
                btnUpdate.IsEnabled = false;
            }
            else
            {
                List<ConfigurationModel> sConfigList = new List<ConfigurationModel>();

                sConfigList.Add(new ConfigurationModel
                {
                    ConfigurationKey = "ReportImagePath",
                    ConfigurationValue = txtFileName.Text.Replace("\\", "\\\\")
                });

                sConfigList.Add(new ConfigurationModel
                {
                    ConfigurationKey = "ContactInfo",
                    ConfigurationValue = txtContactNo.Text
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
