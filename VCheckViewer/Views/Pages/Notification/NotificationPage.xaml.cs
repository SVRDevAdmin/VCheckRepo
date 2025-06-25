using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using Button = System.Windows.Controls.Button;
using Hyperlink = System.Windows.Documents.Hyperlink;
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
        public string? currentNotificationType;
        public string? currentStartDate, currentEndDate, currentKeyword;

        public NotificationPage()
        {
            InitializeComponent();

            reloadData(0, pageSize, null, null, null, null, true);

            pagination.ButtonNextControlClick += new EventHandler(PaginationNextButton_Click);
            pagination.ButtonPrevControlClick += new EventHandler(PaginationPrevButton_Click);
            pagination.ButtonPageControlClick += new EventHandler(PaginationNumButton_Click);
        }

        public void reloadData(int start, int end, string? notificationType, string? startDate, string? endDate, string? keyword, bool reset)
        {
            var currentUserCreatedDate = App.MainViewModel.CurrentUsers.CreatedDate;
            List<NotificationModel> notificationList = sContext.GetNotificationByPage(start, end, notificationType, startDate, endDate, keyword, currentUserCreatedDate);
            createList(notificationList);

            if (reset)
            {
                pagination.iTotalRecords = sContext.GetTotalNotification(notificationType, startDate, endDate, keyword, currentUserCreatedDate);
                pagination.iPaginationLimit = paginationSize;
                pagination.iPageSize = pageSize;
                currentPage = 1;
                pagination.GetPageCountByRecordCountWithLimit();
            }

            pagination.LoadPagingNumberWithLimit();

            NotificationSearch sSearchModel = new NotificationSearch();
            sSearchModel.SearchStart = start;
            sSearchModel.SearchEnd = end;
            if (notificationType == null)
            {
                sSearchModel.SearchType = "All";
            }
            else
            {
                sSearchModel.SearchType = notificationType;
            }
            
            sSearchModel.SearchStartDate = startDate;
            sSearchModel.SearchEndDate = endDate;
            sSearchModel.SearchKeyword = keyword;
            sSearchModel.SearchReset = reset;
            sSearchModel.SearchSelectedDates = RangeDate.SelectedDates;

            App.MainViewModel.SearchModel = sSearchModel;
        }

        public void createList(List<NotificationModel> notificationList)
        {
            String? sColor = System.Windows.Application.Current.Resources["Themes_FontColor"].ToString();
            String? sSeparatorColor = System.Windows.Application.Current.Resources["Themes_GridNotifRowLinesColor"].ToString();
            SolidColorBrush sBrushSeparator = new BrushConverter().ConvertFrom(sSeparatorColor) as SolidColorBrush;

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
                    title.TextAlignment = System.Windows.TextAlignment.Left;
                    title.FontWeight = FontWeights.Bold;
                    title.Foreground = new BrushConverter().ConvertFrom(sColor) as SolidColorBrush;
                    date.Text = notification.CreatedDate;
                    date.TextAlignment = System.Windows.TextAlignment.Right;
                    date.FontWeight = FontWeights.Bold;
                    date.Foreground = new BrushConverter().ConvertFrom(sColor) as SolidColorBrush;
                    content.Text = notification.NotificationContent;
                    content.TextWrapping = TextWrapping.Wrap;
                    content.Foreground = new BrushConverter().ConvertFrom(sColor) as SolidColorBrush;

                    if (notification.NotificationTitle == "Reminder for Software Update")
                    {
                        var contentArray = notification.NotificationContent.Split("###<link>###");

                        content.Inlines.Clear();
                        content.Inlines.Add(contentArray[0]);
                        Hyperlink hyperLink = new Hyperlink()
                        {
                            NavigateUri = new Uri(App.UpdateLink)
                        };
                        hyperLink.Inlines.Add(App.UpdateLink);
                        hyperLink.RequestNavigate += new RequestNavigateEventHandler(delegate (object sender, RequestNavigateEventArgs e)
                        {
                            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                            e.Handled = true;
                        });
                        content.Inlines.Add(hyperLink);
                        content.Inlines.Add(contentArray[1]);
                    }


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
                    if (notification != notificationList.LastOrDefault() || notificationList.Count == 1) { NotificationViewList.Children.Add(new Separator() { BorderBrush = sBrushSeparator, BorderThickness = new Thickness(0.5) }); }
                }
            }
            else
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = Properties.Resources.General_Message_NoData;
                textBlock.Foreground = new BrushConverter().ConvertFrom(sColor) as SolidColorBrush;
                textBlock.FontWeight = FontWeights.Bold;
                textBlock.TextAlignment = System.Windows.TextAlignment.Center;
                NotificationViewList.Children.Add(textBlock);
            }
        }
        
        protected void PaginationNumButton_Click(object? sender, EventArgs? e)
        {
            Button btnNum = sender as Button;
            int iNumberSelected = Convert.ToInt32(btnNum?.Tag);

            currentPage = iNumberSelected;

            pagination.iPageIndex = currentPage;

            if (pagination.iPaginationEnd == currentPage && pagination.iPaginationEnd != pagination.iLastPage) { pagination.iPaginationStart++; pagination.iPaginationEnd++; }
            else if (pagination.iPaginationStart == currentPage && currentPage != 1) { pagination.iPaginationStart--; pagination.iPaginationEnd--; }

            reloadData((currentPage - 1) * pageSize, pageSize, currentNotificationType, currentStartDate, currentEndDate, currentKeyword, false);
        }               

        protected void PaginationNextButton_Click(object? sender, EventArgs? e)
        {
            currentPage++;

            pagination.iPageIndex = currentPage;

            if(pagination.iPaginationEnd == currentPage && pagination.iPaginationEnd != pagination.iLastPage) { pagination.iPaginationStart++; pagination.iPaginationEnd++; }

            reloadData((currentPage - 1) * pageSize, pageSize, currentNotificationType, currentStartDate, currentEndDate, currentKeyword, false);
        }

        protected void PaginationPrevButton_Click(object? sender, EventArgs? e)
        {
            currentPage--;

            pagination.iPageIndex = currentPage;

            if (pagination.iPaginationStart == currentPage && currentPage != 1) { pagination.iPaginationStart--; pagination.iPaginationEnd--; }

            reloadData((currentPage - 1) * pageSize, pageSize, currentNotificationType, currentStartDate, currentEndDate, currentKeyword, false);
        }
                
        private void NotificationKeywordSearch(object sender, System.Windows.Input.KeyEventArgs e)
        {
            CloseCalenderPopup(null, null);
        }

        private void ChangeNotificationType(object sender, RoutedEventArgs e)
        {
            CloseCalenderPopup(null, null);
        }

        private void UpdateFilter(object sender, RoutedEventArgs e)
        {
            currentStartDate = null;
            currentEndDate = null;
            currentKeyword = null;

            if (RangeDate.SelectedDates.Count > 0)
            {
                currentStartDate = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd");
                currentEndDate = RangeDate.SelectedDates.LastOrDefault().AddDays(1).ToString("yyyy-MM-dd");
            }

            if (KeywordSearchBar.Text.Length > 0) { currentKeyword = KeywordSearchBar.Text; }

            var notificationTypeBtn = ((ComboBoxItem)NotificationType.SelectedItem).Tag;

            if (notificationTypeBtn != null) { currentNotificationType = notificationTypeBtn.ToString(); }
            else { currentNotificationType = null; }

            reloadData(0, pageSize, currentNotificationType, currentStartDate, currentEndDate, currentKeyword, true);

            CloseCalenderPopup(null, null);

        }

        private void CloseCalenderPopup(object? sender, MouseButtonEventArgs? e)
        {
            RangeDate.DateRangePicker_Popup.IsOpen = false;
        }
    }
}
