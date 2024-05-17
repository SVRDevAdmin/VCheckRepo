using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;

namespace VCheck.Lib.Data.DBContext
{
    public class ConfigurationDBContext
    {
        private readonly IConfiguration config;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public ConfigurationDBContext(IConfiguration config)
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

        public List<ConfigurationModel> GetConfigurationData(string configurationKey)
        {
            List<ConfigurationModel> sList = new List<ConfigurationModel>();

            try
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("Select * from mst_configuration where ConfigurationKey like '%" + configurationKey + "%'", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sList.Add(new ConfigurationModel()
                            {
                                ConfigurationID = reader.GetInt32("ConfigurationID"),
                                ConfigurationKey = reader["ConfigurationKey"].ToString(),
                                ConfigurationValue = reader["ConfigurationValue"].ToString()
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

        public bool UpdateConfiguration(string ConfigurationKey, string ConfigurationValue)
        {
            string insertQuery = "UPDATE `vcheckdb`.`mst_configuration` SET `ConfigurationValue` = '" + ConfigurationValue + "' WHERE `ConfigurationKey` = '" + ConfigurationKey + "';";

            try
            {

                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                    cmd.ExecuteReader();
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error("Database Error >>> ", ex);
                return false;
            }
        }

        public bool AddConfiguration(string ConfigurationKey, String ConfigurationValue)
        {
            String InsertQuery = "INSERT INTO Mst_Configuration(ConfigurationKey, ConfigurationValue) " +
                                 "VALUES('" + ConfigurationKey + "', '" + ConfigurationValue + "')";

            try
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(InsertQuery, conn);
                    cmd.ExecuteReader();

                    conn.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error("Database Error >>> ", ex);
                return false ;
            }
        }
    }
}
