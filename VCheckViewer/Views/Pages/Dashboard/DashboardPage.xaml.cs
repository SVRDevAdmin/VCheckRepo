//using DocumentFormat.OpenXml.Spreadsheet;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using VCheck.Helper;
using VCheck.Interface.API;
using VCheck.Lib.Data;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;
using VCheckViewer.Services;
using VCheckViewer.Views.Pages.Schedule;
using VCheckViewer.Views.Pages.Setting.Device;
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
    public partial class DashboardPage : System.Windows.Controls.Page
    {
        List<DeviceModel> deviceList = DeviceRepository.GetDeviceList(ConfigSettings.GetConfigurationSettings());

        //List<TestResultModel> resultList = TestResultsRepository.GetAllTestResultList(ConfigSettings.GetConfigurationSettings());
        List<TestResultExtendedModel> resultList = TestResultsRepository.GetTestResultByDates(ConfigSettings.GetConfigurationSettings(), DateTime.Now);

        public DashboardPage()
        {
            InitializeComponent();
            //initializeData();
            generateView();

            var message = Properties.Resources.Dashboard_Message_DownloadLatest.Split("<nextline>");

            updateMessage.Text = message[0] + "\r\n" + message[1];

            //testEmail();
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

            //Random rnd = new Random();
            //int totalElement = rnd.Next(0, 10);
            //int totalElement = 8;
            int totalElement = deviceList.Count;
            int imageHeight = 250;
            int borderHeight = 400;
            int borderWidth = 420;
            int margin = 15;
            int totalRow = 1;
            int totalElementPerRow = 3;
            bool excess = false;
            int remainder = 0;

            if(totalElement == 0)
            {
                totalElementPerRow = 0;
            }
            else if (totalElement > 0 && totalElement < 3)
            {
                totalElementPerRow = totalElement;
            }
            else if (totalElement == 3)
            {
                imageHeight = 170;
                borderHeight = 300;
                borderWidth = 290;
                margin = 10;
            }
            else if (totalElement == 4)
            {
                totalRow = 2;
                totalElementPerRow = 2;
                imageHeight = 100;
                borderHeight = 199;
                borderWidth = 290;
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
                borderHeight = 199;
                borderWidth = 290;
                margin = 3;
            }

            //createElement(totalElementPerRow, imageHeight, borderHeight, borderWidth, margin, totalRow, excess, remainder);
            //createElementUsingGrid(totalElementPerRow, imageHeight, borderHeight, borderWidth, margin, totalRow, excess, remainder);
            createElementUsingGridByDevice(totalElementPerRow, imageHeight, borderHeight, borderWidth, margin, totalRow, excess, remainder);
        }

        public void createElement(int totalElementPerRow, int imageHeight, int borderHeight, int borderWidth, int margin, int totalRow, bool excess, int remainder)
        {
            for (int i = 0; i < totalRow; i++)
            {
                StackPanel mainStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, Margin = new Thickness(10) };

                if(totalElementPerRow == 0) { TextBlock textBlock = new TextBlock() { Text = "No device detected", FontSize = 50}; mainStackPanel.Children.Add(textBlock); }

                if (i == (totalRow - 1) && excess) { totalElementPerRow = remainder; }

                for (int j = 0; j < totalElementPerRow; j++)
                {
                    Random rnd = new Random();
                    int randomNumberInRange = rnd.Next(1, 3);
                    int randomNumberImage = rnd.Next(1, 4);
                    int randomPositive = rnd.Next(1, 100);
                    int randomNegative = rnd.Next(1, 100);
                    string devicePath;
                    string deviceName;

                    if(randomNumberImage == 1) { devicePath = "VCheck2400Image2"; deviceName = "VCheck V2400"; }
                    else if (randomNumberImage == 2) { devicePath = "VCheck200Image2"; deviceName = "VCheck V200"; }
                    else { devicePath = "VCheckM10Image"; deviceName = "VCheck M10"; }

                    Border parentBorder = new Border() { Height = borderHeight, Width = borderWidth, CornerRadius = new CornerRadius(5), Margin = new Thickness(25, 0, 25, 0), Padding = new Thickness(5) };
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

                    TextBlock nameTextBlock = new TextBlock() { Text = deviceName, TextAlignment = TextAlignment.Center, FontWeight = FontWeights.Bold, Margin = new Thickness(margin) };

                    TextBlock resultTextBlock1 = new TextBlock() { Text = "Positive  "};
                    TextBlock resultTextBlock2 = new TextBlock() { Text = randomPositive.ToString(), FontWeight = FontWeights.Bold, Foreground = Brushes.Green, TextAlignment = TextAlignment.Center};
                    Border positiveBorder = new Border() { Width = 20, Child = resultTextBlock2, Margin = new Thickness(0, 0, 20, 0)};
                    Rectangle resultSeperator = new Rectangle() { VerticalAlignment = VerticalAlignment.Stretch, Width = 1, Height = 20, Margin = new Thickness(2), Stroke = Brushes.Black};
                    TextBlock resultTextBlock3 = new TextBlock() { Text = "Negative  ", Margin = new Thickness(20, 0, 0, 0) };
                    TextBlock resultTextBlock4 = new TextBlock() { Text = randomNegative.ToString(), FontWeight = FontWeights.Bold, Foreground = Brushes.Red, TextAlignment = TextAlignment.Center };
                    Border negativeBorder = new Border() { Width = 20, Child = resultTextBlock4 };
                    StackPanel thirdStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                    thirdStackPanel.Children.Add(resultTextBlock1);
                    thirdStackPanel.Children.Add(positiveBorder);
                    thirdStackPanel.Children.Add(resultSeperator);
                    thirdStackPanel.Children.Add(resultTextBlock3);
                    thirdStackPanel.Children.Add(negativeBorder);

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

        public void createElementByDevice(int totalElementPerRow, int imageHeight, int borderHeight, int borderWidth, int margin, int totalRow, bool excess, int remainder)
        {
            for (int i = 0; i < totalRow; i++)
            {
                StackPanel mainStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, Margin = new Thickness(10) };

                if (i == (totalRow - 1) && excess) { totalElementPerRow = remainder; }

                for (int j = 0; j < totalElementPerRow; j++)
                {
                    DeviceModel device = deviceList[j];
                    int totalPositive = resultList.Where(x => x.DeviceSerialNo == device.DeviceSerialNo && (x.TestResultStatus == "Positive" || x.TestResultStatus == "Normal")).Count();
                    int totalNegative = resultList.Where(x => x.DeviceSerialNo == device.DeviceSerialNo && (x.TestResultStatus == "Negative" || x.TestResultStatus == "Abnormal")).Count();

                    Border parentBorder = new Border() { Height = borderHeight, Width = borderWidth, CornerRadius = new CornerRadius(5), Margin = new Thickness(25, 0, 25, 0), Padding = new Thickness(5) };
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

                    TextBlock resultTextBlock1 = new TextBlock() { Text = "Positive  " };
                    TextBlock resultTextBlock2 = new TextBlock() { Text = totalPositive.ToString(), FontWeight = FontWeights.Bold, Foreground = Brushes.Green, TextAlignment = TextAlignment.Center };
                    Border positiveBorder = new Border() { Width = 20, Child = resultTextBlock2, Margin = new Thickness(0, 0, 20, 0) };
                    Rectangle resultSeperator = new Rectangle() { VerticalAlignment = VerticalAlignment.Stretch, Width = 1, Height = 20, Margin = new Thickness(2), Stroke = Brushes.Black };
                    TextBlock resultTextBlock3 = new TextBlock() { Text = "Negative  ", Margin = new Thickness(20, 0, 0, 0) };
                    TextBlock resultTextBlock4 = new TextBlock() { Text = totalNegative.ToString(), FontWeight = FontWeights.Bold, Foreground = Brushes.Red, TextAlignment = TextAlignment.Center };
                    Border negativeBorder = new Border() { Width = 20, Child = resultTextBlock4 };
                    StackPanel thirdStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                    thirdStackPanel.Children.Add(resultTextBlock1);
                    thirdStackPanel.Children.Add(positiveBorder);
                    thirdStackPanel.Children.Add(resultSeperator);
                    thirdStackPanel.Children.Add(resultTextBlock3);
                    thirdStackPanel.Children.Add(negativeBorder);

                    secondStackPanel.Children.Add(childBorder);
                    secondStackPanel.Children.Add(image);
                    secondStackPanel.Children.Add(nameTextBlock);
                    secondStackPanel.Children.Add(thirdStackPanel);

                    parentBorder.Child = secondStackPanel;
                    parentBorder.Background = new SolidColorBrush(Colors.White);
                    DropShadowEffect effect = new DropShadowEffect() { ShadowDepth = 0, Opacity = 0.3, BlurRadius = 15 };
                    parentBorder.Effect = effect;

                    mainStackPanel.Children.Add(parentBorder);
                }

                responsiveView.Children.Add(mainStackPanel);
            }
        }

        
        public void createElementUsingGrid(int totalElementPerRow, int imageHeight, int borderHeight, int borderWidth, int margin, int totalRow, bool excess, int remainder)
        {
            for (int i = 0; i < totalRow; i++)
            {
                Grid testGrid = new Grid() { Margin = new Thickness(0,10,0,10)};

                if (totalElementPerRow == 0) { TextBlock textBlock = new TextBlock() { Text = "No device detected", FontSize = 50, TextAlignment = TextAlignment.Center }; testGrid.Children.Add(textBlock); }

                for (int column = 0; column < totalElementPerRow; column++)
                {
                    ColumnDefinition columnDefinition = new ColumnDefinition() { Width = (GridLength)new GridLengthConverter().ConvertFromString("*") };
                    testGrid.ColumnDefinitions.Add(columnDefinition);
                }

                RowDefinition rowDefinition = new RowDefinition() { Height = (GridLength)new GridLengthConverter().ConvertFromString("*") };
                testGrid.RowDefinitions.Add(rowDefinition);

                if (i == (totalRow - 1) && excess) { totalElementPerRow = remainder; }

                for (int j = 0; j < totalElementPerRow; j++)
                {
                    Random rnd = new Random();
                    int randomNumberInRange = rnd.Next(1, 3);
                    int randomNumberImage = rnd.Next(1, 4);
                    int randomPositive = rnd.Next(1, 100);
                    int randomNegative = rnd.Next(1, 100);
                    string devicePath;
                    string deviceName;

                    if (randomNumberImage == 1) { devicePath = "VCheck2400Image2"; deviceName = "VCheck V2400"; }
                    else if (randomNumberImage == 2) { devicePath = "VCheck200Image2"; deviceName = "VCheck V200"; }
                    else { devicePath = "VCheckM10Image"; deviceName = "VCheck M10"; }

                    Border parentBorder = new Border() { Height = borderHeight, Width = borderWidth, CornerRadius = new CornerRadius(7), Margin = new Thickness(10, 0, 10, 0), Padding = new Thickness(5) };
                                        
                    if(totalElementPerRow > 2)
                    {
                        if (j == 0) { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Left; }
                        else if (j == totalElementPerRow - 1) { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Right; }
                        else { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Center; }
                    }
                    else
                    {
                        parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                        //if (totalElementPerRow == 2 && excess) { Grid.SetColumnSpan(parentBorder, 2); }
                        //else if (totalElementPerRow == 1 && excess) { Grid.SetColumnSpan(parentBorder, 3); }

                        if (excess) 
                        {
                            if (j == 0) { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Left; }
                            else { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Center; }
                        }
                    }

                    StackPanel secondStackPanel = new StackPanel() { Orientation = Orientation.Vertical };

                    TextBlock statusTextBlock = new TextBlock() { FontSize = 14, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                    statusTextBlock.Foreground = randomNumberInRange == 2 ? (Brush)new BrushConverter().ConvertFromString("#fa8219") : (Brush)new BrushConverter().ConvertFromString("#16c933");
                    statusTextBlock.Text = randomNumberInRange == 2 ? "Busy" : "Ready";
                    Border childBorder = new Border() { Height = 30, Width = 70, HorizontalAlignment = System.Windows.HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, CornerRadius = new CornerRadius(5), Child = statusTextBlock };
                    childBorder.Background = randomNumberInRange == 2 ? (Brush)new BrushConverter().ConvertFromString("#ffeed1") : (Brush)new BrushConverter().ConvertFromString("#e0ffe5");

                    Image image = new Image();
                    //var uri = new Uri(@"pack://application:,,,/VCheckViewer;component/Content/Images/" + devicePath + ".png");
                    var uri = new Uri(@"pack://application:,,,/VCheckViewer;component\Storage\Device\Img_F200.png");
                    var bitmap = new BitmapImage(uri);
                    image.Source = bitmap;
                    image.Height = imageHeight;
                    image.Margin = new Thickness(margin);

                    TextBlock nameTextBlock = new TextBlock() { Text = deviceName, TextAlignment = TextAlignment.Center, FontWeight = FontWeights.Bold, Margin = new Thickness(margin) };

                    TextBlock resultTextBlock1 = new TextBlock() { Text = "Positive  " };
                    TextBlock resultTextBlock2 = new TextBlock() { Text = randomPositive.ToString(), FontWeight = FontWeights.Bold, Foreground = Brushes.Green, TextAlignment = TextAlignment.Center };
                    Border positiveBorder = new Border() { Width = 20, Child = resultTextBlock2, Margin = new Thickness(0, 0, 20, 0) };
                    Rectangle resultSeperator = new Rectangle() { VerticalAlignment = VerticalAlignment.Stretch, Width = 1, Height = 20, Margin = new Thickness(2), Stroke = Brushes.Black };
                    TextBlock resultTextBlock3 = new TextBlock() { Text = "Negative  ", Margin = new Thickness(20, 0, 0, 0) };
                    TextBlock resultTextBlock4 = new TextBlock() { Text = randomNegative.ToString(), FontWeight = FontWeights.Bold, Foreground = Brushes.Red, TextAlignment = TextAlignment.Center };
                    Border negativeBorder = new Border() { Width = 20, Child = resultTextBlock4 };
                    StackPanel thirdStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                    thirdStackPanel.Children.Add(resultTextBlock1);
                    thirdStackPanel.Children.Add(positiveBorder);
                    thirdStackPanel.Children.Add(resultSeperator);
                    thirdStackPanel.Children.Add(resultTextBlock3);
                    thirdStackPanel.Children.Add(negativeBorder);

                    secondStackPanel.Children.Add(childBorder);
                    secondStackPanel.Children.Add(image);
                    secondStackPanel.Children.Add(nameTextBlock);
                    secondStackPanel.Children.Add(thirdStackPanel);

                    parentBorder.Child = secondStackPanel;
                    parentBorder.Background = new SolidColorBrush(Colors.White);
                    DropShadowEffect effect = new DropShadowEffect() { ShadowDepth = 0, Opacity = 0.3, BlurRadius = 15 };
                    parentBorder.Effect = effect;

                    Grid.SetColumn(parentBorder, j);

                    testGrid.Children.Add(parentBorder);
                }

                responsiveView.Children.Add(testGrid);
            }
        }

        
        public async Task createElementUsingGridByDevice(int totalElementPerRow, int imageHeight, int borderHeight, int borderWidth, int margin, int totalRow, bool excess, int remainder)
        {
            String? sColor = System.Windows.Application.Current.Resources["Themes_FontColor"].ToString();
            String? sFrameColor = System.Windows.Application.Current.Resources["Themes_DashboardAnalyzerFrameBackground"].ToString();
            SolidColorBrush sBrushFontColor = new BrushConverter().ConvertFrom(sColor) as SolidColorBrush;
            SolidColorBrush sBrushFrameColor = new BrushConverter().ConvertFrom(sFrameColor) as SolidColorBrush;
            int currentDevice = 0;

            List<int> twoWayDevices = DeviceRepository.GetTwoWayCommDevice(ConfigSettings.GetConfigurationSettings()).Select(x => x.id).ToList();

            for (int i = 0; i < totalRow; i++)
            {
                Grid testGrid = new Grid() { Margin = new Thickness(0, 10, 0, 10) };

                if (totalElementPerRow == 0) { TextBlock textBlock = new TextBlock() { Text = Properties.Resources.General_Message_NoDevice, FontSize = 50, TextAlignment = TextAlignment.Center, Foreground = sBrushFontColor }; testGrid.Children.Add(textBlock); }

                for (int column = 0; column < totalElementPerRow; column++)
                {
                    ColumnDefinition columnDefinition = new ColumnDefinition() { Width = (GridLength)new GridLengthConverter().ConvertFromString("*") };
                    testGrid.ColumnDefinitions.Add(columnDefinition);
                }

                RowDefinition rowDefinition = new RowDefinition() { Height = (GridLength)new GridLengthConverter().ConvertFromString("*") };
                testGrid.RowDefinitions.Add(rowDefinition);

                if (i == (totalRow - 1) && excess) { totalElementPerRow = remainder; }

                for (int j = 0; j < totalElementPerRow; j++)
                {
                    DeviceModel device = deviceList[currentDevice++];
                    int totalPositive = resultList.Where(x => x.DeviceSerialNo == device.DeviceSerialNo && (x.TestResultStatus == "Positive" || x.TestResultStatus == "Abnormal")).Count();
                    int totalNegative = resultList.Where(x => x.DeviceSerialNo == device.DeviceSerialNo && (x.TestResultStatus == "Negative" || x.TestResultStatus == "Normal")).Count();

                    Border parentBorder = new Border() { Height = borderHeight, Width = borderWidth, CornerRadius = new CornerRadius(7), Margin = new Thickness(10, 0, 10, 0), Padding = new Thickness(5) };

                    if (totalElementPerRow > 2)
                    {
                        if (j == 0) { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Left; }
                        else if (j == totalElementPerRow - 1) { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Right; }
                        else { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Center; }
                    }
                    else
                    {
                        parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                        //if (totalElementPerRow == 2 && excess) { Grid.SetColumnSpan(parentBorder, 2); }
                        //else if (totalElementPerRow == 1 && excess) { Grid.SetColumnSpan(parentBorder, 3); }

                        if (excess)
                        {
                            if (j == 0) { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Left; }
                            else { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Center; }
                        }
                    }

                    StackPanel secondStackPanel = new StackPanel() { Orientation = Orientation.Vertical };

                    bool isReady = false;

                    if (twoWayDevices.Contains(device.id))
                    {
                        DeviceChecker deviceChecker = new DeviceChecker();
                        isReady = await deviceChecker.IsOnline(device.DeviceIPAddress);
                    }
                    else
                    {
                        isReady = device.status != 2 ? true : false;
                    }
                    TextBlock statusTextBlock = new TextBlock() { FontSize = 14, FontWeight = FontWeights.DemiBold, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                    statusTextBlock.Foreground = isReady ? (Brush)new BrushConverter().ConvertFromString("#16c933") : (Brush)new BrushConverter().ConvertFromString("#fa8219");
                    statusTextBlock.Text = isReady ? Properties.Resources.General_Label_Ready : Properties.Resources.General_Label_Busy;
                    Border childBorder = new Border() { Height = 30, Width = 70, HorizontalAlignment = System.Windows.HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, CornerRadius = new CornerRadius(5), Child = statusTextBlock };
                    childBorder.Background = isReady ? (Brush)new BrushConverter().ConvertFromString("#e0ffe5") : (Brush)new BrushConverter().ConvertFromString("#ffeed1");

                    Image image = new Image();
                    //var uri = new Uri(device.DeviceImagePath);
                    var uri = new Uri(@"pack://application:,,,/VCheckViewer;component/" + device.DeviceImagePath);
                    var bitmap = new BitmapImage(uri);
                    image.Source = bitmap;
                    image.Height = imageHeight;
                    image.Margin = new Thickness(margin);

                    TextBlock nameTextBlock = new TextBlock() { Text = device.DeviceName, TextAlignment = TextAlignment.Center, FontWeight = FontWeights.Bold, Margin = new Thickness(margin), Foreground = sBrushFontColor };

                    TextBlock resultTextBlock1 = new TextBlock() { Text = Properties.Resources.Dashboard_Label_Positive + "  ", Foreground = sBrushFontColor };
                    TextBlock resultTextBlock2 = new TextBlock() { Text = totalPositive.ToString(), FontWeight = FontWeights.Bold, Foreground = Brushes.Green, TextAlignment = TextAlignment.Center };
                    Border positiveBorder = new Border() { Width = 20, Child = resultTextBlock2, Margin = new Thickness(0, 0, 20, 0) };
                    Rectangle resultSeperator = new Rectangle() { VerticalAlignment = VerticalAlignment.Stretch, Width = 1, Height = 20, Margin = new Thickness(2), Stroke = Brushes.Black };
                    TextBlock resultTextBlock3 = new TextBlock() { Text = Properties.Resources.Dashboard_Label_Negative + "  ", Margin = new Thickness(20, 0, 0, 0), Foreground = sBrushFontColor };
                    TextBlock resultTextBlock4 = new TextBlock() { Text = totalNegative.ToString(), FontWeight = FontWeights.Bold, Foreground = Brushes.Red, TextAlignment = TextAlignment.Center };
                    Border negativeBorder = new Border() { Width = 20, Child = resultTextBlock4 };
                    StackPanel thirdStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                    thirdStackPanel.Children.Add(resultTextBlock1);
                    thirdStackPanel.Children.Add(positiveBorder);
                    thirdStackPanel.Children.Add(resultSeperator);
                    thirdStackPanel.Children.Add(resultTextBlock3);
                    thirdStackPanel.Children.Add(negativeBorder);

                    secondStackPanel.Children.Add(childBorder);
                    secondStackPanel.Children.Add(image);
                    secondStackPanel.Children.Add(nameTextBlock);
                    secondStackPanel.Children.Add(thirdStackPanel);

                    parentBorder.Child = secondStackPanel;
                    //parentBorder.Background = new SolidColorBrush(Colors.White);
                    parentBorder.Background = sBrushFrameColor;
                    DropShadowEffect effect = new DropShadowEffect() { ShadowDepth = 0, Opacity = 0.3, BlurRadius = 15 };
                    parentBorder.Effect = effect;

                    Grid.SetColumn(parentBorder, j);

                    testGrid.Children.Add(parentBorder);
                }

                responsiveView.Children.Add(testGrid);
            }
        }
        
        void DownloadButton_Clicked(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(App.UpdateLink) { UseShellExecute = true });
        }

        private void testEmail()
        {
            string sErrorMessage = "";
            var body = "\r\n <!DOCTYPE html>\r\n <html>\r\n \t<body>\r\n \t\tDear ###<staff_fullname>###, </br></br>\r\n \r\n \t\tWe are pleased to inform you that your account has been successfully created!</br></br>\r\n \r\n \t\tBelow are your login details:</br>\r\n \t\tLogin ID: ###<login_id>###</br>\r\n \t\tTemporary Password: ###<password>###</br></br>\r\n \r\n \t\tPlease refrain from replying to this email as it is auto-generated.</br></br>\r\n \r\n \t\tBest regards,</br>\r\n \t\tVCheck Viewer Team</br>\r\n \t</body>\r\n </html>\r\n ";

            EmailObject sEmail = new EmailObject();

            sEmail.SenderEmail = App.SMTP.Sender;

            List<string> sRecipientList = ["azwan.masri.retes@gmail.com", "azwan@svrtech.com.my"];


            sEmail.RecipientEmail = sRecipientList;
            sEmail.IsHtml = true;
            sEmail.Subject = "[VCheck Viewer] Test Email";
            sEmail.Body = body;
            sEmail.SMTPHost = App.SMTP.Host;
            sEmail.PortNo = App.SMTP.Port;
            sEmail.HostUsername = App.SMTP.Username;
            sEmail.HostPassword = App.SMTP.Password;
            sEmail.EnableSsl = true;
            sEmail.UseDefaultCredentials = false;

            EmailHelper.SendEmail(sEmail, out sErrorMessage);
        }

        // ------------ Temporary BEGIN --------------//
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    String sID = "66:a327e327-7174-11ef-84e8-0d99d20d5b74";

        //    var sAPI = new VCheck.Interface.API.openvpmsAPI();

        //    var sResult = sAPI.RetrieveBooking(sID);
        //    if (sResult != null)
        //    {
        //        if (sResult != null)
        //        {
        //            String sMessage = "Retrieve Appointment record from API completed.";

        //            VCheck.Lib.Data.Models.ScheduledTestModel sTestModel = new ScheduledTestModel();

        //            sTestModel.ScheduledDateTime = DateTime.ParseExact(sResult.start, "yyyy-MM-ddTHH:mm:ss.fffzzz", System.Globalization.CultureInfo.InvariantCulture);
        //            sTestModel.ScheduleTestStatus = 0;
        //            sTestModel.CreatedDate = DateTime.Now;
        //            sTestModel.CreatedBy = "SYSTEM";

        //            if (ScheduledTestRepository.InsertScheduledTest(ConfigSettings.GetConfigurationSettings(), sTestModel))
        //            {
        //                //System.Windows.Forms.MessageBox.Show(sMessage);
        //            }
        //            else
        //            {
        //                System.Windows.Forms.MessageBox.Show("Get Appointment record from API Failed.");
        //            }                
        //        }
        //    }
        //    else
        //    {
        //        String abc = "xxx";
        //    }
        //}

        //private void btnSubmitAppt_Click(object sender, RoutedEventArgs e)
        //{
        //    popupAppt.IsOpen = true;
        //}

        //private void btnSubmit_Click(object sender, RoutedEventArgs e)
        //{
        //    var sAPI = new VCheck.Interface.API.openvpmsAPI();
          
        //    System.Windows.Controls.ComboBoxItem cbStart = (System.Windows.Controls.ComboBoxItem)cboStart.SelectedValue;
        //    System.Windows.Controls.ComboBoxItem cbEnd = (System.Windows.Controls.ComboBoxItem)cboEnd.SelectedValue;

        //    DateTime dtApptdate = DateTime.ParseExact(dtAppt.Text, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //    String sStart = cbStart.Content.ToString();
        //    String sEnd = cbEnd.Content.ToString();

        //    String sReqStart = dtApptdate.ToString("yyyy-MM-ddT") + sStart + ":00.000+08:00";
        //    String sReqEnd = dtApptdate.ToString("yyyy-MM-ddT") + sEnd + ":00.000+08:00";

        //    VCheck.Interface.API.openvpms.RequestMessage.SubmitBookingRequest sReq = new VCheck.Interface.API.openvpms.RequestMessage.SubmitBookingRequest();
        //    sReq.location = "17";
        //    sReq.schedule = "21";
        //    sReq.appointmentType = "7";
        //    sReq.start = sReqStart;
        //    sReq.end = sReqEnd;
        //    sReq.firstName = txtFirstName.Text;
        //    sReq.lastName = txtLastName.Text;
        //    sReq.patientName = txtPatient.Text;
        //    sReq.title = "mr.";
        //    sReq.mobile = "01232342434";
        //    sReq.user = "89";

        //    if (sAPI.SubmitBooking(sReq))
        //    {
        //        System.Windows.Forms.MessageBox.Show("Submit Booking successfully.");
        //    }
        //    else
        //    {
        //        System.Windows.Forms.MessageBox.Show("Submit Booking Failed.");
        //    }
        //}

        //-------------- Temporary END ------------------//
    }
}
