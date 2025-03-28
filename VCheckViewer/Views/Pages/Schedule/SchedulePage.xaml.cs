using DocumentFormat.OpenXml.Spreadsheet;
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
using VCheckViewer.Views.Pages.Results;

namespace VCheckViewer.Views.Pages.Schedule
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : System.Windows.Controls.Page
    {

        public SchedulePage()
        {
            InitializeComponent();

            this.SizeChanged += MainWindow_SizeChanged;

            LoadScheduledTestList();
            LoadTestResultList();
            GetSummaryStats();

            if (App.SchedulePageNotInitialized)
            {
                App.CancelSchedule += new EventHandler(CancelTestSchedule);
                App.SchedulePageNotInitialized = false;
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            middleScrollViewer.Height = e.NewSize.Height * 0.83;
            rightScrollViewer.Height = e.NewSize.Height * 0.83;
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
                        ScheduleUniqueID = (string.IsNullOrEmpty(t.ScheduleUniqueID)) ? "" : string.Concat(t.ScheduleUniqueID.TakeLast(8)),
                        PatientID = t.PatientID,
                        PatientIDString = Properties.Resources.Schedule_Label_PatientID,
                        UniqueIDString = Properties.Resources.Schedule_Label_UniqueID,
                        InchargePerson = t.InchargePerson,
                        ScheduleTestStatus = t.ScheduleTestStatus,
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
            //List<TestResultExtendedModel> sTestResultList = VCheck.Lib.Data.TestResultsRepository.GetLatestTestResultDetailsList(ConfigSettings.GetConfigurationSettings());
            //if (sTestResultList != null && sTestResultList.Count > 0)
            //{
            //    List<TestResultModelExtended> sUpdateTestResultList = new List<TestResultModelExtended>();
            //    foreach(var t in sTestResultList)
            //    {
            //        sUpdateTestResultList.Add(new TestResultModelExtended
            //        {
            //            ID = t.ID,
            //            TestResultDateTime = t.TestResultDateTime,
            //            TestResultType = t.TestResultType,
            //            OperatorID = t.OperatorID,
            //            PatientID = t.PatientID,
            //            PatientIDString = Properties.Resources.Schedule_Label_PatientID,
            //            InchargePerson = t.InchargePerson,
            //            ObservationStatus = t.ObservationStatus,
            //            TestResultStatus = t.TestResultStatus,
            //            TestResultValue = t.TestResultValue,
            //            TestResultRules = t.TestResultRules,
            //            CreatedDate = t.CreatedDate,
            //            CreatedBy = t.CreatedBy,
            //            UpdatedDate = t.UpdatedDate,
            //            UpdatedBy = t.UpdatedBy
            //        });
            //    }
            //    icTestResult.ItemsSource = sUpdateTestResultList.ToList();

            //    borderTestsCompleted.Visibility = Visibility.Visible;
            //    borderNoTestsCompleted.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    borderTestsCompleted.Visibility = Visibility.Collapsed;
            //    borderNoTestsCompleted.Visibility = Visibility.Visible;
            //}

            List<TestResultModel> sTestResultList = VCheck.Lib.Data.TestResultsRepository.GetLatestTestResultList(ConfigSettings.GetConfigurationSettings());
            if (sTestResultList != null && sTestResultList.Count > 0)
            {
                List<TestResultModelExtended> sUpdateTestResultList = new List<TestResultModelExtended>();
                foreach (var t in sTestResultList)
                {
                    var status = "";
                    if(t.OverallStatus.ToLower() == "abnormal")
                    {
                        status = Properties.Resources.Schedule_Label_Abnormal;
                    }
                    if (t.OverallStatus.ToLower() == "normal")
                    {
                        status = Properties.Resources.Schedule_Label_Normal;
                    }
                    if (t.OverallStatus.ToLower() == "positive")
                    {
                        status = Properties.Resources.Dashboard_Label_Positive;
                    }
                    if (t.OverallStatus.ToLower() == "negative")
                    {
                        status = Properties.Resources.Dashboard_Label_Negative;
                    }

                    sUpdateTestResultList.Add(new TestResultModelExtended
                    {
                        ID = t.ID,
                        TestResultDateTime = t.TestResultDateTime,
                        TestResultType = t.TestResultType,
                        OperatorID = t.OperatorID,
                        PatientID = t.PatientID,
                        PatientIDString = Properties.Resources.Schedule_Label_PatientID,
                        InchargePerson = t.InchargePerson,
                        TestResultStatus = status,
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

        private async void SendToAnalyzer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock schedule = sender as TextBlock;
            App.ScheduleTestInfo = ScheduledTestRepository.GetScheduledTestByID(ConfigSettings.GetConfigurationSettings(), long.Parse(schedule.Tag.ToString()));

            App.MainViewModel.Origin = "SendToAnalyzer";
            App.PopupHandler(null, null);

            //VCheckViewerAPI.HL7MessageSender.Main sendMessage = new VCheckViewerAPI.HL7MessageSender.Main();
            //bool scheduleSent = await sendMessage.SendMessage(info);

            //if (scheduleSent)
            //{

            //}
            //else
            //{

            //}

            //if (info.ScheduleUniqueID.Split("-")[0] == "VCHECKC1")
            //{
            //    VCheckViewerAPI.HL7MessageSender.Main sendMessage = new VCheckViewerAPI.HL7MessageSender.Main();
            //    bool scheduleSent = await sendMessage.SendMessage(info);

            //    if (scheduleSent)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}            
        }

        private async void CancelSchedule_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock schedule = sender as TextBlock;
            App.ScheduleTestInfo = ScheduledTestRepository.GetScheduledTestByID(ConfigSettings.GetConfigurationSettings(), long.Parse(schedule.Tag.ToString()));

            App.ScheduleTestInfo.ScheduleTestStatus = 1;
            App.ScheduleTestInfo.UpdatedBy = App.MainViewModel.CurrentUsers.FullName;
            App.ScheduleTestInfo.UpdatedDate = DateTime.Now;

            App.MainViewModel.Origin = "CancelSchedule";
            App.PopupHandler(e, sender);

        }

        public void CancelTestSchedule(object sender, EventArgs e)
        {
            LoadScheduledTestList();
        }

        private async void ViewReport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock report = sender as TextBlock;
            App.TestResultID = long.Parse(report.Tag.ToString());

            App.GoToViewResultPageHandler(e, sender);
        }
    }

    public class ScheduledTestModelExtended : VCheck.Lib.Data.Models.ScheduledTestModel
    {
        public String? PatientIDString { get; set; }
        public String? UniqueIDString { get; set; }
    }

    public class TestResultModelExtended : VCheck.Lib.Data.Models.TestResultExtendedModel
    {
        public String? PatientIDString { get; set; }
    }
}
