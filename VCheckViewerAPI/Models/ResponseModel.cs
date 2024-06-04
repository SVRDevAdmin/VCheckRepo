namespace VCheckViewerAPI.Models
{
    public class ResponseModel
    {
        public HeaderModel Header { get; set; }
        public ResponseBody Body { get; set; }
    }

    public class ResponseBody
    {
        public string ResponseCode { get; set; }
        public string ResponseStatus { get; set; }
        public string ResponseMessage { get; set; }
        public object? Results { get; set; }
    }
}
