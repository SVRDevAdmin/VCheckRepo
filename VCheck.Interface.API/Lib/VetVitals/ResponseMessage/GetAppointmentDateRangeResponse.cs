using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.VetVitals.ResponseMessage
{
    public class GetAppointmentDateRangeResponse
    {
        public ResponseHeaderObject header { get; set; }
        public ResponseBodyObject body { get; set; }
    }

    public class ResponseHeaderObject
    {
        public string? timestamp { get; set; }
        public string? authtoken { get; set; }
    }

    public class ResponseBodyObject
    {
        public string? responsecode { get; set; }
        public string? responsestatus { get; set; }
        public string? responsemessage { get; set; }
        public List<AppointmentResultObject> results { get; set; }
    }

    public class AppointmentResultObject
    {
        public string? uniqueid { get; set; }
        public string? branchid { get; set; }
        public string? appointmentdate { get; set; }
        public string? starttime { get; set; }
        public string? endtime { get; set; }
        public string? patientid { get; set; }
        public string? petid { get; set; }
        public string? petname { get; set; }
        public string? ownerid { get; set; }
        public string? ownername { get; set; }
        public string? doctor { get; set; }
        public string? services { get; set; }
        public string? createddate { get; set; }
        public string? createdby { get; set; }
        public string? updateddate { get; set; }
        public string? updatedby { get; set; }
    }
}
