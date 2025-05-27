using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.Lib.General
{
    public class UpdateLocationDataRequest
    {
        public HeaderModel Header { get; set; }
        public UpdateLocationDataRequestBody Body { get; set; }
    }

    public class UpdateLocationDataRequestBody
    {
        public String? ID { get; set; }
        public String? Name { get; set; }
        public String? Address { get; set; }
        public String? ContactName { get; set; }
        public String? PhoneNum { get; set; }
        public String? Email { get; set; }
        public String? Description { get; set; }
        public int? Status { get; set; }
        public String? CreatedBy { get; set; }
    }

    public class ResponseModel
    {
        public HeaderModel Header { get; set; }
        public ResponseBody Body { get; set; }
    }

    public class ResponseBody
    {
        public string ResponseCode { get; set; }
        public string ResponseStatus { get; set; }
        public string ResponseMessage { get; set; }
        public object? Results { get; set; }
    }
}
