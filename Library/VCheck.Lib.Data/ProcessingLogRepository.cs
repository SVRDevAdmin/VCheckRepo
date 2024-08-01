using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using log4net;

namespace VCheck.Lib.Data
{
    public class ProcessingLogRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Get Last Processing Log By Name
        /// </summary>
        /// <param name="sProcessingName"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static ProcessingLogModel? GetProcessingLogByName(String sProcessingName, IConfiguration config)
        {
            try
            {
                using (var ctx = new SystemProcessingLogDBContext(config))
                {
                    return ctx.System_ProcessingLog.Where(x => x.ProcessingTaskName == sProcessingName)
                                                   .OrderByDescending(x => x.ProcessingTaskName)
                                                   .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error("ProcessingLogRepository >>> GetProcessingLogByName >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Add new processing Log
        /// </summary>
        /// <param name="sModel"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool InsertProcessingLog(ProcessingLogModel sModel, IConfiguration config)
        {
            Boolean isSuccess = false;

            try
            {
                using (var ctx = new SystemProcessingLogDBContext(config))
                {
                    ctx.System_ProcessingLog.Add(sModel);
                    ctx.SaveChanges();

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("ProcessingLogRepository >>> InsertProcessingLog >>> " + ex.ToString());
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
