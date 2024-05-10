using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;
using VCheck.Lib.Data.DBContext;

namespace VCheck.Lib.Data
{
    public class ScheduledTestRepository
    {
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
                return null;
            }
        }
    }
}
