using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.Models
{
    public class ResponseModel
    {
        public HeaderModel Header { get; set; }
        public ResponseBody Body { get; set; }
    }

    public class ResponseBody
    {
        public string ResponseCode { get; set; }
        public string ResponseStatus { get; set; }
        public string ResponseMessage { get; set; }
        public object? Results { get; set; }
    }

    public class OrderInfo
    {
        public string? OrderID { get; set; }
        public int AccessionNo { get; set; }
    }
}
