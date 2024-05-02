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
    public class RolesDBContext
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;

        public RolesDBContext(Microsoft.Extensions.Configuration.IConfiguration config)
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

        public List<RolesModel> GetRoles()
        {
            List<RolesModel> sList = new List<RolesModel>();

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from Mst_Roles", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sList.Add(new RolesModel()
                        {
                            RoleID = reader["RoleID"].ToString(),
                            RoleName = reader["RoleName"].ToString(),
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            IsSuperadmin = Convert.ToBoolean(reader["IsSuperadmin"]),
                            IsAdmin = Convert.ToBoolean(reader["IsAdmin"])
                        });
                    }
                }
            }

            return sList;
        }
    }
}
