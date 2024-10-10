using System.IO.IsolatedStorage;
using Microsoft.Extensions.Configuration;
using VCheck.Interface.API;
using VCheck.Lib.Data;
using VCheck.Lib.Data.Models;
using VetConnect = VCheck.Interface.API.VetConnect;
using log4net.Repository;
using log4net.Config;

namespace VCheckTestResultDataSynchTask
{
    internal class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static String? sProcessingUpdateTestResults = "SynchTestResultsDataUpdate";

        static void Main(string[] args)
        {
            ILoggerRepository repository = log4net.LogManager.GetRepository(System.Reflection.Assembly.GetCallingAssembly());
            log4net.Config.XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            if ((System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                var sBuilder = new ConfigurationBuilder();
                sBuilder.SetBasePath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                IConfiguration config = sBuilder.Build();

                log.Info("------------- Start Update Test Results ------------------------");
                UpdateTestResults(config, sProcessingUpdateTestResults);
                log.Info("------------- Update Test Results completed --------------------");
            }
        }

        private static void UpdateTestResults(IConfiguration config, String sProcessName)
        {
            List<VetConnect.UpdateTestResultsRequestBodyResultObject> sResultList = new List<VetConnect.UpdateTestResultsRequestBodyResultObject>();
            VetConnect.UpdateTestResultsRequest sReq = new VetConnect.UpdateTestResultsRequest();
            VetConnect.UpdateTestResultsRequestHeaderObject sHeader = new VetConnect.UpdateTestResultsRequestHeaderObject();
            VetConnect.UpdateTestResultsRequestBodyObject sBody = new VetConnect.UpdateTestResultsRequestBodyObject();

            VetConnectAPI sVPMSAPI = new VetConnectAPI();
            DateTime sStart = DateTime.Now.AddMonths(-12);
            DateTime sEnd = DateTime.Now;

            try
            {
                String? sToken = config.GetSection("VetConnect:clientkey").Value;

                var sProcessingLog = ProcessingLogRepository.GetProcessingLogByName(sProcessName, config);
                if (sProcessingLog != null)
                {
                    sStart = sProcessingLog.ProcessingEndDate.Value;
                }

                var sResult = TestResultsRepository.GetTestResultDetailsByDateRange(config, sStart, sEnd);
                if (sResult != null)
                {
                    foreach(var sRow in sResult)
                    {
                        sResultList.Add(new VetConnect.UpdateTestResultsRequestBodyResultObject
                        {
                            resulttype = sRow.TestResultType,
                            resultdatetime = sRow.TestResultDateTime.Value.ToString("yyyyMMddHHmmss"),
                            operatorid = sRow.OperatorID,
                            patientid = sRow.PatientID,
                            ownerid = "",
                            petid = "",
                            inchargedoctor = sRow.InchargePerson,
                            resultstatus  = sRow.TestResultStatus,
                            resultvalue = sRow.TestResultValue.ToString(),
                            resultparameter = sRow.TestResultRules,
                            referencerange = ""
                        });
                    }

                    sHeader.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                    sHeader.authtoken = sToken;
                    sBody.results = sResultList;

                    sReq.header = sHeader;
                    sReq.body = sBody;

                    log.Info("------------- Send Test Results data to PMS Begin --------------");
                    var sResp = sVPMSAPI.UpdateTestResults(sReq);
                    if (sResp != null)
                    {
                        if (sResp.body.responsecode == "VPMS.0001")
                        {
                            log.Info("Test results sent successfully");
                        }
                        else
                        {
                            log.Info("Response Code : " + sResp.body.responsecode);
                            log.Info("Test results sent unsuccess, please contact admin.");
                        }
                    }
                    else
                    {
                        log.Info("NUll result returned, please contact admin.");
                    }

                    log.Info("------------- Send Test Results data to PMS Completed --------------");
                }

                ProcessingLogModel sLog = new ProcessingLogModel();
                sLog.ProcessingTaskName = sProcessName;
                sLog.ProcessingStartDate = sStart;
                sLog.ProcessingEndDate = sEnd;
                sLog.CreatedDate = DateTime.Now;
                sLog.CreatedBy = "VCheck TestResults Data Synch Task";

                ProcessingLogRepository.InsertProcessingLog(sLog, config);
            }
            catch (Exception ex)
            {
                log.Info("Exception : " + ex.ToString());
            }
        }

    }
}
