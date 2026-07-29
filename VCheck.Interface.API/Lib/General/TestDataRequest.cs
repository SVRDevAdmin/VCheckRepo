using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.Lib.General
{
    public class TestDataRequest
    {
        public HeaderModel Header { get; set; }
        public TestDataRequestBody Body { get; set; }
    }

    public class TestDataRequestBody
    {
        public string? TestCode { get; set; }
        public string? TestName { get; set; }
    }
}
