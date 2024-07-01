using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using VCheck.Lib.Data;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;
using Orientation = System.Windows.Controls.Orientation;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace VCheckViewer.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        List<DeviceModel> deviceList = DeviceRepository.GetDeviceList(ConfigSettings.GetConfigurationSettings());

        List<TestResultModel> resultList = TestResultsRepository.GetTodayTestResultList(ConfigSettings.GetConfigurationSettings());

        public DashboardPage()
        {
            InitializeComponent();
            //initializeData();
            generateView();

            var message = Properties.Resources.Dashboard_Message_DownloadLatest.Split("<nextline>");

            updateMessage.Text = message[0] + "\r\n" + message[1];
        }

        //public void initializeData()
        //{
        //    List<DeviceModel> sScheduledList = VCheck.Lib.Data.DeviceRepository.GetDeviceList(ConfigSettings.GetConfigurationSettings());
        //    //List<DeviceModel> sScheduledList = DeviceList;

        //    if (sScheduledList != null && sScheduledList.Count > 0)
        //    {
        //        icDeviceList.ItemsSource = sScheduledList.ToList();
        //        //icScheduledTest.ItemsSource = sScheduledList.ToList();

        //        borderDeviceList.Visibility = Visibility.Visible;
        //        borderNoDeviceList.Visibility = Visibility.Collapsed;
        //        //borderScheduledTest.Visibility = Visibility.Visible;
        //        //borderNoScheduledTest.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        borderDeviceList.Visibility = Visibility.Collapsed;
        //        borderNoDeviceList.Visibility = Visibility.Visible;
        //        //borderScheduledTest.Visibility = Visibility.Collapsed;
        //        //borderNoScheduledTest.Visibility = Visibility.Visible;
        //    }

        //    RightListContent.Content = new ScheduleTestList();
        //}

        //private void ChangeList(object sender, RoutedEventArgs e)
        //{
        //    Button notificationTypeBtn = sender as Button;

        //    notificationTypeBtn.Background = Brushes.DarkOrange;
        //    notificationTypeBtn.BorderThickness = new Thickness();

        //    Grid parent = (Grid)((Border)notificationTypeBtn.Parent).Parent;

        //    int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

        //    for (int i = 0; i < childrenCount; i++)
        //    {
        //        var firstChild = (Border)VisualTreeHelper.GetChild(parent, i);
        //        var secondChild = (Button)VisualTreeHelper.GetChild(firstChild, 0);

        //        if (secondChild != notificationTypeBtn)
        //        {
        //            secondChild.Background = Brushes.Black;
        //            secondChild.BorderThickness = new Thickness(0);
        //        }
        //    }

        //    if (notificationTypeBtn.Tag.ToString() == "Schedule") { RightListContent.Content = new ScheduleTestList(); }
        //    else if (notificationTypeBtn.Tag.ToString() == "Completed Test Results") { RightListContent.Content = new TestResultList(); }
        //    else { RightListContent.Content = new NotificationList(); }
        //}

        public void generateView()
        {
            responsiveView.Children.Clear();

            int totalElement = 3;
            //int totalElement = deviceList.Count;
            int imageHeight = 170;
            int borderHeight = 300;
            int margin = 10;
            int totalRow = 0;
            int totalElementPerRow = 3;
            bool excess = false;
            int remainder = 0;

            if (totalElement < 4)
            {
                totalRow = 1;
                totalElementPerRow = totalElement;
            }
            else if (totalElement == 4)
            {
                totalRow = 2;
                totalElementPerRow = 2;
                imageHeight = 100;
                borderHeight = 201;
                margin = 3;
            }
            else
            {
                totalRow = Math.DivRem(totalElement, 3, out remainder);
                if (remainder > 0)
                {
                    totalRow += 1;
                    excess = true;
                }
                imageHeight = 100;
                borderHeight = 201;
                margin = 3;
            }

            createElement(totalElementPerRow, imageHeight, borderHeight, margin, totalRow, excess, remainder);
        }

        public void createElement(int totalElementPerRow, int imageHeight, int borderHeight, int margin, int totalRow, bool excess, int remainder)
        {
            for (int i = 0; i < totalRow; i++)
            {
                StackPanel mainStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };

                if (i == (totalRow - 1) && excess) { totalElementPerRow = remainder; }

                for (int j = 0; j < totalElementPerRow; j++)
                {
                    Random rnd = new Random();
                    int randomNumberInRange = rnd.Next(1, 3);
                    string devicePath = randomNumberInRange == 2 ? "VCheck200Image2" : "VCheck2400Image2";

                    Border parentBorder = new Border() { Height = borderHeight, Width = 290, CornerRadius = new CornerRadius(5), Margin = new Thickness(25, 0, 25, 10), Padding = new Thickness(5) };
                    StackPanel secondStackPanel = new StackPanel() { Orientation = Orientation.Vertical};

                    TextBlock statusTextBlock = new TextBlock() { FontSize = 11, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                    statusTextBlock.Foreground = randomNumberInRange == 2 ? (Brush)new BrushConverter().ConvertFromString("#F28C28") : Brushes.DarkCyan;
                    statusTextBlock.Text = randomNumberInRange == 2 ? "Busy" : "Ready";
                    Border childBorder = new Border() { Height = 30, Width = 70, HorizontalAlignment = System.Windows.HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, CornerRadius = new CornerRadius(5), Child = statusTextBlock };
                    childBorder.Background = randomNumberInRange == 2 ? (Brush)new BrushConverter().ConvertFromString("#FAD5A5") : (Brush)new BrushConverter().ConvertFromString("#B5F8E3");

                    Image image = new Image();
                    var uri = new Uri(@"pack://application:,,,/VCheckViewer;component/Content/Images/"+ devicePath + ".png");
                    var bitmap = new BitmapImage(uri);
                    image.Source = bitmap;
                    image.Height = imageHeight;
                    image.Margin = new Thickness(margin);

                    TextBlock nameTextBlock = new TextBlock() { Text = "Vcheck V200", TextAlignment = TextAlignment.Center, FontWeight = FontWeights.Bold, Margin = new Thickness(margin) };

                    TextBlock resultTextBlock1 = new TextBlock() { Text = "Positive "};
                    TextBlock resultTextBlock2 = new TextBlock() { Text = "2", FontWeight = FontWeights.Bold, Foreground = Brushes.Green, Margin = new Thickness(0,0,10,0)};
                    //TextBlock resultTextBlock3 = new TextBlock() { Text = "  |  " };
                    Rectangle resultTextBlock3 = new Rectangle() { VerticalAlignment = VerticalAlignment.Stretch,Width = 1, Margin = new Thickness(2), Stroke = Brushes.Black};
                    TextBlock resultTextBlock4 = new TextBlock() { Text = "Negative ", Margin = new Thickness(10, 0, 0, 0) };
                    TextBlock resultTextBlock5 = new TextBlock() { Text = "4", FontWeight = FontWeights.Bold, Foreground = Brushes.Red };
                    StackPanel thirdStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                    thirdStackPanel.Children.Add(resultTextBlock1);
                    thirdStackPanel.Children.Add(resultTextBlock2);
                    thirdStackPanel.Children.Add(resultTextBlock3);
                    thirdStackPanel.Children.Add(resultTextBlock4);
                    thirdStackPanel.Children.Add(resultTextBlock5);

                    secondStackPanel.Children.Add(childBorder);
                    secondStackPanel.Children.Add(image);
                    secondStackPanel.Children.Add(nameTextBlock);
                    secondStackPanel.Children.Add(thirdStackPanel);

                    parentBorder.Child = secondStackPanel;
                    parentBorder.Background = new SolidColorBrush(Colors.White);
                    DropShadowEffect effect = new DropShadowEffect() { ShadowDepth = 0, Opacity = 0.3, BlurRadius = 15};
                    parentBorder.Effect = effect;

                    mainStackPanel.Children.Add(parentBorder);
                }

                responsiveView.Children.Add(mainStackPanel);
            }
        }

        public void createElementByDevice(List<DeviceModel> deviceList, int totalElementPerRow, int imageHeight, int borderHeight, int margin, int totalRow, bool excess, int remainder)
        {
            for (int i = 0; i < totalRow; i++)
            {
                StackPanel mainStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };

                if (i == (totalRow - 1) && excess) { totalElementPerRow = remainder; }

                for (int j = 0; j < totalElementPerRow; j++)
                {
                    DeviceModel device = deviceList[j];
                    int totalPositive = resultList.Where(x => x.DeviceSerialNo == device.DeviceSerialNo && x.TestResultStatus == "Positive").Count();
                    int totalNegative = resultList.Where(x => x.DeviceSerialNo == device.DeviceSerialNo && x.TestResultStatus == "Negative").Count();

                    Border parentBorder = new Border() { Height = borderHeight, Width = 270, BorderBrush = Brushes.WhiteSmoke, BorderThickness = new Thickness(2), CornerRadius = new CornerRadius(5), Margin = new Thickness(50, 0, 50, 10), Padding = new Thickness(5) };
                    StackPanel secondStackPanel = new StackPanel() { Orientation = Orientation.Vertical };

                    TextBlock statusTextBlock = new TextBlock() { FontSize = 11, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                    statusTextBlock.Foreground = device.status == 2 ? (Brush)new BrushConverter().ConvertFromString("#F28C28") : Brushes.DarkCyan;
                    statusTextBlock.Text = device.status == 2 ? "Busy" : "Ready";
                    Border childBorder = new Border() { Height = 30, Width = 70, HorizontalAlignment = System.Windows.HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, CornerRadius = new CornerRadius(5), Child = statusTextBlock };
                    childBorder.Background = device.status == 2 ? (Brush)new BrushConverter().ConvertFromString("#FAD5A5") : (Brush)new BrushConverter().ConvertFromString("#B5F8E3");

                    Image image = new Image();
                    var uri = new Uri(device.DeviceImagePath);
                    var bitmap = new BitmapImage(uri);
                    image.Source = bitmap;
                    image.Height = imageHeight;
                    image.Margin = new Thickness(margin);

                    TextBlock nameTextBlock = new TextBlock() { Text = device.DeviceName, TextAlignment = TextAlignment.Center, FontWeight = FontWeights.Bold, Margin = new Thickness(margin) };

                    TextBlock resultTextBlock1 = new TextBlock() { Text = "Positive " };
                    TextBlock resultTextBlock2 = new TextBlock() { Text = totalPositive.ToString(), FontWeight = FontWeights.Bold, Foreground = Brushes.Green };
                    TextBlock resultTextBlock3 = new TextBlock() { Text = "  |  " };
                    TextBlock resultTextBlock4 = new TextBlock() { Text = "Negative " };
                    TextBlock resultTextBlock5 = new TextBlock() { Text = totalNegative.ToString(), FontWeight = FontWeights.Bold, Foreground = Brushes.Red };
                    StackPanel thirdStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                    thirdStackPanel.Children.Add(resultTextBlock1);
                    thirdStackPanel.Children.Add(resultTextBlock2);
                    thirdStackPanel.Children.Add(resultTextBlock3);
                    thirdStackPanel.Children.Add(resultTextBlock4);
                    thirdStackPanel.Children.Add(resultTextBlock5);

                    secondStackPanel.Children.Add(childBorder);
                    secondStackPanel.Children.Add(image);
                    secondStackPanel.Children.Add(nameTextBlock);
                    secondStackPanel.Children.Add(thirdStackPanel);

                    parentBorder.Child = secondStackPanel;
                    parentBorder.Background = new SolidColorBrush(Colors.White);
                    DropShadowEffect effect = new DropShadowEffect() { ShadowDepth = 0, Opacity = 0.3 };
                    parentBorder.Effect = effect;

                    mainStackPanel.Children.Add(parentBorder);
                }

                responsiveView.Children.Add(mainStackPanel);
            }
        }

        private void DownloadButton_Clicked(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.bionote.com/software-updates") { UseShellExecute = true });
        }
    }
}
