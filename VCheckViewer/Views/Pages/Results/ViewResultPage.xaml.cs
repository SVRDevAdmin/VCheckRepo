using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using VCheck.Lib.Data;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;
using VCheckViewer.Services;

namespace VCheckViewer.Views.Pages.Results
{
    /// <summary>
    /// Interaction logic for ViewResultPage.xaml
    /// </summary>
    public partial class ViewResultPage : Page
    {
        public ConfigurationDBContext configDBContext = new ConfigurationDBContext(ConfigSettings.GetConfigurationSettings());

        public ViewResultPage()
        {
            InitializeComponent();
            LoadReportInfo();
            LoadResultData();
            LoadResultGraph();

            this.SizeChanged += MainWindow_SizeChanged;

            BackButton.DataContext = App.MainViewModel;
            App.MainViewModel.BackButtonText = Properties.Resources.Setting_Label_BackButton;

            PatientIDLabel.Text = Properties.Resources.Results_Label_PatientID + ":";
            PatientNameLabel.Text = Properties.Resources.Results_Label_PatientName + ":";
            DateTimeLabel.Text = Properties.Resources.Report_Label_DateTime + ":";
        }

        private void LoadReportInfo()
        {
            TestResultModel testInfo = TestResultsRepository.GetTestResultByID(ConfigSettings.GetConfigurationSettings(), App.TestResultID);
            var sReportImagePath = configDBContext.GetConfigurationData("ReportImagePath").FirstOrDefault();
            var sClinicName = configDBContext.GetConfigurationData("ClinicName").FirstOrDefault();
            var sReportTitle = configDBContext.GetConfigurationData("ReportTitle").FirstOrDefault();
            var sClinicAddress = configDBContext.GetConfigurationData("ClinicAddress").FirstOrDefault();

            try
            {
                var bitmap = new BitmapImage();
                Uri uri = new Uri(sReportImagePath != null ? sReportImagePath.ConfigurationValue : "pack://application:,,,/Content/Images/bio_logo_report.png");
                bitmap = new BitmapImage(uri);

                ViewReportLogo.Source = bitmap;
            }
            catch (Exception ex)
            {

            }

            ClinicName.Text = sClinicName != null ? sClinicName.ConfigurationValue : "";
            ReportTitle.Text = sReportTitle != null ? sReportTitle.ConfigurationValue : "";
            ClinicAddress.Text = sClinicAddress != null ? sClinicAddress.ConfigurationValue : "";

            PatientID.Text = testInfo.PatientID;
            PatientName.Text = testInfo.PatientName;
            TestDate.Text = testInfo.TestResultDateTime.Value.ToString("dd/MM/yyyy hh:mm");
        }

        private void LoadResultData()
        {
            var parameterOrder = TestResultsRepository.GetAllParameters(ConfigSettings.GetConfigurationSettings()).Select(x => x.Parameter).ToList();
            List<TestResultDetailsModel> sTestResultDetails = TestResultsRepository.GetResultDetailsByTestResultID(ConfigSettings.GetConfigurationSettings(), App.TestResultID).OrderBy(d => parameterOrder.IndexOf(d.TestParameter)).ToList();
            List<TestResultDetailsExtension> sTestResultDetailsExtension = new List<TestResultDetailsExtension>();


            foreach (var testDetail in sTestResultDetails)
            {
                var firstLayer = 0;
                var secondLayer = 0;
                var barVisibility = "Visible";

                if (!string.IsNullOrEmpty(testDetail.ReferenceRange) && !string.IsNullOrEmpty(testDetail.TestResultValue) && !testDetail.TestResultValue.Contains("-"))
                {
                    barVisibility = "Collapsed";
                    var range = testDetail.ReferenceRange.Replace("[", "").Replace("]", "").Contains(";") ? testDetail.ReferenceRange.Split(";") : testDetail.ReferenceRange.Split("-");
                    firstLayer = CalculateReference(float.Parse(range[0].Replace("[", "")), float.Parse(range[1].Replace("]", "")), float.Parse(testDetail.TestResultValue.Replace("< ", "").Replace("> ", "")));
                    secondLayer = firstLayer - 1;
                }


                TestResultDetailsExtension testDetailExtension = new TestResultDetailsExtension() { 
                    ID = testDetail.ID,
                    TestResultValue = testDetail.TestResultValue + " " + testDetail.TestResultUnit,
                    TestParameter = testDetail.TestParameter, 
                    TestResultStatus = testDetail.TestResultStatus, 
                    FirstLayerReference = firstLayer == 0 ? 1 : firstLayer, 
                    SecondLayerReference = firstLayer == 0 ? 0 : secondLayer, 
                    BarVisibility = barVisibility
                };

                sTestResultDetailsExtension.Add(testDetailExtension);
            }

            dgResultDetails.ItemsSource = sTestResultDetailsExtension;
        }

