using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VCheckViewer.Lib.Function;
using ComboBox = System.Windows.Controls.ComboBox;
using Brushes = System.Windows.Media.Brushes;
using Button = System.Windows.Controls.Button;
using TextBlock = System.Windows.Controls.TextBlock;
using ZstdSharp.Unsafe;
using VCheckViewer.Views.Pages.Schedule;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.DocumentTemplate;
using QuestPDF.Fluent;
using Pomelo.EntityFrameworkCore.MySql.Query.Internal;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using System.Diagnostics;
using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel;
using Spire.Xls;
using Microsoft.Extensions.Logging;
using VCheckViewer.UserControls;
using System.Management;
using System.Globalization;
using DatePicker = System.Windows.Controls.DatePicker;
using VCheck.Interface.API.Greywind.RequestMessage;
using VCheck.Lib.Data;
using System.Drawing.Printing;
using VCheckViewer.Views.Windows;

namespace VCheckViewer.Views.Pages.Results
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ResultPage : System.Windows.Controls.Page
    {
        public ObservableCollection<ComboBoxItem> cbSort { get; set; }
        public ComboBoxItem SelectedcbSort { get; set;  }

        public int currentPage = 1;
        public int paginationSize = 10;
        public int startPagination = 1;
        public int endPagination = 0;

        public static event EventHandler? GoToViewResultPage;

        public ResultPage()
        {
            InitializeComponent();
            DataContext = this;

            cbSort = App.MainViewModel.cbSort;
            SelectedcbSort = cbSort.FirstOrDefault();

            pagination.ButtonNextControlClick += new EventHandler(PaginationNextButton_Click);
            pagination.ButtonPrevControlClick += new EventHandler(PaginationPrevButton_Click);
            pagination.ButtonPageControlClick += new EventHandler(PaginationNumButton_Click);

            LoadResultDataGrid();
        }

        private void menuView_Click(object sender, RoutedEventArgs e)
        {
            TestResultListingExtendedObj sTestResultObj = dgResult.SelectedItem as TestResultListingExtendedObj;
            App.TestResultID = sTestResultObj.ID;
            GoToViewResultPageHandler(e, sender);
        }

        private void menuDownload_Click(object sender, RoutedEventArgs e)
        {
            LoadTestResultData();
        }

        private void menuPrint_Click(object sender, RoutedEventArgs e)
        {
            LoadTestResultData(true);
        }

        private void LoadTestResultData(Boolean isPrint = false)
        {
            //TestResultListingObj sTestResultObj = dgResult.SelectedItem as TestResultListingObj;
            TestResultListingExtendedObj sTestResultObj = dgResult.SelectedItem as TestResultListingExtendedObj;
            sTestResultObj.printedBy = App.MainViewModel.CurrentUsers.FullName;
            sTestResultObj.printedOn = DateTime.Now;
            sTestResultObj.isPrint = isPrint;

            List<TestResultDetailsModel> sTestResultDetails = new List<TestResultDetailsModel>();
            if (sTestResultObj != null)
            {
                sTestResultDetails = TestResultsRepository.GetResultDetailsByTestResultID(ConfigSettings.GetConfigurationSettings(), sTestResultObj.ID);
            }

            List<string> parameters = sTestResultDetails.Select(x => x.TestParameter).ToList();
            App.Parameters = parameters;

            try
            {
                TestResultTemplate sTestResultTemplate = new TestResultTemplate(sTestResultObj, sTestResultDetails);

                if (isPrint)
                {
                    sTestResultTemplate.GeneratePdfAndShow();
                }
                else
                {
                    sTestResultTemplate.GeneratePdf();
                }              
            }
            catch (QuestPDF.Drawing.Exceptions.DocumentDrawingException drawEx)
            {
                Debug.WriteLine(drawEx.ToString());
            }
            catch (Exception ex)
            {
                return;
            }

            if (!isPrint)
            {
                Popup sConfirmPopup = new Popup();
                sConfirmPopup.IsOpen = true;

                App.MainViewModel.Origin = "TestResultDownloadCompleted";
                App.PopupHandler(null, null);
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            startPagination = 1;

            LoadResultDataGrid();
        }

        public void LoadResultDataGrid()
        {
            int iTotalRecord = 0;
            dgResult.ItemsSource = GetTestResultBySearch(paginationSize, out iTotalRecord);

            if (iTotalRecord <= 0)
            {
                btnPrint.Visibility = Visibility.Collapsed;
                btnDownload.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnPrint.Visibility = Visibility.Visible;
                btnDownload.Visibility = Visibility.Visible;
            }

            txtTotalCount.Text = iTotalRecord.ToString();

            pagination.iTotalRecords = iTotalRecord;
            pagination.iPageIndex = startPagination;
            pagination.iPageSize = paginationSize;
            pagination.LoadPagingNumber();
        }

        public List<TestResultListingExtendedObj> GetTestResultBySearch(int pageSize, out int iTotalRecord)
        {
            String sSortDirection = "DESC";
            if (cboSort.SelectedItem != null)
            {
                var sSortObjItem = (ComboBoxItem)cboSort.SelectedItem;
                sSortDirection = sSortObjItem.Tag.ToString();
            }

            String sStart = "";
            String sEnd = "";

            if (RangeDate.SelectedDates.Count > 1)
            {
                sStart = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd HH:mm:ss");
                sEnd = RangeDate.SelectedDates.LastOrDefault().AddDays(1).AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (RangeDate.SelectedDates.Count == 1)
            {
                sStart = RangeDate.SelectedDates.FirstOrDefault().ToString("yyyy-MM-dd HH:mm:ss");
                sEnd = RangeDate.SelectedDates.FirstOrDefault().AddDays(1).AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");
            }
            //else if (RangeDate.SelectedDates.Count == 0)
            //{
            //    sStart = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    sEnd = DateTime.Now.AddDays(1).AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");

            //    RangeDate.DisplayValue = (DateTime.ParseExact(sStart, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).ToString("dd/MM/yyyy") + " - " +
            //                             (DateTime.ParseExact(sEnd, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).ToString("dd/MM/yyyy");
            //}

            String sKeyword = KeywordSearchBar.Text;

            return VCheck.Lib.Data.TestResultsRepository.GetTestResultListBySearch(
                                            ConfigSettings.GetConfigurationSettings(),
                                            sStart, sEnd, sKeyword, sSortDirection,
                                            startPagination, pageSize, out iTotalRecord);
        }

        protected void PaginationNextButton_Click(object sender, EventArgs e)
        {
            startPagination++;

            LoadResultDataGrid();
        }

        protected void PaginationPrevButton_Click(object sender, EventArgs e)
        {
            startPagination--;

            LoadResultDataGrid();
        }

        protected void PaginationNumButton_Click(object sender, EventArgs e)
        {
            System.Windows.Controls.Button btnNum = sender as System.Windows.Controls.Button;
            int iNumberSelected = Convert.ToInt32(btnNum.Tag);

            startPagination = iNumberSelected;

            LoadResultDataGrid();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            CloseCalenderPopup(null, null);
            DownloadSearchResult();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            CloseCalenderPopup(null, null);
            DownloadSearchResult(true);
        }

        private void DownloadSearchResult(Boolean isPrint = false)
        {
            var sBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            String sDownloadPath = sBuilder.Configuration.GetSection("Configuration:DownloadFolderPath").Value;
            String sProcessingPath = System.IO.Path.Combine(sDownloadPath, "Processing");

            if (!System.IO.Path.Exists(sProcessingPath))
            {
                System.IO.Directory.CreateDirectory(sProcessingPath);
            }

            int iTotalCount = 0;
            int iPageSize = 0;

            try
            {
                String sFileName = "TestResultsListing_Download_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                String sExcelOutputPath = System.IO.Path.Combine(sProcessingPath, sFileName + ".xlsx"); 
                String sPDFOutputPath = System.IO.Path.Combine(sDownloadPath, sFileName + ".pdf");

                int.TryParse(txtTotalCount.Text, out iPageSize);

                List<TestResultListingExtendedObj> sResult = new List<TestResultListingExtendedObj>();
                sResult = GetTestResultBySearch(iPageSize, out iTotalCount);

                DataTable sDatatable = Lib.General.Utils.ToDataTable(sResult);
                sDatatable.Columns.Remove("TestResultValue");
                sDatatable.Columns.Remove("TestResultDateTime");
                sDatatable.Columns.Remove("statusBackground");
                sDatatable.Columns.Remove("statusFontColor");
                sDatatable.Columns.Remove("printedBy");
                sDatatable.Columns.Remove("printedOn");
                sDatatable.Columns.Remove("isPrint");
                sDatatable.Columns.Remove("ID");
                sDatatable.Columns.Remove("ObservationStatus");
                sDatatable.Columns.Remove("TestResultValueString");
                sDatatable.Columns.Remove("TestResultRules");
                sDatatable.Columns.Remove("CreatedDate");
                sDatatable.Columns.Remove("CreatedBy");
                sDatatable.Columns.Remove("UpdatedDate");
                sDatatable.Columns.Remove("UpdatedBy");


                sDatatable.Columns["RowNo"].SetOrdinal(0);
                sDatatable.Columns["RowNo"].ColumnName = "No.";
                sDatatable.Columns["TestResultDateTimeString"].SetOrdinal(1);
                sDatatable.Columns["TestResultDateTimeString"].ColumnName = "Observation DateTime";
                sDatatable.Columns["OperatorID"].SetOrdinal(2);
                sDatatable.Columns["OperatorID"].ColumnName = "Operator ID";
                sDatatable.Columns["PatientID"].SetOrdinal(3);
                sDatatable.Columns["PatientID"].ColumnName = "Patient ID";
                sDatatable.Columns["InchargePerson"].SetOrdinal(4);
                sDatatable.Columns["InchargePerson"].ColumnName = "Doctor";
                sDatatable.Columns["TestResultType"].SetOrdinal(5);
                sDatatable.Columns["TestResultType"].ColumnName = "Observation Type";
                sDatatable.Columns["TestResultStatus"].SetOrdinal(6);
                sDatatable.Columns["TestResultStatus"].ColumnName = "Overall Status";
                sDatatable.Columns["DeviceSerialNo"].SetOrdinal(7);
                sDatatable.Columns["DeviceSerialNo"].ColumnName = "Device Serial No";
                
                               
                // ----- Generate Excel file ------- //
                XLWorkbook workbook = new XLWorkbook();
                workbook.Worksheets.Add(sDatatable, "Test Results")
                                   .Columns().AdjustToContents();
                workbook.SaveAs(sExcelOutputPath);

                // --- Output to PDF ----- //
                Spire.Xls.Workbook spireWorkbook = new Spire.Xls.Workbook();
                spireWorkbook.LoadFromFile(sExcelOutputPath);
                spireWorkbook.Worksheets[0].PageSetup.Orientation = PageOrientationType.Landscape;
                spireWorkbook.Worksheets[0].PageSetup.IsFitToPage = true;

                spireWorkbook.SaveToFile(sPDFOutputPath, Spire.Xls.FileFormat.PDF);
                if (System.IO.File.Exists(sPDFOutputPath))
                {
                    System.IO.File.Delete(sExcelOutputPath);
                }

                if (isPrint)
                {
                    // --- Prompt Print Dialog --- //
                    ProcessStartInfo infoPrint = new ProcessStartInfo();
                    infoPrint.FileName = sPDFOutputPath;
                    infoPrint.Verb = "PrintTo";
                    infoPrint.CreateNoWindow = false;
                    infoPrint.WindowStyle = ProcessWindowStyle.Normal;
                    infoPrint.UseShellExecute = true;

                    Process printProcess = new Process();
                    printProcess = Process.Start(infoPrint);
                }
                else
                {
                    Popup sConfirmPopup = new Popup();
                    sConfirmPopup.IsOpen = true;

                    App.MainViewModel.Origin = "ListingDownloadCompleted";
                    App.PopupHandler(null, null);
                }
            }
            catch (Exception ex) { 
                Popup sNotificationPopup = new Popup();
                sNotificationPopup.IsOpen = true;

                App.MainViewModel.Origin = (!isPrint) ? "FailedDownloadListing" : "FailedToShowPrint";
                App.PopupHandler(null, null);
            }
        }

        private void ResultKeywordSearch(object sender, System.Windows.Input.KeyEventArgs e)
        {
            CloseCalenderPopup(null, null);
        }

        private void ChangeSortBy(object sender, RoutedEventArgs e)
        {
            CloseCalenderPopup(null, null);
        }

        private void CloseCalenderPopup(object? sender, MouseButtonEventArgs? e)
        {
            RangeDate.DateRangePicker_Popup.IsOpen = false;
        }

        //---------- POC Testing -----------------//
        private void menuTransfer_Click(object sender, RoutedEventArgs e)
        {
            var sMenu = sender as System.Windows.Controls.MenuItem;

            Popup sInputID = new Popup();
            sInputID.IsOpen = true;

            App.MainViewModel.Origin = "GreywindSendUniqueID";
            App.MainViewModel.TestResultID = sMenu.Tag.ToString();
            App.PopupHandler(e, sender);

            //UpdateResultRequest sRequestAPI = new UpdateResultRequest();
            //long iTestResultID = 0;

            //var sMenu = sender as System.Windows.Controls.MenuItem;
            //if (!String.IsNullOrEmpty(sMenu.Tag.ToString()))
            //{
            //    iTestResultID = Convert.ToInt64(sMenu.Tag);

            //    var sTestResultObj = TestResultsRepository.GetTestResultByID(ConfigSettings.GetConfigurationSettings(), iTestResultID);
            //    if (sTestResultObj != null)
            //    {
            //        List<UpdateResultPanelTestObject> sResultListing = new List<UpdateResultPanelTestObject>();
            //        List<UpdateResultPanelsObject> sPanelListing = new List<UpdateResultPanelsObject>();

            //        String sOrderID = "";
            //        var sScheduledTestObj = ScheduledTestRepository.GetScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), "TBLAM FIA-8");
            //        if (sScheduledTestObj != null)
            //        {
            //            if (sScheduledTestObj.ScheduleUniqueID.Contains("-"))
            //            {
            //                var UniqueIDSplit = sScheduledTestObj.ScheduleUniqueID.Split("-");
            //                if (UniqueIDSplit.Length > 0)
            //                {
            //                    sOrderID = UniqueIDSplit[1];
            //                }
            //            }
            //        } 

            //        sRequestAPI.accessionnumber = iTestResultID.ToString();
            //        sRequestAPI.clinic_id = "";
            //        sRequestAPI.reportdate = sTestResultObj.CreatedDate.Value.ToString("yyyy-MM-dd");
            //        sRequestAPI.providerid = "";

            //        UpdateResultPatientObject sPatientObj = new UpdateResultPatientObject();
            //        sPatientObj.patientid = sTestResultObj.PatientID;
            //        sPatientObj.firstname = (sScheduledTestObj != null) ? sScheduledTestObj.PatientName : "";
            //        sPatientObj.lastname = "";
            //        sPatientObj.gender = (sScheduledTestObj != null) ? sScheduledTestObj.Gender : "";
            //        sPatientObj.birthday = "2023-01-01";
            //        sPatientObj.species = (sScheduledTestObj != null) ? sScheduledTestObj.Species : "";
            //        sPatientObj.breed = "";

            //        sRequestAPI.patient = sPatientObj;

            //        UpdateResultPanelsObject sPanelObj = new UpdateResultPanelsObject();
            //        sPanelObj.code = sTestResultObj.TestResultType;
            //        sPanelObj.name = sTestResultObj.TestResultType;
            //        sPanelObj.status = "F";
            //        sPanelObj.source = "";
            //        sPanelObj.resultdate = sTestResultObj.CreatedDate.Value.ToString("yyyy-MM-dd");


            //        var sDetailsObj = TestResultsRepository.GetResultDetailsByTestResultID(ConfigSettings.GetConfigurationSettings(), iTestResultID);
            //        if (sDetailsObj != null && sDetailsObj.Count > 0)
            //        {
            //            foreach (var d in sDetailsObj) 
            //            {
            //                String[] sRange = new string[0];
            //                if (d.ReferenceRange != null)
            //                {
            //                    String sReferenceRange = d.ReferenceRange.Replace("[", "").Replace("]", "");
            //                    if (sReferenceRange != "")
            //                    {
            //                        sRange = sReferenceRange.Split(";");
            //                    }
            //                }

            //                sResultListing.Add(new UpdateResultPanelTestObject
            //                {
            //                    name = d.TestParameter,
            //                    code = d.TestParameter,
            //                    result = d.TestResultValue,
            //                    referencelow = (sRange.Length > 0) ? sRange[0] : "",
            //                    referencehigh = (sRange.Length > 0) ? sRange[1] : "",
            //                    unitofmeasure = d.TestResultUnit,
            //                    status = "F",
            //                    notes = ""
            //                });
            //            }

            //            sPanelObj.tests = sResultListing;
            //        }
            //        sPanelListing.Add(sPanelObj);

            //        sRequestAPI.panels = sPanelListing;

            //        VCheck.Interface.API.GreywindAPI sAPI = new VCheck.Interface.API.GreywindAPI();
            //        var sRespAPI = sAPI.UpdateResult(sRequestAPI, sOrderID);
            //        if (sRespAPI)
            //        {
            //            System.Windows.Forms.MessageBox.Show("Update Result API Successfully.");
            //        }
            //        else
            //        {
            //            System.Windows.Forms.MessageBox.Show("Update Result API Failed.");
            //        }
            //    }
            //}

        }

        private void updateName_Click(object sender, RoutedEventArgs e)
        {
            var sMenu = sender as System.Windows.Controls.MenuItem;

            Popup sInputID = new Popup();
            sInputID.IsOpen = true;

            App.MainViewModel.Origin = "UpdatePatientName";
            App.MainViewModel.TestResultID = sMenu.Tag.ToString();

            App.TestResultInfo = TestResultsRepository.GetTestResultByID(ConfigSettings.GetConfigurationSettings(), Convert.ToInt64(App.MainViewModel.TestResultID));

            App.PopupHandler(e, sender);
        }

        private static void GoToViewResultPageHandler(EventArgs e, object sender)
        {
            if (GoToViewResultPage != null)
            {
                GoToViewResultPage(sender, e);
            }
        }
    }
}
