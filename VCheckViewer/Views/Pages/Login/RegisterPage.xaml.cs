using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Linq;
using VCheck.Helper;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.ViewModels.Windows;
using Brushes = System.Windows.Media.Brushes;
using TextBox = System.Windows.Controls.TextBox;

namespace VCheckViewer.Views.Pages.Login
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        ConfigurationDBContext ConfigurationContext = App.GetService<ConfigurationDBContext>();
        static MasterCodeDataDBContext sContext = App.GetService<MasterCodeDataDBContext>();
        static RolesDBContext rolesContext = App.GetService<RolesDBContext>();
        static UserDBContext usersContext = App.GetService<UserDBContext>();
        TemplateDBContext TemplateContext = App.GetService<TemplateDBContext>();
        NotificationDBContext NotificationContext = App.GetService<NotificationDBContext>();

        public RegisterPage()
        {
            InitializeComponent();
            CheckThemesSettings();
            UpdatePlaceholderVisibility("");

            var Login_Label_LeftMain_array = Properties.Resources.Login_Label_LeftMain.Split("<nextline>");
            Login_Label_LeftMain.Text = Login_Label_LeftMain_array[0] + "\r\n" + ((Login_Label_LeftMain_array.Length > 1) ? Login_Label_LeftMain_array[1] : "");

            PasswordPlaceholder.Text = Properties.Resources.Login_Label_MainPassword;
            ConfirmPasswordPlaceholder.Text = Properties.Resources.Login_Label_ConfirmPassword;
        }

        private void CheckValue(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = null;
            System.Windows.Controls.Border parentBorder;
            Grid parentGrid;

            textBox = sender as TextBox; parentGrid = textBox.Parent as Grid;

            parentBorder = parentGrid.Parent as System.Windows.Controls.Border;


            if ((textBox != null && textBox.Text == ""))
            {
                parentBorder.BorderBrush = Brushes.Red;
                parentBorder.ToolTip = Properties.Resources.Setting_ErrorMessage_MandatoryField;
            }
            else if (textBox != null && textBox.Name == "Username" && textBox.Text.Length < 5)
            {
                parentBorder.BorderBrush = Brushes.Red;
                parentBorder.ToolTip = Properties.Resources.Setting_ErrorMessage_FiveCharMin;
            }
            else
            {
                parentBorder.BorderBrush = Brushes.Black;
                parentBorder.ToolTip = "No issue";
            }
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = (Username.Parent as Grid).Parent as System.Windows.Controls.Border;

            string errorMessage = "";
            if (Password.Password.Length < 8) { errorMessage = errorMessage + Properties.Resources.Login_ErrorMessage_PasswordLessEightChar; }
            if (!Regex.IsMatch(Password.Password, "^(?=.*?[A-Z]).{1,}$")) { if (errorMessage != "") { errorMessage = errorMessage + "\r\n"; } errorMessage = errorMessage + Properties.Resources.Login_ErrorMessage_NoUpperCase; }
            if (!Regex.IsMatch(Password.Password, "^(?=.*?[a-z]).{1,}$")) { if (errorMessage != "") { errorMessage = errorMessage + "\r\n"; } errorMessage = errorMessage + Properties.Resources.Login_ErrorMessage_NoLowerCase; }
            if (!Regex.IsMatch(Password.Password, "^(?=.*?[0-9]).{1,}$")) { if (errorMessage != "") { errorMessage = errorMessage + "\r\n"; } errorMessage = errorMessage + Properties.Resources.Login_ErrorMessage_NoNumberChar; }


            if (username.ToolTip.ToString() != "No issue")
            {
                ErrorText.Visibility = Visibility.Visible;
                ErrorText.Text = "Please check username again.";
            }
            else if (string.IsNullOrEmpty(Password.Password))
            {
                ErrorText.Visibility = Visibility.Visible;
                ErrorText.Text = "Please input password.";
            }
            else if (errorMessage != "")
            {
                ErrorText.Visibility = Visibility.Visible;
                ErrorText.Text = errorMessage;
            }
            else if (ConfirmPassword.Password != Password.Password)
            {
                ErrorText.Visibility = Visibility.Visible;
                ErrorText.Text = "Password and Confirm Password does not match.";
            }
            else
            {
                var role = rolesContext.GetRoles().FirstOrDefault(x => x.RoleName == "Superadmin");

                UserModel userInfo = new UserModel()
                {
                    EmployeeID = "NA",
                    Title = "",
                    FullName = "Superadmin",
                    RegistrationNo = "NA",
                    Gender = "M",
                    DateOfBirth = DateTime.Now.ToString("yyyy-MM-dd"),
                    EmailAddress = "",
                    StatusID = 1,
                    RoleID = role.RoleID,
                    Role = role.RoleName,
                    LoginID = Username.Text
                };

                App.MainViewModel.Users = userInfo;

                App.newPassword = ConfirmPassword.Password;

                Register(userInfo);

                App.PopupHandler(e, sender);
            }
        }

        private async void Register(UserModel userInfo)
        {
            try
            {
                var user = Activator.CreateInstance<IdentityUser>();

                var emailStore = (IUserEmailStore<IdentityUser>)App.UserStore;

                await App.UserStore.SetUserNameAsync(user, userInfo.LoginID, CancellationToken.None);
                if (userInfo.EmailAddress != "")
                {
                    await emailStore.SetEmailAsync(user, userInfo.EmailAddress, CancellationToken.None);
                }
                var result = await App.UserManager.CreateAsync(user, App.newPassword);

                ConfigurationModel sLangCode = ConfigurationContext.GetConfigurationData("SystemSettings_Language").FirstOrDefault();

                if (result.Succeeded)
                {
                    userInfo.UserId = user.Id;
                    userInfo.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    userInfo.CreatedBy = userInfo.LoginID;

                    if (usersContext.InsertUser(userInfo))
                    {
                        var roleResult = await App.UserManager.AddToRoleAsync(user, userInfo.Role);

                        if (roleResult.Succeeded)
                        {
                            
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                App.log.Error("Register User Error >>> ", ex);
            }
        }

        private void CheckThemesSettings()
        {
            String? sTheme = "Light";

            var sConfigData = ConfigurationContext.GetConfigurationData("SystemSettings_Themes");
            if (sConfigData != null && sConfigData.Count > 0)
            {
                sTheme = sConfigData[0].ConfigurationValue;
            }

            String sThemeName = sTheme + ".xaml";
            AppTheme.ChangeTheme(new Uri("Themes/" + sThemeName, UriKind.Relative));
            if (sTheme.ToLower() == "light")
            {
                btnDarkTheme.Background = System.Windows.Media.Brushes.Transparent;

                System.Windows.Media.Color sBlue = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1e76fb");
                btnLightTheme.Background = new SolidColorBrush(sBlue);
            }
            else if (sTheme.ToLower() == "dark")
            {
                btnLightTheme.Background = System.Windows.Media.Brushes.Transparent;

                System.Windows.Media.Color sBlue = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1e76fb");
                btnDarkTheme.Background = new SolidColorBrush(sBlue);
            }
        }

        private void btnDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            var sButton = (System.Windows.Controls.Button)sender;
            ConfigurationContext.UpdateConfiguration("SystemSettings_Themes", (sButton != null) ? sButton.Tag.ToString() : "");

            CheckThemesSettings();
        }

        private void btnLightTheme_Click(object sender, RoutedEventArgs e)
        {
            var sButton = (System.Windows.Controls.Button)sender;
            ConfigurationContext.UpdateConfiguration("SystemSettings_Themes", (sButton != null) ? sButton.Tag.ToString() : "");

            CheckThemesSettings();
        }

        private void UpdatePlaceholderVisibility(string name)
        {
            if (name == "ConfirmPassword")
            {
                ConfirmPasswordPlaceholder.Visibility = string.IsNullOrEmpty(ConfirmPassword.Password) ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (name == "Password")
            {
                PasswordPlaceholder.Visibility = string.IsNullOrEmpty(Password.Password) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                ConfirmPasswordPlaceholder.Visibility = string.IsNullOrEmpty(ConfirmPassword.Password) ? Visibility.Visible : Visibility.Collapsed;
                PasswordPlaceholder.Visibility = string.IsNullOrEmpty(Password.Password) ? Visibility.Visible : Visibility.Collapsed;
            }

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox password = sender as PasswordBox;
            UpdatePlaceholderVisibility(password.Name);
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox password = sender as PasswordBox;
            if (password.Name == "ConfirmPassword")
            {
                ConfirmPasswordPlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (password.Name == "Password")
            {
                PasswordPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox password = sender as PasswordBox;
            UpdatePlaceholderVisibility(password.Name);
        }

        private void Placeholder_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox placeholder = sender as TextBox;

            if(placeholder.Name == "PasswordPlaceholder")
            {
                Password.Focus();
            }
            else
            {
                ConfirmPassword.Focus();
            }
        }
    }
}
