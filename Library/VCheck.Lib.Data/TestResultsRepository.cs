using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;
using VCheck.Lib.Data.DBContext;
using Microsoft.Extensions.Configuration;

namespace VCheck.Lib.Data
{
    public class TestResultsRepository
    {
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
                return null;
            }
        }
    }
}
