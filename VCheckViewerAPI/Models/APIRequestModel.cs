namespace VCheckViewerAPI.Models
{
    public class APIRequestModel
    {
        public string? PatientID { get; set; }
        public string? ScheduledUniqueID { get; set; }
        public string? ScheduledDatetime { get; set; }
        public string? InchargePerson { get; set; }
    }
}
