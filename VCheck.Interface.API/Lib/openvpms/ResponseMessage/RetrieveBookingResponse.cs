using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.openvpms.ResponseMessage
{
    public class RetrieveBookingResponse
    {
        public int location { get; set; }
        public int schedule { get; set; }
        public int appointmentType { get; set; }
        public String? start { get; set; }
        public String? end { get; set; }    
        public String? title {  get; set; }
        public String? fristName { get; set; }
        public String? lastName { get; set; }
        public String? email { get; set; } 
        public String? phone { get; set; }
        public String? mobile { get; set; }
        public String? patientName { get; set; }
        public String? notes { get; set; }
        public String? user { get; set; }
    }
}
