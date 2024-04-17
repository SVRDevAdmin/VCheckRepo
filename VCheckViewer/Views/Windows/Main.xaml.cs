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
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using VCheckViewer.ViewModels.Windows;
using VCheck.Lib.Logic;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
//using VCheckViewer.Lib.Base;
using Microsoft.EntityFrameworkCore;
//using VCheckViewer.Lib.Function;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VCheck.Lib.Data;
using VCheckViewer.Views.Pages;

namespace VCheckViewer.Views.Windows
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : INavigationWindow
    {
        VCheck.Lib.Data.SampleClass sContext = VCheckViewer.App.GetService<VCheck.Lib.Data.SampleClass>();

        public MainViewModel ViewModel { get;  }
        public Main
        (
            INavigationService navigationService,
            IPageService pageService
        )
        {
            SystemThemeWatcher.Watch(this);

            InitializeComponent();
            //SetPageService(pageService);

            //navigationService.SetNavigationControl(RootNavigation);

            PageTitle.Text = "Dashboard";
        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        INavigationView INavigationWindow.GetNavigation()
        {
            throw new NotImplementedException();
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        private void T1_Click(object sender, RoutedEventArgs e)
        {
            DashboardPage();
        }

        private void T2_Click(object sender, RoutedEventArgs e)
        {
            //Navigate(typeof(Schedulepage));

            PageTitle.Text = "Schedule";
        }

        private void T3_Click(object sender, RoutedEventArgs e)
        {
            //Navigate(typeof(Page2));

            PageTitle.Text = "Results";
        }

        private void RootNavigation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var sContext = VCheckViewer.App.GetService<VCheck.Lib.Data.SampleClass>();
            List<VCheck.Lib.Data.Album> sAlbum = sContext.GetData();
            String abc = "abc";
        }

        private void T5_Click(object sender, RoutedEventArgs e)
        {
            UserPage();
        }

        private void DashboardPage()
        {
            Navigate(typeof(DashboardPage));

            PageTitle.Text = "Dashboard";
        }

        private void UserPage()
        {
            Navigate(typeof(UserPage));

            PageTitle.Text = "Setting";
        }
    }
}
