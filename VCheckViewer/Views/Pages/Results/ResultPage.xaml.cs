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

namespace VCheckViewer.Views.Pages.Results
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ResultPage : Page
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

                //System.Windows.MessageBox.Show("Download completed",
                //               "Download Completed", MessageBoxButton.OK);
                //lbMessage.Text = "Download completed";
                //lbMessage.Foreground = Brushes.Green;
            }
            catch (Exception ex)
            {
                //lbMessage.Text = "Failed to download result, please contact Admininistrator.";
                //lbMessage.Foreground = Brushes.Red;
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

            }
        }

        private void LoadResultDataGrid()
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

            int iTotalRecord = 0;
            dgResult.ItemsSource = VCheck.Lib.Data.TestResultsRepository.GetTestResultListBySearch(
                                            ConfigSettings.GetConfigurationSettings(), 
                                            sStart, sEnd, sKeyword, sSortDirection, 
                                            startPagination, paginationSize, out iTotalRecord);

            //int totalRecord = 10;
            int totalPage = iTotalRecord / paginationSize;
             
            endPagination = totalPage;
            startPagination = 1;

            createPagination(startPagination);
        }

        private void KeywordSearchBar_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            LoadResultDataGrid();
        }

        private void RangeDate_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            LoadResultDataGrid();
        }

        private void cboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadResultDataGrid();
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
            try
            {
                //TestResultTemplate sTest = new TestResultTemplate();
                //sTest.GeneratePdf();
            }
            catch (Exception ex)
            {
                string efg = "efg";
            }
        }
    }
}
