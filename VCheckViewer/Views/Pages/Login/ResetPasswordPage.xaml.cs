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
using VCheck.Lib.Data.DBContext;

namespace VCheckViewer.Views.Pages.Login
{
    /// <summary>
    /// Interaction logic for ResetPassword.xaml
    /// </summary>
    public partial class ResetPasswordPage : Page
    {

        public static event EventHandler GoToLoginPage;

        public ResetPasswordPage()
        {
            InitializeComponent();
        }
        private void PasswordPlaceholderHandler(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;

            if(passwordBox.Name == "NewPassword")
            {
                if (passwordBox.Password == "") { NewPasswordPlaceholder.Visibility = Visibility.Visible; }
                else { NewPasswordPlaceholder.Visibility = Visibility.Collapsed; }

                if(passwordBox.Password.Length < 8) { NewPasswordLength.Visibility = Visibility.Visible; NewpasswordCorrect.Visibility = Visibility.Hidden; }
                else { NewPasswordLength.Visibility = Visibility.Hidden; NewpasswordCorrect.Visibility = Visibility.Visible; }
            }
            else
            {
                if (passwordBox.Password == "") { ConfirmPasswordPlaceholder.Visibility = Visibility.Visible; }
                else { ConfirmPasswordPlaceholder.Visibility = Visibility.Collapsed; }

                if (passwordBox.Password.Length < 8) { ConfirmPasswordLength.Visibility = Visibility.Visible; ConfirmPasswordCorrect.Visibility = Visibility.Hidden; }
                else { ConfirmPasswordLength.Visibility = Visibility.Hidden; ConfirmPasswordCorrect.Visibility = Visibility.Visible; }
            }
        }

        private void BackToLogin(object sender, RoutedEventArgs e)
        {
            GoToLoginPageHandler(e, sender);
        }

        private static void GoToLoginPageHandler(EventArgs e, object sender)
        {
            if (GoToLoginPage != null)
            {
                GoToLoginPage(sender, e);
            }
        }

        private async void ResetPassword(object sender, RoutedEventArgs e)
        {
            var user = await App.UserManager.FindByEmailAsync(Email.Text);

            if (user != null)
            {
                var passwordToken = await App.UserManager.GeneratePasswordResetTokenAsync(user);
                var task = App.UserManager.ResetPasswordAsync(user, passwordToken, ConfirmPassword.Password);
            }
        }
    }
}
