using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.ezyVet.ResponseMessage
{
    public class AppointmentUpdateStatusObject
    {
        public metaApptObject? meta {  get; set; }
        public List<itemsApptObject>? items { get; set; }
    }

    public class metaApptObject
    {
        public String? items_processed { get; set; }
        public String? transaction_id {  get; set; }
        public String? timestamp {  get; set; }
    }

    public class itemsApptObject
    {
        public ApptObject appointment { get; set; }
    }

    public class ApptObject
    {
        public String? id { get; set; }
        public String? created_at { get; set; }
        public String? modified_at { get; set;  }
        public String? active {  get; set; }
        public String? start_at { get; set; }
        public String? duration { get; set; }
        public String? type_id { get; set; }
        public String? status_id { get; set;  }
        public String? description { get; set; }
        public String? animal_id { get; set; }
        public String? consult_id { get; set;  }
        public String? contact_id { get; set;  }
        public String? sales_resource { get; set; }
        public List<resourcesAppObject>? resources { get; set; }
        public String? cancellation_reason { get; set; }
        public String? cancellation_reason_text { get; set; }
    }

    public class resourcesAppObject
    {
        public String? id { get; set; }
    }
}
