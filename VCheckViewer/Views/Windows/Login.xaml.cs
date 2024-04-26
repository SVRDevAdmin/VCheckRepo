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
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace VCheckViewer.Views.Windows
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    //public partial class Login : INavigationWindow
    public partial class Login : Window
    {
        INavigationService _navigationService;
        IPageService _pageService;

        static UserLoginDBContext sContext = App.GetService<UserLoginDBContext>();

        public Login
        (
            //INavigationService navigationService,
            //IPageService pageService
        )
        {
            SystemThemeWatcher.Watch(this);
            InitializeComponent();
            //_navigationService = navigationService;
            //_pageService = pageService;
        }


        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        //public INavigationView GetNavigation()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Navigate(Type pageType)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SetPageService(IPageService pageService)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SetServiceProvider(IServiceProvider serviceProvider)
        //{
        //    throw new NotImplementedException();
        //}

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var userLogin = sContext.ValidateLogin(Username.Text, Password.Password);

            if (userLogin != null && userLogin.UserId != 0)
            {
                App.MainViewModel.CurrentUsers = userLogin;
                //Main main = new Main(_navigationService, _pageService);
                //this.CloseWindow();
                //main.Show();
                //main.Navigate(typeof(DashboardPage));
                Main main = new Main();
                this.CloseWindow();
                main.Show();
                main.frameContent.Content = new DashboardPage();

            }
        }

        private void PasswordPlaceholderHandler(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "") { PasswordPlaceholder.Visibility = Visibility.Visible; }
            else { PasswordPlaceholder.Visibility = Visibility.Collapsed; }
        }
    }
}
