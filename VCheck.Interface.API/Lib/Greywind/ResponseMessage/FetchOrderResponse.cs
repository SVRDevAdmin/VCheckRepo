using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.Greywind.ResponseMessage
{
    public class FetchOrderResponse
    {
        public String? order_id {  get; set; }
        public String? partner_guid { get; set; }
        public String? clinic_id { get; set; }
        public String? provider_id { get; set; }
        public FetchOrderPatientObject patient { get; set; }
        public FetchOrderOwnerObject owner { get; set; }
        public String? req_date { get; set; }
        public String? spec_date { get; set; }
        public String? comments { get; set; }
        public List<FetchOrderTestsObject> tests { get; set; }
    }

    public class FetchOrderPatientObject
    {
        public String? id { get; set; }
        public String? firstname { get; set; }
        public String? lastname { get; set; }
        public String? sex { get; set; }
        public String? species {  get; set; }
        public String? birthday { get; set; }
        public String? breed { get; set; }
    }

    public class FetchOrderOwnerObject
    {
        public String? id { get; set; }
        public String? firstname { get; set; }
        public String? lastname { get; set; }
    }

    public class FetchOrderTestsObject
    {
        public String? code { get; set; }
        public String? source { get; set; }
    }
}
