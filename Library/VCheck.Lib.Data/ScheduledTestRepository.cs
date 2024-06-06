﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;
using VCheck.Lib.Data.DBContext;
using log4net;
using System.Reflection;
using log4net;

namespace VCheck.Lib.Data
{
    public class ScheduledTestRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Get upcoming scheduled tests
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<ScheduledTestModel> GetCurrentScheduledTestList(IConfiguration config)
        {
            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    return ctx.Txn_ScheduledTests.Where(x => x.ScheduledDateTime >= DateTime.Now)
                                                .OrderBy(x => x.ScheduledDateTime)
                                                .Take(5)
                                                .ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get Scheduled Test  Info by Unique ID
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sUniqueID"></param>
        /// <returns></returns>
        public static ScheduledTestModel GetScheduledTestByUniqueID(IConfiguration config, String sUniqueID)
        {
            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    return ctx.Txn_ScheduledTests.Where(x => x.ScheduleUniqueID == sUniqueID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Update Scheduled Test Information by Unique ID
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sScheduledTestObj"></param>
        /// <returns></returns>
        public static Boolean UpdateScheduledTestByUniqueID(IConfiguration config, ScheduledTestModel sScheduledTestObj)
        {
            Boolean isSuccess = false;

            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    var sData = ctx.Txn_ScheduledTests.Where(x => x.ScheduleUniqueID == sScheduledTestObj.ScheduleUniqueID).FirstOrDefault();
                    if (sData != null)
                    {
                        if (sData.ScheduledDateTime != sScheduledTestObj.ScheduledDateTime)
                        {
                            sData.ScheduledDateTime = sScheduledTestObj.ScheduledDateTime;
                        }

                        if (sData.InchargePerson != sScheduledTestObj.InchargePerson)
                        {
                            sData.InchargePerson = sScheduledTestObj.InchargePerson;
                        }

                        ctx.SaveChanges();

                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
