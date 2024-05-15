using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.Models
{
    public class NotificationModel : AuditModel
    {
        public int NotificationID { get; set; }
        public string NotificationType { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationContent { get; set; }
        public string? Receiver {  get; set; }
        //public string CreatedDate { get; set; }
        //public string CreatedBy { get; set;}
    }
}
