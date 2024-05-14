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
    public class NotificationDBContext
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;

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

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from txn_notification " + sqlQueryCondition+" order by CreatedDate DESC LIMIT " + start + "," + end, conn);

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
                            CreatedDate = reader.GetDateTime("CreatedDate").ToString("dd MMMM yyyy HH:mm"),
                            CreatedBy = reader.GetString("CreatedBy")
                        });
                    }
                }
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

            return total;
        }

        public void InsertNotification(NotificationModel notification)
        {
            string insertQuery = "INSERT INTO `vcheckdb`.`txn_notification` (`NotificationType`,`NotificationTitle`,`NotificationContent`,`CreatedDate`,`CreatedBy`) ";

            insertQuery += "Values ('" + notification.NotificationType + "','" + notification.NotificationTitle + "', '" + notification.NotificationContent + "', '" + notification.CreatedDate + "', '" + notification.CreatedBy + "')";

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                cmd.ExecuteReader();
            }
        }
    }
}
