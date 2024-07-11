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
            TestResultListingObj sTestResultObj = dgResult.SelectedItem as TestResultListingObj;
            sTestResultObj.printedBy = App.MainViewModel.CurrentUsers.FullName;
            sTestResultObj.printedOn = DateTime.Now;
            sTestResultObj.isPrint = isPrint;

            try
            {
                TestResultTemplate sTestResultTemplate = new TestResultTemplate(sTestResultObj);

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

        public List<TestResultListingObj> GetTestResultBySearch(int pageSize, out int iTotalRecord)
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

                List<TestResultListingObj> sResult = new List<TestResultListingObj>();
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
                sDatatable.Columns.Remove("TestResultRules");
                sDatatable.Columns.Remove("CreatedDate");
                sDatatable.Columns.Remove("CreatedBy");
                sDatatable.Columns.Remove("UpdatedDate");
                sDatatable.Columns.Remove("UpdatedBy");


                sDatatable.Columns["RowNo"].SetOrdinal(0);
                sDatatable.Columns["RowNo"].ColumnName = "No.";
                sDatatable.Columns["TestResultDateTimeString"].SetOrdinal(1);
                sDatatable.Columns["TestResultDateTimeString"].ColumnName = "Observation DateTime";
                sDatatable.Columns["TestResultType"].SetOrdinal(2);
                sDatatable.Columns["TestResultType"].ColumnName = "Observation Type";
                sDatatable.Columns["TestResultStatus"].SetOrdinal(3);
                sDatatable.Columns["TestResultStatus"].ColumnName = "Result Status";
                sDatatable.Columns["TestResultValueString"].SetOrdinal(4);
                sDatatable.Columns["TestResultValueString"].ColumnName = "Results Value";
                sDatatable.Columns["OperatorID"].SetOrdinal(5);
                sDatatable.Columns["OperatorID"].ColumnName = "Operator ID";
                sDatatable.Columns["PatientID"].SetOrdinal(6);
                sDatatable.Columns["PatientID"].ColumnName = "Patient ID";
                sDatatable.Columns["InchargePerson"].SetOrdinal(7);
                sDatatable.Columns["InchargePerson"].ColumnName = "Doctor";
                
                               
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
            catch (Exception ex)
            {
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
    }
}
