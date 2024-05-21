using System;
using System.Collections.Generic;
using System.Linq;
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
using VCheck.Lib.Data.Models;
using VCheckViewer.Views.Windows;
using WPFLocalizeExtension.Providers;
using Button = System.Windows.Controls.Button;

namespace VCheckViewer.Views.Pages.Setting.User
{
    /// <summary>
    /// Interaction logic for ViewUserPage.xaml
    /// </summary>
    public partial class ViewUserPage : Page
    {
        UserModel userInfoViewPage;

        public static event EventHandler GoToUpdateCurrentUserPage;

        public ViewUserPage()
        {
            InitializeComponent();

            userInfoViewPage = App.MainViewModel.Users;

            Title.Text = userInfoViewPage.Title;
            //Surname.Text = userInfoViewPage.FirstName;
            FullName.Text = userInfoViewPage.FullName;
            StaffID.Text = userInfoViewPage.EmployeeID;
            RegistrationNo.Text = userInfoViewPage.RegistrationNo;
            Gender.Text = userInfoViewPage.Gender;
            DateOfBirth.Text = userInfoViewPage.DateOfBirth;
            Role.Text = userInfoViewPage.Role;
            EmailAddress.Text = userInfoViewPage.EmailAddress;
            Status.Text = userInfoViewPage.Status;
            LoginID.Text = userInfoViewPage.LoginID;

            if (App.MainViewModel.CurrentUsers.UserId == userInfoViewPage.UserId) { Edit.Visibility = Visibility.Visible; }


            if (App.MainViewModel.CurrentUsers.Role == "Lab User")
            {
                btnSettings.IsEnabled = false;
                btnDeviceSetting.IsEnabled = false;

                UserPage.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnSettings.IsEnabled = true;
                btnDeviceSetting.IsEnabled = true;

                UserPage.DataContext = App.MainViewModel;

                App.MainViewModel.BackButtonText = Properties.Resources.Setting_Label_UserBackButton;
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.GoPreviousPageHandler(e, sender);
        }

        private void UpdateCurrentButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.Users = userInfoViewPage;

            GoToUpdateUserPageHandler(e, sender);
        }

        private static void GoToUpdateUserPageHandler(EventArgs e, object sender)
        {
            if (GoToUpdateCurrentUserPage != null)
            {
                GoToUpdateCurrentUserPage(sender, e);
            }
        }

        private void LanguageCountry(object sender, RoutedEventArgs e)
        {
            App.GoToSettingLanguageCountryPageHandler(e, sender);
        }

        private void btnDevice_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingDevicePageHandler(e, sender);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingConfigurationPageHandler(e, sender);
        }
    }
}
