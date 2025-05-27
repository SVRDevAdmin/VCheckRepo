using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.Greywind.RequestMessage
{
    public class InsertLocationRequest
    {
        public string? clinic_id { get; set; }
        public string name { get; set; }
        public string address_1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string phone { get; set; }
        public string api_access { get; set; }
    }
}
