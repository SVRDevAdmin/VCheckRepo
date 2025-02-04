using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using VCheck.Lib.Data.DBContext;

namespace VCheck.Lib.Data
{
    public class LocationRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Get Location Lists
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<LocationModel> GetLocationList(IConfiguration config)
        {
            try
            {
                using (var ctx = new LocationDBContext(config))
                {
                    return ctx.mst_location.ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("LocationRepository >>> GetLocationList >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Validate Is Location ID Valid
        /// </summary>
        /// <param name="config"></param>
        /// <param name="iID"></param>
        /// <returns></returns>
        public static Boolean IsLocationIdExists(IConfiguration config, int iID)
        {
            try
            {
                using (var ctx = new LocationDBContext(config))
                {
                    return ctx.mst_location.Where(x => x.ID == iID && x.Status == 1).Any();
                }
            }
            catch (Exception ex)
            {
                log.Error("LocationRepository >>> GetLocationById >>> " + ex.ToString());
                return false;
            }
        }
    }
}
