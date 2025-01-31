﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API.VetVitals.RequestMessage
{
    public class UpdateTestResultsRequest
    {
        public UpdateTestResultsRequestHeaderObject header { get; set; }
        public UpdateTestResultsRequestBodyObject body { get; set; }
    }

    public class UpdateTestResultsRequestHeaderObject
    {
        public String? timestamp { get; set; }
        public String? authtoken { get; set; }
    }

    public class UpdateTestResultsRequestBodyObject
    {
        public List<UpdateTestResultsRequestBodyResultObject> results { get; set; }
    }

    public class UpdateTestResultsRequestBodyResultObject
    {
        public String resulttype { get; set; }
        public String resultdatetime { get; set; }
        public String operatorid { get; set; }
        public String patientid { get; set; }
        public String petid { get; set; }
        public String inchargeperson { get; set; }
        public String overallstatus { get; set; }
        public String devicename { get; set; }
        public List<UpdateTestResultsDetailsObject> resultdetails { get; set; }
    }

    public class UpdateTestResultsDetailsObject
    {
        public String resultparameter { get; set; }
        public String resultstatus { get; set; }
        public String resultvalue { get; set; }
        public String resultunit { get; set; }
        public String referencerange { get; set; }
    }
}
