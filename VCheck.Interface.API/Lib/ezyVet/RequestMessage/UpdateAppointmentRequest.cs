using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.ezyVet.RequestMessage
{
    public class UpdateAppointmentRequest
    {
        public String? status_id {  get; set; }
        public String? cancel { get; set;  }
        public String? cancellation_reason_text {  get; set; }
        public String? cancellation_reason { get; set; }
    }
}
