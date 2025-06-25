namespace VCheckViewerAPI.Message.General
{
    public class ClientDataRequest
    {
        public HeaderModel Header { get; set; }
        public ClientDataRequestBody Body { get; set; }
    }

    public class ClientDataRequestBody
    {
        public string Version { get; set; }
    }
}
