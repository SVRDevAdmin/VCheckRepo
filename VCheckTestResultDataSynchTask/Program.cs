using System.IO.IsolatedStorage;
using Microsoft.Extensions.Configuration;
using VCheck.Interface.API;
using VCheck.Lib.Data;
using VCheck.Lib.Data.Models;
using VetVitals = VCheck.Interface.API.VetVitals;
using log4net.Repository;
using log4net.Config;
using VCheck.Interface.API.VetVitals.RequestMessage;

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
            List<VetVitals.RequestMessage.UpdateTestResultsRequestBodyResultObject> sResultList = new List<VetVitals.RequestMessage.UpdateTestResultsRequestBodyResultObject>();
            VetVitals.RequestMessage.UpdateTestResultsRequest sReq = new VetVitals.RequestMessage.UpdateTestResultsRequest();
            VetVitals.RequestMessage.UpdateTestResultsRequestHeaderObject sHeader = new VetVitals.RequestMessage.UpdateTestResultsRequestHeaderObject();
            VetVitals.RequestMessage.UpdateTestResultsRequestBodyObject sBody = new VetVitals.RequestMessage.UpdateTestResultsRequestBodyObject();

            VetVitalsAPI sVPMSAPI = new VetVitalsAPI();
            DateTime sStart = DateTime.Now.AddMonths(-12);
            DateTime sEnd = DateTime.Now;

            try
            {
                String? sToken = config.GetSection("VetVitals:clientkey").Value;

                var sProcessingLog = ProcessingLogRepository.GetProcessingLogByName(sProcessName, config);
                if (sProcessingLog != null)
                {
                    sStart = sProcessingLog.ProcessingEndDate.Value;
                }

                var sResult = TestResultsRepository.GetTestResultDetailsByDateRange(config, sStart, sEnd);
                if (sResult != null)
                {
                    var sResultIDs = sResult.GroupBy(x => x.ID).Select(x => new { x.Key }).ToList();
                    foreach(var x in sResultIDs)
                    {
                        var sTestResult = sResult.Where(y => y.ID == x.Key).FirstOrDefault();
                        if (sTestResult != null)
                        {
                            List<UpdateTestResultsDetailsObject> sResultDetList = new List<UpdateTestResultsDetailsObject>();

                            var sTestResultDet = sResult.Where(y => y.ID == x.Key).ToList();
                            foreach(var itm in sTestResultDet)
                            {
                                sResultDetList.Add(new UpdateTestResultsDetailsObject
                                {
                                    resultparameter = itm.TestResultParameter,
                                    resultstatus = itm.TestResultStatus,
                                    resultvalue = itm.TestResultValue,
                                    resultunit = itm.TestResultUnit,
                                    referencerange = itm.ReferenceRange
                                });
                            }

                            sResultList.Add(new VetVitals.RequestMessage.UpdateTestResultsRequestBodyResultObject
                            {
                                resulttype = sTestResult.TestResultType,
                                resultdatetime = sTestResult.TestResultDateTime.Value.ToString("yyyyMMddHHmmss"),
                                operatorid = sTestResult.OperatorID,
                                patientid = sTestResult.PatientID,
                                petid = sTestResult.PatientID,
                                inchargeperson = sTestResult.InchargePerson,
                                overallstatus = sTestResult.OverallStatus,
                                devicename = sTestResult.DeviceSerialNo,
                                resultdetails = sResultDetList
                            });
                        }
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
