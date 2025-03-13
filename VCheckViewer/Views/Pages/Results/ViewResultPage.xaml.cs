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
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;

namespace VCheckViewer.Views.Pages.Results
{
    /// <summary>
    /// Interaction logic for ViewResultPage.xaml
    /// </summary>
    public partial class ViewResultPage : Page
    {
        public ViewResultPage()
        {
            InitializeComponent();
            LoadResultData();

            this.SizeChanged += MainWindow_SizeChanged;

            BackButton.DataContext = App.MainViewModel;
            App.MainViewModel.BackButtonText = "Back";
        }

        private void LoadResultData()
        {
            List<TestResultDetailsModel> sTestResultDetails = TestResultsRepository.GetResultDetailsByTestResultID(ConfigSettings.GetConfigurationSettings(), App.TestResultID);

            //List<TestResultDetailsModel> sTestResultDetails = new List<TestResultDetailsModel>();
            //sTestResultDetails.Add(new TestResultDetailsModel() { TestParameter = "A/G", TestResultStatus = "Negative", TestResultValue = "-" });
            //sTestResultDetails.Add(new TestResultDetailsModel() { TestParameter = "BUN/CREA", TestResultStatus = "Negative", TestResultValue = "-" });
            //sTestResultDetails.Add(new TestResultDetailsModel() { TestParameter = "GLOB", TestResultStatus = "Negative", TestResultValue = "- g/dl" });
            //sTestResultDetails.Add(new TestResultDetailsModel() { TestParameter = "AMY", TestResultStatus = "Positive", TestResultValue = "< 5 U/L" });

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
