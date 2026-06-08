using System.ComponentModel.DataAnnotations;

namespace VCheckListener.Lib.Models
{
    public class txn_notification
    {
        [Key]
        public int NotificationID { get; set; }
        public string NotificationType { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationContent { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }

    public class mst_template
    {
        [Key]
        public int TemplateID { get; set; }
        public string? TemplateType { get; set; }
        public string? TemplateCode { get; set; }
        public string? TemplateTitle { get; set; }
        public string? TemplateContent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class mst_template_details
    {
        [Key]
        public int ID { get; set; }
        public int TemplateID { get; set; }
        public string? LangCode { get; set; }
        public string? TemplateTitle { get; set; }
        public string? TemplateContent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class NotificationTemplateLang
    {
        public int TemplateID { get; set; }
        public string? TemplateType { get; set; }
        public string? TemplateCode { get; set; }
        public string? TemplateTitle { get; set; }
        public string? TemplateContent { get; set; }
        public string? TemplateLangCode { get; set; }
    }

    public class mst_configuration
    {
        [Key]
        public int ConfigurationID { get; set; }
        public string? ConfigurationKey { get; set; }
        public string? ConfigurationValue { get; set; }
    }

    public class ScheduledTestModel
    {
        [Key]
        public long ID { get; set; }
        public string? ScheduledTestType { get; set; }
        public DateTime? ScheduledDateTime { get; set; }
        public string? ScheduledBy { get; set; }
        public string? PatientID { get; set; }
        public string? PatientName { get; set; }
        public string? Gender { get; set; }
        public string? Species { get; set; }
        public string? OwnerName { get; set; }
        public string? InchargePerson { get; set; }
        public int? ScheduleTestStatus { get; set; }
        public int? TestCompleted { get; set; }
        public string? ScheduleUniqueID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? LocationID { get; set; }
    }

    public class DeviceModel
    {
        [Key]
        public int id { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceSerialNo { get; set; }
        public string? Description { get; set; }
        public string? DeviceIPAddress { get; set; }
        public string? DeviceImagePath { get; set; }
        public int? status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public int? DeviceTypeID { get; set; }
    }

    public class DeviceTypeModel
    {
        [Key]
        public int id { get; set; }
        public string? TypeName { get; set; }
        public string? ParameterType { get; set; }
        public int? Status { get; set; }
        public int? SeqNo { get; set; }
        public int? TwoWayCommunication { get; set; }
        public int? PosNegRequired { get; set; }
        public string? ImageSource { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class MainModel
    {
        string _deviceSerialNum;

        public string DeviceSerialNum
        {
            get { return _deviceSerialNum; }
            set { _deviceSerialNum = value; }
        }

        string _deviceType;

        public string DeviceType
        {
            get { return _deviceType; }
            set { _deviceType = value; }
        }
    }
}
