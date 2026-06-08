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
using System.Windows.Shapes;
using VCheckListener.Services;

namespace VCheckListener.Views
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();

            Clear_Button.Content = App._localizationService.GetString("General_Label_Logs");

            // optional: minimize on startup
            this.Hide();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // 🔹 Prevent window from closing completely — just hide it.
            e.Cancel = true;
            this.Hide();
        }

        public void AddLog(string message)
        {
            Dispatcher.Invoke(() =>
            {
                LogList.Items.Add($"{System.DateTime.Now:HH:mm:ss} - {message}");
                LogList.ScrollIntoView(LogList.Items[LogList.Items.Count - 1]);
            });
        }

        private void ClearLogs_Click(object sender, RoutedEventArgs e)
        {
            LogList.Items.Clear();
        }
    }
}
