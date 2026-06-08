using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.Lib.General
{
    public class ScheduleDataRequest
    {
        public HeaderModel Header { get; set; }
        public ScheduleDataRequestBody Body { get; set; }
    }

    public class ScheduleDataRequestBody
    {
        public bool ExtendDateTime { get; set; }
        public string? ScheduledUniqueID { get; set; }
        public string? LocationID { get; set; }
        public string? PatientID { get; set; }
        public string? OrderID { get; set; }
        public string? ClientName { get; set; }
        public int Status { get; set; }
        public List<string> Parameters { get; set; }
        public string? AnalyzerName { get; set; }
        public string TestName { get; set; }
        public bool IgnoreOrderStatus { get; set; } = true;
    }
}
