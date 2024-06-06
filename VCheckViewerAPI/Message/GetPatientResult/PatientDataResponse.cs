using VCheckViewerAPI.Message.General;
using VCheck.Lib.Data.Models;

namespace VCheckViewerAPI.Message.GetPatientResult
{
    public class PatientDataResponse
    {
        public HeaderModel Header { get; set; }
        public PatientDataResponseBody Body { get; set; }
    }

    public class PatientDataResponseBody 
    {
        public String? responsecode { get; set; }
        public String? responsestatus { get; set; }
        public String? responsemessage { get; set; }
        public List<PatientDataObject>? results { get; set; }
    }
}
