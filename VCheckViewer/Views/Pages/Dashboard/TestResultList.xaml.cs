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
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;

namespace VCheckViewer.Views.Pages.Dashboard
{
    /// <summary>
    /// Interaction logic for TestResultList.xaml
    /// </summary>
    public partial class TestResultList : Page
    {
        public TestResultList()
        {
            InitializeComponent();
            initializeData();
        }

        public void initializeData()
        {
            List<TestResultExtendedModel> sTestResultList = VCheck.Lib.Data.TestResultsRepository.GetLatestTestResultList(ConfigSettings.GetConfigurationSettings());

            if (sTestResultList != null && sTestResultList.Count > 0)
            {
                icTestResult.ItemsSource = sTestResultList.ToList();

                borderTestResult.Visibility = Visibility.Visible;
                borderNoTestResult.Visibility = Visibility.Collapsed;
            }
            else
            {
                borderTestResult.Visibility = Visibility.Collapsed;
                borderNoTestResult.Visibility = Visibility.Visible;
            }
        }
    }
}
