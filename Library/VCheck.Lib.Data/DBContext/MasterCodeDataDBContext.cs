using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;

namespace VCheck.Lib.Data.DBContext
{
    public class MasterCodeDataDBContext
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;

        public MasterCodeDataDBContext(Microsoft.Extensions.Configuration.IConfiguration config)
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

        public List<MasterCodeDataModel> GetMasterCodeData()
        {
            List<MasterCodeDataModel> sList = new List<MasterCodeDataModel>();

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from Mst_MasterCodeData", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sList.Add(new MasterCodeDataModel()
                        {
                            CodeGroup = reader["CodeGroup"].ToString(),
                            CodeID = reader["CodeID"].ToString(),
                            CodeName = reader["CodeName"].ToString()
                        });
                    }
                }
            }

            return sList;
        }
    }
}
