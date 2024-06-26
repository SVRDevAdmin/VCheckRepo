﻿using log4net;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;

namespace VCheck.Lib.Data.DBContext
{
    public class MasterCodeDataDBContext
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

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

        public List<MasterCodeDataModel> GetMasterCodeData(string codeGroup)
        {
            List<MasterCodeDataModel> sList = new List<MasterCodeDataModel>();

            try
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("Select * from Mst_MasterCodeData where CodeGroup = '" + codeGroup + "' AND IsActive = 1;", conn);

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
            }
            catch (Exception ex)
            {
                log.Error("Database Error >>> ", ex);
            }

            return sList;
        }
    }
}
