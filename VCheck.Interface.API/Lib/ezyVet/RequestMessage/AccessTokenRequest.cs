using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.ezyVet.RequestMessage
{
    public class AccessTokenRequest
    {
        public String? partner_id {  get; set; }
        public String? client_id { get; set; }
        public String? client_secret {  get; set; }
        public String? grant_type { get; set; }
        public String? scope { get; set;  }
    }
}
