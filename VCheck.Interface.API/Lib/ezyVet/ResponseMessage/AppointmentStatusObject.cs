using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Interface.API.ezyVet.ResponseMessage;

namespace VCheck.Interface.API.ezyVet.ResponseMessage
{
    public class AppointmentStatusObject
    {
        public metaApptStatusObject meta { get; set; }
        public List<itemsApptStatusObject> items {  get; set; }
    }

    public class metaApptStatusObject : MetaGeneralObject
    {

    }

    public class itemsApptStatusObject
    {
        public ApptStatusObject appointmentstatus { get; set; }
    }

    public class ApptStatusObject
    {
        public String? id { get; set; }
        public String? active { get; set; }
        public String? created_at { get; set; }
        public String? modified_at { get; set; }
        public String? name { get; set; }
    }
}
