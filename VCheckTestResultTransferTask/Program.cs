using System.IO.IsolatedStorage;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using VCheck.Lib.Data;
using VCheck.Lib.Data.DBContext;
using System.Data;
using log4net.Config;
using log4net.Repository;
using VCheck.Lib.Data.Models;

namespace VCheckTestResultTransferTask
{
    public class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static String? sProcessingName = "GenerateTestResultInFile";

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

                GenerateResultToFile(config);
            }
        }

        /// <summary>
        /// Main Logic 
        /// </summary>
        /// <param name="config"></param>
        private static void GenerateResultToFile(IConfiguration config)
        {
            String? sFTPURL = config.GetSection("FTPInfo:ServerUrl").Value;
            String? sFTPUsername = config.GetSection("FTPInfo:FTPUsername").Value;
            String? sFTPPassword = config.GetSection("FTPInfo:FTPPassword").Value;

            try
            {
                Console.WriteLine("---------------- Start Generate Test Result to File -----------------------------");
                log.Info("---------------- Start Generate Test Result to File -----------------------------");

                // --------- Retrieve Last Processing Log ---------------- //
                DateTime dtStart = DateTime.MinValue;
                DateTime dtEnd = DateTime.Now;
                var sProcessingLogObj = ProcessingLogRepository.GetProcessingLogByName(sProcessingName, config);
                if (sProcessingLogObj != null)
                {
                    dtStart = sProcessingLogObj.ProcessingEndDate.Value;
                }
                else
                {
                    dtStart = DateTime.Now.AddMonths(-3);
                }


                String sDirectoryPath = System.IO.Directory.GetCurrentDirectory();
                String sFileName = "TestResultUpdate_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                String sFilePath = System.IO.Path.Combine(sDirectoryPath, "Processing", sFileName);
                String sFileArchivedPath = System.IO.Path.Combine(sDirectoryPath, "Processing\\Archived");

				List<TestResultOutputFileModel> sResult = TestResultsRepository.GetTestResultByDateRange(config, dtStart, dtEnd);
                if (sResult != null)
                {
                    DataTable sResultTable = Lib.General.Utils.ToDataTable(sResult);
                    sResultTable.Columns["ID"].SetOrdinal(0);
                    sResultTable.Columns["TestResultDateTime"].SetOrdinal(1);
                    sResultTable.Columns["TestResultType"].SetOrdinal(2);
					sResultTable.Columns["DeviceSerialNo"].SetOrdinal(3);
					sResultTable.Columns["PatientID"].SetOrdinal(4);
                    sResultTable.Columns["InchargePerson"].SetOrdinal(5);
                    sResultTable.Columns["OperatorID"].SetOrdinal(6);
					sResultTable.Columns["TestParameter"].SetOrdinal(7);
					sResultTable.Columns["TestResultStatus"].SetOrdinal(8);
					sResultTable.Columns["TestResultValue"].SetOrdinal(9);
					sResultTable.Columns["TestResultUnit"].SetOrdinal(10);
                    sResultTable.Columns["ReferenceRange"].SetOrdinal(11);

                    sResultTable.Columns.Remove("CreatedDate");
					sResultTable.Columns.Remove("CreatedBy");
					sResultTable.Columns.Remove("UpdatedDate");
					sResultTable.Columns.Remove("UpdatedBy");
					sResultTable.Columns.Remove("ObservationStatus");
					sResultTable.Columns.Remove("OverallStatus");
					sResultTable.Columns.Remove("TestResultRules");

					Lib.General.Utils.ExportToCSV(sResultTable, sFilePath);

                    // ----- Output to file -------//
                    if (System.IO.File.Exists(sFilePath))
                    {
                        try
                        {
                            // ---------- FTP Upload ---------//
                            FtpStatusCode sResultCode = VCheck.Helper.FTPLib.UploadFile(sFilePath, sFTPURL, sFTPUsername, sFTPPassword);
                            if (sResultCode == FtpStatusCode.ClosingData)
                            {
                                if (!Directory.Exists(sFileArchivedPath))
                                {
                                    Directory.CreateDirectory(sFileArchivedPath);
                                }

                                System.IO.File.Move(sFilePath, System.IO.Path.Combine(sFileArchivedPath, Path.GetFileName(sFilePath)));

                                // --------- Insert Log ----------------//
                                ProcessingLogModel sProcessingLog = new ProcessingLogModel();
                                sProcessingLog.ProcessingTaskName = sProcessingName;
                                sProcessingLog.ProcessingStartDate = dtStart;
                                sProcessingLog.ProcessingEndDate = dtEnd;
                                sProcessingLog.CreatedDate = DateTime.Now;
                                sProcessingLog.CreatedBy = "VCheckTestResultTransferTask";
                                ProcessingLogRepository.InsertProcessingLog(sProcessingLog, config);
                            }
                            else
                            {
                                log.Error("Upload FTP Result Failed : " + sResultCode.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("VCheckTestResultTransferTask >>> GenerateResultToFile >>> Upload Exception >>> " + ex.ToString());
                        }
                    }
                }

                Console.WriteLine("---------------- Generate Test Result to File Completed. ---------------------------------");
                log.Info("---------------- Generate Test Result to File Completed. ---------------------------------");
            }
            catch (Exception ex)
            {
                log.Error("VCheckTestResultTransferTask >>> GenerateResultToFile >>> " + ex.ToString());
            }
        }
    }
}
