using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.ezyVet.RequestMessage
{
    public class GetAppointmentRequest
    {
        public String? id { get; set; }
        public String? created_at { get; set; }
        public String? modified_at { get; set; }
        public String? active { get; set; }
        public String? start_at { get; set; }
        public String? duration { get; set; }
        public String? type_id { get; set; }
        public String? status_id { get; set; }
        public String? animal_id { get; set; }
        public String? consult_id { get; set; }
        public String? sales_resource { get; set; }
    }
}
