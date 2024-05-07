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
    public class TemplateDBContext
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;

        public TemplateDBContext(Microsoft.Extensions.Configuration.IConfiguration config)
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

        public TemplateModel GetTemplateByCode(string templateCode)
        {
            TemplateModel sList = new TemplateModel();

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from mst_template where TemplateCode = '" + templateCode + "'", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sList.TemplateID = reader.GetInt32("TemplateID");
                        sList.TemplateType = reader["TemplateType"].ToString();
                        sList.TemplateCode = reader["TemplateCode"].ToString();
                        sList.TemplateTitle = reader["TemplateTitle"].ToString();
                        sList.TemplateContent = reader["TemplateContent"].ToString();

                    }
                }
            }

            return sList;
        }
    }
}
