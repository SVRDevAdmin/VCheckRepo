using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.ezyVet.ResponseMessage
{
    public class AppointmentTypeObject
    {
        public metaApptTypeObject meta { get; set; }
        public List<itemsAppType> items { get; set; } 
    } 

    public class metaApptTypeObject : MetaGeneralObject
    {
        public String? transaction_id { get; set; }
    }

    public class itemsAppType
    {
        public ApptObject appointmenttype { get; set; }
    }

    public class ApptTypeObject
    {
        public String? id { get; set; }
        public String? uid { get; set; }
        public String? active { get; set; }
        public String? name {  get; set; }
        public String? default_duration { get; set; }
}
