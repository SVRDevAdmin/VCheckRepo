using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using VCheckViewer.Views.Windows;
using Wpf.Ui;
using Brushes = System.Windows.Media.Brushes;
using TextBox = System.Windows.Controls.TextBox;

namespace VCheckViewer.Views.Pages.Login
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {

        //INavigationService _navigationService;
        //IPageService _pageService;
        int maxLoginAttempt = 5;

        static UserDBContext sContext = App.GetService<UserDBContext>();
        static ConfigurationDBContext ConfigurationContext = App.GetService<ConfigurationDBContext>();

        public static event EventHandler GoToResetPasswordPage;
        public static event EventHandler GoToMainWindow;

        public LoginPage()
        {
            InitializeComponent();
            CheckThemesSettings();

            var Login_Label_LeftMain_array = Properties.Resources.Login_Label_LeftMain.Split("<nextline>");
            Login_Label_LeftMain.Text = Login_Label_LeftMain_array[0] + "\r\n" + ((Login_Label_LeftMain_array.Length > 1) ? Login_Label_LeftMain_array[1] : "");
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //VCheck.Lib.Data.Models.UserModel sUser = new VCheck.Lib.Data.Models.UserModel();
            //sUser.UserId = "Tester";
            //App.MainViewModel.CurrentUsers = sUser;
            //GoToMainWindowHandler(e, sender);
            //return;

            try
            {
                IdentityUser user = await App.UserManager.FindByNameAsync(Username.Text);

                if (user != null)
                {
                    SignInResult loginAttempt = new SignInResult();
                    if (user != null) { loginAttempt = await App.SignInManager.CheckPasswordSignInAsync(user, Password.Password, lockoutOnFailure: false); }

                    if (loginAttempt.Succeeded)
                    {
                        var userAcount = sContext.GetUserByID(user.Id);

                        if (userAcount == null)
                        {
                            ErrorText.Visibility = Visibility.Visible;
                            ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongLoginID;
                        }
                        else if (userAcount.Status == "Active")
                        {
                            userAcount.LastLoginDate = DateTime.Now;
                            userAcount.Gender = userAcount.Gender == "Male" ? "M" : "F";
                            userAcount.DateOfBirth = Convert.ToDateTime(userAcount.DateOfBirth).ToString("yyyy-MM-dd");
                            userAcount.UpdatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            userAcount.UpdatedBy = "System";
                            App.MainViewModel.CurrentUsers = userAcount;

                            sContext.UpdateUser(userAcount);

                            App.MainViewModel.CurrentUsers.Gender = App.MainViewModel.CurrentUsers.Gender == "M" ? "Male" : "Female";

                            GoToMainWindowHandler(e, sender);
                        }
                        else
                        {
                            ErrorText.Visibility = Visibility.Visible;
                            //ErrorText.Text = "Account are deactivated. Please contact administrator.";
                            ErrorText.Text = Properties.Resources.Login_ErrorMessage_AccountDeactivated;
                        }

                    }
                    else if (loginAttempt.IsLockedOut)
                    {
                        ErrorText.Visibility = Visibility.Visible;
                        //ErrorText.Text = "Account are locked. Please contact administrator.";
                        ErrorText.Text = Properties.Resources.Login_ErrorMessage_AccountLocked;
                    }
                    else if (loginAttempt.IsNotAllowed)
                    {
                        ErrorText.Visibility = Visibility.Visible;
                        //ErrorText.Text = "Account are not confirmed yet. Please confirm the account through email.";
                        ErrorText.Text = Properties.Resources.Login_ErrorMessage_AccountNotConfirmed;
                    }
                    else
                    {
                        int attemptleft = (maxLoginAttempt - user.AccessFailedCount);
                        ErrorText.Visibility = Visibility.Visible;
                        //ErrorText.Text = "Wrong password. You have " + attemptleft + " attemp[s] left before account are locked.";
                        //ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongPassword.Replace("<attemptcount>", attemptleft.ToString());
                        ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongUsernamePassword;
                    }
                }
                else
                {
                    ErrorText.Visibility = Visibility.Visible;
                    //ErrorText.Text = "Wrong login ID.";
                    //ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongLoginID;
                    ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongUsernamePassword;
                }
            }
            catch (Exception ex)
            {
                ErrorText.Visibility = Visibility.Visible;
                ErrorText.Text = Properties.Resources.General_Message_Error;
                App.log.Error("Login Error >>> ", ex);
            }
            



            //IdentityUser newUser = Activator.CreateInstance<IdentityUser>();

            //await App.UserStore.SetUserNameAsync(newUser, "123456", CancellationToken.None);

            //var result = await App.UserManager.CreateAsync(newUser, "Retes123@Retes");

            //if (result.Succeeded)
            //{
            //    var test = newUser;
            //}


            //var applicationRoleAdministrator = new IdentityRole("Lab User");
            //App.RoleManager.CreateAsync(applicationRoleAdministrator);

            //var test = App.RoleManager.Roles.ToList();

            //int attempt = 0;
            //while (user.LockoutEnd == null)
            //{
            //    test = await App.SignInManager.CheckPasswordSignInAsync(user, Password.Password, lockoutOnFailure: true);
            //    attempt++;
            //}


            //var userLogin = sContext.ValidateLogin(Username.Text, Password.Password);

            //if (userLogin != null && userLogin.UserId != 0)
            //{
            //    App.MainViewModel.CurrentUsers = userLogin;
            //    //Main main = new Main(_navigationService, _pageService);
            //    //this.CloseWindow();
            //    //main.Show();
            //    //main.Navigate(typeof(DashboardPage));
            //    Main main = new Main();
            //    this.CloseWindow();
            //    main.Show();
            //    main.frameContent.Content = new DashboardPage();
            //}
        }

        private void CheckValue(object sender, System.Windows.Input.KeyEventArgs e)
        {
            PasswordBox password = null;
            TextBox username = null;
            Border parentBorder;
            Grid parentGrid;

            if (sender.GetType() == typeof(TextBox)) { username = sender as TextBox; parentGrid = username.Parent as Grid; }
            else { password = sender as PasswordBox; parentGrid = password.Parent as Grid; }

            parentBorder = parentGrid.Parent as Border;


            if ((username != null && username.Text == "") || (password != null && password.Password == ""))
            {
                parentBorder.BorderBrush = Brushes.Red;
                parentBorder.ToolTip = Properties.Resources.Setting_ErrorMessage_MandatoryField;
            }
            else if (username != null && username.Text.Length < 5)
            {
                parentBorder.BorderBrush = Brushes.Red;
                parentBorder.ToolTip = Properties.Resources.Setting_ErrorMessage_FiveCharMin;
            }
            else if (password != null && (password.Password.Length < 8 || !Regex.IsMatch(password.Password, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{0,}$")))
            {
                string errorMessage = "";
                parentBorder.BorderBrush = Brushes.Red;

                if (password.Password.Length < 8) { errorMessage = errorMessage + "- " + Properties.Resources.Login_ErrorMessage_PasswordLessEightChar; }
                if (!Regex.IsMatch(password.Password, "^(?=.*?[A-Z]).{1,}$")) { if (errorMessage != "") { errorMessage = errorMessage + "\r\n"; } errorMessage = errorMessage + "- " + Properties.Resources.Login_ErrorMessage_NoUpperCase; }
                if (!Regex.IsMatch(password.Password, "^(?=.*?[a-z]).{1,}$")) { if (errorMessage != "") { errorMessage = errorMessage + "\r\n"; } errorMessage = errorMessage + "- " + Properties.Resources.Login_ErrorMessage_NoLowerCase; }
                if (!Regex.IsMatch(password.Password, "^(?=.*?[0-9]).{1,}$")) { if (errorMessage != "") { errorMessage = errorMessage + "\r\n"; } errorMessage = errorMessage + "- " + Properties.Resources.Login_ErrorMessage_NoNumberChar; }
                parentBorder.ToolTip = errorMessage;
            }
            else
            {
                parentBorder.BorderBrush = Brushes.Black;
                parentBorder.ToolTip = "No issue";
            }
        }

        private void PasswordPlaceholderHandler(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "") { PasswordPlaceholder.Visibility = Visibility.Visible; }
            else { PasswordPlaceholder.Visibility = Visibility.Collapsed; }
        }

        private void ResetPassword(object sender, RoutedEventArgs e)
        {
            GoToResetPasswordPageHandler(e, sender);
        }

        private static void GoToResetPasswordPageHandler(EventArgs e, object sender)
        {
            if (GoToResetPasswordPage != null)
            {
                GoToResetPasswordPage(sender, e);
            }
        }

        private static void GoToMainWindowHandler(EventArgs e, object sender)
        {
            if (GoToMainWindow != null)
            {
                GoToMainWindow(sender, e);
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
    }
}
