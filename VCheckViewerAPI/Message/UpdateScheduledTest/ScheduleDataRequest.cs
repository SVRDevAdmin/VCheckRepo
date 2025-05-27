using VCheckViewerAPI.Message.General;

namespace VCheckViewerAPI.Message.UpdateScheduledTest
{
    public class ScheduleDataRequest
    {
        public HeaderModel Header { get; set; }
        public ScheduleDataRequestBody Body { get; set; }
    }

    public class ScheduleDataRequestBody
    {
        public string? ScheduledUniqueID { get; set; }
        public string? ScheduledDatetime { get; set; }
        public string? InchargePerson { get; set; }
        public string? OrderID { get; set; }
        public string? LocationID { get; set; }
        public string? PatientID { get; set; }
        public string? ClientName { get; set; }
        public List<string>? Parameters { get; set; }
    }
}
