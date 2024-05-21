using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;

namespace VCheck.Lib.Data.DBContext
{
    public class UserDBContext
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        public ObservableCollection<UserModel> GetUserListByPage(int start, int end)
        {
            ObservableCollection<UserModel> sList = new ObservableCollection<UserModel>();
            int index = start + 1;

            try
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("Select * from userlist where IsDeleted = 0 order by UserID LIMIT " + start + "," + end, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sList.Add(new UserModel()
                            {
                                No = index++,
                                UserId = reader["UserId"].ToString(),
                                EmployeeID = reader["EmployeeID"].ToString(),
                                Title = reader["Title"].ToString(),
                                //FirstName = reader["FirstName"].ToString(),
                                //LastName = reader["LastName"].ToString(),
                                StaffName = reader["Title"].ToString() + " " + reader["FullName"].ToString(),
                                FullName = reader["FullName"].ToString(),
                                RegistrationNo = reader["RegistrationNo"].ToString(),
                                Gender = reader["Gender"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateofBirth"]).ToString("dd MMMM yyyy"),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                Status = reader["Status"].ToString(),
                                Role = reader["Role"].ToString(),
                                LoginID = reader["LoginID"].ToString(),

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

        public bool InsertUser(UserModel user)
        {
            string insertQuery = "INSERT INTO `mst_user` (`UserID`,`EmployeeID`,`Title`,`FullName`,`RegistrationNo`,`Gender`,`DateofBirth`,`EmailAddress`,`Status`,`RoleID`, `IsDeleted`, `CreatedDate`, `CreatedBy`) ";

            insertQuery += "Values ('"+user.UserId+"','"+user.EmployeeID+"', '"+user.Title+"', '"+user.FullName+"', '"+user.RegistrationNo+"', '"+user.Gender+"', '"+user.DateOfBirth+"', '"+user.EmailAddress+"', "+user.StatusID+", '"+user.RoleID+"', 0, '"+user.CreatedDate+"', '"+user.CreatedBy+"' )";

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
                log.Error("Database Error >>> ", ex);
                return false;
            }
        }

        public bool UpdateUser(UserModel user)
        {
            var lastLoginDatetimeColumn = "";
            if (user.LastLoginDate != null) { lastLoginDatetimeColumn = ", `LastLoginDateTime` = '" + user.LastLoginDate.ToString("yyyy-MM-dd HH:mm:ss")+"'"; }

            string insertQuery = "UPDATE `mst_user` SET `EmployeeID` = '"+user.EmployeeID+"',`Title` = '"+user.Title+ "',`FullName` = '" + user.FullName+ "',`RegistrationNo` = '"+user.RegistrationNo+"',`Gender` = '"+user.Gender+"',`DateofBirth` = '"+user.DateOfBirth+"',`EmailAddress` = '"+user.EmailAddress+"',`Status` = "+user.StatusID+",`RoleID` = '"+user.RoleID+"'"+lastLoginDatetimeColumn+", `UpdatedDate` = '"+user.UpdatedDate+"', `UpdatedBy` = '"+user.updatedBy+"'  WHERE `UserID` = '" + user.UserId+"' and IsDeleted = 0;";

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
                log.Error("Database Error >>> ", ex);
                return false;
            }
        }

        public bool DeleteUser(string userID)
        {
            //string insertQuery = "DELETE FROM `vcheckdb`.`mst_user` WHERE UserID = '"+userID+"';";
            string insertQuery = "Update `mst_user` set IsDeleted = 1 WHERE UserID = '" + userID + "';";

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
                log.Error("Database Error >>> ", ex);
                return false;
            }
        }

        public int GetTotalUser()
        {
            string insertQuery = "SELECT Count(Title) FROM `mst_user` where IsDeleted = 0";
            int total = 0;

            try
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            total = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Database Error >>> ", ex);
            }

            return total;
        }

        public UserModel GetUserByID(string userID)
        {
            UserModel model = new UserModel();

            try
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("Select * from userlist where UserID = '" + userID + "' and IsDeleted = 0", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            model.UserId = reader["UserId"].ToString();
                            model.EmployeeID = reader["EmployeeID"].ToString();
                            model.Title = reader["Title"].ToString();
                            //model.FirstName = reader["FirstName"].ToString();
                            //model.LastName = reader["LastName"].ToString();
                            model.StaffName = reader["Title"].ToString() + " " + reader["FullName"].ToString();
                            model.FullName = reader["FullName"].ToString();
                            model.RegistrationNo = reader["RegistrationNo"].ToString();
                            model.Gender = reader["Gender"].ToString();
                            model.DateOfBirth = Convert.ToDateTime(reader["DateofBirth"]).ToString("dd MMMM yyyy");
                            model.EmailAddress = reader["EmailAddress"].ToString();
                            model.Status = reader["Status"].ToString();
                            model.StatusID = Convert.ToInt32(reader["StatusID"]);
                            model.Role = reader["Role"].ToString();
                            model.RoleID = reader["RoleID"].ToString();
                            model.LoginID = reader["LoginID"].ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Database Error >>> ", ex);
            }            

            return model;

        }
    }
}
