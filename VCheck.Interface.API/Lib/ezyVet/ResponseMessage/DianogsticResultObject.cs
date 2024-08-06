using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.ezyVet.ResponseMessage
{
    public class DianogsticResultObject
    {
        public metaDignosticObject meta { get; set; }
        public List<itemsDiagnosticObject> items { get; set; }
    }

    public class metaDignosticObject
    {
        public String? timestamp { get; set; }
    }

    public class itemsDiagnosticObject
    {
        public diagResultObject diagnosticresult { get; set; }
    }

    public class diagResultObject : DiagnosticGeneralObject
    {
    }
}
