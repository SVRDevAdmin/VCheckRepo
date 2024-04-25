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
using VCheckViewer.Views.Pages.Setting.User;
using VCheck.Lib.Data.Models;
using VCheck.Lib.Data.DBContext;
using System.Collections.ObjectModel;
using System.Windows.Media.Effects;
using VCheckViewer.Lib.Models;
using System.Reflection;
using System.ComponentModel;

namespace VCheckViewer.Views.Windows
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : INavigationWindow
    {
        INavigationService _navigationService;
        IPageService _pageService;

        static MasterCodeDataDBContext sContext = App.GetService<MasterCodeDataDBContext>();
        static RolesDBContext rolesContext = App.GetService<RolesDBContext>();
        static UserDBContext usersContext = App.GetService<UserDBContext>();

        List<MasterCodeDataModel> masterCodeDataList = sContext.GetMasterCodeData();
        List<RolesModel> roleList = rolesContext.GetRoles();

        public static event EventHandler DeleteRow;

        public Main
        (
            INavigationService navigationService,
            IPageService pageService
        )
        {
            InitializeComponent();
            //SetPageService(pageService);

            //navigationService.SetNavigationControl(RootNavigation);

            _pageService = pageService;
            _navigationService = navigationService;
            DataContext = this;

            //page
            UserPage.GoToAddUserPage += new EventHandler(GoToAddUserPage);
            UserPage.GoToUpdateUserPage += new EventHandler(GoToUpdateUserPage);
            UserPage.GoToViewUserPage += new EventHandler(GoToViewUserPage);
            ViewUserPage.GoToUpdateCurrentUserPage += new EventHandler(GoToUpdateUserPage);

            //popup
            App.Popup += new EventHandler(Popup);

            App.GoPreviousPage += new EventHandler(PreviousPage);

            PageTitle.Text = "Dashboard";

            initializedDropdownSelectionList();

            //App.MainViewModel.CurrentUsers = new UserModel() { Title = "Dr.", FirstName = "Lee", LastName = "Eunji", StaffName = "Dr. Lee Eunji", EmployeeID = "456783", RegistrationNo = "456783", Gender = "Male", DateOfBirth = "15 March 1991", Role = "Superadmin", EmailAddress = "eunji@gmail.com", Status = "Active" };

            Username.Header = App.MainViewModel.CurrentUsers.StaffName;
        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);

        //    // Make sure that closing this window will begin the process of closing the application.
        //    Application.Current.Shutdown();
        //}

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

            RootNavigation.GoBack();

            GoToViewUserPage(sender,e);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.Origin = "Logout";
            Popup(sender, e);
        }

        private void T1_Click(object sender, RoutedEventArgs e)
        {
            isActive(sender as NavigationViewItem);
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
            isActive(sender as NavigationViewItem);
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

        void Popup(object sender, EventArgs e)
        {
            if (App.MainViewModel.Origin == "UserDeleteRow") { PopupContent.Text = "Are you sure you want to delete this user profile?"; }
            if (App.MainViewModel.Origin == "UserAddRow") { PopupContent.Text = "Are you sure you want to create this user profile?"; }
            if (App.MainViewModel.Origin == "UserUpdateRow") { PopupContent.Text = "Are you sure you want to update this user profile?"; }
            if (App.MainViewModel.Origin == "Logout") { PopupContent.Text = "Are you sure you want to logout?"; }


            PopupBackground.Background = Brushes.DimGray;
            PopupBackground.Opacity = 0.5;
            popup.IsOpen = true;
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;

            PopupBackground.Background = null;

            if (App.MainViewModel.Origin == "UserDeleteRow") { DeleteUserRowHandler(e, sender); }
            if (App.MainViewModel.Origin == "UserAddRow") { AddUserRowHandler(e, sender); }
            if (App.MainViewModel.Origin == "UserUpdateRow") { UpdateUserRowHandler(e, sender); }
            if (App.MainViewModel.Origin == "Logout")
            {
                Login login = new Login(_navigationService, _pageService);
                this.CloseWindow();
                login.Show();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Enable main window
            this.IsEnabled = true;
            // Close the popup
            popup.IsOpen = false;

            PopupBackground.Background = null;
        }

        private void PreviousPage(object sender, EventArgs e)
        {
            var originButton = (System.Windows.Controls.Button)sender;

            if(originButton.Name.ToString() == "UserPage") { MainUserPage(); }
            else { RootNavigation.GoBack(); }
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

        private void isActive(NavigationViewItem navigationItem)
        {
            var childItems = (RootNavigation as NavigationView).MenuItems;

            foreach(NavigationViewItem childItem in childItems)
            {
                childItem.IsActive = false;
            }

            navigationItem.IsActive = true;
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


            var cbDefaultItem = new ComboBoxItem { Content = "" };
            App.MainViewModel.SelectedcbTitle = cbDefaultItem;
            App.MainViewModel.cbTitle.Add(cbDefaultItem);
            foreach (var item in titleList) { App.MainViewModel.cbTitle.Add(new ComboBoxItem { Content = item.CodeID }); }

            cbDefaultItem = new ComboBoxItem { Content = "" };
            App.MainViewModel.SelectedcbGender = cbDefaultItem;
            App.MainViewModel.cbGender.Add(cbDefaultItem);
            foreach (var item in genderList) { App.MainViewModel.cbGender.Add(new ComboBoxItem { Content = item.CodeName, Tag = item.CodeID }); }

            cbDefaultItem = new ComboBoxItem { Content = "" };
            App.MainViewModel.SelectedcbRoles = cbDefaultItem;
            App.MainViewModel.cbRoles.Add(cbDefaultItem);
            foreach (var item in rolesList) { App.MainViewModel.cbRoles.Add(new ComboBoxItem { Content = item.RoleName, Tag = item.RoleID }); }

            cbDefaultItem = new ComboBoxItem { Content = "" };
            App.MainViewModel.SelectedcbStatus = cbDefaultItem;
            App.MainViewModel.cbStatus.Add(cbDefaultItem);
            foreach (var item in statusList) { App.MainViewModel.cbStatus.Add(new ComboBoxItem { Content = item.CodeName, Tag = item.CodeID }); }
        }



        private static void DeleteUserRowHandler(EventArgs e, object sender)
        {
            usersContext.DeleteUser(App.MainViewModel.Users.UserId);

            if (DeleteRow != null)
            {
                DeleteRow(sender, e);
            }
        }


        private void AddUserRowHandler(EventArgs e, object sender)
        {
            usersContext.InsertUser(App.MainViewModel.Users);

            PreviousPage(sender, e);
        }


        private void UpdateUserRowHandler(EventArgs e, object sender)
        {
            usersContext.UpdateUser(App.MainViewModel.Users);

            if(App.MainViewModel.CurrentUsers.UserId == App.MainViewModel.Users.UserId)
            {
                App.MainViewModel.Users = App.MainViewModel.CurrentUsers;
                Username.Header = App.MainViewModel.CurrentUsers.StaffName;
            }


            PreviousPage(sender, e);
        }
    }
}
