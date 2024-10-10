using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.VetConnect
{
    public class UpdateTestResultsResponse
    {
        public UpdateTestResultsResponseHeaderObject header { get; set; }
        public UpdateTestResultsResponseBodyObject body { get; set; }
    }

    public class UpdateTestResultsResponseHeaderObject
    {
        public String? timestamp { get; set; }
        public String? authtoken { get; set; }
    }

    public class UpdateTestResultsResponseBodyObject
    {
        public String responsecode { get; set; }
        public String responsestatus { get; set; }
        public String responsemessage { get; set; }
    }
}
