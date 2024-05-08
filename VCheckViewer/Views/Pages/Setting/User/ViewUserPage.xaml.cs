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
        UserModel userInfo;

        public static event EventHandler GoToUpdateCurrentUserPage;

        public ViewUserPage()
        {
            InitializeComponent();

            userInfo = App.MainViewModel.Users;

            Title.Text = userInfo.Title;
            //Surname.Text = userInfo.FirstName;
            FullName.Text = userInfo.FullName;
            StaffID.Text = userInfo.EmployeeID;
            RegistrationNo.Text = userInfo.RegistrationNo;
            Gender.Text = userInfo.Gender;
            DateOfBirth.Text = userInfo.DateOfBirth;
            Role.Text = userInfo.Role;
            EmailAddress.Text = userInfo.EmailAddress;
            Status.Text = userInfo.Status;
            LoginID.Text = userInfo.LoginID;

            if (App.MainViewModel.CurrentUsers.UserId == userInfo.UserId) { Edit.Visibility = Visibility.Visible; }

            UserPage.DataContext = App.MainViewModel;

            App.MainViewModel.BackButtonText = Properties.Resources.Setting_Label_UserBackButton;
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.GoPreviousPageHandler(e, sender);
        }

        private void UpdateCurrentButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.Users = App.MainViewModel.CurrentUsers;

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
    }
}
