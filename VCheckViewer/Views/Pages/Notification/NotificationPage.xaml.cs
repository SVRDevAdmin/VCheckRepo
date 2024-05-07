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
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using Wpf.Ui.Controls;
using Brushes = System.Windows.Media.Brushes;
using Button = System.Windows.Controls.Button;
using TextBlock = System.Windows.Controls.TextBlock;

namespace VCheckViewer.Views.Pages.Notification
{
    /// <summary>
    /// Interaction logic for NotificationPage.xaml
    /// </summary>
    public partial class NotificationPage : Page
    {
        NotificationDBContext sContext = App.GetService<NotificationDBContext>();

        public int pageSize = 4;
        public int paginationSize = 5;
        public int totalNotification = 0;
        public int startPagination = 1;
        public int endPagination = 5;
        public int currentPage = 1;
        public string currentNotificationType;

        public NotificationPage()
        {
            InitializeComponent();

            List<NotificationModel> notificationList = sContext.GetNotificationByPage(0, pageSize, null, null, null, null);
            createList(notificationList);



            totalNotification = sContext.GetTotalNotification(null, null, null, null);
            int totalpage = totalNotification / pageSize;

            if (totalNotification > (pageSize * totalpage)) { totalpage++; }

            if (totalpage > paginationSize) { totalpage = paginationSize; }

            endPagination = totalpage;
            startPagination = 1;

            createPagination(startPagination);
        }

