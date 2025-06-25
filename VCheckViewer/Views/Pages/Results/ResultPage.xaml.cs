using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using VCheckViewer.Lib.Function;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.DocumentTemplate;
using QuestPDF.Fluent;
using System.Data;
using ClosedXML.Excel;
using System.Diagnostics;
using Spire.Xls;
using VCheck.Lib.Data;
using System.Drawing.Printing;
using log4net;
using System.Reflection;
using VCheckViewer.Views.Pages.Login;

namespace VCheckViewer.Views.Pages.Results
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ResultPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        public ObservableCollection<ComboBoxItem> cbSort { get; set; }
        public ComboBoxItem SelectedcbSort { get; set;  }

        public int currentPage = 1;
        public int paginationSize = 10;
        public int startPagination = 1;
        public int endPagination = 0;
        public int iTotalCount = 0;

        public static event EventHandler? DownloadPrintReport;
        public static event EventHandler? UpdatePatientNameEvent;

        public ResultPage()
        {
            InitializeComponent();
            DataContext = this;

            cbSort = translateSort(App.MainViewModel.cbSort.ToList());
            SelectedcbSort = cbSort.FirstOrDefault();

            pagination.ButtonNextControlClick += new EventHandler(PaginationNextButton_Click);
            pagination.ButtonPrevControlClick += new EventHandler(PaginationPrevButton_Click);
            pagination.ButtonPageControlClick += new EventHandler(PaginationNumButton_Click);

            LoadResultDataGrid();

            UpdatePatientNameEvent = null;
            DownloadPrintReport = null;

            UpdatePatientNameEvent += new EventHandler(UpdatePatientName);
            DownloadPrintReport += new EventHandler(LoadTestResultData);
        }

        private ObservableCollection<ComboBoxItem> translateSort(List<ComboBoxItem> cbSort)
        {
            ObservableCollection<ComboBoxItem> translatedSort = new ObservableCollection<ComboBoxItem>();

            foreach (var sort in cbSort)
            {
                if(sort.Content.ToString() == "Latest") { translatedSort.Add(new ComboBoxItem { Content = Properties.Resources.Results_Label_Latest, Tag = sort.Tag }); }
                else if (sort.Content.ToString() == "Earliest") { translatedSort.Add(new ComboBoxItem { Content = Properties.Resources.Results_Label_Earliest, Tag = sort.Tag }); }
                else if (sort.Content.ToString() == "Result (Ascending)") { translatedSort.Add(new ComboBoxItem { Content = Properties.Resources.Results_Label_Ascending, Tag = sort.Tag }); }
                else if (sort.Content.ToString() == "Result (Descending)") { translatedSort.Add(new ComboBoxItem { Content = Properties.Resources.Results_Label_Descending, Tag = sort.Tag }); }
            }

            return translatedSort;
        }

        private void menuView_Click(object sender, RoutedEventArgs e)
        {
            TestResultListingExtendedObj sTestResultObj = dgResult.SelectedItem as TestResultListingExtendedObj;
            App.TestResultID = sTestResultObj.ID;

            App.GoToViewResultPageHandler(e, sender);
        }

        private void menuDownload_Click(object sender, RoutedEventArgs e)
        {
            DownloadPrint(false);
        }

        private void menuPrint_Click(object sender, RoutedEventArgs e)
        {
            DownloadPrint(true);
        }

        private void DownloadPrint(bool IsPrint)
        {
            var parameterOrder = TestResultsRepository.GetAllParameters(ConfigSettings.GetConfigurationSettings()).Select(x => x.Parameter).ToList();
            App.IsPrint = IsPrint;
            var selectedResult = dgResult.SelectedItem as TestResultListingExtendedObj;
            App.TestResultID = selectedResult.ID;
            App.Parameters = TestResultsRepository.GetResultDetailsByTestResultID(ConfigSettings.GetConfigurationSettings(), selectedResult.ID).Select(x => x.TestParameter).OrderBy(d => parameterOrder.IndexOf(d)).ToList();

            App.isEmptyName = false;

            if (selectedResult.PatientName == "-")
            {
                App.isEmptyName = true;
            }

            DownloadPrintResultModel downloadPrintResultModel = new DownloadPrintResultModel();
            App.DowloadPrintObject = new List<DownloadPrintResultModel>();

            TestResultModel previousTest = new TestResultModel();
            var TestResult = TestResultsRepository.GetTestResultByID(ConfigSettings.GetConfigurationSettings(), selectedResult.ID);
            downloadPrintResultModel.TestResult = TestResult;
            downloadPrintResultModel.TestResultDetails = TestResultsRepository.GetResultDetailsByTestResultID(ConfigSettings.GetConfigurationSettings(), TestResult.ID);
            downloadPrintResultModel.PreviousTestResultDetails = TestResultsRepository.GetPreviousTestRecord(ConfigSettings.GetConfigurationSettings(), TestResult, out previousTest);
            downloadPrintResultModel.PreviousTestResult = previousTest;

            App.DowloadPrintObject.Add(downloadPrintResultModel);

            App.Device = new List<TestDeviceName>();
            App.Device.Add(new TestDeviceName() { DeviceName = DeviceRepository.GetDeviceNameBySerialNo(ConfigSettings.GetConfigurationSettings(), TestResult.DeviceSerialNo), TestID = TestResult.ID });

            if (App.isEmptyName)
            {
                updateName(TestResult.ID.ToString(), null, null);
            }
            else
            {
                App.MainViewModel.Origin = "SelectAdditionalTestResult";
                App.PopupHandler(null, null);
            }

        }

        public void LoadTestResultData(object sender, EventArgs e)
        {
            try
            {
                var parameterOrder = TestResultsRepository.GetAllParameters(ConfigSettings.GetConfigurationSettings()).Select(x => x.Parameter).ToList();

                foreach(var test in App.DowloadPrintObject)
                {
                    test.TestResultDetails = test.TestResultDetails.Where(x => App.Parameters.Contains(x.TestParameter)).OrderBy(d => parameterOrder.IndexOf(d.TestParameter)).ToList();
                    test.PreviousTestResultDetails = test.PreviousTestResultDetails != null ? test.PreviousTestResultDetails.Where(x => App.Parameters.Contains(x.TestParameter)).OrderBy(d => parameterOrder.IndexOf(d.TestParameter)).ToList() : null;
                }

                TestResultTemplate sTestResultTemplate = new TestResultTemplate(App.DowloadPrintObject, App.IsPrint);

                if (App.IsPrint)
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
                log.Error("Error >>> " + ex.ToString());
                App.ErrorOccur = true;
            }

            if (!App.IsPrint)
            {
                Popup sConfirmPopup = new Popup();
                sConfirmPopup.IsOpen = true;

                if (App.ErrorOccur)
                {
                    App.MainViewModel.Origin = "FailedToDownload";
                }
                else
                {
                    App.MainViewModel.Origin = "TestResultDownloadCompleted";
                }

                App.PopupHandler(null, null);
            }
            else
            {
                if (App.ErrorOccur)
                {
                    App.MainViewModel.Origin = "FailedToPrint";
                    App.PopupHandler(null, null);
                }
            }
        }

        public void UpdatePatientName(object sender, EventArgs e)
        {
            LoadResultDataGrid();
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            startPagination = 1;

            LoadResultDataGrid();
        }

        public async Task LoadResultDataGrid()
        {
            dgResult.ItemsSource = await GetTestResultBySearch(paginationSize);

            if (iTotalCount <= 0)
            {
                btnPrint.Visibility = Visibility.Collapsed;
                btnDownload.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnPrint.Visibility = Visibility.Visible;
                btnDownload.Visibility = Visibility.Visible;
            }

            txtTotalCount.Text = iTotalCount.ToString();

            pagination.iTotalRecords = iTotalCount;
            pagination.iPageIndex = startPagination;
            pagination.iPageSize = paginationSize;
            pagination.LoadPagingNumber();
        }

        public async Task<List<TestResultListingExtendedObj>> GetTestResultBySearch(int pageSize)
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

            String sKeyword = KeywordSearchBar.Text;

            LoginPage loginPage = new LoginPage();
            await loginPage.CheckPMSUser();

            return VCheck.Lib.Data.TestResultsRepository.GetTestResultListBySearch(
                                            ConfigSettings.GetConfigurationSettings(),
                                            sStart, sEnd, sKeyword, sSortDirection,
                                            startPagination, pageSize, App.PMSFunction, out iTotalCount);
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

        private async void DownloadSearchResult(Boolean isPrint = false)
        {
            var sBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            String sDownloadPath = sBuilder.Configuration.GetSection("Configuration:DownloadFolderPath").Value;
            String sProcessingPath = System.IO.Path.Combine(sDownloadPath, "Processing");

            if (!System.IO.Path.Exists(sProcessingPath))
            {
                System.IO.Directory.CreateDirectory(sProcessingPath);
            }

            int iPageSize = 0;

            try
            {
                String sFileName = "TestResultsListing_Download_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                String sExcelOutputPath = System.IO.Path.Combine(sProcessingPath, sFileName + ".xlsx"); 
                String sPDFOutputPath = System.IO.Path.Combine(sDownloadPath, sFileName + ".pdf");

                int.TryParse(txtTotalCount.Text, out iPageSize);

                List<TestResultListingExtendedObj> sResult = new List<TestResultListingExtendedObj>();
                sResult = await GetTestResultBySearch(iPageSize);

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
            var sMenu = sender as MenuItem;

            Popup sInputID = new Popup();
            sInputID.IsOpen = true;

            App.MainViewModel.Origin = "GreywindSendUniqueID";
            App.MainViewModel.TestResultID = sMenu.Tag.ToString();
            App.PopupHandler(e, sender);

        }

        private void updateNameButton_Click(object sender, RoutedEventArgs e)
        {
            App.isEmptyName = false;
            var sMenu = sender as System.Windows.Controls.MenuItem;

            updateName(sMenu.Tag.ToString(), sender, e);
        }

        private void updateName(string TestResultID, object sender, RoutedEventArgs e)
        {
            Popup sInputID = new Popup();
            sInputID.IsOpen = true;

            App.MainViewModel.Origin = "UpdatePatientName";
            App.MainViewModel.TestResultID = TestResultID;

            App.TestResultInfo = TestResultsRepository.GetTestResultByID(ConfigSettings.GetConfigurationSettings(), Convert.ToInt64(App.MainViewModel.TestResultID));

            App.PopupHandler(e, sender);
        }

        public static void DownloadPrintReportHandler(EventArgs e, object sender)
        {
            if (DownloadPrintReport != null)
            {
                DownloadPrintReport(sender, e);
            }
        }

        public static void UpdatePatientNameHandler(EventArgs e, object sender)
        {
            if (UpdatePatientNameEvent != null)
            {
                UpdatePatientNameEvent(sender, e);
            }
        }
    }
}
