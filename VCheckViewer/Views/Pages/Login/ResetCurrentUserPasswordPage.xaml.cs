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
using Brushes = System.Windows.Media.Brushes;

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
            if(NewPassword.Password == ConfirmPassword.Password)
            {
                IdentityUser user = await App.UserManager.FindByIdAsync(App.MainViewModel.CurrentUsers.UserId.ToString());

                var changePassword = await App.UserManager.ChangePasswordAsync(user, OldPassword.Password, ConfirmPassword.Password);

                if (changePassword.Succeeded)
                {
                    ErrorText.Text = Properties.Resources.Login_Message_PasswordResetted;
                    ErrorText.Foreground = Brushes.Green;
                }
                else
                {
                    //string errorText = "";

                    //var errors = changePassword.Errors.ToList();

                    //foreach (var error in errors)
                    //{
                    //    errorText += "- " + error.Description + "\r\n";
                    //}
                    if(changePassword.Errors.Any(x => x.Code.ToString() == "PasswordMismatch")) { ErrorText.Text = Properties.Resources.Login_Message_WrongCurrentPassword; }
                    else { ErrorText.Text = Properties.Resources.Login_Message_PasswordRequirement; }

                    //ErrorText.Text = errorText;
                    //ErrorText.Text = Properties.Resources.Login_Message_PasswordRequirement;
                    ErrorText.Foreground = Brushes.Red;
                }
            }
            else
            {
                ErrorText.Text = Properties.Resources.Login_Message_PasswordNewConfirmMismatch;
                ErrorText.Foreground = Brushes.Red;
            }
            
        }

        private void ValueCheck(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (OldPassword.Password != "" && NewPassword.Password != "" && ConfirmPassword.Password != "") { ResetPassword.IsEnabled = true; }
            else { ResetPassword.IsEnabled = false; }
        }
    }
}
