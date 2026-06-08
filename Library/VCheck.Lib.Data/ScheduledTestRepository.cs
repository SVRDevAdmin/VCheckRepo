using Microsoft.Extensions.Configuration;
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
using System.Xml;

namespace VCheck.Lib.Data
{
    public class ScheduledTestRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        private static DateTime MinimumDatetime = DateTime.Now.ToUniversalTime().AddHours(-48);
        private static string[] TwoWayCommAnalyzers = { "C10", "H6" };

        /// <summary>
        /// Get upcoming scheduled tests
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<ScheduledTestModel> GetCurrentScheduledTestList(IConfiguration config)
        {
            DateTime MinimumDatetime = DateTime.Now.AddHours(-48);

            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    return ctx.Txn_ScheduledTests.Where(x => x.ScheduledDateTime >= MinimumDatetime && x.ScheduleTestStatus == 0)
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
        public static ScheduledTestModel GetScheduledTestByUniqueID(IConfiguration config, String sUniqueID, String? sPatientID = "")
        {
            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    if (string.IsNullOrEmpty(sPatientID))
                    {
                        return ctx.Txn_ScheduledTests.Where(x => x.ScheduleUniqueID == sUniqueID).FirstOrDefault();
                    }
                    else if (string.IsNullOrEmpty(sUniqueID))
                    {
                        return ctx.Txn_ScheduledTests.FirstOrDefault(x => x.PatientID == sPatientID && x.ScheduleTestStatus == 0);
                    }
                    else if (sPatientID == "Unique")
                    {
                        return ctx.Txn_ScheduledTests.FirstOrDefault(x => x.ScheduleUniqueID.Substring(x.ScheduleUniqueID.Length - 8) == sUniqueID && x.ScheduleTestStatus != 2);
                    }
                    else
                    {
                        return ctx.Txn_ScheduledTests.Where(x => x.ScheduleUniqueID.Substring(x.ScheduleUniqueID.Length - 8) == sUniqueID && x.PatientID == sPatientID).OrderByDescending(y => y.CreatedDate).LastOrDefault();
                    }
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
        public static ScheduledTestModel GetScheduledTestByOrderID(IConfiguration config, String sOrderID, String? sClientName, String? sLocationID)
        {
            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    if (string.IsNullOrEmpty(sLocationID))
                    {
                        return ctx.Txn_ScheduledTests.Where(x => x.CreatedBy == sClientName).AsEnumerable().FirstOrDefault(y => y.ScheduleUniqueID.Split("-")[1] == sOrderID);
                    }
                    else
                    {
                        return ctx.Txn_ScheduledTests.Where(x => x.CreatedBy == sClientName && x.LocationID == sLocationID).AsEnumerable().FirstOrDefault(y => y.ScheduleUniqueID.Split("-")[1] == sOrderID);
                    }

                        

                    //return ctx.Txn_ScheduledTests.Where(x => x.ScheduleUniqueID.Substring(x.ScheduleUniqueID.IndexOf("-")+1, x.ScheduleUniqueID.Length - 15) == sOrderID).FirstOrDefault();
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

                        if (sData.ScheduleTestStatus != sScheduledTestObj.ScheduleTestStatus)
                        {
                            sData.ScheduleTestStatus = sScheduledTestObj.ScheduleTestStatus;
                        }

                        if (sData.SentToAnalyzer != sScheduledTestObj.SentToAnalyzer)
                        {
                            sData.SentToAnalyzer = sScheduledTestObj.SentToAnalyzer;
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

        /// <summary>
        /// Update Scheduled Test Information by Order ID
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sScheduledTestObj"></param>
        /// <returns></returns>
        public static Boolean UpdateScheduledTestStatus(IConfiguration config, ScheduledTestModel scheduledTest, string sUpdatedBy)
        {
            Boolean isSuccess = false;

            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    //var sData = ctx.Txn_ScheduledTests.Where(x => x.CreatedBy == sClientName && x.LocationID == sLocationID).AsEnumerable().Where(y => y.ScheduleUniqueID.Split("-")[1] == sOrderID);
                    //if (sData != null)
                    //{
                    //    foreach(var sSchedule in sData)
                    //    {
                    //        if (sSchedule.ScheduleTestStatus != sStatus)
                    //        {
                    //            sSchedule.ScheduleTestStatus = sStatus;
                    //            sSchedule.UpdatedDate = DateTime.Now;
                    //            sSchedule.UpdatedBy = sUpdatedBy;
                    //        }
                    //    }

                    //    ctx.Update(scheduledTest);

                    //    ctx.SaveChanges();

                    //    isSuccess = true;
                    //}

                    ctx.Update(scheduledTest);

                    ctx.SaveChanges();

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Insert new Scheduled Test 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sScheduledTestObj"></param>
        /// <returns></returns>
        public static Boolean InsertUpdateScheduledTest(IConfiguration config, ScheduledTestModel sScheduledTestObj)
        {
            Boolean isSuccess = false;

            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    ctx.Txn_ScheduledTests.Update(sScheduledTestObj);
                    ctx.SaveChanges();

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Get Scheduled Test  Info by Unique ID
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sUniqueID"></param>
        /// <returns></returns>
        public static ScheduledTestModel GetScheduledTestByID(IConfiguration config, long sID)
        {
            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    return ctx.Txn_ScheduledTests.Where(x => x.ID == sID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get Scheduled Test List Info by Location ID
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sUniqueID"></param>
        /// <returns></returns>
        public static List<ScheduledTestModel> GetScheduleListByLocation(IConfiguration config, string locationID)
        {
            DateTime MinimumDatetime = DateTime.Now.ToUniversalTime().AddHours(-48);
            var sBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            int dataRowLimit = sBuilder.Configuration.GetValue<int>("DataRowLimit");

            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    return ctx.Txn_ScheduledTests.Where(x => x.ScheduledDateTime >= MinimumDatetime && x.LocationID == locationID)
                                                .OrderByDescending(x => x.CreatedDate)
                                                .Take(dataRowLimit)
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
        /// Get Scheduled Test List Info by Location ID
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sUniqueID"></param>
        /// <returns></returns>
        public static List<ScheduledTestModel> GetNotCompletedScheduleListByLocation(IConfiguration config, string locationID, string testName, bool ExtendDateTime)
        {
            DateTime MinimumDatetime = DateTime.Now.ToUniversalTime();
            var sBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            int dataRowLimit = sBuilder.Configuration.GetValue<int>("DataRowLimit");

            if (ExtendDateTime)
            {
                MinimumDatetime = MinimumDatetime.AddDays(-10);
            }
            else
            {
                MinimumDatetime = MinimumDatetime.AddHours(-48);
            }

            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    var scheduleList = ctx.Txn_ScheduledTests.Where(x => x.ScheduledDateTime >= MinimumDatetime && x.LocationID == locationID && x.ScheduleTestStatus != 2).ToList();

                    scheduleList = ExtendDateTime ? scheduleList : scheduleList.Where(x => x.TestCompleted == 0).ToList();

                    return scheduleList.AsEnumerable().Where(x => x.ScheduledTestType.Split(", ").Contains(testName))
                                    .OrderBy(x => x.ScheduledDateTime)
                                    .Take(dataRowLimit)
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
        /// Get Scheduled Test List Info by Location ID that espired
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sUniqueID"></param>
        /// <returns></returns>
        public static List<ScheduledTestModel> GetExpiredScheduleListByLocation(IConfiguration config, string locationID)
        {
            DateTime MinimumDatetime = DateTime.Now.ToUniversalTime().AddHours(-48);

            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    var expiredOrder = ctx.Txn_ScheduledTests.Where(x => x.ScheduledDateTime < MinimumDatetime && x.LocationID == locationID && x.TestCompleted == 0 && x.ScheduleTestStatus < 3).ToList();

                    foreach (var item in expiredOrder)
                    {
                        item.TestCompleted = 1;
                    }

                    ctx.UpdateRange(expiredOrder);
                    ctx.SaveChanges();

                    return expiredOrder;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get Not Sent Scheduled Test List Info by Location ID
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sUniqueID"></param>
        /// <returns></returns>
        public static List<ScheduledTestModelExtended> GetScheduleListByLocationNotSent(IConfiguration config, string locationID, string uniqueID = null, bool ignoreOrderStatus = false)
        {
            //DateTime MinimumDatetime = DateTime.Now.ToUniversalTime().AddHours(-48);
            List<ScheduledTestModelExtended> scheduledTestModelExtendeds = new List<ScheduledTestModelExtended>();

            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    List<ScheduledTestModel> ScheduleList = new List<ScheduledTestModel>();

                    if (string.IsNullOrEmpty(uniqueID))
                    {
                        if (ignoreOrderStatus)
                        {
                            ScheduleList = ctx.Txn_ScheduledTests.Where(x => x.ScheduledDateTime >= MinimumDatetime && x.ScheduleTestStatus != 2 && x.LocationID == locationID && x.TestCompleted == 0).ToList();
                        }
                        else
                        {
                            ScheduleList = ctx.Txn_ScheduledTests.Where(x => x.ScheduledDateTime >= MinimumDatetime && x.ScheduleTestStatus == 0 && x.LocationID == locationID && x.TestCompleted == 0).ToList();
                        }
                    }
                    else
                    {
                        ScheduleList = ctx.Txn_ScheduledTests.Where(x => x.ScheduleUniqueID.Substring(x.ScheduleUniqueID.Length - 8) == uniqueID).ToList();
                    }


                    foreach (var schedule in ScheduleList)
                    {
                        //var parameters =  TestResultsRepository.GetAllTestParameterByTestName(config, schedule.ScheduledTestType.Split(","));
                        //ScheduledTestModelExtended scheduledExtended = new ScheduledTestModelExtended() { Schedule = schedule, Parameters = parameters };
                        //var testIDAnalyzers = TestResultsRepository.GetAllAnalyzerByTestName(config, schedule.ScheduledTestType.Split(", "));
                        //ScheduledTestModelExtended scheduledExtended = new ScheduledTestModelExtended() { Schedule = schedule, IDAnalyzers = testIDAnalyzers };

                        string[] sentAnalyzer = Array.Empty<string>();
                        sentAnalyzer = schedule.SentToAnalyzer!= null ? schedule.SentToAnalyzer.Split(",") : Array.Empty<string>();
                        var testIDAnalyzers = TestResultsRepository.GetAllAnalyzerByTestName(config, schedule.ScheduledTestType.Split(", "));
                        testIDAnalyzers = testIDAnalyzers != null && testIDAnalyzers.Count() != 0 ? testIDAnalyzers.Where(x => !sentAnalyzer.Contains(x.Analyzers) && TwoWayCommAnalyzers.Contains(x.Analyzers)).ToList() : testIDAnalyzers;

                        if(testIDAnalyzers != null && testIDAnalyzers.Count() != 0)
                        {
                            ScheduledTestModelExtended scheduledExtended = new ScheduledTestModelExtended() { Schedule = schedule, IDAnalyzers = testIDAnalyzers };

                            scheduledTestModelExtendeds.Add(scheduledExtended);
                        }
                    }                    
                }               

                return scheduledTestModelExtendeds;
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get Scheduled Test List Info by Location ID & Patient ID
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sUniqueID"></param>
        /// <returns></returns>
        //public static ScheduledTestModel GetScheduleByLocationPatientID(IConfiguration config, string locationID, string patientID, string[] testType)
        public static ScheduledTestModel GetScheduleByLocationPatientID(IConfiguration config, string locationID, string patientID, string testName)
        {
            try
            {
                using (var ctx = new ScheduleDBContext(config))
                {
                    var scheduleList = ctx.Txn_ScheduledTests.Where(x => x.ScheduledDateTime >= MinimumDatetime && x.PatientID == patientID && x.LocationID == locationID && x.TestCompleted == 0);
                    var schedule = scheduleList.AsEnumerable().OrderByDescending(y => y.CreatedDate).FirstOrDefault(x => x.ScheduledTestType.Split(", ").Contains(testName));

                    //var schedule = ctx.Txn_ScheduledTests.OrderByDescending(y => y.CreatedDate).FirstOrDefault(x => x.ScheduledDateTime >= MinimumDatetime && x.PatientID == patientID && x.LocationID == locationID && x.TestCompleted == 0 && x.ScheduledTestType == testName);

                    return schedule;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }
    }
}
