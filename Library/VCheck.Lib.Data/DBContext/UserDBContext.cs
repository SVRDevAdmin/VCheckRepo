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
                            Title = reader["Title"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            StaffName = reader["Title"].ToString() + " " + reader["FirstName"].ToString() + " " + reader["LastName"].ToString(),
                            RegistrationNo = reader["RegistrationNo"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateOfBirth = reader["DateofBirth"].ToString(),
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
            int index = 1;

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
                            No = index++,
                            UserId = Convert.ToInt32(reader["UserId"]),
                            EmployeeID = reader["EmployeeID"].ToString(),
                            Title = reader["Title"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            StaffName = reader["Title"].ToString() + " " + reader["FirstName"].ToString() + " " + reader["LastName"].ToString(),
                            RegistrationNo = reader["RegistrationNo"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateofBirth"]).ToString("dd MMMM yyyy"),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            Status = reader["Status"].ToString(),
                            Role = reader["Role"].ToString()
                        });
                    }
                }
            }

            return sList;
        }

        public void InsertUser(UserModel user)
        {
            string insertQuery = "INSERT INTO `vcheckdb`.`mst_user` (`EmployeeID`,`Title`,`FirstName`,`LastName`,`RegistrationNo`,`Gender`,`DateofBirth`,`EmailAddress`,`Status`,`RoleID`) ";

            insertQuery += "Values ('"+user.EmployeeID+"', '"+user.Title+"', '"+user.FirstName+"', '"+user.LastName+"', '"+user.RegistrationNo+"', '"+user.Gender+"', '"+user.DateOfBirth+"', '"+user.EmailAddress+"', "+user.StatusID+", "+user.RoleID+")";

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                cmd.ExecuteReader();
            }
        }

        public void UpdateUser(UserModel user)
        {
            string insertQuery = "UPDATE `vcheckdb`.`mst_user` SET `EmployeeID` = '"+user.EmployeeID+"',`Title` = '"+user.Title+"',`FirstName` = '"+user.FirstName+ "',`LastName` = '" + user.LastName + "',`RegistrationNo` = '"+user.RegistrationNo+"',`Gender` = '"+user.Gender+"',`DateofBirth` = '"+user.DateOfBirth+"',`EmailAddress` = '"+user.EmailAddress+"',`Status` = "+user.StatusID+",`RoleID` = "+user.RoleID+" WHERE `UserID` = " + user.UserId+";";

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                cmd.ExecuteReader();
            }
        }

        public void DeleteUser(int userID)
        {
            //user.DateOfBirth = "1991-03-15";

            string insertQuery = "DELETE FROM `vcheckdb`.`mst_user` WHERE UserID = "+userID;

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                cmd.ExecuteReader();
            }
        }
    }
}
