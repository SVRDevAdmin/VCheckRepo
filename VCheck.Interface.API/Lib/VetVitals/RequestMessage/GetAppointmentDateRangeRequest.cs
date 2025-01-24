using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.VetVitals.RequestMessage
{
    public class GetAppointmentDateRangeRequest
    {
        public RequestHeaderObject header { get; set; }
        public RequestBodyObject body { get; set; }
    }

    public class RequestHeaderObject
    {
        public String? timestamp { get; set; }
        public String? authtoken { get; set; }
    }

    public class RequestBodyObject
    {
        public String? transtype { get; set; }
        public String? startdate { get; set; }
        public String? enddate { get; set; }
    }
}
