using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcheck_Listener.Lib.Models
{
    public class txn_testresults
    {
        public long ID { get; set; }
        public DateTime? TestResultDateTime { get; set; }
        public string? TestResultType { get; set; }
        public string? OperatorID { get; set; }
        public string? PatientID { get; set; }
        public string? PatientName { get; set; }
        public string? InchargePerson { get; set; }
        public string? OverallStatus { get; set; }
        public string PMSFunction { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeviceSerialNo { get; set; }
    }

    public class txn_testresults_details
    {
        public long ID { get; set; }
        public long TestResultRowID { get; set; }
        public string? TestParameter { get; set; }
        public string? SubID { get; set; }
        public string? ProceduralControl { get; set; }
        public string? TestResultStatus { get; set; }
        public string? TestResultValue { get; set; }
        public string? TestResultUnit { get; set; }
        public string? ReferenceRange { get; set; }
        public string? MeasuringRange { get; set; }
        public string? Interpretation { get; set; }
    }

    public class txn_testresults_details_extended : txn_testresults_details
    {
        public string? PatientID { get; set; }
    }
}
