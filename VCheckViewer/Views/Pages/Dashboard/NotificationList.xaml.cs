using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
using Brushes = System.Windows.Media.Brushes;

namespace VCheckViewer.Views.Pages.Dashboard
{
    /// <summary>
    /// Interaction logic for NotificationList.xaml
    /// </summary>
    public partial class NotificationList : Page
    {
        NotificationDBContext sContext = App.GetService<NotificationDBContext>();

        public NotificationList()
        {
            InitializeComponent();
            //initializeData();
        }

        public void initializeData()
        {
            var currentUserCreatedDate = App.MainViewModel.CurrentUsers.CreatedDate;
            var notificationList = sContext.GetNotificationByPage(0, 5, null, null, null, null, currentUserCreatedDate);

            if (notificationList != null && notificationList.Count > 0)
            {
                icNotification.ItemsSource = notificationList.ToList();

                borderNotification.Visibility = Visibility.Visible;
                borderNoNotification.Visibility = Visibility.Collapsed;
            }
            else
            {
                borderNotification.Visibility = Visibility.Collapsed;
                borderNoNotification.Visibility = Visibility.Visible;
            }
        }
    }
}
