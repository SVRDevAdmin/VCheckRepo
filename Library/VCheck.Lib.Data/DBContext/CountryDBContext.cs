using log4net;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;

namespace VCheck.Lib.Data.DBContext
{
    public class CountryDBContext
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CountryDBContext(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            this.config = config;
        }

        private MySqlConnection Connection
        {
            get
            {
                String? connectionString = this.config.GetConnectionString("DefaultConnection");
                return new MySqlConnection(connectionString);
            }
        }

        public List<CountryModel> GetCountryData(string partialName)
        {
            List<CountryModel> sList = new List<CountryModel>();

            try
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("Select * from mst_countrylist where CountryName like '%" + partialName + "%'", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sList.Add(new CountryModel()
                            {
                                CountryCode = reader["CountryCode"].ToString(),
                                CountryName = reader["CountryName"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Database Error >>> ", ex);
            }            

            return sList;
        }
    }
}
