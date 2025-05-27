using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.Lib.General
{
    public class URLDataRequest
    {
        public HeaderModel Header { get; set; }
        public URLDataRequestBody Body { get; set; }
    }

    public class URLDataRequestBody
    {
        public int? ClientID { get; set; }
        public string? URL { get; set; }
    }
}
