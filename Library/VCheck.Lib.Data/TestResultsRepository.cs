using System;
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

namespace VCheck.Lib.Data
{
    public class TestResultsRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Get recent test result 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<TestResultModel> GetLatestTestResultList(IConfiguration config)
        {
            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    return ctx.txn_testResults.OrderBy(x => x.TestResultDateTime)
                                              .OrderByDescending(x => x.TestResultDateTime)
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
        public static List<TestResultListingObj> GetTestResultListBySearch(IConfiguration config, String dStartDate, String dEndDate, 
                                                                           String sKeyword, String sSortDirection, int pageIndex, int pageSize,
                                                                           out int totalRecords)
        {
            List<TestResultListingObj> sResult = new List<TestResultListingObj>();
            totalRecords = 0;
            int iIndex = 1;

            try
            {
                using (var ctx = new TestResultDBContext(config))
                {
                    using (MySqlConnection sConn = new MySql.Data.MySqlClient.MySqlConnection(ctx.Database.GetConnectionString()))
                    {
                        sConn.Open();

                        String sSelectCommand = "SELECT * " +
                                                "FROM txn_testresults " +
                                                "WHERE (TestResultDateTime >= '" + dStartDate + "' AND TestResultDateTime <= '" + dEndDate + "')" +
                                                "AND (OperatorID LIKE '%" + sKeyword + "%' OR " +
                                                "InchargePerson LIKE '%" + sKeyword + "%' OR " +
                                                "PatientID LIKE '%" + sKeyword + "%') " +
                                                ((!sSortDirection.Contains("Status")) ? 
                                                 "ORDER BY TestResultDateTime " + sSortDirection : 
                                                 "ORDER BY TestResultStatus " + sSortDirection.Replace("Status", "").Trim());

                        using (MySqlCommand sCommand = new MySqlCommand(sSelectCommand, sConn))
                        {
                            using (var sReader = sCommand.ExecuteReader())
                            {
                                while (sReader.Read())
                                {
                                    sResult.Add(new TestResultListingObj
                                    {
                                        RowNo = iIndex,
                                        ID = sReader.GetInt64("ID"),
                                        TestResultDateTime = Convert.ToDateTime(sReader["TestResultDateTime"]),
                                        TestResultDateTimeString = (Convert.ToDateTime(sReader["TestResultDateTime"])).ToString("dd/MM/yyyy HH:mm"),
                                        TestResultType = sReader["TestResultType"].ToString(),
                                        OperatorID = sReader["OperatorID"].ToString(),
                                        PatientID = sReader["PatientID"].ToString(),
                                        InchargePerson = sReader["InchargePerson"].ToString(),
                                        ObservationStatus = sReader["ObservationStatus"].ToString(),
                                        TestResultStatus = sReader["TestResultStatus"].ToString(),
                                        TestResultValue = Convert.ToDecimal(sReader["TestResultValue"]),
                                        TestResultValueString = (Convert.ToDecimal(sReader["TestResultValue"])).ToString("n2"),
                                        TestResultRules = sReader["TestResultRules"].ToString(),
                                        CreatedDate = Convert.ToDateTime(sReader["CreatedDate"]),
                                        CreatedBy = sReader["CreatedBy"].ToString(),
                                        statusBackground = (sReader["TestResultStatus"].ToString() == "Positive") ? "#F5B7B1" : "#D1F2EB ",
                                        statusFontColor = (sReader["TestResultStatus"].ToString() == "Positive") ? "#ff2c29" : "#57baa5"
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
    }
}
