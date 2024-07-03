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
using VCheckViewer.Views.Pages.Results;

namespace VCheckViewer.UserControls
{
    /// <summary>
    /// Interaction logic for PagingControl.xaml
    /// </summary>
    public partial class PagingControl : System.Windows.Controls.UserControl
    {
        public event EventHandler? ButtonNextControlClick;
        public event EventHandler? ButtonPrevControlClick;
        public event EventHandler? ButtonPageControlClick;

        public int iTotalRecords = 0;
        public int iPageIndex = 0;
        public int iPageSize = 0;
        public int iTotalPage = 0;

        //Below are new variable to implement limit to the total pagination showed
        public int iPaginationLimit = 0;
        public int iPaginationStart = 0;
        public int iPaginationEnd = 0;
        public int iLastPage = 0;

        public PagingControl()
        {
            InitializeComponent();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            ButtonPrevControlClick(sender, e);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ButtonNextControlClick(sender, e);
        }

        public void btnNumber_Click(object sender, RoutedEventArgs e)
        {
            ButtonPageControlClick(sender, e);
        }

        public int GetPageCountByRecordCount(int TotalRecords, int PageSize)
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

        public void LoadPagingNumber()
        {
            iTotalPage = GetPageCountByRecordCount(iTotalRecords, iPageSize);

            numberingSection.Children.Clear();

            int iCurrentNumber = 1;
            for (int i = 1; i <= iTotalPage; i++)
            {
                System.Windows.Controls.Button newBtn = new System.Windows.Controls.Button();
                newBtn.Style = (Style)System.Windows.Application.Current.FindResource("PaginationButton");
                newBtn.Content = String.Format("{0:00}", i);
                newBtn.Tag = i;
                newBtn.Width = 40;
                newBtn.Margin = new Thickness(5, 0, 5, 0);
                newBtn.FontWeight = FontWeights.Bold;
                newBtn.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;

                if (i == iPageIndex)
                {
                    newBtn.BorderBrush = System.Windows.Media.Brushes.DarkOrange;
                    newBtn.Background = System.Windows.Media.Brushes.DarkOrange;
                    newBtn.Foreground = System.Windows.Media.Brushes.White;
                }
                else
                {
                    newBtn.BorderBrush = System.Windows.Media.Brushes.DarkOrange;
                    newBtn.Background = System.Windows.Media.Brushes.Transparent;
                    newBtn.Foreground = System.Windows.Media.Brushes.DarkOrange;
                }

                numberingSection.Children.Add(newBtn);
                newBtn.Click += new RoutedEventHandler(btnNumber_Click);
            }

            if (iTotalPage == 0)
            {
                btnNext.Visibility = Visibility.Collapsed;
                btnPrev.Visibility = Visibility.Collapsed;
            }
            else if (iPageIndex == 1 && iPageIndex == iTotalPage)
            {
                btnNext.Visibility = Visibility.Collapsed;
                btnPrev.Visibility = Visibility.Collapsed;
            }
            else if (iPageIndex == iTotalPage)
            {
                btnNext.Visibility = Visibility.Collapsed;
                btnPrev.Visibility = Visibility.Visible;
            }
            else if (iPageIndex == 1)
            {
                btnNext.Visibility = Visibility.Visible;
                btnPrev.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnNext.Visibility = Visibility.Visible;
                btnPrev.Visibility = Visibility.Visible;
            }
        }

        //Below are new method to implement limit to the total pagination showed
        public void LoadPagingNumberWithLimit()
        {
            numberingSection.Children.Clear();
            bool recordExist = true;

            if (iTotalRecords == 0)
            {
                btnNext.Visibility = Visibility.Collapsed;
                btnPrev.Visibility = Visibility.Collapsed;
                recordExist = false;
            }
            else if (iPageIndex == 1 && iPageIndex == iLastPage)
            {
                btnNext.Visibility = Visibility.Collapsed;
                btnPrev.Visibility = Visibility.Collapsed;
            }
            else if (iPageIndex == iLastPage)
            {
                btnNext.Visibility = Visibility.Collapsed;
                btnPrev.Visibility = Visibility.Visible;
            }
            else if (iPageIndex == 1)
            {
                btnNext.Visibility = Visibility.Visible;
                btnPrev.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnNext.Visibility = Visibility.Visible;
                btnPrev.Visibility = Visibility.Visible;
            }

            if (recordExist)
            {
                for (int i = iPaginationStart; i <= iPaginationEnd; i++)
                {
                    System.Windows.Controls.Button newBtn = new System.Windows.Controls.Button();
                    newBtn.Style = (Style)System.Windows.Application.Current.FindResource("PaginationButton");
                    newBtn.Content = String.Format("{0:00}", i);
                    newBtn.Tag = i;
                    newBtn.Width = 40;
                    newBtn.Margin = new Thickness(5, 0, 5, 0);
                    newBtn.FontWeight = FontWeights.Bold;
                    newBtn.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;

                    if (i == iPageIndex)
                    {
                        newBtn.BorderBrush = System.Windows.Media.Brushes.DarkOrange;
                        newBtn.Background = System.Windows.Media.Brushes.DarkOrange;
                        newBtn.Foreground = System.Windows.Media.Brushes.White;
                    }
                    else
                    {
                        newBtn.BorderBrush = System.Windows.Media.Brushes.DarkOrange;
                        newBtn.Background = System.Windows.Media.Brushes.Transparent;
                        newBtn.Foreground = System.Windows.Media.Brushes.DarkOrange;
                    }

                    numberingSection.Children.Add(newBtn);
                    newBtn.Click += new RoutedEventHandler(btnNumber_Click);
                }
            }
        }

        public void GetPageCountByRecordCountWithLimit()
        {
            int pageCount = 0;
            int remainder = 0;

            pageCount = Math.DivRem(iTotalRecords, iPageSize, out remainder);
            if (remainder > 0)
            {
                pageCount += 1;
            }

            iLastPage = pageCount;

            pageCount = pageCount < iPaginationLimit ? pageCount : iPaginationLimit;

            iPaginationStart = 1;
            iPaginationEnd = pageCount;
            iPageIndex = 1;
            iTotalPage = pageCount;
        }
    }
}
