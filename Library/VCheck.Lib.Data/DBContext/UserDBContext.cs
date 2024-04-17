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
    public class UserDBContext
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;

        public UserDBContext(Microsoft.Extensions.Configuration.IConfiguration config)
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

        public ObservableCollection<UserModel> GetUserList()
        {
            ObservableCollection<UserModel> sList = new ObservableCollection<UserModel>();

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from userlist", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sList.Add(new UserModel()
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            EmployeeID = reader["EmployeeID"].ToString(),
                            //Title = reader["Title"].ToString(),
                            //FirstName = reader["FirstName"].ToString(),
                            //LastName = reader["LastName"].ToString(),
                            StaffName = reader["Title"].ToString() + ". " + reader["FirstName"].ToString() + " " + reader["LastName"].ToString(),
                            RegistrationNo = reader["RegistrationNo"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateOfBirth = DateOnly.FromDateTime(Convert.ToDateTime(reader["DateofBirth"])),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            Status = reader["Status"].ToString(),
                            Role = reader["Role"].ToString()
                        });
                    }
                }
            }

            return sList;
        }

        public ObservableCollection<UserModel> GetUserListByPage(int start, int end)
        {
            ObservableCollection<UserModel> sList = new ObservableCollection<UserModel>();

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from userlist order by UserID LIMIT " + start + "," + end, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sList.Add(new UserModel()
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            EmployeeID = reader["EmployeeID"].ToString(),
                            //Title = reader["Title"].ToString(),
                            //FirstName = reader["FirstName"].ToString(),
                            //LastName = reader["LastName"].ToString(),
                            StaffName = reader["Title"].ToString() + ". " + reader["FirstName"].ToString() + " " + reader["LastName"].ToString(),
                            RegistrationNo = reader["RegistrationNo"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateOfBirth = DateOnly.FromDateTime(Convert.ToDateTime(reader["DateofBirth"])),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            Status = reader["Status"].ToString(),
                            Role = reader["Role"].ToString()
                        });
                    }
                }
            }

            return sList;
        }
    }
}
