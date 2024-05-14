using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using VCheckViewer.Properties;

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
                List<ScheduledTestModelExtended> sUpdateScheduledList = new List<ScheduledTestModelExtended>();
                foreach(var t in sScheduledList)
                {
                    sUpdateScheduledList.Add(new ScheduledTestModelExtended
                    {
                        ID = t.ID,
                        ScheduledTestType = t.ScheduledTestType,
                        ScheduledDateTime = t.ScheduledDateTime,
                        ScheduledBy = t.ScheduledBy,
                        PatientID = t.PatientID,
                        PatientIDString = Properties.Resources.Schedule_Label_PatientID,
                        InchargePerson = t.InchargePerson,
                        TestCompleted = t.TestCompleted,
                        CreatedDate = t.CreatedDate,
                        CreatedBy = t.CreatedBy,
                        UpdatedDate = t.UpdatedDate,
                        UpdatedBy = t.UpdatedBy
                    });
                }

                icScheduledTest.ItemsSource = sUpdateScheduledList.ToList();

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
                List<TestResultModelExtended> sUpdateTestResultList = new List<TestResultModelExtended>();
                foreach(var t in sTestResultList)
                {
                    sUpdateTestResultList.Add(new TestResultModelExtended
                    {
                        ID = t.ID,
                        TestResultDateTime = t.TestResultDateTime,
                        TestResultType = t.TestResultType,
                        OperatorID = t.OperatorID,
                        PatientID = t.PatientID,
                        PatientIDString = Properties.Resources.Schedule_Label_PatientID,
                        InchargePerson = t.InchargePerson,
                        ObservationStatus = t.ObservationStatus,
                        TestResultStatus = t.TestResultStatus,
                        TestResultValue = t.TestResultValue,
                        TestResultRules = t.TestResultRules,
                        CreatedDate = t.CreatedDate,
                        CreatedBy = t.CreatedBy,
                        UpdatedDate = t.UpdatedDate,
                        UpdatedBy = t.UpdatedBy
                    });
                }
                icTestResult.ItemsSource = sUpdateTestResultList.ToList();

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

    public class ScheduledTestModelExtended : VCheck.Lib.Data.Models.ScheduledTestModel
    {
        public String? PatientIDString { get; set; }
    }

    public class TestResultModelExtended : VCheck.Lib.Data.Models.TestResultModel
    {
        public String? PatientIDString { get; set; }
    }
}
