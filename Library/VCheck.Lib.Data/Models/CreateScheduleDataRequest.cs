using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.Models
{
    public class CreateScheduledDataRequest
    {
        public HeaderModel Header { get; set; }
        public CreateScheduledDataRequestBody body { get; set; }
    }

    public class HeaderModel
    {
        public string? timestamp { get; set; }
        public string? clientKey { get; set; }
    }

    public class CreateScheduledDataRequestBody
    {
        public String? LocationID { get; set; }
        public String? ScheduledTestName { get; set; }
        public String? ScheduledDateTime { get; set; }
        //public String? ScheduledUniqueID { get; set; }
        public String? TestUniqueID { get; set; }
        public String? ScheduledBy { get; set; }
        public String? PersonIncharges { get; set; }
        public String? PatientID { get; set; }
        public String? PatientName { get; set; }
        public String? Gender { get; set; }
        public String? Species { get; set; }
        public String? OwnerName { get; set; }
        public String? ScheduledCreatedDate { get; set; }
        public String? PatientDOB { get; set; }

        public Boolean ValidateMandatoryField()
        {
            Boolean isValid = true;

            if (String.IsNullOrEmpty(LocationID))
            {
                isValid = false;
            }

            if (String.IsNullOrEmpty(ScheduledTestName))
            {
                isValid = false;
            }

            if (String.IsNullOrEmpty(ScheduledDateTime))
            {
                isValid = false;
            }

            if (String.IsNullOrEmpty(TestUniqueID))
            {
                isValid = false;
            }

            if (String.IsNullOrEmpty(PersonIncharges))
            {
                isValid = false;
            }

            if (String.IsNullOrEmpty(PatientID))
            {
                isValid = false;
            }

            if (String.IsNullOrEmpty(PatientName))
            {
                isValid = false;
            }

            if (String.IsNullOrEmpty(Gender))
            {
                isValid = false;
            }

            if (String.IsNullOrEmpty(Species))
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
