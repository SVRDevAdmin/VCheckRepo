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
using VCheck.Lib.Data;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;

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

            this.SizeChanged += MainWindow_SizeChanged;

            BackButton.DataContext = App.MainViewModel;
            App.MainViewModel.BackButtonText = "Back";

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
            //var parameterOrder = App.iConfig.GetSection("Configuration:ParameterOrder").Value.Split(",").ToList();
            var parameterOrder = TestResultsRepository.GetAllParameters(ConfigSettings.GetConfigurationSettings()).Select(x => x.Parameter).ToList();
            List<TestResultDetailsModel> sTestResultDetails = TestResultsRepository.GetResultDetailsByTestResultID(ConfigSettings.GetConfigurationSettings(), App.TestResultID).OrderBy(d => parameterOrder.IndexOf(d.TestParameter)).ToList();

            foreach (var testDetail in sTestResultDetails)
            {
                testDetail.TestResultValue = testDetail.TestResultValue + " " + testDetail.TestResultUnit;
            }

            dgResultDetails.ItemsSource = sTestResultDetails;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            dgResultDetails.MaxHeight = e.NewSize.Height * 0.65;
            ViewReportLogo.Height = e.NewSize.Height * 0.2;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.GoPreviousPageHandler(e, sender);
        }
    }
}
