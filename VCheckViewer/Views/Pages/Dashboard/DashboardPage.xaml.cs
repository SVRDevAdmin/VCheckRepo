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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;
using VCheckViewer.Views.Pages.Dashboard;
using Brushes = System.Windows.Media.Brushes;
using Button = System.Windows.Controls.Button;

namespace VCheckViewer.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public List<DeviceModel> DeviceList = new List<DeviceModel>()
        {
            new DeviceModel()
            {
                DeviceImagePath = "../../../Content/Images/VCheck200Image.png",
                DeviceName = "VCheck 200",
                status = 1
            },
            new DeviceModel()
            {
                DeviceImagePath = "../../../Content/Images/VCheck2400Image.png",
                DeviceName = "VCheck 2400",
                status = 0
            },
            new DeviceModel()
            {
                DeviceImagePath = "../../../Content/Images/VCheck200Image.png",
                DeviceName = "VCheck 200 (2)",
                status = 1
            },
            new DeviceModel()
            {
                DeviceImagePath = "../../../Content/Images/VCheck2400Image.png",
                DeviceName = "VCheck 2400 (2)",
                status = 0
            }

        };

        public DashboardPage()
        {
            InitializeComponent();
            initializeData();

            var message = Properties.Resources.Dashboard_Message_DownloadLatest.Split("<nextline>");

            updateMessage.Text = message[0] + "\r\n" + message[1];
        }

        public void initializeData()
        {
            List<DeviceModel> sScheduledList = DeviceList;
            if (sScheduledList != null && sScheduledList.Count > 0)
            {
                icDeviceList.ItemsSource = sScheduledList.ToList();
                //icScheduledTest.ItemsSource = sScheduledList.ToList();

                borderDeviceList.Visibility = Visibility.Visible;
                borderNoDeviceList.Visibility = Visibility.Collapsed;
                //borderScheduledTest.Visibility = Visibility.Visible;
                //borderNoScheduledTest.Visibility = Visibility.Collapsed;
            }
            else
            {
                borderDeviceList.Visibility = Visibility.Collapsed;
                borderNoDeviceList.Visibility = Visibility.Visible;
                //borderScheduledTest.Visibility = Visibility.Collapsed;
                //borderNoScheduledTest.Visibility = Visibility.Visible;
            }

            RightListContent.Content = new ScheduleTestList();
        }

        private void ChangeList(object sender, RoutedEventArgs e)
        {
            Button notificationTypeBtn = sender as Button;

            notificationTypeBtn.Background = Brushes.DarkOrange;
            notificationTypeBtn.BorderThickness = new Thickness();

            Grid parent = (Grid)((Border)notificationTypeBtn.Parent).Parent;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var firstChild = (Border)VisualTreeHelper.GetChild(parent, i);
                var secondChild = (Button)VisualTreeHelper.GetChild(firstChild, 0);

                if (secondChild != notificationTypeBtn)
                {
                    secondChild.Background = Brushes.Black;
                    secondChild.BorderThickness = new Thickness(0);
                }
            }

            if (notificationTypeBtn.Tag.ToString() == "Schedule") { RightListContent.Content = new ScheduleTestList(); }
            else if (notificationTypeBtn.Tag.ToString() == "Completed Test Results") { RightListContent.Content = new TestResultList(); }
            else { RightListContent.Content = new NotificationList(); }
        }
    }
}
