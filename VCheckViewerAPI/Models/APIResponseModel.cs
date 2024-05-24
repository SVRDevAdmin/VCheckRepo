namespace VCheckViewerAPI.Models
{
    public class APIResponseModel
    {
        public string ResponseCode { get; set; }
        public string ResponseStatus { get; set; }
        public string ResponseMessage { get; set; }
        public object? Results { get; set; }

        public APIResponseModel(string ResponseCode, string ResponseStatus, string ResponseMessage, object? Results)
        {
            this.ResponseCode = ResponseCode;
            this.ResponseStatus = ResponseStatus;
            this.ResponseMessage = ResponseMessage;
            this.Results = Results;
        }
    }
}
