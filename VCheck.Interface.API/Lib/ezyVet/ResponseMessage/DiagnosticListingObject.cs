using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.ezyVet.ResponseMessage
{
    public class DiagnosticListingObject
    {
        public metaDiagnosticObject meta {  get; set; }
        public List<itemsDiagResultsObject> items { get; set; }
    }

    public class metaDiagnosticObject : MetaGeneralObject
    {
    }

    public class itemsDiagResultsObject
    {
        public DiagResultsObject diagnosticresult { get; set; }
    }

    public class DiagResultsObject : DiagnosticGeneralObject
    {
        public String? reference_number { get; set; }
    }
}
