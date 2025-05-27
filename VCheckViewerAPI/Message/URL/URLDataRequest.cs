using VCheck.Lib.Data.Models;
using VCheckViewerAPI.Message.General;

namespace VCheckViewerAPI.Message.URL
{
    public class URLDataRequest
    {
        public HeaderModel Header { get; set; }
        public URLDataRequestBody Body { get; set; }
    }

    public class URLDataRequestBody
    {
        public int? ClientID { get; set; }
        public string? URL { get; set; }
    }
}
