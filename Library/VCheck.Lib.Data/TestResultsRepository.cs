﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;
using VCheck.Lib.Data.DBContext;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Runtime.CompilerServices;
using System.CodeDom;
using log4net;
using System.Reflection;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace VCheck.Lib.Data
{
    public class TestResultsRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Get recent test result details
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<TestResultExtendedModel> GetLatestTestResultDetailsList(IConfiguration config)
        {
            List<TestResultExtendedModel> sResultList = new List<TestResultExtendedModel>();

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    MySqlConnection sConn = new MySqlConnection(ctx.Database.GetConnectionString());
                    sConn.Open();

                    String sSelectCommand = "SELECT T1.ID, T1.TestResultDateTime, T1.TestResultType, T1.OperatorID, T1.PatientID, " +
                                            "T1.DeviceSerialNo, T1.InchargePerson,  T2.ProceduralControl, T2.TestResultStatus, " +
                                            "T2.TestResultValue, T2.TestResultUnit, T1.CreatedDate, T1.CreatedBy, T1.UpdatedDate, " +
                                            "T1.UpdatedBy " +
                                            "FROM txn_testresults AS T1 " +
                                            "INNER JOIN txn_testresults_details AS T2 ON T2.TestResultRowID = T1.ID " +
                                            "ORDER BY T1.TestResultDateTime DESC ";

                    using (MySqlCommand sCommand = new MySqlCommand(sSelectCommand, sConn))
                    {
                        using (var sReader = sCommand.ExecuteReader())
                        {
                            while (sReader.Read())
                            {
                                TestResultExtendedModel sResult = new TestResultExtendedModel();
                                sResult.ID = Convert.ToInt64(sReader["ID"]);
                                sResult.TestResultDateTime = Convert.ToDateTime(sReader["TestResultDateTime"]);
                                sResult.TestResultType = sReader["TestResultType"].ToString();
                                sResult.OperatorID = sReader["OperatorID"].ToString();
                                sResult.PatientID = sReader["PatientID"].ToString();
                                sResult.DeviceSerialNo = sReader["DeviceSerialNo"].ToString();
                                sResult.InchargePerson = sReader["InchargePerson"].ToString();
                                sResult.ObservationStatus = sReader["ProceduralControl"].ToString();
                                sResult.TestResultStatus = sReader["TestResultStatus"].ToString();
                                sResult.TestResultValue = sReader["TestResultValue"].ToString();
                                sResult.TestResultRules = sReader["TestResultUnit"].ToString();

                                if (!String.IsNullOrEmpty(sReader["CreatedDate"].ToString()))
                                {
                                    sResult.CreatedDate = Convert.ToDateTime(sReader["CreatedDate"]);
                                }
                                sResult.CreatedBy = sReader["CreatedBy"].ToString();

                                if (!String.IsNullOrEmpty(sReader["UpdatedDate"].ToString()))
                                {
                                    sResult.UpdatedDate = Convert.ToDateTime(sReader["UpdatedDate"]);
                                }
                                sResult.UpdatedBy = sReader["UpdatedDate"].ToString();


                                sResultList.Add(sResult);
                            }
                        }
                    }

                    sConn.Close();

                    return sResultList.Take(5).ToList();     
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }

        public static List<TestResultModel> GetLatestTestResultList(IConfiguration config)
        {
            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    return ctx.txn_testResults.OrderByDescending(x => x.CreatedDate).Take(5).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get today's completed tests
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<TestResultModel> GetTodayTestResultList(IConfiguration config)
        {

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    return ctx.txn_testResults.Where(x => x.TestResultDateTime.Value.Date == DateTime.Now.Date)
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
        /// Get today's completed tests
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<TestResultModel> GetAllTestResultList(IConfiguration config)
        {

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    return ctx.txn_testResults.ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get Test Result by Test Date
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sResultDate"></param>
        /// <returns></returns>
        public static List<TestResultExtendedModel> GetTestResultByDates(IConfiguration config, DateTime sResultDate)
        {
            List<TestResultExtendedModel> sResultList = new List<TestResultExtendedModel>();

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    MySqlConnection sConn = new MySqlConnection(ctx.Database.GetConnectionString());
                    sConn.Open();

                    String sSelectCommand = "SELECT T1.ID, T1.TestResultDateTime, T1.TestResultType, T1.OperatorID, T1.PatientID, " +
                                            "T1.DeviceSerialNo, T1.InchargePerson,  T2.ProceduralControl, T2.TestResultStatus, " + 
                                            "T2.TestResultValue, T2.TestResultUnit, T1.CreatedDate, T1.CreatedBy, T1.UpdatedDate, " + 
                                            "T1.UpdatedBy " +
                                            "FROM txn_testresults AS T1 " +
                                            "INNER JOIN txn_testresults_details AS T2 ON T2.TestResultRowID = T1.ID " +
                                            "WHERE DATE(T1.TestResultDateTime) = '" + sResultDate.ToString("yyyy-MM-dd") + "' ";

                    using (MySqlCommand sCommand = new MySqlCommand(sSelectCommand, sConn))
                    {
                        using (var sReader = sCommand.ExecuteReader())
                        {
                            while (sReader.Read())
                            {
                                TestResultExtendedModel sResult = new TestResultExtendedModel();
                                sResult.ID = Convert.ToInt64(sReader["ID"]);
                                sResult.TestResultDateTime = Convert.ToDateTime(sReader["TestResultDateTime"]);
                                sResult.TestResultType = sReader["TestResultType"].ToString();
                                sResult.OperatorID = sReader["OperatorID"].ToString();
                                sResult.PatientID = sReader["PatientID"].ToString();
                                sResult.DeviceSerialNo = sReader["DeviceSerialNo"].ToString();
                                sResult.InchargePerson = sReader["InchargePerson"].ToString();
                                sResult.ObservationStatus = sReader["ProceduralControl"].ToString();
                                sResult.TestResultStatus = sReader["TestResultStatus"].ToString();
                                sResult.TestResultValue = sReader["TestResultValue"].ToString();
                                sResult.TestResultRules = sReader["TestResultUnit"].ToString();
                                
                                if (!String.IsNullOrEmpty(sReader["CreatedDate"].ToString()))
                                {
                                    sResult.CreatedDate = Convert.ToDateTime(sReader["CreatedDate"]);
                                }
                                sResult.CreatedBy = sReader["CreatedBy"].ToString();

                                if (!String.IsNullOrEmpty(sReader["UpdatedDate"].ToString()))
                                {
                                    sResult.UpdatedDate = Convert.ToDateTime(sReader["UpdatedDate"]);
                                }
                                sResult.UpdatedBy = sReader["UpdatedDate"].ToString();


                                sResultList.Add(sResult);
                            }
                        }
                    }

                    sConn.Close();

                    return sResultList;
                }
            }
            catch (Exception ex)
            {
                log.Error("TestResultRepository >>> GetTestResultByDates >>> " + ex.ToString());
                return null;
            }
        }
        //------- Version 1 -------- //
        //public static List<TestResultModel> GetTestResultByDates(IConfiguration config, DateTime sResultDate)
        //{
        //    try
        //    {
        //        using (var ctx = new TestResultDBContext(config))
        //        {
        //            return ctx.txn_testResults.Where(x => x.TestResultDateTime.Value.Date == sResultDate.Date).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("TestResultRepository >>> GetTestResultByDates >>> " + ex.ToString());
        //        return null;
        //    }
        //}

        /// <summary>
        /// Get Test Result by Test Date Range
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sStartDate"></param>
        /// <param name="sEndDate"></param>
        /// <returns></returns>
        public static List<TestResultOutputFileModel> GetTestResultByDateRange(IConfiguration config, DateTime sStartDate, DateTime sEndDate)
        {
            List<TestResultOutputFileModel> sResultList = new List<TestResultOutputFileModel>();

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    MySqlConnection sConn = new MySqlConnection(ctx.Database.GetConnectionString());
                    sConn.Open();

                    String sSelectCommand = "SELECT  A.ID, A.TestResultDateTime, A.TestResultType, A.PatientID, A.OperatorID, " +
                                            "A.DeviceSerialNo, A.OverallStatus, B.TestParameter, B.TestResultStatus, B.TestResultValue, " +
                                            "B.TestResultUnit, B.ReferenceRange, A.CreatedDate, A.CreatedBy " +
                                            "FROM txn_testresults AS A " + 
                                            "INNER JOIN txn_testresults_details AS B ON B.TestResultRowID = A.ID " + 
                                            "WHERE A.TestResultDateTime >= '" + sStartDate.ToString("yyyy-MM-dd HH:mm:ss")  + "' AND " +
                                            "A.TestResultDateTime <= '" + sEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; ";

                    using (MySqlCommand sCommand = new MySqlCommand(sSelectCommand, sConn))
                    {
                        using (var sReader = sCommand.ExecuteReader())
                        {
                            while (sReader.Read())
                            {
								TestResultOutputFileModel sResult = new TestResultOutputFileModel();
                                sResult.ID = Convert.ToInt64(sReader["ID"]);
                                sResult.TestResultDateTime = Convert.ToDateTime(sReader["TestResultDateTime"]);
                                sResult.TestResultType = sReader["TestResultType"].ToString();
                                sResult.PatientID = sReader["PatientID"].ToString();
                                sResult.OperatorID = sReader["OperatorID"].ToString();
                                sResult.DeviceSerialNo = sReader["DeviceSerialNo"].ToString();
                                //sResult.ObservationStatus = sReader["OverallStatus"].ToString();
                                sResult.TestParameter = sReader["TestParameter"].ToString();
                                sResult.TestResultStatus = sReader["TestResultStatus"].ToString();
                                //sResult.TestResultValue = sReader["TestResultValue"].ToString();
                                sResult.TestResultUnit = sReader["TestResultUnit"].ToString();
                                sResult.ReferenceRange = sReader["ReferenceRange"].ToString();

								sResultList.Add(sResult);
							}
                        }
                    }

                    sConn.Close();

                    return sResultList;
				}
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return null;
            }
        }


        /// <summary>
        /// Get Test results listing by search criteria
        /// </summary>
        /// <param name="config"></param>
        /// <param name="dStartDate"></param>
        /// <param name="dEndDate"></param>
        /// <param name="sKeyword"></param>
        /// <param name="sSortDirection"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public static List<TestResultListingExtendedObj> GetTestResultListBySearch(IConfiguration config, String dStartDate, String dEndDate, 
                                                                           String sKeyword, String sSortDirection, int pageIndex, int pageSize, string isPMSUser,
                                                                           out int totalRecords)
        {
            List<TestResultListingExtendedObj> sResult = new List<TestResultListingExtendedObj>();
            totalRecords = 0;
            int iIndex = 1;

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    using (MySqlConnection sConn = new MySql.Data.MySqlClient.MySqlConnection(ctx.Database.GetConnectionString()))
                    {
                        sConn.Open();

                        //String sSelectCommand = "SELECT * " +
                        //                        "FROM txn_testresults " +
                        //                        "WHERE (TestResultDateTime >= '" + dStartDate + "' AND TestResultDateTime <= '" + dEndDate + "')" +
                        //                        "AND (OperatorID LIKE '%" + sKeyword + "%' OR " +
                        //                        "InchargePerson LIKE '%" + sKeyword + "%' OR " +
                        //                        "PatientID LIKE '%" + sKeyword + "%') " +
                        //                        ((!sSortDirection.Contains("Status")) ? 
                        //                         "ORDER BY TestResultDateTime " + sSortDirection : 
                        //                         "ORDER BY TestResultStatus " + sSortDirection.Replace("Status", "").Trim());
                        // ----------- Version 1 -------------//
                        //String sSelectCommand = "SELECT * FROM txn_testresults WHERE ";

                        //if(dStartDate != "") { sSelectCommand = sSelectCommand + "(TestResultDateTime >= '" + dStartDate + "' AND TestResultDateTime <= '" + dEndDate + "') AND"; }

                        //sSelectCommand = sSelectCommand + "(OperatorID LIKE '%" + sKeyword + "%' OR " +
                        //                        "InchargePerson LIKE '%" + sKeyword + "%' OR " +
                        //                        "PatientID LIKE '%" + sKeyword + "%') " +
                        //                        ((!sSortDirection.Contains("Status")) ?
                        //                         "ORDER BY TestResultDateTime " + sSortDirection :
                        //                         "ORDER BY TestResultStatus " + sSortDirection.Replace("Status", "").Trim());

                        //------------- Version 2 --------------//
                        //String sSelectCommand = "SELECT T1.ID, T1.TestResultDateTime, T1.TestResultType, T1.OperatorID, " +
                        //                        "T1.PatientID, T1.InchargePerson, T2.ProceduralControl as 'ObservationStatus', " +
                        //                        "T2.TestResultStatus, T2.TestResultValue, T2.TestResultUnit as 'TestResultRules', " +
                        //                        "T1.CreatedDate, T1.CreatedBy " +
                        //                        "FROM txn_testresults as T1 INNER JOIN txn_testresults_details as T2 ON T2.TestResultRowID = T1.ID WHERE ";

                        //if (dStartDate != "") { sSelectCommand = sSelectCommand + "(T1.TestResultDateTime >= '" + dStartDate + "' AND T1.TestResultDateTime <= '" + dEndDate + "') AND"; }

                        //sSelectCommand = sSelectCommand + "(T1.OperatorID LIKE '%" + sKeyword + "%' OR " +
                        //                        "T1.InchargePerson LIKE '%" + sKeyword + "%' OR " +
                        //                        "T1.PatientID LIKE '%" + sKeyword + "%') " +
                        //                        ((!sSortDirection.Contains("Status")) ?
                        //                         "ORDER BY T1.TestResultDateTime " + sSortDirection :
                        //                         "ORDER BY T1.TestResultStatus " + sSortDirection.Replace("Status", "").Trim());

                        String sSelectCommand = "SELECT T1.ID, T1.TestResultDateTime, T1.TestResultType, T1.OperatorID, T1.PatientID, T1.PatientName, " +
                                                "T1.InchargePerson, T1.OverallStatus, T1.DeviceSerialNo, T1.CreatedDate, T1.CreatedBy " +
                                                "FROM txn_testresults AS T1 " +
                                                "WHERE ";

                        if (dStartDate != "") { sSelectCommand += "(T1.TestResultDateTime >= '" + dStartDate + "' AND T1.TestResultDateTime <= '" + dEndDate + "') AND "; }

                        sSelectCommand += "(T1.OperatorID LIKE '%" + sKeyword + "%' OR " +
                                          "T1.InchargePerson LIKE '%" + sKeyword + "%' OR " +
                                          "T1.PatientID LIKE '%" + sKeyword + "%') " +
                                          ((!sSortDirection.Contains("Status")) ?
                                          "ORDER BY T1.TestResultDateTime " + sSortDirection :
                                          "ORDER BY T1.TestResultStatus " + sSortDirection.Replace("Status", "").Trim());

                        using (MySqlCommand sCommand = new MySqlCommand(sSelectCommand, sConn))
                        {
                            using (var sReader = sCommand.ExecuteReader())
                            {
                                while (sReader.Read())
                                {
                                    sResult.Add(new TestResultListingExtendedObj
                                    {
                                        RowNo = iIndex,
                                        ID = sReader.GetInt64("ID"),
                                        TestResultDateTime = Convert.ToDateTime(sReader["TestResultDateTime"]),
                                        TestResultDateTimeString = (Convert.ToDateTime(sReader["TestResultDateTime"])).ToString("dd/MM/yyyy HH:mm"),
                                        TestResultType = sReader["TestResultType"].ToString(),
                                        OperatorID = sReader["OperatorID"].ToString(),
                                        PatientID = sReader["PatientID"].ToString(),
                                        PatientName = string.IsNullOrEmpty(sReader["PatientName"].ToString()) ? "-" : sReader["PatientName"].ToString(),
                                        InchargePerson = sReader["InchargePerson"].ToString(),
                                        TestResultStatus = sReader["OverallStatus"].ToString(),
                                        CreatedDate = Convert.ToDateTime(sReader["CreatedDate"]),
                                        CreatedBy = sReader["CreatedBy"].ToString(),
                                        statusBackground = (sReader["OverallStatus"].ToString() == "Normal" || 
                                                            sReader["OverallStatus"].ToString().Contains("titer")) ?  
                                                            "#D1F2EB" : "#F5B7B1",
                                        statusFontColor = (sReader["OverallStatus"].ToString() == "Abnormal" || 
                                                           sReader["OverallStatus"].ToString() == "Invalid") ? 
                                                           "#ff2c29" : "#57baa5",
                                        DeviceSerialNo = sReader["DeviceSerialNo"].ToString(),
                                        PMSFunction = isPMSUser
                                    }); ;

                                    iIndex++;
                                }
                            }
                        };


                        sConn.Close();
                    }
                }

                totalRecords = sResult.Count();

                return sResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
            }

            return sResult;
        }


        public static TestResultModel GetTestResultByID(IConfiguration config, long iTestResultID)
        {
            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    return ctx.txn_testResults.Where(x => x.ID == iTestResultID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<TestResultDetailsModel> GetResultDetailsByTestResultID(IConfiguration config, long iTestResultID)
        {
            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    return ctx.txn_testresults_details.Where(x => x.TestResultRowID == iTestResultID).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get Test Results by PatientID
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sPatientID"></param>
        /// <returns></returns>
        public static List<PatientDataObject> GetTestResultByPatientID(IConfiguration config, String sPatientID)
        {
            var sPatientObj = new List<PatientDataObject>();

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    MySqlConnection sConn = new MySqlConnection(ctx.Database.GetConnectionString());
                    sConn.Open();

                    String sSelectCommand = "SELECT A.ID, A.TestResultDateTime, A.OperatorID, A.PatientID, A.InchargePerson, " +
                                            "A.DeviceSerialNo, B.TestParameter, B.ProceduralControl, B.TestResultStatus, " +
                                            "B.TestResultValue, B.TestResultUnit, B.ReferenceRange " +
                                            "FROM txn_testresults AS A " +
                                            "INNER JOIN txn_testresults_details AS B ON B.TestResultRowID = A.ID " +
                                            "WHERE A.PatientID ='" + sPatientID + "' ";

                    using (MySqlCommand sCommand = new MySqlCommand(sSelectCommand, sConn))
                    {
                        using (var sReader = sCommand.ExecuteReader())
                        {
                            while (sReader.Read())
                            {
                                sPatientObj.Add(new PatientDataObject
                                {
                                    patientid = sReader["PatientID"].ToString(),
                                    observationdatetime = (sReader["TestResultDateTime"] != null) ?
                                                           Convert.ToDateTime(sReader["TestResultDateTime"]).ToString("yyyyMMddHHmmss") : null,
                                    observationtype = sReader["TestParameter"].ToString(),
                                    observationvalue = sReader["TestResultValue"].ToString(),
                                    observationresult = sReader["TestResultStatus"].ToString(),
                                    observationrules = sReader["TestResultUnit"].ToString(),
                                    inchargeperson = sReader["InchargePerson"].ToString(),
                                    observationby = sReader["OperatorID"].ToString()
                                });
                            }
                        }
                    }

                    sConn.Close();
                }

                return sPatientObj;
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get Test Results Details By Date Range
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sStart"></param>
        /// <param name="sEnd"></param>
        /// <returns></returns>
        public static List<TestResultAPIObject> GetTestResultDetailsByDateRange(IConfiguration config, DateTime sStart, DateTime sEnd)
        {
            List<TestResultAPIObject> sResult = new List<TestResultAPIObject>();

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    MySqlConnection sConn = new MySql.Data.MySqlClient.MySqlConnection(ctx.Database.GetConnectionString());
                    sConn.Open();

                    String sSelectCommand = "SELECT A.ID, A.TestResultDateTime, A.TestResultType, A.OperatorID, A.PatientID, A.InchargePerson, " +
                                            "A.DeviceSerialNo, A.OverallStatus, B.TestParameter, B.ProceduralControl, B.TestResultStatus, " +
                                            "B.TestResultValue, B.TestResultUnit, B.ReferenceRange " +
                                            "FROM txn_testresults AS A " +
                                            "INNER JOIN txn_testresults_details AS B ON B.TestResultRowID = A.ID " + 
                                            "WHERE A.TestResultDateTime >= '" + sStart.ToString("yyyy-MM-dd HH:mm:ss") + "' AND A.TestResultDateTime <= '" + sEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                            "ORDER BY A.TestResultDateTime";

                    using (MySqlCommand sCommand = new MySqlCommand(sSelectCommand, sConn))
                    {
                        using (var sReader = sCommand.ExecuteReader())
                        {
                            while (sReader.Read())
                            {
                                sResult.Add(new TestResultAPIObject
                                {
                                    ID  = Convert.ToInt64(sReader["ID"]),
                                    TestResultDateTime = Convert.ToDateTime(sReader["TestResultDateTime"]),
                                    TestResultType = sReader["TestResultType"].ToString(),
                                    OperatorID = sReader["OperatorID"].ToString(),
                                    DeviceSerialNo = sReader["DeviceSerialNo"].ToString(),
                                    PatientID = sReader["PatientID"].ToString(),
                                    InchargePerson = sReader["InchargePerson"].ToString(),
                                    OverallStatus = sReader["OverallStatus"].ToString(),
                                    TestResultParameter = sReader["TestParameter"].ToString(),
                                    ProceduralControl = sReader["ProceduralControl"].ToString(),
                                    TestResultStatus = sReader["TestResultStatus"].ToString(),
                                    TestResultValue = sReader["TestResultValue"].ToString(),
                                    TestResultUnit = sReader["TestResultUnit"].ToString(),
                                    ReferenceRange = sReader["ReferenceRange"].ToString()
                                });
                            }
                        }
                    }

                    sConn.Close();

                    return sResult;
                }
            }
            catch (Exception ex)
            {
                log.Error("TestResultRepository >>> GetTestResultDetailsByDateRange >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get list of test
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<TestListModel> GetAllTestList(IConfiguration config)
        {

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    return ctx.mst_testlist.ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Update test result info
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static void UpdateTestResult(IConfiguration config, TestResultModel testResult)
        {

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    ctx.txn_testResults.Update(testResult);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
            }
        }

        /// <summary>
        /// Get parameter list
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<ParametersModel> GetAllParameters(IConfiguration config)
        {

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    return ctx.mst_parameters.ToList().OrderBy(x => x.Analyzer).ThenBy(x => x.Order).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// Get previous test record
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<TestResultDetailsModel> GetPreviousTestRecord(IConfiguration config, TestResultModel currenttestInfo, out TestResultModel previousTest)
        {
            previousTest = new TestResultModel();

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    var previousTestTemp = ctx.txn_testResults.OrderByDescending(x => x.CreatedDate).FirstOrDefault(y => y.DeviceSerialNo == currenttestInfo.DeviceSerialNo && y.PatientID == currenttestInfo.PatientID && y.CreatedDate < currenttestInfo.CreatedDate);
                    previousTest = previousTestTemp;

                    if (previousTest != null)
                    {
                        return ctx.txn_testresults_details.Where(x => x.TestResultRowID == previousTestTemp.ID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// Get related test result
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<TestDeviceName> GetRelatedTestResultByPatientID(IConfiguration config, string patientID, string excludeDeviceName)
        {
            List<string> serialNoList = new List<string>();
            List<TestResultModel> testList = new List<TestResultModel>();
            List<TestDeviceName> testDeviceNames = new List<TestDeviceName>();

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    serialNoList = ctx.txn_testResults.Where(x => x.PatientID == patientID).Select(z => z.DeviceSerialNo).Distinct().ToList();
                    testList = ctx.txn_testResults.Where(x => x.PatientID == patientID).OrderByDescending(z => z.CreatedDate).ToList().DistinctBy(y => y.DeviceSerialNo).ToList();
                }

                foreach(var test in testList)
                {
                    var deviceName = DeviceRepository.GetDeviceNameBySerialNo(config, test.DeviceSerialNo);

                    if(deviceName != excludeDeviceName) 
                    {
                        testDeviceNames.Add(new TestDeviceName() { TestID = test.ID, DeviceName = DeviceRepository.GetDeviceNameBySerialNo(config, test.DeviceSerialNo) });
                    }
                    
                }

                return testDeviceNames;
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
            }

            return null;
        }
    }
}