        public void LoadResultGraph()
        {
            List<TestResultGraphModel> TestResultsGraph = TestResultsRepository.GetResultGraphsByTestResultID(ConfigSettings.GetConfigurationSettings(), App.TestResultID);

            int totalElement = TestResultsGraph.Count;
            int imageHeight = 250;
            int borderHeight = 400;
            int borderWidth = 420;
            int margin = 15;
            int totalRow = 1;
            int totalElementPerRow = 3;
            bool excess = false;
            int remainder = 0;

            if (totalElement == 0)
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

            createElementUsingGridByGraph(TestResultsGraph, totalElementPerRow, imageHeight, borderHeight, borderWidth, margin, totalRow, excess, remainder);
        }

        public async Task createElementUsingGridByGraph(List<TestResultGraphModel> graphs, int totalElementPerRow, int imageHeight, int borderHeight, int borderWidth, int margin, int totalRow, bool excess, int remainder)
        {
            string graphFolder = Host.CreateApplicationBuilder().Configuration.GetSection("Configuration:GraphFolder").Value;
            String? sColor = System.Windows.Application.Current.Resources["Themes_FontColor"].ToString();
            String? sFrameColor = System.Windows.Application.Current.Resources["Themes_DashboardAnalyzerFrameBackground"].ToString();
            SolidColorBrush sBrushFontColor = new BrushConverter().ConvertFrom(sColor) as SolidColorBrush;
            SolidColorBrush sBrushFrameColor = new BrushConverter().ConvertFrom(sFrameColor) as SolidColorBrush;
            int currentGraph = 0;

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
                    TestResultGraphModel graph = graphs[currentGraph++];

                    Border parentBorder = new Border() { CornerRadius = new CornerRadius(7), Margin = new Thickness(10, 0, 10, 0), Padding = new Thickness(5) };

                    if (totalElementPerRow > 2)
                    {
                        if (j == 0) { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Left; }
                        else if (j == totalElementPerRow - 1) { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Right; }
                        else { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Center; }
                    }
                    else
                    {
                        parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                        if (excess)
                        {
                            if (j == 0) { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Left; }
                            else { parentBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Center; }
                        }
                    }

                    StackPanel secondStackPanel = new StackPanel() { Orientation = System.Windows.Controls.Orientation.Vertical };

                    bool isReady = false;
                    bool resultRequired = false;

                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    var uri = new Uri(graphFolder + graph.TestResultRowID + "\\" + graph.FileName);
                    var bitmap = new BitmapImage(uri);
                    image.Source = bitmap;
                    image.Height = borderHeight;
                    image.Margin = new Thickness(margin);


                    secondStackPanel.Children.Add(image);

                    parentBorder.Child = secondStackPanel;
                    parentBorder.Background = sBrushFrameColor;
                    DropShadowEffect effect = new DropShadowEffect() { ShadowDepth = 0, Opacity = 0.3, BlurRadius = 15 };
                    parentBorder.Effect = effect;

                    Grid.SetColumn(parentBorder, j);

                    testGrid.Children.Add(parentBorder);
                }

                if (i == 0) { responsiveView.Children.Clear(); }
                responsiveView.Children.Add(testGrid);
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var windowHeight = App.WindowHeight;

            dgResultDetails.MaxHeight = windowHeight * 0.48;
            ViewReportLogo.Height = e.NewSize.Height * 0.2;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.GoPreviousPageHandler(e, sender);
        }

        public int CalculateReference(float start, float end, float value)
        {
            float range = end - start;
            float actualStart = start - range;
            float actualEnd = end + range;
            float percentage = 0;

            float onceWidth = (float)33.3333;
            float twiceWidth = (float)66.6666;
            float fullwidth = 100;

            if (value > start && end > value)
            {
                float balance = value - start;
                percentage = (balance / range) * onceWidth + onceWidth;
            }
            else if (value < start && actualStart < value)
            {
                float balance = value - actualStart;
                percentage = (balance / range) * onceWidth;
            }
            else if (value > end && actualEnd > value)
            {
                float balance = value - end;
                percentage = (balance / range) * onceWidth + twiceWidth;
            }
            else if (value > actualEnd)
            {
                percentage = fullwidth;
            }

            return Convert.ToInt32(percentage);
        }
    }

    public class TestResultDetailsExtension : TestResultDetailsModel
    {
        public int FirstLayerReference { get; set; }
        public int SecondLayerReference { get; set; }
        public string BarVisibility { get; set; }
    }
}
