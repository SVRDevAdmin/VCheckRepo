using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.Lib.General
{
    public class ClientDataRequest
    {
        public HeaderModel Header { get; set; }
        public ClientDataRequestBody Body { get; set; }
    }

    public class ClientDataRequestBody
    {
        public string? Version { get; set; }
    }
}