        public void createList(List<NotificationModel> notificationList)
        {
            NotificationViewList.Children.Clear();

            if(notificationList.Count > 0)
            {
                foreach (NotificationModel notification in notificationList)
                {
                    DockPanel mainPanel = new DockPanel();
                    DockPanel panel1 = new DockPanel();
                    DockPanel panel2 = new DockPanel();
                    TextBlock title = new TextBlock();
                    TextBlock date = new TextBlock();
                    TextBlock content = new TextBlock();

                    title.Text = notification.NotificationTitle;
                    title.TextAlignment = TextAlignment.Left;
                    title.FontWeight = FontWeights.Bold;
                    date.Text = notification.CreatedDate;
                    date.TextAlignment = TextAlignment.Right;
                    date.FontWeight = FontWeights.Bold;
                    content.Text = notification.NotificationContent;
                    content.TextWrapping = TextWrapping.Wrap;

                    DockPanel.SetDock(panel1, Dock.Top);
                    DockPanel.SetDock(title, Dock.Left);
                    DockPanel.SetDock(date, Dock.Right);
                    DockPanel.SetDock(panel2, Dock.Bottom);
                    DockPanel.SetDock(content, Dock.Left);

                    panel1.Children.Add(title);
                    panel1.Children.Add(date);
                    panel2.Margin = new Thickness(0, 10, 0, 0);
                    panel2.Children.Add(content);

                    if (notification != notificationList.LastOrDefault()) { mainPanel.Margin = new Thickness(0, 10, 0, 10); }
                    else { mainPanel.Margin = new Thickness(0, 10, 0, 0); }

                    mainPanel.Height = 80;
                    mainPanel.Children.Add(panel1);
                    mainPanel.Children.Add(panel2);

                    NotificationViewList.Children.Add(mainPanel);
                    if (notification != notificationList.LastOrDefault() || notificationList.Count == 1) { NotificationViewList.Children.Add(new Separator() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) }); }
                }
            }
            else
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "No data available";
                textBlock.TextAlignment = TextAlignment.Center;
                NotificationViewList.Children.Add(textBlock);
            }
        }

        public void createPagination(int highligtedIndex)
        {
            paginationPanel.Children.Clear();

            currentPage = highligtedIndex;

            System.Windows.Controls.Button newBtn = new System.Windows.Controls.Button();
            newBtn.Content = "Prev";
            newBtn.Tag = "Prev";
            newBtn.BorderThickness = new Thickness(0);
            newBtn.FontWeight = FontWeights.Bold;
            paginationPanel.Children.Add(newBtn);
            newBtn.Click += new RoutedEventHandler(PreviousUserList_Click);

            for (int i = startPagination; i <= endPagination; i++)
            {
                newBtn = new System.Windows.Controls.Button();

                if (i < 10) { newBtn.Content = "0" + i; }
                else { newBtn.Content = i; }

                newBtn.Tag = i;
                newBtn.Style = (Style)System.Windows.Application.Current.FindResource("RoundButton");
                newBtn.Width = 30;
                newBtn.Margin = new Thickness(5, 0, 5, 0);
                newBtn.FontWeight = FontWeights.Bold;
                newBtn.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;

                if (i == highligtedIndex)
                {
                    newBtn.BorderBrush = Brushes.DarkOrange;
                    newBtn.Background = Brushes.DarkOrange;
                    newBtn.Foreground = Brushes.White;
                }
                else
                {
                    newBtn.BorderBrush = Brushes.DarkOrange;
                    newBtn.Background = Brushes.Transparent;
                    newBtn.Foreground = Brushes.DarkOrange;
                }

                paginationPanel.Children.Add(newBtn);
                newBtn.Click += new RoutedEventHandler(newBtn_Click);
            }

            newBtn = new System.Windows.Controls.Button();
            newBtn.Content = "Next";
            newBtn.Tag = "Next";
            newBtn.BorderThickness = new Thickness(0);
            newBtn.FontWeight = FontWeights.Bold;
            newBtn.Foreground = Brushes.DarkOrange;
            paginationPanel.Children.Add(newBtn);
            newBtn.Click += new RoutedEventHandler(NextUserList_Click);
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;

            btn.BorderBrush = Brushes.DarkOrange;
            btn.Background = Brushes.DarkOrange;
            btn.Foreground = Brushes.White;

            string startDate = null, endDate = null, keyword = null;

            if (RangeDate.SelectedDates.Count > 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.LastOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (RangeDate.SelectedDates.Count == 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.FirstOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }

            if (KeywordSearchBar.Text.Length > 0) {  keyword = KeywordSearchBar.Text; }

            var currentlist = sContext.GetNotificationByPage((((int)btn.Tag) - 1) * pageSize, pageSize, currentNotificationType, startDate, endDate, keyword);
            createList(currentlist);

            int childrenCount = VisualTreeHelper.GetChildrenCount(btn.Parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(btn.Parent, i);
                var frameworkElement = child as System.Windows.Controls.Button;
                if (frameworkElement.Tag.ToString() == currentPage.ToString() && childrenCount > 3)
                {
                    frameworkElement.BorderBrush = Brushes.DarkOrange;
                    frameworkElement.Background = Brushes.Transparent;
                    frameworkElement.Foreground = Brushes.DarkOrange;
                }
            }

            currentPage = (int)btn.Tag;

            if (currentPage == endPagination) { GoToNextPaginationGroup(); return; }
            else if (currentPage == startPagination && currentPage != 1) { GoToPreviousPaginationGroup(); return; }

            RangeDate.DateRangePicker_Popup.IsOpen = false;
        }

        private void NextUserList_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int childrenCount = VisualTreeHelper.GetChildrenCount(btn.Parent);

            var nextpage = currentPage + 1;

            string startDate = null, endDate = null, keyword = null;

            if (RangeDate.SelectedDates.Count > 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.LastOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (RangeDate.SelectedDates.Count == 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.FirstOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }

            if (KeywordSearchBar.Text.Length > 0) { keyword = KeywordSearchBar.Text; }

            var currentlist = sContext.GetNotificationByPage((nextpage - 1) * pageSize, pageSize, currentNotificationType, startDate, endDate, keyword);

            if (!(currentlist.Count == 0))
            {
                createList(currentlist);

                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(btn.Parent, i);
                    var frameworkElement = child as System.Windows.Controls.Button;
                    if (frameworkElement.Tag.ToString() == currentPage.ToString())
                    {
                        frameworkElement.BorderBrush = Brushes.DarkOrange;
                        frameworkElement.Background = Brushes.Transparent;
                        frameworkElement.Foreground = Brushes.DarkOrange;
                    }
                    else if (frameworkElement.Tag.ToString() == nextpage.ToString())
                    {
                        frameworkElement.BorderBrush = Brushes.DarkOrange;
                        frameworkElement.Background = Brushes.DarkOrange;
                        frameworkElement.Foreground = Brushes.White;
                    }
                }

                currentPage = nextpage;
            }

            if (currentPage == endPagination) { GoToNextPaginationGroup(); return; }
        }

        private void PreviousUserList_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            int childrenCount = VisualTreeHelper.GetChildrenCount(btn.Parent);

            var nextPage = currentPage - 1;
            List<NotificationModel> currentlist;

            string startDate = null, endDate = null, keyword = null;

            if (RangeDate.SelectedDates.Count > 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.LastOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (RangeDate.SelectedDates.Count == 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.FirstOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }

            if (KeywordSearchBar.Text.Length > 0) { keyword = KeywordSearchBar.Text; }

            if (!(nextPage < 1))
            {
                currentlist = sContext.GetNotificationByPage((nextPage - 1) * pageSize, pageSize, currentNotificationType, startDate, endDate, keyword);
                createList(currentlist);

                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(btn.Parent, i);
                    var frameworkElement = child as System.Windows.Controls.Button;
                    if (frameworkElement.Tag.ToString() == currentPage.ToString())
                    {
                        frameworkElement.BorderBrush = Brushes.DarkOrange;
                        frameworkElement.Background = Brushes.Transparent;
                        frameworkElement.Foreground = Brushes.DarkOrange;
                    }
                    else if (frameworkElement.Tag.ToString() == nextPage.ToString())
                    {
                        frameworkElement.BorderBrush = Brushes.DarkOrange;
                        frameworkElement.Background = Brushes.DarkOrange;
                        frameworkElement.Foreground = Brushes.White;
                    }
                }

                currentPage = nextPage;
            }

            if (currentPage == startPagination && currentPage != 1) { GoToPreviousPaginationGroup(); return; }

            RangeDate.DateRangePicker_Popup.IsOpen = false;
        }

        private void GoToNextPaginationGroup()
        {
            string startDate = null, endDate = null, keyword = null;

            if (RangeDate.SelectedDates.Count > 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.LastOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (RangeDate.SelectedDates.Count == 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.FirstOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }

            if (KeywordSearchBar.Text.Length > 0) { keyword = KeywordSearchBar.Text; }

            var currentlist = sContext.GetNotificationByPage(endPagination * pageSize, pageSize, currentNotificationType, startDate, endDate, keyword);

            if (!(currentlist.Count == 0))
            {
                startPagination++;
                endPagination++;

                createPagination(endPagination - 1);
            }

        }

        private void GoToPreviousPaginationGroup()
        {
            startPagination--;
            endPagination--;

            createPagination(startPagination + 1);
        }

        private void RangeDate_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string startDate = null, endDate = null, keyword = null;

            if (RangeDate.SelectedDates.Count > 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.LastOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (RangeDate.SelectedDates.Count == 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.FirstOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }

            if (KeywordSearchBar.Text.Length > 0) { keyword = KeywordSearchBar.Text; }

            var notificationList = sContext.GetNotificationByPage(0, pageSize, currentNotificationType, startDate, endDate, keyword);

            createList(notificationList);

            totalNotification = sContext.GetTotalNotification(currentNotificationType, startDate, endDate, keyword);
            int totalpage = totalNotification / pageSize;

            if (totalNotification > (pageSize * totalpage)) { totalpage++; }

            if (totalpage > paginationSize) { totalpage = paginationSize; }

            endPagination = totalpage;
            startPagination = 1;
            
            createPagination(startPagination);
        }

        private void NotificationKeywordSearch(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string startDate = null, endDate = null, keyword = null;

            if (RangeDate.SelectedDates.Count > 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.LastOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (RangeDate.SelectedDates.Count == 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.FirstOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }

            if (KeywordSearchBar.Text.Length > 0) { keyword = KeywordSearchBar.Text; }

            var notificationList = sContext.GetNotificationByPage(0, pageSize, currentNotificationType, startDate, endDate, keyword);

            createList(notificationList);

            totalNotification = sContext.GetTotalNotification(currentNotificationType, startDate, endDate, keyword);
            int totalpage = totalNotification / pageSize;

            if (totalNotification > (pageSize * totalpage)) { totalpage++; }

            if (totalpage > paginationSize) { totalpage = paginationSize; }

            endPagination = totalpage;
            startPagination = 1;

            createPagination(startPagination);

            RangeDate.DateRangePicker_Popup.IsOpen = false;
        }

        private void ChangeNotificationType(object sender, RoutedEventArgs e)
        {
            Button notificationTypeBtn = sender as Button;

            notificationTypeBtn.Background = Brushes.DarkOrange;
            notificationTypeBtn.BorderThickness = new Thickness();

            if(notificationTypeBtn.Tag != null) { currentNotificationType = notificationTypeBtn.Tag.ToString(); }
            else { currentNotificationType = null; }

            Grid parent = (Grid)((Border)notificationTypeBtn.Parent).Parent;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var firstChild = (Border)VisualTreeHelper.GetChild(parent, i);
                var secondChild = (Button)VisualTreeHelper.GetChild(firstChild, 0);

                if(secondChild != notificationTypeBtn)
                {
                    secondChild.Background = Brushes.Black;
                    secondChild.BorderThickness = new Thickness(0);
                }
            }

            string startDate = null, endDate = null, keyword = null;

            if (RangeDate.SelectedDates.Count > 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.LastOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (RangeDate.SelectedDates.Count == 1)
            {
                startDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                endDate = RangeDate.SelectedDates.FirstOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }

            if (KeywordSearchBar.Text.Length > 0) { keyword = KeywordSearchBar.Text; }

            var notificationList = sContext.GetNotificationByPage(0, pageSize, currentNotificationType, startDate, endDate, keyword);

            createList(notificationList);

            totalNotification = sContext.GetTotalNotification(currentNotificationType, startDate, endDate, keyword);
            int totalpage = totalNotification / pageSize;

            if (totalNotification > (pageSize * totalpage)) { totalpage++; }

            if (totalpage > paginationSize) { totalpage = paginationSize; }

            endPagination = totalpage;
            startPagination = 1;

            createPagination(startPagination);

            RangeDate.DateRangePicker_Popup.IsOpen = false;

        }

        private void CloseCalenderPopup(object sender, MouseButtonEventArgs e)
        {
            RangeDate.DateRangePicker_Popup.IsOpen = false;
        }
    }
}
