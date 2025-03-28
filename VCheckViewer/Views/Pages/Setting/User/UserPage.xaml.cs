﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using Brushes = System.Windows.Media.Brushes;
using Application = System.Windows.Application;
using VCheckViewer.Views.Pages.Setting.Device;
using DocumentFormat.OpenXml.Wordprocessing;
using Mysqlx.Session;

namespace VCheckViewer.Views.Pages
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        UserDBContext sContext = App.GetService<UserDBContext>();

        public static event EventHandler? GoToAddUserPage;
        public static event EventHandler? GoToUpdateUserPage;
        public static event EventHandler? GoToViewUserPage;
        public static event EventHandler? DeleteUser;
        public static event EventHandler? GoToLanguageCountryPage;
        public int pageSize = 10;
        public int paginationSize = 3;
        public int totalUser = 0;
        public int startPagination = 1;
        public int endPagination = 5;
        public int currentPage = 1;

        public UserPage()
        {
            InitializeComponent();

            Main.InitializedUserPage += new EventHandler(initializedPage);
            pagination.ButtonNextControlClick += new EventHandler(PaginationNextButton_Click);
            pagination.ButtonPrevControlClick += new EventHandler(PaginationPrevButton_Click);
            pagination.ButtonPageControlClick += new EventHandler(PaginationNumButton_Click);

            initializedPage(null,null);

            if (App.MainViewModel.CurrentUsers.Role == "Lab User")
            {
                btnSettings.IsEnabled = false;
                btnDevice.IsEnabled = false;
            }
            else
            {
                btnSettings.IsEnabled = true;
                btnDevice.IsEnabled = true;
            }
        }

        public void initializedPage(object? sender, EventArgs? e)
        {
            //var userList = GetUserList(0, pageSize);

            //dataGrid.ItemsSource = GetUserList(0, pageSize);

            //paginationPanel.Children.Clear();

            //if (userList.Count > 0)
            //{
            //    totalUser = sContext.GetTotalUser();
            //    int totalpage = totalUser / pageSize;

            //    if (totalUser > (pageSize * totalpage)) { totalpage++; }

            //    if (totalpage > paginationSize) { totalpage = paginationSize; }

            //    endPagination = totalpage;
            //    startPagination = 1;

            //    createPagination(startPagination);
            //}

            reloadData(0, pageSize, true);

        }

        public void reloadData(int start, int end, bool reset)
        {
            dataGrid.ItemsSource = sContext.GetUserListByPage(start, end);
            App.MainViewModel.CurrentUserIndexStart = start + 1;

            if (reset)
            {
                pagination.iTotalRecords = sContext.GetTotalUser();
                pagination.iPaginationLimit = paginationSize;
                pagination.iPageSize = pageSize;
                currentPage = 1;
                pagination.GetPageCountByRecordCountWithLimit();
            }

            pagination.LoadPagingNumberWithLimit();

            //pagination.iTotalRecords = sContext.GetTotalUser();
            //pagination.iPageIndex = currentPage;
            //pagination.iPageSize = pageSize;
            //pagination.LoadPagingNumber();
        }

        //public ObservableCollection<UserModel> GetUserList(int start, int end)
        //{
        //    ObservableCollection<UserModel> UserList = sContext.GetUserListByPage(start, end);
        //    App.MainViewModel.CurrentUserIndexStart = start + 1;
        //    return UserList;
        //}

        //public void createPagination(int highligtedIndex)
        //{
        //    currentPage = highligtedIndex;

        //    //System.Windows.Controls.Button newBtn = new System.Windows.Controls.Button();
        //    //newBtn.Content = Properties.Resources.General_Label_Previous;
        //    //newBtn.Tag = "Prev";
        //    //newBtn.BorderThickness = new Thickness(0);
        //    //newBtn.FontWeight = FontWeights.Bold;
        //    //newBtn.Foreground = Brushes.Gray;
        //    ////paginationPanel.Children.Add(newBtn);
        //    //newBtn.Click += new RoutedEventHandler(PreviousUserList_Click);

        //    //for (int i = startPagination; i <= endPagination; i++)
        //    //{
        //    //    newBtn = new System.Windows.Controls.Button();

        //    //    if(i < 10) { newBtn.Content = "0"+i; }
        //    //    else { newBtn.Content = i; }

        //    //    newBtn.Tag = i;
        //    //    newBtn.Style = (Style)Application.Current.FindResource("RoundButton");
        //    //    newBtn.Width = 40;
        //    //    newBtn.Height = 40;
        //    //    newBtn.Margin = new Thickness(5, 0, 5, 0);
        //    //    newBtn.FontWeight = FontWeights.Bold;
        //    //    newBtn.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;

        //    //    if(i == highligtedIndex)
        //    //    {
        //    //        newBtn.BorderBrush = Brushes.DarkOrange;
        //    //        newBtn.Background = Brushes.DarkOrange;
        //    //        newBtn.Foreground = Brushes.White;
        //    //    }
        //    //    else
        //    //    {
        //    //        newBtn.BorderBrush = Brushes.DarkOrange;
        //    //        newBtn.Background = Brushes.Transparent;
        //    //        newBtn.Foreground = Brushes.DarkOrange;
        //    //    }

        //    //    //paginationPanel.Children.Add(newBtn);
        //    //    newBtn.Click += new RoutedEventHandler(newBtn_Click);
        //    //}

        //    //newBtn = new System.Windows.Controls.Button();
        //    //newBtn.Content = Properties.Resources.General_Label_Next;
        //    //newBtn.Tag = "Next";
        //    //newBtn.BorderThickness = new Thickness(0);
        //    //newBtn.FontWeight = FontWeights.Bold;
        //    //newBtn.Foreground = Brushes.DarkOrange;
        //    ////paginationPanel.Children.Add(newBtn);
        //    //newBtn.Click += new RoutedEventHandler(NextUserList_Click);
        //}

        //private void newBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;

        //    btn.BorderBrush = Brushes.DarkOrange;
        //    btn.Background = Brushes.DarkOrange;
        //    btn.Foreground = Brushes.White;

        //    var currentlist = GetUserList((((int)btn.Tag) - 1) * pageSize, pageSize);
        //    dataGrid.ItemsSource = currentlist;

        //    int childrenCount = VisualTreeHelper.GetChildrenCount(btn.Parent);

        //    for (int i = 0; i < childrenCount; i++)
        //    {
        //        var child = VisualTreeHelper.GetChild(btn.Parent, i);
        //        var frameworkElement = child as System.Windows.Controls.Button;
        //        if (frameworkElement.Tag.ToString() == currentPage.ToString() && childrenCount > 3)
        //        {
        //            frameworkElement.BorderBrush = Brushes.DarkOrange;
        //            frameworkElement.Background = Brushes.Transparent;
        //            frameworkElement.Foreground = Brushes.DarkOrange;
        //        }
        //    }

        //    currentPage = (int)btn.Tag;

        //    if (currentPage == endPagination) { GoToNextPaginationGroup(); return; }
        //    else if (currentPage == startPagination && currentPage != 1) { GoToPreviousPaginationGroup(); return; }
        //}

        protected void PaginationNumButton_Click(object? sender, EventArgs? e)
        {
            System.Windows.Controls.Button btnNum = sender as System.Windows.Controls.Button;
            int iNumberSelected = Convert.ToInt32(btnNum?.Tag);

            currentPage = iNumberSelected;

            pagination.iPageIndex = currentPage;

            if (pagination.iPaginationEnd == currentPage && pagination.iPaginationEnd != pagination.iLastPage) { pagination.iPaginationStart++; pagination.iPaginationEnd++; }
            else if (pagination.iPaginationStart == currentPage && currentPage != 1) { pagination.iPaginationStart--; pagination.iPaginationEnd--; }

            reloadData((currentPage - 1) * pageSize, pageSize, false);
        }

        //private void NextUserList_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
        //    int childrenCount = VisualTreeHelper.GetChildrenCount(btn.Parent);

        //    var nextpage = currentPage + 1;

        //    var currentlist = GetUserList((nextpage - 1) * pageSize, pageSize);

        //    if(!(currentlist.Count == 0))
        //    {
        //        dataGrid.ItemsSource = currentlist;

        //        for (int i = 0; i < childrenCount; i++)
        //        {
        //            var child = VisualTreeHelper.GetChild(btn.Parent, i);
        //            var frameworkElement = child as System.Windows.Controls.Button;
        //            if (frameworkElement.Tag.ToString() == currentPage.ToString())
        //            {
        //                frameworkElement.BorderBrush = Brushes.DarkOrange;
        //                frameworkElement.Background = Brushes.Transparent;
        //                frameworkElement.Foreground = Brushes.DarkOrange;
        //            }
        //            else if (frameworkElement.Tag.ToString() == nextpage.ToString())
        //            {
        //                frameworkElement.BorderBrush = Brushes.DarkOrange;
        //                frameworkElement.Background = Brushes.DarkOrange;
        //                frameworkElement.Foreground = Brushes.White;
        //            }
        //        }

        //        currentPage = nextpage;
        //    }

        //    if (currentPage == endPagination) { GoToNextPaginationGroup(); return; }
        //}

        protected void PaginationNextButton_Click(object? sender, EventArgs? e)
        {
            currentPage++;

            pagination.iPageIndex = currentPage;

            if (pagination.iPaginationEnd == currentPage && pagination.iPaginationEnd != pagination.iLastPage) { pagination.iPaginationStart++; pagination.iPaginationEnd++; }

            reloadData((currentPage - 1) * pageSize, pageSize, false);
        }

        //private void GoToNextPaginationGroup()
        //{
        //    var currentlist = GetUserList((endPagination) * pageSize, pageSize);

        //    if(!(currentlist.Count == 0))
        //    {
        //        startPagination++;
        //        endPagination++;

        //        //paginationPanel.Children.Clear();
        //        createPagination(endPagination - 1); 
        //    }
            
        //}

        //private void GoToPreviousPaginationGroup()
        //{
        //    //paginationPanel.Children.Clear();

        //    startPagination--;
        //    endPagination--;

        //    createPagination(startPagination + 1);
        //}


        //private void PreviousUserList_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
        //    int childrenCount = VisualTreeHelper.GetChildrenCount(btn.Parent);

        //    var nextPage = currentPage - 1;   
        //    ObservableCollection<UserModel> currentlist;

        //    if (!(nextPage < 1))
        //    {
        //        currentlist = GetUserList((nextPage - 1) * pageSize, pageSize);
        //        dataGrid.ItemsSource = currentlist;

        //        for (int i = 0; i < childrenCount; i++)
        //        {
        //            var child = VisualTreeHelper.GetChild(btn.Parent, i);
        //            var frameworkElement = child as System.Windows.Controls.Button;
        //            if (frameworkElement.Tag.ToString() == currentPage.ToString())
        //            {
        //                frameworkElement.BorderBrush = Brushes.DarkOrange;
        //                frameworkElement.Background = Brushes.Transparent;
        //                frameworkElement.Foreground = Brushes.DarkOrange;
        //            }
        //            else if (frameworkElement.Tag.ToString() == nextPage.ToString())
        //            {
        //                frameworkElement.BorderBrush = Brushes.DarkOrange;
        //                frameworkElement.Background = Brushes.DarkOrange;
        //                frameworkElement.Foreground = Brushes.White;
        //            }
        //        }

        //        currentPage = nextPage;
        //    }

        //    if (currentPage == startPagination && currentPage != 1) { GoToPreviousPaginationGroup(); return; }
        //}

        protected void PaginationPrevButton_Click(object? sender, EventArgs? e)
        {
            currentPage--;

            pagination.iPageIndex = currentPage;

            if (pagination.iPaginationStart == currentPage && currentPage != 1) { pagination.iPaginationStart--; pagination.iPaginationEnd--; }

            reloadData((currentPage - 1) * pageSize, pageSize, false);
        }

        private void AddUserList_Click(object sender, RoutedEventArgs e)
        {
            GoToAddUserPageHandler(e, sender);
        }

        private void ViewUserButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.Users = dataGrid.SelectedItem as UserModel;

            GoToViewUserPageHandler(e, sender);
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.Users = dataGrid.SelectedItem as UserModel;

            GoToUpdateUserPageHandler(e, sender);
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.Origin = "UserDeleteRow";

            App.MainViewModel.Users = (dataGrid.SelectedItem as UserModel);

            App.PopupHandler(e, sender);
        }

        private void LanguageCountry(object sender, RoutedEventArgs e)
        {
            GoToLanguageCountryPageHandler(e, sender);
        }

        private static void GoToAddUserPageHandler(EventArgs e, object sender)
        {
            if (GoToAddUserPage != null)
            {
                GoToAddUserPage(sender, e);
            }
        }

        private static void GoToViewUserPageHandler(EventArgs e, object sender)
        {
            if (GoToViewUserPage != null)
            {
                GoToViewUserPage(sender, e);
            }
        }

        private static void GoToUpdateUserPageHandler(EventArgs e, object sender)
        {
            if (GoToUpdateUserPage != null)
            {
                GoToUpdateUserPage(sender, e);
            }
        }

        private static void GoToLanguageCountryPageHandler(EventArgs e, object sender)
        {
            if (GoToLanguageCountryPage != null)
            {
                GoToLanguageCountryPage(sender, e);
            }
        }

        private void btnDevice_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingDevicePageHandler(e, sender);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingConfigurationPageHandler(e, sender);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingReportPageHandler(e, sender);
        }
    }
}
