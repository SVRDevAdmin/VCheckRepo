using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.ezyVet.RequestMessage
{
    public class CreateDiagnosticRequest
    {
        public string? active { get; set; }
        public String? consult_id { get; set; }
        public String? animal_id { get; set; }
        public String? supplier_id { get; set; }
        public String? vet_id { get; set; }
        public String? diagnostic_request_id { get; set; }
        public String? timestamp { get; set; }
        public String? specifies { get; set; }
        public String? outcome { get; set; }
        public List<DiagnosticResultItemObject>? diagnostic_result_item_list { get; set; }
        public String? external_link { get; set; }
        
    }

    public class DiagnosticResultItemObject
    {
        public String? name { get; set;  }
        public String? value { get; set; }
        public String? unit { get; set; }
        public String? range_low { get; set; }
        public String? range_high { get; set; }
    }
}
