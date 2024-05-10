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

namespace VCheckViewer.Views.Pages.Schedule
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            InitializeComponent();

            LoadScheduledTestList();
            LoadTestResultList();
            GetSummaryStats();
        }

        public void LoadScheduledTestList()
        {
            List<ScheduledTestModel> sScheduledList = VCheck.Lib.Data.ScheduledTestRepository.GetCurrentScheduledTestList(ConfigSettings.GetConfigurationSettings());
            if (sScheduledList != null && sScheduledList.Count > 0)
            {
                icScheduledTest.ItemsSource = sScheduledList.ToList();

                borderScheduledTest.Visibility = Visibility.Visible;
                borderNoScheduledTest.Visibility = Visibility.Collapsed;
            }
            else
            {
                borderScheduledTest.Visibility = Visibility.Collapsed;
                borderNoScheduledTest.Visibility = Visibility.Visible;
            }
        }

        public void LoadTestResultList()
        {
            List<TestResultModel> sTestResultList = VCheck.Lib.Data.TestResultsRepository.GetLatestTestResultList(ConfigSettings.GetConfigurationSettings());
            if (sTestResultList != null && sTestResultList.Count > 0)
            {
                icTestResult.ItemsSource = sTestResultList.ToList();

                borderTestsCompleted.Visibility = Visibility.Visible;
                borderNoTestsCompleted.Visibility = Visibility.Collapsed;
            }
            else
            {
                borderTestsCompleted.Visibility = Visibility.Collapsed;
                borderNoTestsCompleted.Visibility = Visibility.Visible;
            }
        }

        public void GetSummaryStats()
        {
            var sTotalTestCompleted = VCheck.Lib.Data.TestResultsRepository.GetTodayTestResultList(ConfigSettings.GetConfigurationSettings());
            if (sTotalTestCompleted != null && sTotalTestCompleted.Count > 0)
            {
                lbTotalTest.Text = sTotalTestCompleted.Count().ToString();
                lbTotalPatients.Text = sTotalTestCompleted.GroupBy(x => x.PatientID).Count().ToString();
            }
            else
            {
                lbTotalTest.Text = "0";
                lbTotalPatients.Text = "0";
            }
        }
    }
}
