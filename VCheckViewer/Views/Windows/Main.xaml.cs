﻿using System;
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
using VCheckViewer.Views.Pages.Setting.User;
using VCheck.Lib.Data.Models;
using VCheck.Lib.Data.DBContext;
using System.Collections.ObjectModel;

namespace VCheckViewer.Views.Windows
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : INavigationWindow
    {
        static MasterCodeDataDBContext sContext = App.GetService<MasterCodeDataDBContext>();
        static RolesDBContext rolesContext = App.GetService<RolesDBContext>();

        List<MasterCodeDataModel> masterCodeDataList = sContext.GetMasterCodeData();
        List<RolesModel> roleList = rolesContext.GetRoles();

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

            UserPage.GoToAddUserPage += new EventHandler(GoToAddUserPage);
            UserPage.GoToUpdateUserPage += new EventHandler(GoToUpdateUserPage);
            UserPage.GoToViewUserPage += new EventHandler(GoToViewUserPage);

            App.GoPreviousPage += new EventHandler(PreviousPage);

            PageTitle.Text = "Dashboard";

            initializedDropdownSelectionList();

            App.MainViewModel.CurrentUsers = new UserModel() { Title = "Dr.", FirstName = "Test", LastName = "Test" , EmployeeID = "456783", RegistrationNo = "456783", Gender = "Male", DateOfBirth = "15 March 1991", Role = "Superadmin", EmailAddress = "Test", Status = "Active"};
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

        private void ViewProfileButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.Users = App.MainViewModel.CurrentUsers;

            GoToViewUserPage(sender,e);
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

        private void T5_Click(object sender, RoutedEventArgs e)
        {
            MainUserPage();
        }

        void GoToAddUserPage(object sender, EventArgs e)
        {
            Navigate(typeof(AddUserPage));
        }

        void GoToUpdateUserPage(object sender, EventArgs e)
        {
            Navigate(typeof(UpdateUserPage));
        }

        void GoToViewUserPage(object sender, EventArgs e)
        {
            Navigate(typeof(ViewUserPage));
        }

        private void PreviousPage(object sender, EventArgs e)
        {
            RootNavigation.GoBack();
        }

        private void DashboardPage()
        {
            Navigate(typeof(DashboardPage));

            PageTitle.Text = "Dashboard";
        }

        private void MainUserPage()
        {
            Navigate(typeof(UserPage));

            PageTitle.Text = "Setting";
        }

        public void initializedDropdownSelectionList()
        {
            var titleList = masterCodeDataList.Where(a => a.CodeGroup == "Title");
            var genderList = masterCodeDataList.Where(a => a.CodeGroup == "Gender");
            var rolesList = roleList.Where(a => a.IsActive);
            var statusList = masterCodeDataList.Where(a => a.CodeGroup == "UserStatus");

            App.MainViewModel.cbTitle = new ObservableCollection<ComboBoxItem>();
            App.MainViewModel.cbGender = new ObservableCollection<ComboBoxItem>();
            App.MainViewModel.cbRoles = new ObservableCollection<ComboBoxItem>();
            App.MainViewModel.cbStatus = new ObservableCollection<ComboBoxItem>();


            var cbDefaultItem = new ComboBoxItem { Content = "<--Select Title-->" };
            App.MainViewModel.SelectedcbTitle = cbDefaultItem;
            App.MainViewModel.cbTitle.Add(cbDefaultItem);
            foreach (var item in titleList) { App.MainViewModel.cbTitle.Add(new ComboBoxItem { Content = item.CodeID }); }

            cbDefaultItem = new ComboBoxItem { Content = "<--Select Gender-->" };
            App.MainViewModel.SelectedcbGender = cbDefaultItem;
            App.MainViewModel.cbGender.Add(cbDefaultItem);
            foreach (var item in genderList) { App.MainViewModel.cbGender.Add(new ComboBoxItem { Content = item.CodeName, Tag = item.CodeID }); }

            cbDefaultItem = new ComboBoxItem { Content = "<--Select Role-->" };
            App.MainViewModel.SelectedcbRoles = cbDefaultItem;
            App.MainViewModel.cbRoles.Add(cbDefaultItem);
            foreach (var item in rolesList) { App.MainViewModel.cbRoles.Add(new ComboBoxItem { Content = item.RoleName, Tag = item.RoleID }); }

            cbDefaultItem = new ComboBoxItem { Content = "<--Select Status-->" };
            App.MainViewModel.SelectedcbStatus = cbDefaultItem;
            App.MainViewModel.cbStatus.Add(cbDefaultItem);
            foreach (var item in statusList) { App.MainViewModel.cbStatus.Add(new ComboBoxItem { Content = item.CodeName, Tag = item.CodeID }); }
        }
    }
}
