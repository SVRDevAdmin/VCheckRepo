using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

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

    public class NotificationSearch 
    {
        public int SearchStart { get; set; }
        public int SearchEnd { get; set; }
        public String? SearchType { get; set; }
        public String? SearchStartDate { get; set; }
        public String? SearchEndDate { get; set;}
        public String? SearchKeyword { get; set; }
        public bool SearchReset { get; set; }
        public ObservableCollection<DateTime>? SearchSelectedDates { get; set; }
    }
}
