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
    /// Interaction logic for GeneralTestList.xaml
    /// </summary>
    public partial class ScheduleTestList : Page
    {
        public ScheduleTestList()
        {
            InitializeComponent();
            initializeData();
        }

        public void initializeData()
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
    }
}
