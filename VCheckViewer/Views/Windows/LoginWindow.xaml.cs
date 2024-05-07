using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic.Logging;
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
using System.Windows.Shapes;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.ViewModels.Windows;
using VCheckViewer.Views.Pages;
using VCheckViewer.Views.Pages.Login;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace VCheckViewer.Views.Windows
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    //public partial class Login : INavigationWindow
    public partial class LoginWindow : Window
    {
        INavigationService _navigationService;
        IPageService _pageService;
        int maxLoginAttempt = 5;


        public static event EventHandler ResetPassword;

        public LoginWindow
        (
        //INavigationService navigationService,
        //IPageService pageService
        )
        {
            SystemThemeWatcher.Watch(this);
            InitializeComponent();
            //_navigationService = navigationService;
            //_pageService = pageService;

            LoginPage.GoToResetPasswordPage += new EventHandler(GoToResetPasswordPage);
            ResetPasswordPage.GoToLoginPage += new EventHandler(GoToLoginPage);
            PasswordRecoveryPage.GoToLoginPage += new EventHandler(GoToLoginPage);

            LoginPage.GoToMainWindow += new EventHandler(GoToMainWindow);

            //popup
            PasswordRecoveryPage.Popup += new EventHandler(OpenPopup);

        }


        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        public void Navigate(object sender) => LoginFrame.Content = sender;

        public void GoToResetPasswordPage(object sender, EventArgs e)
        {
            //LoginFrame.Content = new ResetPasswordPage();
            LoginFrame.Content = new PasswordRecoveryPage();
        }

        public void GoToLoginPage(object sender, EventArgs e)
        {
            LoginFrame.Content = new LoginPage();
        }

        public void GoToMainWindow(object sender, EventArgs e)
        {
            if (!System.Windows.Application.Current.Windows.OfType<Main>().Any())
            {
                Main main = new Main();
                main.frameContent.Content = new DashboardPage();
                main.Show();
            }
            Close();
        }

        public void OpenPopup(object sender, EventArgs e)
        {
            OKButton.Visibility = Visibility.Collapsed;
            ContinueButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Visible;

            PopupBackground.Background = System.Windows.Media.Brushes.DimGray;
            PopupBackground.Opacity = 0.5;
            PopupContent.Text = "Are you sure you want to recover your password?";
            popup.IsOpen = true;
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            OKButton.Visibility = Visibility.Visible;
            ContinueButton.Visibility = Visibility.Collapsed;
            CancelButton.Visibility = Visibility.Collapsed;

            Run bold = new Run();
            bold.Text = "'Reset Password'";

            bold.FontWeight = FontWeights.Bold;

            PopupContent.Text = "";

            PopupContent.Inlines.Add("We have send you a temporary password to your email. You can use it to log in. \r\n \r\n Once you are logged in, you can change it to your preferred password in ");
            PopupContent.Inlines.Add(bold);
            PopupContent.Inlines.Add(" section");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;

            // Enable main window
            this.IsEnabled = true;
            // Close the popup
            popup.IsOpen = false;

            PopupBackground.Background = null;

            if(button.Name == "OKButton")
            {
                LoginFrame.Content = new LoginPage();
                ResetPasswordHandler(e, sender);
            }
        }

        private static void ResetPasswordHandler(EventArgs e, object sender)
        {
            if (ResetPassword != null)
            {
                ResetPassword(sender, e);
            }
        }


    }
}
