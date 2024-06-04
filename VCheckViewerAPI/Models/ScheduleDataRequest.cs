namespace VCheckViewerAPI.Models
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
    }
}
