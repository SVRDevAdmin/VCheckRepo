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
using VCheckViewer.Lib.Culture;

namespace VCheckViewer.Views.Pages.Test
{
    /// <summary>
    /// Interaction logic for LocalizationTestPage.xaml
    /// </summary>
    public partial class LocalizationTestPage : Page
    {
        public LocalizationTestPage()
        {
            InitializeComponent();
        }

        private void btnZh_Click(object sender, RoutedEventArgs e)
        {
            String sLangCode = "zh";

            System.Globalization.CultureInfo sZHCulture = new System.Globalization.CultureInfo("zh");

            CultureResources.ChangeCulture(sZHCulture);
        }

        private void btnEn_Click(object sender, RoutedEventArgs e)
        {
            String sLangCode = "en";

            System.Globalization.CultureInfo sZHCulture = new System.Globalization.CultureInfo("en");

            CultureResources.ChangeCulture(sZHCulture);
        }
    }
}
