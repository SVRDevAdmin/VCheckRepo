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

        public UserPage()
        {
            InitializeComponent();

            dataGrid.ItemsSource = GetUserList(0, 5);
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
            GoToAddUserPageHandler(e, sender);
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            //App.MainViewModel.Users = new Users() { UserId = 1 };

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    // var row = (DataGrid)vis;

                    var rows = GetDataGridRowsForButtons(dataGrid);

                    int id;
                    foreach (DataGridRow dr in rows)
                    {
                        id = (dr.Item as UserModel).UserId;
                        UserModel userModel = dr.Item as UserModel;
                        App.MainViewModel.Users = dr.Item as UserModel;
                        break;
                    }

                    break;
                }

            GoToUpdateUserPageHandler(e, sender);
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    // var row = (DataGrid)vis;

                    var rows = GetDataGridRowsForButtons(dataGrid);

                    int id;
                    foreach (DataGridRow dr in rows)
                    {
                        id = (dr.Item as UserModel).UserId;
                        //MessageBox.Show(id.ToString());
                        sContext.DeleteUser(id);
                        var currentPage = Convert.ToInt32(Page.Text);
                        var currentlist = GetUserList((currentPage - 1) * 5, 5);
                        dataGrid.ItemsSource = currentlist;
                        break;
                    }

                    break;
                }
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

        private IEnumerable<DataGridRow> GetDataGridRowsForButtons(DataGrid grid)
        {
            //IQueryable
            if (!(grid.ItemsSource is IEnumerable itemsSource))
                yield return null;

            foreach (var item in grid.ItemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row & row.IsSelected)
                    yield return row;
            }
        }
    }
}
