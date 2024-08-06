using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.ezyVet.ResponseMessage
{
    public class DiagnosticGeneralObject
    {
        public String? id { get; set; }
        public String? active { get; set; }
        public String? created_at { get; set; }
        public String? modified_at { get; set; }
        public String? ownership_id { get; set; }
        public String? contact_id { get; set; }
        public String? consult_id { get; set; }
        public String? animal_id { get; set; }
        public String? vet_id { get; set; }
        public String? diagnostic_request_id { get; set; }
        public String? timestamp { get; set; }
        public String? specifies { get; set; }
        public String? outcome { get; set; }
        public List<String> diagnostic_result_item_list { get; set; }
    }
}
