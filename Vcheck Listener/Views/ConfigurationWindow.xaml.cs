using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Vcheck_Listener.Lib.Models;

namespace Vcheck_Listener.Views
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        public Action<string>? OnConfigurationUpdated;
        public Action<bool>? OnRunScheduleAutomationProcess;
        public Action<bool>? OnReRunSocketListenernProcess;
        bool notInitialLoad = false;

        ListenerConfig result  = new ListenerConfig();

        public ConfigurationWindow()
        {
            InitializeComponent();

            // optional: minimize on startup
            this.Hide();
            
            var listenerConfig = App.db.GetCollection<ListenerConfig>("ListenerConfig");

            result = listenerConfig.FindOne(x => x.Id == 1);

            ListenerPort.Text = "8585";

            LoadListenerInfo();
            LoadConfig();
            notInitialLoad= true;

            LayoutUpdated += ConfigurationsGrid_LayoutUpdated;

        }

        private void ConfigurationsGrid_LayoutUpdated(object sender, EventArgs e)
        {
            if (ConfigurationsGrid == null) return;

            double totalHeight = 0;

            var visibleGrids = ConfigurationsGrid.Children.OfType<Grid>().Where(el => el.Visibility == Visibility.Visible);

            foreach(var visibleGrid in visibleGrids)
            {
                totalHeight += visibleGrid.ActualHeight + visibleGrid.Margin.Top + visibleGrid.Margin.Bottom;
            }

            double visibleBorderHeight = ConfigurationsGrid.Children
                .OfType<Border>()
                .Where(el => el.Visibility == Visibility.Visible)
                .Sum(el => el.ActualHeight);

            // Add margins/paddings if needed
            totalHeight = totalHeight + visibleBorderHeight + ConfigurationsGrid.Margin.Top + ConfigurationsGrid.Margin.Bottom + 10;

            Height = totalHeight;
        }

        public void LoadListenerInfo()
        {
            ListenerIP.Text = App.IPAddress;
        }

        public void LoadConfig()
        {
            if (result != null)
            {
                AnalyzerIP.Text = result.AnalyzerIP;
                AnalyzerPort.Text = result.AnalyzerPort;
                ClinicID.Text = result.ClinicID;
                PIMSPort.Text = result.PIMSPort;
                PIMSIP.Text = result.PIMSIP;
                PIMSPort.Text = result.PIMSPort;
                PIMSUsername.Text = result.PIMSUsername;
                PIMSPassword.Text = result.PIMSPassword;

                var selectedChart = ConversionChart.Items.OfType<ComboBoxItem>().FirstOrDefault(x => x.Tag.ToString() == result.ConversionChart);
                ConversionChart.SelectedItem = selectedChart;

                var selectedAnalyzer = AnalyzerType.Items.OfType<ComboBoxItem>().FirstOrDefault(x => x.Tag.ToString() == result.AnalyzerType);
                AnalyzerType.SelectedItem = selectedAnalyzer;

                ScheduleAutomationSwitch.IsChecked = result.IsScheduleAutomationActive == 1 ? true : false;
                SwitchButton_Click(null, null);

                App.DeviceType = result.AnalyzerType;
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // 🔹 Prevent window from closing completely — just hide it.
            e.Cancel = true;
            LoadConfig();
            this.Hide();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var conversionChart = ConversionChart.SelectedItem as ComboBoxItem;
            var selectedChart = conversionChart != null && conversionChart.Tag != null ? conversionChart.Tag.ToString() : "";

            var analyzerType = AnalyzerType.SelectedItem as ComboBoxItem;
            var selectedAnalyzer = analyzerType != null && analyzerType.Tag != null ? analyzerType.Tag.ToString() : "";

            var listenerConfig = App.db.GetCollection<ListenerConfig>("ListenerConfig");

            if(listenerConfig.FindOne(x => x.Id == 1) != null)
            {
                listenerConfig.Update(new ListenerConfig { Id = 1, IsScheduleAutomationActive = ScheduleAutomationSwitch.IsChecked.Value ? 1 : 0, AnalyzerIP = AnalyzerIP.Text, AnalyzerPort = AnalyzerPort.Text, AnalyzerType = selectedAnalyzer, PIMSIP = PIMSIP.Text, PIMSPort = PIMSPort.Text, PIMSUsername = PIMSUsername.Text, PIMSPassword = PIMSPassword.Text, ConversionChart = selectedChart, ClinicID = ClinicID.Text });
            }
            else
            {
                listenerConfig.Insert(new ListenerConfig { Id = 1, IsScheduleAutomationActive = ScheduleAutomationSwitch.IsChecked.Value ? 1 : 0, AnalyzerIP = AnalyzerIP.Text, AnalyzerPort = AnalyzerPort.Text, AnalyzerType = selectedAnalyzer, PIMSIP = PIMSIP.Text, PIMSPort = PIMSPort.Text, PIMSUsername = PIMSUsername.Text, PIMSPassword = PIMSPassword.Text, ConversionChart = selectedChart, ClinicID = ClinicID.Text });
            }

            OnConfigurationUpdated?.Invoke("");

            if(!ScheduleAutomationSwitch.IsChecked.Value)
            {
                App.runScheduleAutomation = false;
            }
            else if(selectedAnalyzer == "C10")
            {
                OnRunScheduleAutomationProcess?.Invoke(true);
            }

            if ((selectedAnalyzer == "H6" && App.DeviceType != selectedAnalyzer) || (App.DeviceType == "H6" && App.DeviceType != selectedAnalyzer))
            {
                OnReRunSocketListenernProcess?.Invoke(true);
            }
            else
            {
                OnReRunSocketListenernProcess?.Invoke(false);
            }

            App.DeviceType = selectedAnalyzer;

            this.Hide();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            LoadConfig();
            this.Hide();
        }

        private void ConversionChart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var conversionChart = ConversionChart.SelectedItem as ComboBoxItem;
            var selectedChart = conversionChart != null && conversionChart.Tag != null ? conversionChart.Tag.ToString() : "";

            if(selectedChart == "DRX - Taiwan")
            {
                PIMSGrid1.Visibility = Visibility.Visible;
                PIMSGrid2.Visibility = Visibility.Visible;
                ClinicIDGrid.Visibility = Visibility.Visible;
                PIMSIPGrid.Visibility = Visibility.Visible;
                PIMSPortGrid.Visibility = Visibility.Visible;
                PIMSUsernameGrid.Visibility = Visibility.Visible;
                PIMSPasswordGrid.Visibility = Visibility.Visible;
            }
            else if(selectedChart == "Greywind")
            {
                PIMSGrid1.Visibility = Visibility.Visible;
                PIMSGrid2.Visibility = Visibility.Collapsed;
                ClinicIDGrid.Visibility = Visibility.Visible;
                PIMSIPGrid.Visibility = Visibility.Collapsed;
                PIMSPortGrid.Visibility = Visibility.Collapsed;
                PIMSUsernameGrid.Visibility = Visibility.Collapsed;
                PIMSPasswordGrid.Visibility = Visibility.Collapsed;
            }
            else if(notInitialLoad)
            {
                PIMSGrid1.Visibility = Visibility.Collapsed;
                PIMSGrid2.Visibility = Visibility.Collapsed;
            }            
        }

        private void TitleBar_MouseLeftButton(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                try
                {
                    DragMove(); // Moves the window
                }
                catch
                {
                    // Ignore exceptions if DragMove is called during resize
                }
            }
            else if (e.ButtonState == MouseButtonState.Released)
            {
                WindowState = WindowState.Normal;
            }
        }
        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleAutomationSwitch.IsChecked.Value)
            {
                AnalyzerGrid.Visibility = Visibility.Visible;
            }
            else
            {
                AnalyzerGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void ComboBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.ComboBox comboBox = sender as System.Windows.Controls.ComboBox;

            if (comboBox.IsDropDownOpen)
            {
                comboBox.IsDropDownOpen = false;
            }
            else
            {
                comboBox.IsDropDownOpen = true;
            }
        }

        private void AnalyzerType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var analyzerType = AnalyzerType.SelectedItem as ComboBoxItem;
            var selectedType = analyzerType != null && analyzerType.Tag != null ? analyzerType.Tag.ToString() : "";

            if (selectedType == "C10" || selectedType == "H6")
            {
                ScheduleAutomationSwitch.IsChecked = true;
            }
            else
            {
                ScheduleAutomationSwitch.IsChecked = false;
            }

            if (notInitialLoad)
            {
                SwitchButton_Click(null, null);
            }
        }
    }
}
