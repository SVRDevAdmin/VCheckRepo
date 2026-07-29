using VCheck.Lib.Data.Models;
using VCheckViewerAPI.Message.General;

namespace VCheckViewerAPI.Message.Error
{
    public class ErrorDataRequest
    {
        public HeaderModel Header { get; set; }
        public ErrorDataRequestBody Body { get; set; }
    }

    public class ErrorDataRequestBody
    {
        public string? ClinicID { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
