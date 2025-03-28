using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VCheck.Lib.Data.Models
{
    public class TestResultModel
    {
        [Key]
        public long ID { get; set; }
        public DateTime? TestResultDateTime { get; set; }
        public String? TestResultType { get; set; }
        public String? OperatorID { get; set; }
        public String? DeviceSerialNo { get; set; }
        public String? PatientID { get; set; }
        public String? PatientName { get; set; } //added to include patient name in report (azwan - 20250214)
        public String? InchargePerson { get; set; }
        //public String? ObservationStatus { get; set; }
        public String? OverallStatus { get; set; }
        //public String? TestResultStatus { get; set; }
        //public Decimal? TestResultValue { get; set;  }
		//public String? TestResultValue { get; set; }
		//public String? TestResultRules { get; set; }
        public DateTime? CreatedDate { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public String? UpdatedBy { get; set; }
    }

    public class TestResultDetailsModel
    {
        [Key]
        public long ID { get; set; }
        public long TestResultRowID { get; set; }
        public String? TestParameter { get; set; }
        public String? SubID { get; set; }
        public String? ProceduralControl {  get; set; }
        public String? TestResultStatus { get; set; }
        public String? TestResultValue { get; set; }
        public String? TestResultUnit { get; set; }
        public String? ReferenceRange { get; set; }
        public String? Interpretation { get; set; }

    }

    public class TestResultOutputFileModel : TestResultModel
    {
        public String? TestParameter { get; set; }
        public String? TestResultStatus { get; set; }
        //public String? TestResultValue { get; set; }
        public String? TestResultUnit { get; set; }
        public String? ReferenceRange { get; set; }
    }

    public class TestResultExtendedModel
    {
        [Key]
        public long ID { get; set; }
        public DateTime? TestResultDateTime { get; set; }
        public String? TestResultType { get; set; }
        public String? OperatorID { get; set; }
        public String? DeviceSerialNo { get; set; }
        public String? PatientID { get; set; }
        public String? PatientName { get; set; }
        public String? InchargePerson { get; set; }
        public String? ObservationStatus { get; set; }
        public String? TestResultStatus { get; set; }
        public String? TestResultValue { get; set; }
        public String? TestResultRules { get; set; }
        public DateTime? CreatedDate { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public String? UpdatedBy { get; set; }
    }

    public class TestResultAPIObject
    {
        public long ID { get; set; }
        public DateTime? TestResultDateTime { get; set; }
        public String? TestResultType { get; set; }
        public String? OperatorID { get; set; }
        public String? DeviceSerialNo { get; set; }
        public String? PatientID { get; set; }
        public String? InchargePerson { get; set; }
        public String? OverallStatus { get; set; }
        public String? TestResultParameter { get; set; }
        public String? ProceduralControl { get; set; }
        public String? TestResultStatus { get; set; }
        public String? TestResultValue { get; set; }
        public String? TestResultUnit { get; set; }
        public String? ReferenceRange { get; set; }
    }

    public class TestResultListingObj : TestResultModel 
    { 
        public long RowNo { get; set; }
        public String? TestResultDateTimeString { get; set; }
        public String? TestResultValueString { get; set; }
        public String? statusBackground { get; set; }
        public String? statusFontColor { get; set; }
        public String? printedBy { get; set;  }
        public DateTime? printedOn { get; set; }
        public Boolean isPrint { get; set; }
    }

    public class TestResultListingExtendedObj : TestResultExtendedModel
    {
        public long RowNo { get; set; }
        public String? TestResultDateTimeString { get; set; }
        public String? TestResultValueString { get; set; }
        public String? statusBackground { get; set; }
        public String? statusFontColor { get; set; }
        public String? printedBy { get; set; }
        public DateTime? printedOn { get; set; }
        public Boolean isPrint { get; set; }
        public string PMSFunction { get; set; }
    }

    public class PatientDataObject
    {
        public String? patientid { get; set; }
        public String? observationdatetime { get; set; }
        public String? observationtype { get; set; }
        public String? observationvalue { get; set; }
        public String? observationresult { get; set; }
        public String? observationrules { get; set; }
        public String? inchargeperson { get; set; }
        public String? observationby { get; set; }
    }

    public class TestListModel
    {
        [Key]
        public String? TestID { get; set; }
        public String? TestName { get; set; }
        public String? TestDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public String? UpdatedBy { get; set; }
    }

    public class ParametersModel
    {
        [Key]
        public int ID { get; set; }
        public String? Analyzer { get; set; }
        public String? Category { get; set; }
        public String? Parameter { get; set; }
        public int Order { get; set; }
    }

    public class DownloadPrintResultModel
    {
        public TestResultModel TestResult { get; set; }
        public List<TestResultDetailsModel> TestResultDetails { get; set; }
        public TestResultModel PreviousTestResult { get; set; }
        public List<TestResultDetailsModel> PreviousTestResultDetails { get; set; }
    }

    public class TestDeviceName
    {
        public long TestID { get; set; }
        public string DeviceName { get; set; }
    }
}
