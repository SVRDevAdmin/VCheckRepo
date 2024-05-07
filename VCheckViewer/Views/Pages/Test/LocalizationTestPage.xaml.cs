using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
using VCheck.Helper;
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

        private void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            var sBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            string sErrorMessage = "";

            try
            {
                EmailObject sEmail = new EmailObject();

                sEmail.SenderEmail = sBuilder.Configuration.GetSection("Smtp:Sender").Value;

                List<String> sRecipientList = new List<string>();
                sRecipientList.Add("svrkenny@hotmail.com");


                sEmail.RecipientEmail = sRecipientList;
                sEmail.IsHtml = true;
                sEmail.Subject = "Testing 123";
                sEmail.SMTPHost = sBuilder.Configuration.GetSection("Smtp:Host").Value;
                sEmail.PortNo = Convert.ToInt32(sBuilder.Configuration.GetSection("Smtp:Port").Value);
                sEmail.HostUsername = sBuilder.Configuration.GetSection("Smtp:Username").Value;
                sEmail.HostPassword = sBuilder.Configuration.GetSection("Smtp:Password").Value;
                sEmail.EnableSsl = true;
                sEmail.UseDefaultCredentials = false;

                EmailHelper.SendEmail(sEmail, out sErrorMessage);
            }
            catch (Exception ex)
            {
                String abc = "Abc";
            }
        }
    }
}
