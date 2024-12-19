using log4net.Repository;
using log4net.Config;
using System.IO.IsolatedStorage;
using Microsoft.Extensions.Configuration;
using System.Net.NetworkInformation;
using System.Reflection;
using VCheck.Interface.API;
using VCheck.Lib.Data;
using VCheck.Lib.Data.Models;
using VetVitals = VCheck.Interface.API.VetVitals;

namespace VCheckAppointmentSynchTask
{
    public class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static String? sProcessingNewApptName = "GetAppointmentSynchUpdate";
        private static String? sProcessingUpdatedApptName = "GetUpdatedAppointmentSynchUpdate";

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
                //sBuilder.SetBasePath(Directory.GetCurrentDirectory())
                sBuilder.SetBasePath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                        //.AddEnvironmentVariables();

                IConfiguration config = sBuilder.Build();

                // Synch New appointment
                log.Info("----------------- Start retrieve new appointment record ----------------");
                AppointmentUpdateSynch(config, "NEW", sProcessingNewApptName);
                log.Info("----------------- Retrieve new appointment completed -------------------");

                log.Info("----------------- Start Synch updated appointment record ----------------");
                AppointmentUpdateSynch(config, "UPDATE", sProcessingUpdatedApptName);
                log.Info("----------------- Synch updated appoitnemnt completed -------------------");
            }
        }

        /// <summary>
        /// Main Logic retrieve appointment data
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sTransType"></param>
        /// <param name="processName"></param>
        private static void AppointmentUpdateSynch(IConfiguration config, String sTransType, String processName)
        {
            VetVitals.RequestMessage.GetAppointmentDateRangeRequest sReq = new VetVitals.RequestMessage.GetAppointmentDateRangeRequest();
            VetVitals.RequestMessage.RequestHeaderObject sHeader = new VetVitals.RequestMessage.RequestHeaderObject();
            VetVitals.RequestMessage.RequestBodyObject sBody = new VetVitals.RequestMessage.RequestBodyObject();

            VetVitalsAPI sVPMSAPI = new VetVitalsAPI();
            DateTime sStart = DateTime.Now.AddMonths(-3);
            DateTime sEnd = DateTime.Now;

            try
            {
                String? sToken = config.GetSection("VetConnect:clientkey").Value;

                sHeader.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                sHeader.authtoken = sToken;

                sBody.transtype = sTransType;

                String? sConnStr = config.GetSection("ConnectionStrings:DefaultConnection").Value;
                var sProcessingLog = ProcessingLogRepository.GetProcessingLogByName(processName, config);
                if (sProcessingLog != null)
                {
                    sStart = sProcessingLog.ProcessingEndDate.Value;
                }

                sBody.startdate = sStart.ToString("yyyyMMddHHmmss");
                sBody.enddate = sEnd.ToString("yyyyMMddHHmmss");

                sReq.header = sHeader;
                sReq.body = sBody;
                var sResp = sVPMSAPI.GetAppointmentByDateRange(sReq);
                if (sResp != null)
                {
                    if (sResp.body.responsecode == "VPMS.0001")
                    {
                        if (sResp.body.results != null && sResp.body.results.Count > 0)
                        {
                            foreach(var r in sResp.body.results)
                            {
                                DateTime dtCreate = DateTime.ParseExact(r.createddate, "yyyyMMddHHmmss",
                                                             System.Globalization.CultureInfo.InvariantCulture);

                                String strApptDate = r.appointmentdate + r.starttime;
                                DateTime dtAppt = DateTime.ParseExact(strApptDate, "yyyyMMddHHmmss", 
                                                             System.Globalization.CultureInfo.InvariantCulture);

                                ScheduledTestModel sApptScheduled = new ScheduledTestModel();
                                sApptScheduled.ScheduledTestType = r.services;
                                sApptScheduled.ScheduledDateTime = dtAppt;
                                sApptScheduled.ScheduledBy = r.createdby;
                                sApptScheduled.PatientID = r.patientid;
                                sApptScheduled.ScheduleUniqueID = r.uniqueid;
                                sApptScheduled.CreatedDate = DateTime.Now;
                                sApptScheduled.CreatedBy = r.createdby;

                                ScheduledTestRepository.InsertScheduledTest(config, sApptScheduled);
                            }
                        }

                        ProcessingLogModel sLog =  new ProcessingLogModel();
                        sLog.ProcessingTaskName = processName;
                        sLog.ProcessingStartDate = sStart;
                        sLog.ProcessingEndDate = sEnd;
                        sLog.CreatedDate = DateTime.Now;
                        sLog.CreatedBy = "VCheck Appointment Synch Task";

                        ProcessingLogRepository.InsertProcessingLog(sLog, config);
                    }
                    else
                    {
                        log.Error("VCheckAppointmentSynchTask >>> AppointmentUpdateSynch (" + sTransType + ") >>> " +
                                  "Get Appointment Failed ---> <br/>" +
                                  "code : " + sResp.body.responsecode + "<br/>" +
                                  "status : " + sResp.body.responsemessage + "<br />");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("VCheckAppointmentSynchTask >>> AppointmentUpdateSynch (" + sTransType + ") >>> " + ex.ToString());
            }
        }
    }
}
