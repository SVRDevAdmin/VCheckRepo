using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.Lib.General
{
    public class ErrorDataRequest
    {
        public HeaderModel Header { get; set; }
        public ErrorDataRequestBody Body { get; set; }
    }

    public class ErrorDataRequestBody
    {
        public string? ClinicID { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
