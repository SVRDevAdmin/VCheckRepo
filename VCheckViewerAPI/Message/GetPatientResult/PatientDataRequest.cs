using VCheckViewerAPI.Message.General;

namespace VCheckViewerAPI.Message.GetPatientResult
{
    public class PatientDataRequest
    {
        public HeaderModel Header { get; set; }
        public PatientDataRequestBody Body { get; set; }
    }

    public class PatientDataRequestBody
    {
        public string? PatientID { get; set; }
    }
}
