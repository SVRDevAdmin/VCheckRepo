using Microsoft.AspNetCore.Identity;
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
using VCheckViewer.Views.Windows;

namespace VCheckViewer.Views.Pages.Login
{
    /// <summary>
    /// Interaction logic for PasswordRecoveryPage.xaml
    /// </summary>
    public partial class PasswordRecoveryPage : Page
    {
        public static event EventHandler Popup;
        public static event EventHandler GoToLoginPage;
        public string newPassword;
        public IdentityUser user;

        public PasswordRecoveryPage()
        {
            InitializeComponent();

            LoginWindow.ResetPassword += new EventHandler(ProceedResetPassword);
        }

        private async void ResetPassword(object sender, RoutedEventArgs e)
        {
            user = await App.UserManager.FindByNameAsync(Username.Text);

            if(user != null)
            {
                if (user.NormalizedEmail == Email.Text.ToUpper())
                {
                    newPassword = RandomPasswordGenerator();

                    PopupHandler(e, sender);
                }
                else
                {
                    ErrorText.Visibility = Visibility.Visible;
                    //ErrorText.Text = "Wrong email linked to the account, please verify it is the correct email and try again.";
                    ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongEmail;
                }
            }
            else
            {
                ErrorText.Visibility = Visibility.Visible;
                //ErrorText.Text = "Cannot find user according to Login ID, please verify it is the correct login ID and try again.";
                ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongLoginIDRecovery;
            }
        }

        private async void ProceedResetPassword(object sender, EventArgs e)
        {
            await App.UserManager.RemovePasswordAsync(user);
            var test = await App.UserManager.AddPasswordAsync(user, newPassword);            
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

        private static void PopupHandler(EventArgs e, object sender)
        {
            if (Popup != null)
            {
                Popup(sender, e);
            }
        }

        private string RandomPasswordGenerator()
        {
            Random res = new Random();

            // String that contain alphabets, numbers and special character
            String lowerCase = "abcdefghijklmnopqrstuvwxyz";
            String upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            String number = "0123456789";
            String specialChar = "!@#$%^&*()_+-={}|[];,./:<>?";

            // Initializing the empty string 
            String randomstring = "";

            randomstring += lowerCase[res.Next(lowerCase.Length)];
            randomstring += upperCase[res.Next(upperCase.Length)];
            randomstring += number[res.Next(number.Length)];
            randomstring += specialChar[res.Next(specialChar.Length)];
            randomstring += lowerCase[res.Next(lowerCase.Length)];
            randomstring += upperCase[res.Next(upperCase.Length)];
            randomstring += number[res.Next(number.Length)];
            randomstring += specialChar[res.Next(specialChar.Length)];

            return randomstring;
        }
    }
}
