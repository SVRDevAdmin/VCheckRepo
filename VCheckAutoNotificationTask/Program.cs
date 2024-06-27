namespace VCheckAutoNotificationTask;

using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using static Org.BouncyCastle.Math.EC.ECCurve;
using log4net;
using Microsoft.Extensions.Logging;
using System.Reflection;
using log4net.Config;
using log4net.Repository;

public class Program
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
    static void Main(string[] args)
    {
        ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
        log4net.Config.XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

        if ((System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1))
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        else
        {
            var sBuilder = new ConfigurationBuilder();
            sBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = sBuilder.Build();


            processSendFirmwareUpdateReminder(config);
        }
    }

    private static void processSendFirmwareUpdateReminder(IConfiguration config)
    {
        try
        {
            Console.WriteLine("------Start Process Send Firmware Update Reminder -------------");
            log.Info("------Start Process Send Firmware Update Reminder -------------");

            TemplateDBContext TemplateContext = new TemplateDBContext(config);
            NotificationDBContext NotificationContext = new NotificationDBContext(config);
            ConfigurationDBContext ConfigurationContext = new ConfigurationDBContext(config);

            ConfigurationModel sLangCode = ConfigurationContext.GetConfigurationData("SystemSettings_Language").FirstOrDefault();

            String sNotificationContent = "";

            //var sTemplateObj = TemplateContext.GetTemplateByCode("SF01");
            var sTemplateObj = TemplateContext.GetTemplateByCodeLang("SF01", (sLangCode != null) ? sLangCode.ConfigurationValue : "");
            if (sTemplateObj != null)
            {
                sNotificationContent = sTemplateObj.TemplateContent.Replace("'", "''");
            }
            NotificationModel sNotificationSend = new NotificationModel()
            {
                NotificationType = "Updates",
                NotificationTitle = (sTemplateObj != null) ? sTemplateObj.TemplateTitle : "",
                NotificationContent = sNotificationContent,
                CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                CreatedBy = "VCheck Auto Notification Task"
            };
            NotificationContext.InsertNotification(sNotificationSend);

            Console.WriteLine("------Process Send Firmware Update Reminder Completed.---------------");
            log.Info("------Process Send Firmware Update Reminder Completed.---------------");
        }
        catch (Exception ex)
        {
            log.Error("Error >>> " + ex.ToString(), ex);
        }

    }
}
