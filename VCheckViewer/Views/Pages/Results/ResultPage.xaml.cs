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
        public int paginationSize = 2;
        public int startPagination = 0;
        public int endPagination = 0;

        public ResultPage()
        {
            InitializeComponent();
            DataContext = this;

            cbSort = App.MainViewModel.cbSort;
            SelectedcbSort = cbSort.FirstOrDefault();

            LoadResultDataGrid();
        }

        private void menuDownload_Click(object sender, RoutedEventArgs e)
        {
            TestResultListingObj sTestResultObj = dgResult.SelectedItem as TestResultListingObj;
            sTestResultObj.printedBy = App.MainViewModel.CurrentUsers.FullName;
            sTestResultObj.printedOn = DateTime.Now;
            sTestResultObj.isPrint = false;

            try
            {
                TestResultTemplate sTestResultTemplate = new TestResultTemplate(sTestResultObj);
                sTestResultTemplate.GeneratePdf();

                //todo : prompt msg
            }
            catch (Exception ex)
            {
                String abc = "efg";
            }
        }

        private void menuPrint_Click(object sender, RoutedEventArgs e)
        {
            TestResultListingObj sTestResultObj = dgResult.SelectedItem as TestResultListingObj;
            sTestResultObj.printedBy = App.MainViewModel.CurrentUsers.FullName;
            sTestResultObj.printedOn = DateTime.Now;
            sTestResultObj.isPrint = true;

            try
            {
                TestResultTemplate sTestResultTemplate = new TestResultTemplate(sTestResultObj);
                sTestResultTemplate.GeneratePdfAndShow();
            }
            catch (Exception ex)
            {
                String abc = "anc";
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            LoadResultDataGrid();
        }

        private void LoadResultDataGrid()
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
            int totalPage = iTotalRecord / paginationSize;
             
            endPagination = totalPage;
            startPagination = 1;

            createPagination(startPagination);
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
            else if (RangeDate.SelectedDates.Count == 0)
            {
                sStart = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sEnd = DateTime.Now.AddDays(1).AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");
            }

            String sKeyword = KeywordSearchBar.Text;

            return VCheck.Lib.Data.TestResultsRepository.GetTestResultListBySearch(
                                            ConfigSettings.GetConfigurationSettings(),
                                            sStart, sEnd, sKeyword, sSortDirection,
                                            startPagination, pageSize, out iTotalRecord);
        }

        public void createPagination(int highligtedIndex)
        {
            paginationPanel.Children.Clear();

            currentPage = highligtedIndex;

            Button newBtn = new Button();
            newBtn.Content = Properties.Resources.General_Label_Previous;
            newBtn.Tag = "Prev";
            newBtn.BorderThickness = new Thickness(0);
            newBtn.FontWeight = FontWeights.Bold;
            paginationPanel.Children.Add(newBtn);
            newBtn.Click += new RoutedEventHandler(PreviousUserList_Click);

            for (int i = startPagination; i <= endPagination; i++)
            {
                newBtn = new Button();

                if (i < 10) { newBtn.Content = "0" + i; }
                else { newBtn.Content = i; }

                newBtn.Tag = i;
                newBtn.Style = (Style)System.Windows.Application.Current.FindResource("RoundButton");
                newBtn.Width = 40;
                newBtn.Margin = new Thickness(5, 0, 5, 0);
                newBtn.FontWeight = FontWeights.Bold;
                newBtn.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;

                if (i == highligtedIndex)
                {
                    newBtn.BorderBrush = Brushes.DarkOrange;
                    newBtn.Background = Brushes.DarkOrange;
                    newBtn.Foreground = Brushes.White;
                }
                else
                {
                    newBtn.BorderBrush = Brushes.DarkOrange;
                    newBtn.Background = Brushes.Transparent;
                    newBtn.Foreground = Brushes.DarkOrange;
                }

                paginationPanel.Children.Add(newBtn);
                newBtn.Click += new RoutedEventHandler(newBtn_Click);
            }

            newBtn = new Button();
            newBtn.Content = Properties.Resources.General_Label_Next;
            newBtn.Tag = "Next";
            newBtn.BorderThickness = new Thickness(0);
            newBtn.FontWeight = FontWeights.Bold;
            newBtn.Foreground = Brushes.DarkOrange;
            paginationPanel.Children.Add(newBtn);
            newBtn.Click += new RoutedEventHandler(NextUserList_Click);
        }

        public static int GetPageCountByRecordCount(int TotalRecords, int PageSize)
        {
            int pageCount = 0;
            int remainder = 0;

            pageCount = Math.DivRem(TotalRecords, PageSize, out remainder);
            if (remainder > 0)
            {
                pageCount += 1;
            }

            return pageCount;
        }

        public void NextUserList_Click(object sender, RoutedEventArgs e)
        {
            startPagination++;

            LoadResultDataGrid();
        }

        public void newBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            btn.BorderBrush = Brushes.DarkOrange;
            btn.Background = Brushes.DarkOrange;
            btn.Foreground = Brushes.White;

            int childrenCount = VisualTreeHelper.GetChildrenCount(btn.Parent);
            for (int i = 0; i < VisualChildrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(btn.Parent, i);
                var frameworkElement = child as System.Windows.Controls.Button;
                if (frameworkElement.Tag.ToString() == currentPage.ToString() && childrenCount > 3)
                {
                    frameworkElement.BorderBrush = Brushes.DarkOrange;
                    frameworkElement.Background = Brushes.Transparent;
                    frameworkElement.Foreground = Brushes.DarkOrange;
                }
            }

            startPagination = Convert.ToInt32(btn.Content);
            LoadResultDataGrid();
        }

        public void PreviousUserList_Click(object sender, RoutedEventArgs e)
        {
            startPagination--;

            LoadResultDataGrid();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            DownloadSearchResult();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
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
                    System.Windows.MessageBox.Show("Download completed.");
                }
            }
            catch (Exception ex)
            {
                string efg = "efg";
            }
        }
    }
}
