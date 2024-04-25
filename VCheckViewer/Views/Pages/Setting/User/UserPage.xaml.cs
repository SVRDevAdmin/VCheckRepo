using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using VCheck.Lib.Data.Models;
using VCheckViewer.ViewModels.Windows;
using VCheckViewer.Views.Windows;
using Wpf.Ui.Controls;

namespace VCheckViewer.Views.Pages
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        UserDBContext sContext = App.GetService<UserDBContext>();

        public static event EventHandler GoToAddUserPage;
        public static event EventHandler GoToUpdateUserPage;
        public int pageSize = 10;

        public UserPage()
        {
            InitializeComponent();

            //dataGrid.ItemsSource = GetUserList(0, pageSize);
        }

        public ObservableCollection<UserModel> GetUserList(int start, int end)
        {
            ObservableCollection<UserModel> UserList = sContext.GetUserListByPage(start, end);
            return UserList;

        }

        private void NextUserList_Click(object sender, RoutedEventArgs e)
        {
            var currentPage = Convert.ToInt32(Page.Text);
            var nextpage = currentPage + 1;

            var currentlist = GetUserList((nextpage - 1) * pageSize, pageSize);

            if(!(currentlist.Count == 0))
            {
                dataGrid.ItemsSource = currentlist;
                Page.Text = (currentPage + 1).ToString();
            }
        }


        private void PreviousUserList_Click(object sender, RoutedEventArgs e)
        {
            var currentPage = Convert.ToInt32(Page.Text);
            var nextPage = currentPage - 1;   
            ObservableCollection<UserModel> currentlist = null;

            if (!(nextPage < 1))
            {
                currentlist = GetUserList((nextPage - 1) * pageSize, pageSize);
                dataGrid.ItemsSource = currentlist;
                Page.Text = (currentPage - 1).ToString();
            }
        }

        private void AddUserList_Click(object sender, RoutedEventArgs e)
        {
            GoToAddUserPageHandler(e, sender);
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.Users = dataGrid.SelectedItem as UserModel;

            GoToUpdateUserPageHandler(e, sender);
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            sContext.DeleteUser((dataGrid.SelectedItem as UserModel).UserId);
            var currentPage = Convert.ToInt32(Page.Text);
            var currentlist = GetUserList((currentPage - 1) * pageSize, pageSize);
            dataGrid.ItemsSource = currentlist;
        }

        private static void GoToAddUserPageHandler(EventArgs e, object sender)
        {
            if (GoToAddUserPage != null)
            {
                GoToAddUserPage(sender, e);
            }
        }

        private static void GoToUpdateUserPageHandler(EventArgs e, object sender)
        {
            if (GoToUpdateUserPage != null)
            {
                GoToUpdateUserPage(sender, e);
            }
        }
    }
}
