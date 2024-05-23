using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;

namespace VCheck.Lib.Data.DBContext
{
    public class NotificationDBContext
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NotificationDBContext(Microsoft.Extensions.Configuration.IConfiguration config)
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

        public List<NotificationModel> GetNotificationByPage(int start, int end, string notificationType, string startDate, string endDate, string keyword)
        {
            List<NotificationModel> sList = new List<NotificationModel>();
            string sqlQueryCondition = "";

            if (notificationType != null) { sqlQueryCondition += "WHERE NotificationType = '" + notificationType + "'"; }
            else { sqlQueryCondition += "WHERE NotificationType <> 'Email'"; }

            if (startDate != null && endDate != null) { if (sqlQueryCondition != "") { sqlQueryCondition += " AND "; } else { sqlQueryCondition += "WHERE "; } sqlQueryCondition += "CreatedDate between '"+startDate+"' and '"+endDate+"'"; }
            else if(startDate != null && endDate == null) { if (sqlQueryCondition != "") { sqlQueryCondition += " AND "; } else { sqlQueryCondition += "WHERE "; } sqlQueryCondition += "CreatedDate = '" + startDate + "'"; }

            if (keyword != null) { if (sqlQueryCondition != "") { sqlQueryCondition += " AND "; } else { sqlQueryCondition += "WHERE "; } sqlQueryCondition += "(NotificationTitle like '%" + keyword + "%' OR NotificationContent like '%"+keyword+"%')"; }

            try
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("Select * from txn_notification " + sqlQueryCondition + " order by CreatedDate DESC LIMIT " + start + "," + end, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sList.Add(new NotificationModel()
                            {
                                NotificationID = reader.GetInt32("NotificationID"),
                                NotificationType = reader.GetString("NotificationType"),
                                NotificationTitle = reader.GetString("NotificationTitle"),
                                NotificationContent = reader.GetString("NotificationContent"),

                                Receiver = !reader.IsDBNull("Receiver") ? reader.GetString("Receiver") : null,

                                CreatedDate = reader.GetDateTime("CreatedDate").ToString("dd MMMM yyyy HH:mm"),
                                CreatedBy = reader.GetString("CreatedBy")
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

        public int GetTotalNotification(string notificationType, string startDate, string endDate, string keyword)
        {
            string sqlQueryCondition = "";

            if (notificationType != null) { sqlQueryCondition += "WHERE NotificationType = '" + notificationType + "'"; }
            else { sqlQueryCondition += "WHERE NotificationType <> 'Email'"; }

            if (startDate != null && endDate != null) { if (sqlQueryCondition != "") { sqlQueryCondition += " AND "; } else { sqlQueryCondition += "WHERE "; } sqlQueryCondition += "CreatedDate between '" + startDate + "' and '" + endDate + "'"; }
            else if (startDate != null && endDate == null) { if (sqlQueryCondition != "") { sqlQueryCondition += " AND "; } else { sqlQueryCondition += "WHERE "; } sqlQueryCondition += "CreatedDate = '" + startDate + "'"; }

            if (keyword != null) { if (sqlQueryCondition != "") { sqlQueryCondition += " AND "; } else { sqlQueryCondition += "WHERE "; } sqlQueryCondition += "(NotificationTitle like '%" + keyword + "%' OR NotificationContent like '%" + keyword + "%')"; }

            int total = 0;

            try
            {
                using (MySqlConnection conn = this.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("Select Count(NotificationTitle) from txn_notification " + sqlQueryCondition, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            total = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Database Error >>> ", ex);
            }

            return total;
        }

        public bool InsertNotification(NotificationModel notification)
        {
            string receiverColumn = "";
            string receiverData = "";
            if(notification.Receiver != null)
            {
                receiverColumn = ", `Receiver`";
                receiverData = ", '"+notification.Receiver+"'";
            }

            string insertQuery = "INSERT INTO `txn_notification` (`NotificationType`,`NotificationTitle`,`NotificationContent`,`CreatedDate`,`CreatedBy`" + receiverColumn + ") ";

            insertQuery += "Values ('" + notification.NotificationType + "','" + notification.NotificationTitle + "', '" + notification.NotificationContent + "', '" + notification.CreatedDate + "', '" + notification.CreatedBy + "'" + receiverData + ")";

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
    }
}
