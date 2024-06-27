using log4net;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;

namespace VCheck.Lib.Data.DBContext
{
    public class TemplateDBContext
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

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

            try
            {
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
            }
            catch (Exception ex)
            {
                log.Error("Database Error >>> ", ex);
            }

            return sList;
        }

        public TemplateModel GetTemplateByCodeLang(string templateCode, string langCode = "en")
        {
            TemplateModel sList = new TemplateModel();

            try
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT A.TemplateID, A.TemplateType, A.TemplateCode, B.LangCode, B.TemplateTitle, B.TemplateContent " +
                                                        "FROM mst_template AS A LEFT JOIN mst_template_details AS B ON B.TemplateID = A.TemplateID " +
                                                        "Where A.TemplateCode = '" + templateCode + "' AND B.LangCode = '" + langCode + "'", conn);

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
            }
            catch (Exception ex)
            {
                log.Error("Database Error >>> ", ex);
            }

            return sList;
        }
    }
}
