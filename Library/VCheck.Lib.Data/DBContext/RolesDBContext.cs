﻿using Microsoft.Extensions.Configuration;
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

            try
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("Select * from Mst_Roles where IsActive = 1", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sList.Add(new RolesModel()
                            {
                                RoleID = reader["RoleID"].ToString(),
                                RoleName = reader["RoleName"].ToString(),
                                IsSuperadmin = Convert.ToBoolean(reader["IsSuperadmin"]),
                                IsAdmin = Convert.ToBoolean(reader["IsAdmin"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return sList;
        }

        public bool InsertRole(RolesModel role)
        {
            string insertQuery = "INSERT INTO `vcheckdb`.`mst_roles` (`RoleID`,`RoleName`,`IsActive`,`IsSuperadmin`,`IsAdmin`) ";

            insertQuery += "Values ('" + role.RoleID + "','" + role.RoleName + "', " + role.IsActive + ", " + role.IsSuperadmin + ", " + role.IsAdmin + ")";

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
                return false;
            }
        }
    }
}
