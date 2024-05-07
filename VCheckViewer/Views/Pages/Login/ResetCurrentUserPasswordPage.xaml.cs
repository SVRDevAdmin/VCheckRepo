using Microsoft.AspNetCore.Identity;
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

namespace VCheckViewer.Views.Pages.Login
{
    /// <summary>
    /// Interaction logic for ResetCurrentUserPasswordPage.xaml
    /// </summary>
    public partial class ResetCurrentUserPasswordPage : Page
    {
        public ResetCurrentUserPasswordPage()
        {
            InitializeComponent();
        }

        private async void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            IdentityUser user = await App.UserManager.FindByIdAsync(App.MainViewModel.CurrentUsers.UserId.ToString());

            var changePassword = await App.UserManager.ChangePasswordAsync(user, OldPassword.Password, ConfirmPassword.Password);

            if (changePassword.Succeeded)
            {

            }
            else
            {
                string errorText = "";

                var errors = changePassword.Errors.ToList();

                foreach (var error in errors)
                {
                    errorText += "- " + error.Description + "\r\n";
                }

                ErrorText.Text = errorText;
            }
        }
    }
}
