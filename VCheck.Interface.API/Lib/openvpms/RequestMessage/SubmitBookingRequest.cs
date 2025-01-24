using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.openvpms.RequestMessage
{
    public class SubmitBookingRequest
    {
        public String? location {  get; set; }
        public String? schedule {  get; set; }
        public String? appointmentType { get; set; }
        public String? start {  get; set; }
        public String? end { get; set; } 
        public String? firstName { get; set; }
        public String? lastName { get; set; }
        public String? patientName { get; set; }
        public String? user {  get; set; }
        public String? mobile { get; set; }
        public String? title { get; set; }
    }
}
