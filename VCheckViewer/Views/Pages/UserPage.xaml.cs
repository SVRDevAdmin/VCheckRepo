using Microsoft.EntityFrameworkCore;
using System;
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
using VCheck.Lib.Data.Models;
using VCheckViewer.Views.Windows;

namespace VCheckViewer.Views.Pages
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        VCheck.Lib.Data.DBContext.UserDBContext sContext = VCheckViewer.App.GetService<VCheck.Lib.Data.DBContext.UserDBContext > ();

        public UserPage()
        {
            InitializeComponent();

            dataGrid.ItemsSource = GetUserList(0, 5);
            //dataGrid.DataContext = GetUserList(0,5);
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

            var currentlist = GetUserList((nextpage - 1) * 5, 5);

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
                currentlist = GetUserList((nextPage - 1) * 5, 5);
                dataGrid.ItemsSource = currentlist;
                Page.Text = (currentPage - 1).ToString();
            }
        }

        private void AddUserList_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
