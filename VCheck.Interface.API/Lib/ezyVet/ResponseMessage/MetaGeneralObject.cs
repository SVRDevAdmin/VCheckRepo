using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.ezyVet.ResponseMessage
{
    public class MetaGeneralObject
    {
        public String? items_page { get; set; }
        public String? items_page_total { get; set; }
        public String? items_page_size { get; set; }
        public String? items_total { get; set; }
        public String? transaction_id { get; set; }
    }
}
