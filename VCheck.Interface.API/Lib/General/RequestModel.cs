using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.Lib.General
{
    public class RequestModel
    {
        public HeaderModel Header { get; set; }
        public RequestBody Body { get; set; }
    }

    public class HeaderModel
    {
        public string? timestamp { get; set; }
        public string? clientKey { get; set; }
    }

    public class RequestBody
    {
        public int? ClientID { get; set; }
        public string? URL { get; set; }
        public int? LocationID { get; set; }
        public LocationModel? LocationInfo { get; set; }
    }
}
