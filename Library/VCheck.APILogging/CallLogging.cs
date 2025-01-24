using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SQLite;
using Microsoft.Extensions.Configuration;

namespace VCheck.APILogging
{
    public class CallLogging
    {
        static System.Object lockFile = new System.Object();
        static String sAPILogFile = "api-call";
        static String sAPIErrorLogFile = "api-error";
        static String sAPITemplateLog = "vcheckAPILog.db";
        static String sErrorTemplateLog = "vcheckAPIErrorLog.db";
        static IConfiguration? config;

        /// <summary>
        /// Insert API Request & Response Payload
        /// </summary>
        /// <param name="APIName"></param>
        /// <param name="logID"></param>
        /// <param name="sRequestDate"></param>
        /// <param name="sRequestPayload"></param>
        /// <param name="sResponseDate"></param>
        /// <param name="sResponsePayload"></param>
        /// <param name="sResponseCode"></param>
        /// <param name="sResponseStatus"></param>
        /// <param name="sResponseMessage"></param>
        /// <param name="BranchID"></param>
        public static void InsertAPiLog(String APIName, String logID, String sRequestDate, String sRequestPayload, 
                                        String sResponseDate, String sResponsePayload, String sResponseCode, String sResponseStatus,
                                        String sResponseMessage, int? BranchID = null)
        {
            String sLogFilePath = GetLogPath(sAPILogFile, sAPITemplateLog);

            String sConnectionString = String.Format("data source=\"{0}\";Synchronous=Full;Pooling=True;Max Pool Size=50;Compress=True", sLogFilePath);

            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
            {
                conn.ConnectionString = sConnectionString;
                conn.Open();

                String sInsertCommand = "INSERT INTO ApiLog(AutoID, LogDate, RequestAPIName, RequestDate, RequestPayload, ResponseDate, ResponsePayload, BranchID, " + 
                                        "ResponseCode, ResponseStatus, ResponseMessage) " +
                                        "VALUES('" + logID + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + "', '" + APIName + "', " + 
                                        "'" + sRequestDate + "', '" + sRequestPayload + "', '" + sResponseDate + "', '" + sResponsePayload + "', " + 
                                        "'" + BranchID + "', '" + sResponseCode + "', '" + sResponseStatus + "', '" + sResponseMessage + "')";

                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sInsertCommand;
                    cmd.ExecuteNonQuery();
                }
                
                conn.Close();
            }
        }

        /// <summary>
        /// Insert API Exception Error
        /// </summary>
        /// <param name="APIName"></param>
        /// <param name="logID"></param>
        /// <param name="sResponseCode"></param>
        /// <param name="sResponseStatus"></param>
        /// <param name="sResponseMessage"></param>
        /// <param name="sDetailsMessage"></param>
        /// <param name="BranchID"></param>
        public static void InsertErrorLog(String APIName, String logID, String sResponseCode, String sResponseStatus, String sResponseMessage, 
                                         String sDetailsMessage, int? BranchID = null)
        {
            String sErrorLogFilePath = GetLogPath(sAPIErrorLogFile, sErrorTemplateLog);

            String sConnectionString = String.Format("data source=\"{0}\";Synchronous=Full;Pooling=True;Max Pool Size=50;Compress=True", sErrorLogFilePath);

            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
            {
                conn.ConnectionString = sConnectionString;
                conn.Open();

                String sInsertCommand = "INSERT INTO ErrorLog(AutoID, LogDate, APIName, BranchID, ResponseCode, ResponseStatus, ResponseMessage, DetailsMessage) " +
                                        "VALUES('" + logID + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + "', '" + APIName + "', " +
                                        "'" + BranchID + "', '" + sResponseCode + "', '" + sResponseStatus + "', '" + sResponseMessage + "', '" + sDetailsMessage + "')";

                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sInsertCommand;
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        /// <summary>
        /// Get file path
        /// </summary>
        /// <returns></returns>
        public static String GetLogPath(String LogFilePrefix, String TemplateFile)
        {
            InitiateConfigFile();

            String sLogFilePath = config.GetSection("APILogging:dbapilog_filepath").Value;
            System.IO.Directory.CreateDirectory(sLogFilePath);
            sLogFilePath = System.IO.Path.Combine(sLogFilePath, String.Format("{0}-{1}.sqlite", LogFilePrefix, DateTime.Now.ToString("yyyyMMddHH")));
            //sLogFilePath = System.IO.Path.Combine(sLogFilePath, String.Format("{0}-{1}.sqlite", "api-call", DateTime.Now.ToString("yyyyMMddHH")));

            lock (lockFile)
            {
                if (System.IO.File.Exists(sLogFilePath) == false)
                {
                    String sLocalDBPath = config.GetSection("APILogging:dbtemplate_filepath").Value;
                    //sLocalDBPath = System.IO.Path.Combine(sLocalDBPath, "vcheckAPILog.db");
                    sLocalDBPath = System.IO.Path.Combine(sLocalDBPath, TemplateFile);
                    System.IO.File.Copy(sLocalDBPath, sLogFilePath);
                }
            }

            return sLogFilePath;
        }

        /// <summary>
        /// initiate Configuration Settings
        /// </summary>
        public static void InitiateConfigFile()
        {
            var sBuilder = new ConfigurationBuilder();
            sBuilder.SetBasePath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location))
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            config = sBuilder.Build();
        }
    }


}
