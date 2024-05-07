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

            return sList;
        }

        public void UpdateConfiguration(string ConfigurationKey, string ConfigurationValue)
        {
            string insertQuery = "UPDATE `vcheckdb`.`mst_configuration` SET `ConfigurationValue` = '" + ConfigurationValue + "' WHERE `ConfigurationKey` = '" + ConfigurationKey + "';";

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                cmd.ExecuteReader();
            }
        }
    }
}
