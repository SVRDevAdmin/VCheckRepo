﻿using Microsoft.AspNetCore.Identity;
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
using VCheckViewer.Views.Windows;
using Wpf.Ui;

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
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            IdentityUser user = await App.UserManager.FindByNameAsync(Username.Text);

            if (user != null)
            {
                SignInResult loginAttempt = new SignInResult();
                if (user != null) { loginAttempt = await App.SignInManager.CheckPasswordSignInAsync(user, Password.Password, lockoutOnFailure: true); }

                if (loginAttempt.Succeeded)
                {
                    App.MainViewModel.CurrentUsers = sContext.GetUserByID(user.Id);

                    GoToMainWindowHandler(e, sender);

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
                    ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongPassword.Replace("<attemptcount>", attemptleft.ToString());
                }
            }
            else
            {
                ErrorText.Visibility = Visibility.Visible;
                //ErrorText.Text = "Wrong login ID.";
                ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongLoginID;
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
    }
}