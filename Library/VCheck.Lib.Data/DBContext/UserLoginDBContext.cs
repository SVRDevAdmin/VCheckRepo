using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class UserLoginDBContext
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;

        public UserLoginDBContext(Microsoft.Extensions.Configuration.IConfiguration config)
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

        public UserModel ValidateLogin(string LoginID, string password)
        {
            UserModel sList = new UserModel();
            bool isExist = false;
            int UserID = 0;

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select UserID from mst_userlogin where LoginID = '"+LoginID+"' AND LoginPassword = '"+password+"' AND isLocked = 0", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserID = reader.GetInt32(0);
                    }
                }
            }

            if(UserID != 0)
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("Select * from userlist where UserID = " + UserID, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sList.UserId = reader["UserId"].ToString();
                            sList.EmployeeID = reader["EmployeeID"].ToString();
                            sList.Title = reader["Title"].ToString();
                            sList.FirstName = reader["FirstName"].ToString();
                            sList.LastName = reader["LastName"].ToString();
                            sList.StaffName = reader["Title"].ToString() + " " + reader["FirstName"].ToString() + " " + reader["LastName"].ToString();
                            sList.RegistrationNo = reader["RegistrationNo"].ToString();
                            sList.Gender = reader["Gender"].ToString();
                            sList.DateOfBirth = Convert.ToDateTime(reader["DateofBirth"]).ToString("dd MMMM yyyy");
                            sList.EmailAddress = reader["EmailAddress"].ToString();
                            sList.Status = reader["Status"].ToString();
                            sList.Role = reader["Role"].ToString();
                            
                        }
                    }
                }
            }
            return sList;
        }
    }
}
